using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoxCulvert.Model;

namespace BoxCulvert.Core
{
    public class Desarrollo
    {
        // ====  CÁLCULO DE CARGAS Y EMPUJES  ==== //
        public List<ResultCargas> CalculoEmpujes(GeomEstructura GeomEst, Materiales materiales, Suelo suelo, Cargas cargas)
        {
            List<ResultCargas> listaresultado = new List<ResultCargas>();
            ResultCargas resulCargas = new ResultCargas();

            // Peso carpeta de rodadura
            resulCargas.DW = cargas.PesoPavimento(GeomEst.ep, materiales.rp);

            // Presión vertical del material de lleno
            resulCargas.EV = cargas.PresionVertSuelo(GeomEst.er, GeomEst.Bc, suelo.rs);

            // Incremento carga vertical
            resulCargas.IM = cargas.IncrCargaVehic(GeomEst.er);

            // Long y ancho de la distribución de cargas vehiculares a una profundidad "H"

            resulCargas.Ww = cargas.Ancho_profH(GeomEst.H, cargas.Wt, cargas.Sw, GeomEst.Di, cargas.LLDF);
            if (cargas.Vehiculo == 0)
            {
                resulCargas.lw = cargas.Long_profH(GeomEst.H, cargas.lt, cargas.LLDF, cargas.Sac);
            }
            else
            {
                resulCargas.lw = cargas.Long_profH(GeomEst.H, cargas.lt, cargas.LLDF, cargas.Sat);
            }

            // Carga viva vehicular
            if (cargas.Vehiculo == 0)
            {
                resulCargas.LL = cargas.CargaVehicular(GeomEst.H, cargas.Wt, cargas.lt, cargas.Pcd, resulCargas.IM, cargas.m, cargas.Sw, GeomEst.Di, cargas.LLDF, cargas.Sac);
            }
            else
            {
                resulCargas.LL = cargas.CargaVehicular(GeomEst.H, cargas.Wt, cargas.lt, cargas.Ptd, resulCargas.IM, cargas.m, cargas.Sw, GeomEst.Di, cargas.LLDF, cargas.Sat);
            }

            // Empuje horizontal del suelo o hidrostático
            if (suelo.H1 == 0)
            {
                resulCargas.EH = cargas.EmpujeHorizontal(suelo.fis, suelo.rs, GeomEst.HT);
            }
            else
            {
                resulCargas.WA0 = cargas.PresionAgua0(suelo.fis, suelo.rs);
                resulCargas.WA1 = cargas.PresionAgua1(suelo.fis, suelo.rs, suelo.H1);
                resulCargas.WA2 = cargas.PresionAgua2(GeomEst.HT, suelo.H1, suelo.fis, suelo.rs, suelo.rsat, suelo.rw);
            }

            // Empuje por sobrecarga del suelo
            resulCargas.ES = cargas.SobrecargaSuelo(suelo.fis, resulCargas.DW, resulCargas.EV);

            // Empuje por sobrecarga viva
            if (cargas.SentidoMuro == 0)
            {
                resulCargas.LSs_per = cargas.SobrecargaVivaS_per(GeomEst.H, suelo.fis, suelo.rs);
                resulCargas.LSi_per = cargas.SobrecargaVivaI_per(GeomEst.HT, suelo.fis, suelo.rs);
            }
            else
            {
                resulCargas.LSs_par = cargas.SobrecargaVivaS_par(GeomEst.H, suelo.fis, suelo.rs);
                resulCargas.LSi_par = cargas.SobrecargaVivaI_par(GeomEst.HT, suelo.fis, suelo.rs);
            }
            
            listaresultado.Add(resulCargas);
            return listaresultado;
        }
        // ------------------------------------------------------------------------------------------------------------------------------- //


        // ====  ANÁLISIS DE ESTABILIDAD  ==== //
        public List<ResultEstabilidad> CalculoEstabilidad(Estabilidad estabilidad, Suelo suelo)
        {
            List<ResultEstabilidad> listaresultado = new List<ResultEstabilidad>();
            ResultEstabilidad resultEstabilidad = new ResultEstabilidad();
            resultEstabilidad.Esfuerzo = estabilidad.Esfuerzo(estabilidad.Qs, suelo.Qadm);
            resultEstabilidad.Asentamiento = estabilidad.Asentamiento(estabilidad.Si, estabilidad.Sadm);
            listaresultado.Add(resultEstabilidad);
            return listaresultado;
        }
        
        // ------------------------------------------------------------------------------------------------------------------------------- //


        // ====  DISEÑO LOSA INFERIOR  ==== //
        public List<ResultLosaInf> CalculoLosaInf(Materiales materiales, LosaInf losaInf, DiseñoElementos diseñoElementos, Refuerzo refuerzo)
        {
            List<ResultLosaInf> listaresultado = new List<ResultLosaInf>();

            // Parrilla superior
            for (int i = 0; i < losaInf.LMun.Length; i++)
            {
                ResultLosaInf resultLosaInf = new ResultLosaInf();
                resultLosaInf.Mun = losaInf.LMun[i];
                resultLosaInf.Msn = losaInf.LMsn[i];
                resultLosaInf.Asmin = diseñoElementos.AceroMinimo(losaInf.pmin, losaInf.b, losaInf.h);
                resultLosaInf.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, losaInf.LMun[i], losaInf.b, losaInf.ds);
                resultLosaInf.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, losaInf.LMun[i], losaInf.b, losaInf.ds);
                resultLosaInf.As = diseñoElementos.Acero(resultLosaInf.Asreq, resultLosaInf.Asmin);
                resultLosaInf.Asb = refuerzo.AreaBarra(losaInf.Nbs);
                resultLosaInf.Cant = diseñoElementos.CantBarras(resultLosaInf.As, resultLosaInf.Asb);
                resultLosaInf.Sep = diseñoElementos.SepBarras(losaInf.b, resultLosaInf.As, resultLosaInf.Asb);

                // Chqueo fisuración
                resultLosaInf.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosaInf.preq, losaInf.LMsn[i], resultLosaInf.As, losaInf.ds);
                resultLosaInf.db = refuerzo.DiametroBarra(losaInf.Nbs);
                resultLosaInf.fsadm = diseñoElementos.EsfuerzoAdmisible(losaInf.h, resultLosaInf.db, resultLosaInf.Sep);

                if (resultLosaInf.fsadm >= 170 && resultLosaInf.fsadm <= 250 && resultLosaInf.fs <= resultLosaInf.fsadm)
                {
                    resultLosaInf.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosaInf.fs, resultLosaInf.fsadm);
                }
                else
                {
                    resultLosaInf.Msn = losaInf.LMsn[i];
                    resultLosaInf.Cant = losaInf.Cantt_ps;
                    resultLosaInf.Ascol = diseñoElementos.AceroColocado(resultLosaInf.Cant, resultLosaInf.Asb);
                    resultLosaInf.Sep = diseñoElementos.SepBarras(losaInf.b, resultLosaInf.Ascol, resultLosaInf.Asb);
                    resultLosaInf.preq = diseñoElementos.N_Cuantia(resultLosaInf.Ascol, losaInf.b, losaInf.ds);

                    // Chqueo fisuracion
                    resultLosaInf.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosaInf.preq, losaInf.LMsn[i], resultLosaInf.Ascol, losaInf.ds);
                    resultLosaInf.db = refuerzo.DiametroBarra(losaInf.Nbs);
                    resultLosaInf.fsadm = diseñoElementos.EsfuerzoAdmisible(losaInf.h, resultLosaInf.db, resultLosaInf.Sep);
                    resultLosaInf.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosaInf.fs, resultLosaInf.fsadm);
                }

                // Diseño por durabilidad
                resultLosaInf.Mureq = diseñoElementos.MomtUltimoRequ(resultLosaInf.Mun, resultLosaInf.Msn, materiales.fy, resultLosaInf.fs);
                resultLosaInf.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultLosaInf.Mureq, losaInf.b, losaInf.ds);
                resultLosaInf.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultLosaInf.Mureq, losaInf.b, losaInf.ds);
                resultLosaInf.Asmin = diseñoElementos.AceroMinimo(losaInf.pmin, losaInf.b, losaInf.h);
                resultLosaInf.As = diseñoElementos.Acero(resultLosaInf.Asreq, resultLosaInf.Asmin);
                resultLosaInf.Asb = refuerzo.AreaBarra(losaInf.Nbs);
                resultLosaInf.Cant = losaInf.Cantt_ps;
                resultLosaInf.Sep = diseñoElementos.SepBarras(losaInf.b, resultLosaInf.As, resultLosaInf.Asb);
                resultLosaInf.Ascol = diseñoElementos.AceroColocado(resultLosaInf.Cant, resultLosaInf.Asb);

                // Chequeos
                resultLosaInf.Ascol = diseñoElementos.AceroColocado(resultLosaInf.Cant, resultLosaInf.Asb);
                resultLosaInf.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultLosaInf.Ascol, losaInf.b, losaInf.ds);
                resultLosaInf.ChequeoMomentoNominal = losaInf.ChequeoMomentoNominal(resultLosaInf.Mureq, resultLosaInf.Mn);

                listaresultado.Add(resultLosaInf);
            }


            // Parilla inferior
            for (int i = 0; i < losaInf.LMup.Length; i++)
            {
                ResultLosaInf resultLosaInf = new ResultLosaInf();
                resultLosaInf.Mup = losaInf.LMup[i];
                resultLosaInf.Msp = losaInf.LMsp[i];
                resultLosaInf.Asmin = diseñoElementos.AceroMinimo(losaInf.pmin, losaInf.b, losaInf.h);
                resultLosaInf.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, losaInf.LMup[i], losaInf.b, losaInf.di);
                resultLosaInf.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, losaInf.LMup[i], losaInf.b, losaInf.di);
                resultLosaInf.As = diseñoElementos.Acero(resultLosaInf.Asreq, resultLosaInf.Asmin);
                resultLosaInf.Asb = refuerzo.AreaBarra(losaInf.Nbi);
                resultLosaInf.Cant = diseñoElementos.CantBarras(resultLosaInf.As, resultLosaInf.Asb);
                resultLosaInf.Sep = diseñoElementos.SepBarras(losaInf.b, resultLosaInf.As, resultLosaInf.Asb);

                // ChequeoFisuración
                resultLosaInf.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosaInf.preq, losaInf.LMsp[i], resultLosaInf.As, losaInf.di);
                resultLosaInf.db = refuerzo.DiametroBarra(losaInf.Nbi);
                resultLosaInf.fsadm = diseñoElementos.EsfuerzoAdmisible(losaInf.h, resultLosaInf.db, resultLosaInf.Sep);

                if (resultLosaInf.fsadm >= 170 && resultLosaInf.fsadm <= 250 && resultLosaInf.fs <= resultLosaInf.fsadm)
                {
                    resultLosaInf.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosaInf.fs, resultLosaInf.fsadm);
                }
                else
                {
                    resultLosaInf.Msp = losaInf.LMsp[i];
                    resultLosaInf.Cant = losaInf.Cantt_pi;
                    resultLosaInf.Ascol = diseñoElementos.AceroColocado(resultLosaInf.Cant, resultLosaInf.Asb);
                    resultLosaInf.Sep = diseñoElementos.SepBarras(losaInf.b, resultLosaInf.Ascol, resultLosaInf.Asb);
                    resultLosaInf.preq = diseñoElementos.N_Cuantia(resultLosaInf.Ascol, losaInf.b, losaInf.di);

                    // Chqueo fisuracion
                    resultLosaInf.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosaInf.preq, losaInf.LMsp[i], resultLosaInf.Ascol, losaInf.di);
                    resultLosaInf.db = refuerzo.DiametroBarra(losaInf.Nbi);
                    resultLosaInf.fsadm = diseñoElementos.EsfuerzoAdmisible(losaInf.h, resultLosaInf.db, resultLosaInf.Sep);
                    resultLosaInf.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosaInf.fs, resultLosaInf.fsadm);
                }

                // Diseño por durabilidad
                resultLosaInf.Mureq = diseñoElementos.MomtUltimoRequ(resultLosaInf.Mup, resultLosaInf.Msp, materiales.fy, resultLosaInf.fs);
                resultLosaInf.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultLosaInf.Mureq, losaInf.b, losaInf.di);
                resultLosaInf.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultLosaInf.Mureq, losaInf.b, losaInf.di);
                resultLosaInf.Asmin = diseñoElementos.AceroMinimo(losaInf.pmin, losaInf.b, losaInf.h);
                resultLosaInf.As = diseñoElementos.Acero(resultLosaInf.Asreq, resultLosaInf.Asmin);
                resultLosaInf.Asb = refuerzo.AreaBarra(losaInf.Nbi);
                resultLosaInf.Cant = losaInf.Cantt_pi;
                resultLosaInf.Sep = diseñoElementos.SepBarras(losaInf.b, resultLosaInf.As, resultLosaInf.Asb);
                resultLosaInf.Ascol = diseñoElementos.AceroColocado(resultLosaInf.Cant, resultLosaInf.Asb);

                // Chequeos
                resultLosaInf.Ascol = diseñoElementos.AceroColocado(resultLosaInf.Cant, resultLosaInf.Asb);
                resultLosaInf.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultLosaInf.Ascol, losaInf.b, losaInf.di);
                resultLosaInf.ChequeoMomentoNominal = losaInf.ChequeoMomentoNominal(resultLosaInf.Mureq, resultLosaInf.Mn);

                listaresultado.Add(resultLosaInf);
            }

            // Chequeo Cortante
            for (int i = 0; i < losaInf.LVu.Length; i++)
            {
                ResultLosaInf resultLosaInf = new ResultLosaInf();
                resultLosaInf.Vu = losaInf.LVu[i];
                resultLosaInf.Vc = diseñoElementos.CortConcreto(materiales.fiv, materiales.fc, losaInf.b, losaInf.d);
                resultLosaInf.ChequeoCortante = losaInf.ChequeoCortante(resultLosaInf.Vu, resultLosaInf.Vc);

                listaresultado.Add(resultLosaInf);
            }

            return listaresultado;
        }
        // ------------------------------------------------------------------------------------------------------------------------------- //


        // ====  DISEÑO LOSA SUPERIOR  ==== //

        public List<ResultLosaSup> CalculoLosaSup(Materiales materiales, LosaSup losaSup, DiseñoElementos diseñoElementos, Refuerzo refuerzo)
        {
            List<ResultLosaSup> listaresultado = new List<ResultLosaSup>();

            // Parrilla superior
            for (int i = 0; i < losaSup.LMun.Length; i++)
            {
                ResultLosaSup resultLosaSup = new ResultLosaSup();
                resultLosaSup.Mun = losaSup.LMun[i];
                resultLosaSup.Msn = losaSup.LMsn[i];
                resultLosaSup.Asmin = diseñoElementos.AceroMinimo(losaSup.pmin, losaSup.b, losaSup.h);
                resultLosaSup.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, losaSup.LMun[i], losaSup.b, losaSup.ds);
                resultLosaSup.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, losaSup.LMun[i], losaSup.b, losaSup.ds);
                resultLosaSup.As = diseñoElementos.Acero(resultLosaSup.Asreq, resultLosaSup.Asmin);
                resultLosaSup.Asb = refuerzo.AreaBarra(losaSup.Nbs);
                resultLosaSup.Cant = diseñoElementos.CantBarras(resultLosaSup.As, resultLosaSup.Asb);
                resultLosaSup.Sep = diseñoElementos.SepBarras(losaSup.b, resultLosaSup.As, resultLosaSup.Asb);

                // Chqueo fisuracion
                resultLosaSup.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosaSup.preq, losaSup.LMsn[i], resultLosaSup.As, losaSup.ds);
                resultLosaSup.db = refuerzo.DiametroBarra(losaSup.Nbs);
                resultLosaSup.fsadm = diseñoElementos.EsfuerzoAdmisible(losaSup.h, resultLosaSup.db, resultLosaSup.Sep);

                if (resultLosaSup.fsadm >= 170 && resultLosaSup.fsadm <= 250 && resultLosaSup.fs <= resultLosaSup.fsadm)
                {
                    resultLosaSup.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosaSup.fs, resultLosaSup.fsadm);
                }
                else
                {
                    resultLosaSup.Msn = losaSup.LMsn[i];
                    resultLosaSup.Cant = losaSup.Cantt_ps;
                    resultLosaSup.Ascol = diseñoElementos.AceroColocado(resultLosaSup.Cant, resultLosaSup.Asb);
                    resultLosaSup.Sep = diseñoElementos.SepBarras(losaSup.b, resultLosaSup.Ascol, resultLosaSup.Asb);
                    resultLosaSup.preq = diseñoElementos.N_Cuantia(resultLosaSup.Ascol, losaSup.b, losaSup.ds);

                    // Chqueo fisuracion
                    resultLosaSup.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosaSup.preq, losaSup.LMsn[i], resultLosaSup.Ascol, losaSup.ds);
                    resultLosaSup.db = refuerzo.DiametroBarra(losaSup.Nbs);
                    resultLosaSup.fsadm = diseñoElementos.EsfuerzoAdmisible(losaSup.h, resultLosaSup.db, resultLosaSup.Sep);
                    resultLosaSup.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosaSup.fs, resultLosaSup.fsadm);
                }

                // Diseño por durabilidad
                resultLosaSup.Mureq = diseñoElementos.MomtUltimoRequ(resultLosaSup.Mun, resultLosaSup.Msn, materiales.fy, resultLosaSup.fs);
                resultLosaSup.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultLosaSup.Mureq, losaSup.b, losaSup.ds);
                resultLosaSup.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultLosaSup.Mureq, losaSup.b, losaSup.ds);
                resultLosaSup.Asmin = diseñoElementos.AceroMinimo(losaSup.pmin, losaSup.b, losaSup.h);
                resultLosaSup.As = diseñoElementos.Acero(resultLosaSup.Asreq, resultLosaSup.Asmin);
                resultLosaSup.Asb = refuerzo.AreaBarra(losaSup.Nbs);
                resultLosaSup.Cant = losaSup.Cantt_ps;
                resultLosaSup.Sep = diseñoElementos.SepBarras(losaSup.b, resultLosaSup.As, resultLosaSup.Asb);
                resultLosaSup.Ascol = diseñoElementos.AceroColocado(resultLosaSup.Cant, resultLosaSup.Asb);

                // Chequeos
                resultLosaSup.Ascol = diseñoElementos.AceroColocado(resultLosaSup.Cant, resultLosaSup.Asb);
                resultLosaSup.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultLosaSup.Ascol, losaSup.b, losaSup.ds);
                resultLosaSup.ChequeoMomentoNominal = losaSup.ChequeoMomentoNominal(resultLosaSup.Mureq, resultLosaSup.Mn);

                listaresultado.Add(resultLosaSup);
            }


            // Parilla inferior
            for (int i = 0; i < losaSup.LMup.Length; i++)
            {
                ResultLosaSup resultLosaSup = new ResultLosaSup();
                resultLosaSup.Mup = losaSup.LMup[i];
                resultLosaSup.Msp = losaSup.LMsp[i];
                resultLosaSup.Asmin = diseñoElementos.AceroMinimo(losaSup.pmin, losaSup.b, losaSup.h);
                resultLosaSup.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, losaSup.LMup[i], losaSup.b, losaSup.di);
                resultLosaSup.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, losaSup.LMup[i], losaSup.b, losaSup.di);
                resultLosaSup.As = diseñoElementos.Acero(resultLosaSup.Asreq, resultLosaSup.Asmin);
                resultLosaSup.Asb = refuerzo.AreaBarra(losaSup.Nbi);
                resultLosaSup.Cant = diseñoElementos.CantBarras(resultLosaSup.As, resultLosaSup.Asb);
                resultLosaSup.Sep = diseñoElementos.SepBarras(losaSup.b, resultLosaSup.As, resultLosaSup.Asb);

                // ChequeoFisuracion
                resultLosaSup.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosaSup.preq, losaSup.LMsp[i], resultLosaSup.As, losaSup.di);
                resultLosaSup.db = refuerzo.DiametroBarra(losaSup.Nbi);
                resultLosaSup.fsadm = diseñoElementos.EsfuerzoAdmisible(losaSup.h, resultLosaSup.db, resultLosaSup.Sep);

                if (resultLosaSup.fsadm >= 170 && resultLosaSup.fsadm <= 250 && resultLosaSup.fs <= resultLosaSup.fsadm)
                {
                    resultLosaSup.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosaSup.fs, resultLosaSup.fsadm);
                }
                else
                {
                    resultLosaSup.Msp = losaSup.LMsp[i];
                    resultLosaSup.Cant = losaSup.Cantt_pi;
                    resultLosaSup.Ascol = diseñoElementos.AceroColocado(resultLosaSup.Cant, resultLosaSup.Asb);
                    resultLosaSup.Sep = diseñoElementos.SepBarras(losaSup.b, resultLosaSup.Ascol, resultLosaSup.Asb);
                    resultLosaSup.preq = diseñoElementos.N_Cuantia(resultLosaSup.Ascol, losaSup.b, losaSup.di);

                    // Chqueo fisuracion
                    resultLosaSup.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultLosaSup.preq, losaSup.LMsp[i], resultLosaSup.Ascol, losaSup.di);
                    resultLosaSup.db = refuerzo.DiametroBarra(losaSup.Nbi);
                    resultLosaSup.fsadm = diseñoElementos.EsfuerzoAdmisible(losaSup.h, resultLosaSup.db, resultLosaSup.Sep);
                    resultLosaSup.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultLosaSup.fs, resultLosaSup.fsadm);
                }

                // Diseño por durabilidad
                resultLosaSup.Mureq = diseñoElementos.MomtUltimoRequ(resultLosaSup.Mup, resultLosaSup.Msp, materiales.fy, resultLosaSup.fs);
                resultLosaSup.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultLosaSup.Mureq, losaSup.b, losaSup.di);
                resultLosaSup.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultLosaSup.Mureq, losaSup.b, losaSup.di);
                resultLosaSup.Asmin = diseñoElementos.AceroMinimo(losaSup.pmin, losaSup.b, losaSup.h);
                resultLosaSup.As = diseñoElementos.Acero(resultLosaSup.Asreq, resultLosaSup.Asmin);
                resultLosaSup.Asb = refuerzo.AreaBarra(losaSup.Nbi);
                resultLosaSup.Cant = losaSup.Cantt_pi;
                resultLosaSup.Sep = diseñoElementos.SepBarras(losaSup.b, resultLosaSup.As, resultLosaSup.Asb);
                resultLosaSup.Ascol = diseñoElementos.AceroColocado(resultLosaSup.Cant, resultLosaSup.Asb);

                // Chequeos
                resultLosaSup.Ascol = diseñoElementos.AceroColocado(resultLosaSup.Cant, resultLosaSup.Asb);
                resultLosaSup.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultLosaSup.Ascol, losaSup.b, losaSup.di);
                resultLosaSup.ChequeoMomentoNominal = losaSup.ChequeoMomentoNominal(resultLosaSup.Mureq, resultLosaSup.Mn);

                listaresultado.Add(resultLosaSup);
            }

            // Chequeo Cortante
            for (int i = 0; i < losaSup.LVu.Length; i++)
            {
                ResultLosaSup resultLosaSup = new ResultLosaSup();
                resultLosaSup.Vu = losaSup.LVu[i];
                resultLosaSup.Vc = diseñoElementos.CortConcreto(materiales.fiv, materiales.fc, losaSup.b, losaSup.d);
                resultLosaSup.ChequeoCortante = losaSup.ChequeoCortante(resultLosaSup.Vu, resultLosaSup.Vc);

                listaresultado.Add(resultLosaSup);
            }

            return listaresultado;
        }

        // ------------------------------------------------------------------------------------------------------------------------------- //


        // ====  DISEÑO MUROS  ==== //

        public List<ResultMuros> CaculoMuros(Materiales materiales, Muros muros, DiseñoElementos diseñoElementos, Refuerzo refuerzo)
        {
            List<ResultMuros> listaresultado = new List<ResultMuros>();

            // Refuerzo Vertical
            for (int i = 0; i < muros.LMuy.Length; i++)
            {
                ResultMuros resultMuros = new ResultMuros();
                resultMuros.Muy = muros.LMuy[i];
                resultMuros.Msy = muros.LMsy[i];
                resultMuros.Asminv = diseñoElementos.AceroMinimo(muros.pminv, muros.b, muros.h);
                resultMuros.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, muros.LMuy[i], muros.b, muros.d);
                resultMuros.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, muros.LMuy[i], muros.b, muros.d);
                resultMuros.As = diseñoElementos.Acero(resultMuros.Asreq, resultMuros.Asminv);
                resultMuros.Asb = refuerzo.AreaBarra(muros.Nbv);
                resultMuros.Cant = diseñoElementos.CantBarras(resultMuros.As, resultMuros.Asb);
                resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.As, resultMuros.Asb);

                // Chequeo fisuración
                resultMuros.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultMuros.preq, muros.LMsy[i], resultMuros.As, muros.d);
                resultMuros.db = refuerzo.DiametroBarra(muros.Nbv);
                resultMuros.fsadm = diseñoElementos.EsfuerzoAdmisible(muros.h, resultMuros.db, resultMuros.Sep);

                if (resultMuros.fsadm >= 170 && resultMuros.fsadm <= 250 && resultMuros.fs <= resultMuros.fsadm)
                {
                    resultMuros.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultMuros.fs, resultMuros.fsadm);
                }
                else
                {
                    resultMuros.Msy = muros.LMsy[i];
                    resultMuros.Cant = muros.Cantt_pv;
                    resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);
                    resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.Ascol, resultMuros.Asb);
                    resultMuros.preq = diseñoElementos.N_Cuantia(resultMuros.Ascol, muros.b, muros.d);

                    // Cheqquo fisuración
                    resultMuros.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultMuros.preq, muros.LMsy[i], resultMuros.Ascol, muros.d);
                    resultMuros.db = refuerzo.DiametroBarra(muros.Nbv);
                    resultMuros.fsadm = diseñoElementos.EsfuerzoAdmisible(muros.h, resultMuros.db, resultMuros.Sep);
                    resultMuros.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultMuros.fs, resultMuros.fsadm);
                }

                // Diseño por durabilidad
                resultMuros.Mureq = diseñoElementos.MomtUltimoRequ(resultMuros.Muy, resultMuros.Msy, materiales.fy, resultMuros.fs);
                resultMuros.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultMuros.Mureq, muros.b, muros.d);
                resultMuros.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultMuros.Mureq, muros.b, muros.d);
                resultMuros.Asminv = diseñoElementos.AceroMinimo(muros.pminv, muros.b, muros.h);
                resultMuros.As = diseñoElementos.Acero(resultMuros.Asreq, resultMuros.Asminv);
                resultMuros.Asb = refuerzo.AreaBarra(muros.Nbv);
                resultMuros.Cant = muros.Cantt_pv;
                resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.As, resultMuros.Asb);
                resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);

                // Chequeos
                resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);
                resultMuros.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultMuros.Ascol, muros.b, muros.d);
                resultMuros.ChequeoMomentoNominal = muros.ChequeoMomentoNominal(resultMuros.Mureq, resultMuros.Mn);

                listaresultado.Add(resultMuros);
            }

            // Refuerzo Horizontal
            for (int i = 0; i < muros.LMux.Length; i++)
            {
                ResultMuros resultMuros = new ResultMuros();
                resultMuros.Mux = muros.LMux[i];
                resultMuros.Msx = muros.LMsx[i];
                resultMuros.Asminh = diseñoElementos.AceroMinimo(muros.pminh, muros.b, muros.h);
                resultMuros.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, muros.LMux[i], muros.b, muros.d);
                resultMuros.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, muros.LMux[i], muros.b, muros.d);
                resultMuros.As = diseñoElementos.Acero(resultMuros.Asreq, resultMuros.Asminh);
                resultMuros.Asb = refuerzo.AreaBarra(muros.Nbh);
                resultMuros.Cant = diseñoElementos.CantBarras(resultMuros.As, resultMuros.Asb);
                resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.As, resultMuros.Asb);

                // Chequeo fisuración
                resultMuros.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultMuros.preq, muros.LMsx[i], resultMuros.As, muros.d);
                resultMuros.db = refuerzo.DiametroBarra(muros.Nbh);
                resultMuros.fsadm = diseñoElementos.EsfuerzoAdmisible(muros.h, resultMuros.db, resultMuros.Sep);

                if (resultMuros.fsadm >= 170 && resultMuros.fsadm <= 250 && resultMuros.fs <= resultMuros.fsadm)
                {
                    resultMuros.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultMuros.fs, resultMuros.fsadm);
                }
                else
                {
                    resultMuros.Msx = muros.LMsx[i];
                    resultMuros.Cant = muros.Cantt_ph;
                    resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);
                    resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.Ascol, resultMuros.Asb);
                    resultMuros.preq = diseñoElementos.N_Cuantia(resultMuros.Ascol, muros.b, muros.d);

                    // Cheqquo fisuración
                    resultMuros.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultMuros.preq, muros.LMsx[i], resultMuros.Ascol, muros.d);
                    resultMuros.db = refuerzo.DiametroBarra(muros.Nbh);
                    resultMuros.fsadm = diseñoElementos.EsfuerzoAdmisible(muros.h, resultMuros.db, resultMuros.Sep);
                    resultMuros.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultMuros.fs, resultMuros.fsadm);
                }

                // Diseño por durabilidad
                resultMuros.Mureq = diseñoElementos.MomtUltimoRequ(resultMuros.Mux, resultMuros.Msx, materiales.fy, resultMuros.fs);
                resultMuros.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultMuros.Mureq, muros.b, muros.d);
                resultMuros.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultMuros.Mureq, muros.b, muros.d);
                resultMuros.Asminh = diseñoElementos.AceroMinimo(muros.pminv, muros.b, muros.h);
                resultMuros.As = diseñoElementos.Acero(resultMuros.Asreq, resultMuros.Asminh);
                resultMuros.Asb = refuerzo.AreaBarra(muros.Nbh);
                resultMuros.Cant = muros.Cantt_ph;
                resultMuros.Sep = diseñoElementos.SepBarras(muros.b, resultMuros.As, resultMuros.Asb);
                resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);

                // Chequeos
                resultMuros.Ascol = diseñoElementos.AceroColocado(resultMuros.Cant, resultMuros.Asb);
                resultMuros.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultMuros.Ascol, muros.b, muros.d);
                resultMuros.ChequeoMomentoNominal = muros.ChequeoMomentoNominal(resultMuros.Mureq, resultMuros.Mn);

                listaresultado.Add(resultMuros);
            }

            // Chequeo Cortante
            for (int i = 0; i < muros.LVu.Length; i++)
            {
                ResultMuros resultMuros = new ResultMuros();
                resultMuros.Vu = muros.LVu[i];
                resultMuros.Vc = diseñoElementos.CortConcreto(materiales.fiv, materiales.fc, muros.b, muros.d);
                resultMuros.ChequeoCortante = muros.ChequeoCortante(resultMuros.Vu, resultMuros.Vc);

                listaresultado.Add(resultMuros);
            }

            return listaresultado;
        }

        // ------------------------------------------------------------------------------------------------------------------------------- //

        // ====  DISEÑO TABIQUE  ==== //

        public List<ResultTabique> CaculoTabique(Materiales materiales, Tabique tabique, DiseñoElementos diseñoElementos, Refuerzo refuerzo)
        {
            List<ResultTabique> listaresultado = new List<ResultTabique>();

            // Refuerzo Vertical
            for (int i = 0; i < tabique.LMuy.Length; i++)
            {
                ResultTabique resultTabique = new ResultTabique();
                resultTabique.Muy = tabique.LMuy[i];
                resultTabique.Msy = tabique.LMsy[i];
                resultTabique.Asminv = diseñoElementos.AceroMinimo(tabique.pminv, tabique.b, tabique.h);
                resultTabique.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, tabique.LMuy[i], tabique.b, tabique.d);
                resultTabique.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, tabique.LMuy[i], tabique.b, tabique.d);
                resultTabique.As = diseñoElementos.Acero(resultTabique.Asreq, resultTabique.Asminv);
                resultTabique.Asb = refuerzo.AreaBarra(tabique.Nbv);
                resultTabique.Cant = diseñoElementos.CantBarras(resultTabique.As, resultTabique.Asb);
                resultTabique.Sep = diseñoElementos.SepBarras(tabique.b, resultTabique.As, resultTabique.Asb);

                // Chequeo fisuración
                resultTabique.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultTabique.preq, tabique.LMsy[i], resultTabique.As, tabique.d);
                resultTabique.db = refuerzo.DiametroBarra(tabique.Nbv);
                resultTabique.fsadm = diseñoElementos.EsfuerzoAdmisible(tabique.h, resultTabique.db, resultTabique.Sep);

                if (resultTabique.fsadm >= 170 && resultTabique.fsadm <= 250 && resultTabique.fs <= resultTabique.fsadm)
                {
                    resultTabique.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultTabique.fs, resultTabique.fsadm);
                }
                else
                {
                    resultTabique.Msy = tabique.LMsy[i];
                    resultTabique.Cant = tabique.Cantt_pv;
                    resultTabique.Ascol = diseñoElementos.AceroColocado(resultTabique.Cant, resultTabique.Asb);
                    resultTabique.Sep = diseñoElementos.SepBarras(tabique.b, resultTabique.Ascol, resultTabique.Asb);
                    resultTabique.preq = diseñoElementos.N_Cuantia(resultTabique.Ascol, tabique.b, tabique.d);

                    // Cheqquo fisuración
                    resultTabique.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultTabique.preq, tabique.LMsy[i], resultTabique.Ascol, tabique.d);
                    resultTabique.db = refuerzo.DiametroBarra(tabique.Nbv);
                    resultTabique.fsadm = diseñoElementos.EsfuerzoAdmisible(tabique.h, resultTabique.db, resultTabique.Sep);
                    resultTabique.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultTabique.fs, resultTabique.fsadm);
                }

                // Diseño por durabilidad
                resultTabique.Mureq = diseñoElementos.MomtUltimoRequ(resultTabique.Muy, resultTabique.Msy, materiales.fy, resultTabique.fs);
                resultTabique.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultTabique.Mureq, tabique.b, tabique.d);
                resultTabique.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultTabique.Mureq, tabique.b, tabique.d);
                resultTabique.Asminv = diseñoElementos.AceroMinimo(tabique.pminv, tabique.b, tabique.h);
                resultTabique.As = diseñoElementos.Acero(resultTabique.Asreq, resultTabique.Asminv);
                resultTabique.Asb = refuerzo.AreaBarra(tabique.Nbv);
                resultTabique.Cant = tabique.Cantt_pv;
                resultTabique.Sep = diseñoElementos.SepBarras(tabique.b, resultTabique.As, resultTabique.Asb);
                resultTabique.Ascol = diseñoElementos.AceroColocado(resultTabique.Cant, resultTabique.Asb);

                // Chequeos
                resultTabique.Ascol = diseñoElementos.AceroColocado(resultTabique.Cant, resultTabique.Asb);
                resultTabique.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultTabique.Ascol, tabique.b, tabique.d);
                resultTabique.ChequeoMomentoNominal = tabique.ChequeoMomentoNominal(resultTabique.Mureq, resultTabique.Mn);

                listaresultado.Add(resultTabique);
            }

            // Refuerzo Horizontal
            for (int i = 0; i < tabique.LMux.Length; i++)
            {
                ResultTabique resultTabique = new ResultTabique();
                resultTabique.Mux = tabique.LMux[i];
                resultTabique.Msx = tabique.LMsx[i];
                resultTabique.Asminh = diseñoElementos.AceroMinimo(tabique.pminh, tabique.b, tabique.h);
                resultTabique.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, tabique.LMux[i], tabique.b, tabique.d);
                resultTabique.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, tabique.LMux[i], tabique.b, tabique.d);
                resultTabique.As = diseñoElementos.Acero(resultTabique.Asreq, resultTabique.Asminh);
                resultTabique.Asb = refuerzo.AreaBarra(tabique.Nbh);
                resultTabique.Cant = diseñoElementos.CantBarras(resultTabique.As, resultTabique.Asb);
                resultTabique.Sep = diseñoElementos.SepBarras(tabique.b, resultTabique.As, resultTabique.Asb);

                // Chequeo fisuración
                resultTabique.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultTabique.preq, tabique.LMsx[i], resultTabique.As, tabique.d);
                resultTabique.db = refuerzo.DiametroBarra(tabique.Nbh);
                resultTabique.fsadm = diseñoElementos.EsfuerzoAdmisible(tabique.h, resultTabique.db, resultTabique.Sep);

                if (resultTabique.fsadm >= 170 && resultTabique.fsadm <= 250 && resultTabique.fs <= resultTabique.fsadm)
                {
                    resultTabique.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultTabique.fs, resultTabique.fsadm);
                }
                else
                {
                    resultTabique.Msx = tabique.LMsx[i];
                    resultTabique.Cant = tabique.Cantt_ph;
                    resultTabique.Ascol = diseñoElementos.AceroColocado(resultTabique.Cant, resultTabique.Asb);
                    resultTabique.Sep = diseñoElementos.SepBarras(tabique.b, resultTabique.Ascol, resultTabique.Asb);
                    resultTabique.preq = diseñoElementos.N_Cuantia(resultTabique.Ascol, tabique.b, tabique.d);

                    // Cheqquo fisuración
                    resultTabique.fs = diseñoElementos.EsfuerzoAcero(materiales.Ec, resultTabique.preq, tabique.LMsx[i], resultTabique.Ascol, tabique.d);
                    resultTabique.db = refuerzo.DiametroBarra(tabique.Nbh);
                    resultTabique.fsadm = diseñoElementos.EsfuerzoAdmisible(tabique.h, resultTabique.db, resultTabique.Sep);
                    resultTabique.ChequeoFisuracion = diseñoElementos.ChequeoFisuracion(resultTabique.fs, resultTabique.fsadm);
                }

                // Diseño por durabilidad
                resultTabique.Mureq = diseñoElementos.MomtUltimoRequ(resultTabique.Mux, resultTabique.Msx, materiales.fy, resultTabique.fs);
                resultTabique.preq = diseñoElementos.Cuantia(materiales.fc, materiales.fy, resultTabique.Mureq, tabique.b, tabique.d);
                resultTabique.Asreq = diseñoElementos.AceroRequerido(materiales.fc, materiales.fy, resultTabique.Mureq, tabique.b, tabique.d);
                resultTabique.Asminh = diseñoElementos.AceroMinimo(tabique.pminv, tabique.b, tabique.h);
                resultTabique.As = diseñoElementos.Acero(resultTabique.Asreq, resultTabique.Asminh);
                resultTabique.Asb = refuerzo.AreaBarra(tabique.Nbh);
                resultTabique.Cant = tabique.Cantt_ph;
                resultTabique.Sep = diseñoElementos.SepBarras(tabique.b, resultTabique.As, resultTabique.Asb);
                resultTabique.Ascol = diseñoElementos.AceroColocado(resultTabique.Cant, resultTabique.Asb);

                // Chequeos
                resultTabique.Ascol = diseñoElementos.AceroColocado(resultTabique.Cant, resultTabique.Asb);
                resultTabique.Mn = diseñoElementos.MomNominal(materiales.fc, materiales.fy, resultTabique.Ascol, tabique.b, tabique.d);
                resultTabique.ChequeoMomentoNominal = tabique.ChequeoMomentoNominal(resultTabique.Mureq, resultTabique.Mn);

                listaresultado.Add(resultTabique);
            }

            // Chequeo Cortante
            for (int i = 0; i < tabique.LVu.Length; i++)
            {
                ResultTabique resultTabique = new ResultTabique();
                resultTabique.Vu = tabique.LVu[i];
                resultTabique.Vc = diseñoElementos.CortConcreto(materiales.fiv, materiales.fc, tabique.b, tabique.d);
                resultTabique.ChequeoCortante = tabique.ChequeoCortante(resultTabique.Vu, resultTabique.Vc);

                listaresultado.Add(resultTabique);
            }
            return listaresultado;
        }
        // -------------------------------------------------------------------------------------------------------------------------------  
    }
}
    


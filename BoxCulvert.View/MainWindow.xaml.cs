using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BoxCulvert.Core;
using BoxCulvert.Model;

namespace BoxCulvert.View
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Prueba();
        }

        public void Prueba()
        {
            Desarrollo desarrollo = new Desarrollo();

            // -------------------------------------------------------------------------------------------------------------------------------
            // GEOMETRÍA //
            GeomEstructura GeomEst = new GeomEstructura();
            GeomEst.ep = 0.20;
            GeomEst.er = 0.20;
            GeomEst.el = 0.25;
            GeomEst.h2 = 1.50;
            GeomEst.h3 = 1.00;
            GeomEst.a = 1.50;
            GeomEst.b = 1.00;
            GeomEst.em = 0.25;
            GeomEst.et = 0.25;
            GeomEst.Li = 3.80;
            GeomEst.H = Math.Round(GeomEst.ep + GeomEst.er, 2);
            GeomEst.HT = Math.Round(GeomEst.H + GeomEst.h2 + (2*GeomEst.el), 2);
            GeomEst.Di = Math.Round(GeomEst.a + GeomEst.b + GeomEst.et, 2);
            GeomEst.Bc = Math.Round((2 * GeomEst.em) + GeomEst.Di, 2);

            // -------------------------------------------------------------------------------------------------------------------------------
            // MATERIALES //
            Materiales materiales = new Materiales();
            materiales.fc = 28;
            materiales.fy = 420;
            materiales.fiv = 0.75;
            materiales.rc = 24;
            materiales.rp = 23;
            materiales.Ecuacion = 0;
            materiales.Ec = materiales.ElasticidadConcreto(materiales.Ecuacion, materiales.fc);

            // -------------------------------------------------------------------------------------------------------------------------------
            // SUELO //
            Suelo suelo = new Suelo();
            suelo.Qadm = 110;
            suelo.rs = 18;
            suelo.fis = 25;
            suelo.rsat = 19;
            suelo.rw = 10;
            suelo.H1 = 0.20;

            // -------------------------------------------------------------------------------------------------------------------------------
            // CARGAS //
            Cargas cargas = new Cargas();
            cargas.DC = 284.46;
            cargas.SentidoMuro = 1;
            cargas.Vehiculo = 1;

            // Prpiedades del vehículo sobre la estructura
            cargas.Pcd = 160;
            cargas.Sac = 4.30;
            cargas.Ptd = 125;
            cargas.Sat = 1.20;

            // Parametros para el cálculo de cargas vehiculares
            cargas.m = 1.20;
            cargas.Sw = 1.80;
            cargas.Wt = 0.51;
            cargas.lt = 0.25;
            cargas.LLDF = 1.15;

            List<ResultCargas> resulCargas = desarrollo.CalculoEmpujes(GeomEst, materiales, suelo, cargas);

            // -------------------------------------------------------------------------------------------------------------------------------
            // ESTABILIDAD //
            Estabilidad estabilidad = new Estabilidad();
            estabilidad.Qs = 88.15;
            estabilidad.Si = 0.59;
            estabilidad.Sadm = 2.54;

            List<ResultEstabilidad> resultEstabilidad = desarrollo.CalculoEstabilidad(estabilidad, suelo);

            // -------------------------------------------------------------------------------------------------------------------------------
            // DISEÑO LOSAS - LOSAS INFERIOR//
            DiseñoElementos diseñoElementos = new DiseñoElementos();
            Refuerzo refuerzo = new Refuerzo();
            LosaInf losaInf = new LosaInf();
            losaInf.b = 100;
            losaInf.rs = 5;
            losaInf.ri = 7.5;
            losaInf.pmin = 0.0018;
            losaInf.Nbs = 5;
            losaInf.Nbi = 5;
            losaInf.h = GeomEst.el * 100;
            losaInf.ds = losaInf.h - losaInf.rs;
            losaInf.di = losaInf.h - losaInf.ri;
            losaInf.d = Math.Min(losaInf.di, losaInf.ds);

            // Solicitaciones sobre la losa
            double[] LI_Mun = { 2771, 960 };
            double[] LI_Mup = { 3038, 1913 };
            double[] LI_Vu = { 109.9, 33.82 };
            double[] LI_Msn = { 1744, 687};
            double[] LI_Msp = { 1111, 1287};
            losaInf.LMun = LI_Mun;
            losaInf.LMup = LI_Mup;
            losaInf.LVu = LI_Vu;
            losaInf.LMsn = LI_Msn;
            losaInf.LMsp = LI_Msp;

            // Canidad de barras a aumentar para cumplir fisuración
            losaInf.Cantt_ps = 7;
            losaInf.Cantt_pi = 7;

            List<ResultLosaInf> resultLosaInf = desarrollo.CalculoLosaInf(materiales, losaInf, diseñoElementos, refuerzo);

            // -------------------------------------------------------------------------------------------------------------------------------
            // DISEÑO LOSAS - LOSAS SUPERIOR//
            LosaSup losaSup = new LosaSup();
            losaSup.b = 100;
            losaSup.rs = 5;
            losaSup.ri = 5;
            losaSup.pmin = 0.0018;
            losaSup.Nbs = 5;
            losaSup.Nbi = 5;
            losaSup.h = GeomEst.el * 100;
            losaSup.ds = losaSup.h - losaSup.rs;
            losaSup.di = losaSup.h - losaSup.ri;
            losaSup.d = Math.Min(losaSup.di, losaSup.ds);

            // Solicitaciones sobre la losa
            double[] LS_Mun = { 1438, 1690 };
            double[] LS_Mup = { 2613, 742 };
            double[] LS_Vu = { 88.9, 27.6 };
            double[] LS_Msn = { 925, 1075 };
            double[] LS_Msp = { 1599, 445 };
            losaSup.LMun = LS_Mun;
            losaSup.LMup = LS_Mup;
            losaSup.LVu = LS_Vu;
            losaSup.LMsn = LS_Msn;
            losaSup.LMsp = LS_Msp;

            // Canidad de barras a aumentar para cumplir fisuración
            losaSup.Cantt_ps = 7;
            losaSup.Cantt_pi = 7;

            List<ResultLosaSup> resultLosaSup = desarrollo.CalculoLosaSup(materiales, losaSup, diseñoElementos, refuerzo);

            // -------------------------------------------------------------------------------------------------------------------------------
            // DISEÑO MUROS //           
            Muros muros = new Muros();
            muros.b = 100;
            muros.r = 5;
            muros.pminv = 0.0018;
            muros.pminh = 0.0018;
            muros.Nbv = 5;
            muros.Nbh = 5;
            muros.h = GeomEst.em * 100;
            muros.d = muros.h - muros.r;

            // Solicitaciones sobre el muro
            double[] Muro_Muv = { 998, 2181 };
            double[] Muro_Muh = { 829, 1438 };
            double[] Muro_Vu = { 61, 51.8 };
            double[] Muro_Msv = { 520, 1438 };
            double[] Muro_Msh = { 579, 909 };
            muros.LMuy = Muro_Muv;
            muros.LMux = Muro_Muh;
            muros.LVu = Muro_Vu;
            muros.LMsy = Muro_Msv;
            muros.LMsx = Muro_Msh;

            // Canidad de barras a aumentar para cumplir fisuración
            muros.Cantt_pv = 7;
            muros.Cantt_ph = 7;

            List<ResultMuros> resultMuros = desarrollo.CaculoMuros(materiales, muros, diseñoElementos, refuerzo);

            // -------------------------------------------------------------------------------------------------------------------------------
            // DISEÑO TABIQUE // 
            Tabique tabique = new Tabique();
            tabique.b = 100;
            tabique.r = 5;
            tabique.pminv = 0.0018;
            tabique.pminh = 0.0018;
            tabique.Nbv = 5;
            tabique.Nbh = 5;
            tabique.h = GeomEst.et * 100;
            tabique.d = tabique.h - tabique.r;

            // Solicitaciones sobre el muro
            double[] Tabique_Muv = { 2764, 866 };
            double[] Tabique_Muh = { 1669, 1434 };
            double[] Tabique_Vu = { 69.7, 45.7 };
            double[] Tabique_Msv = { 1838, 575 };
            double[] Tabique_Msh = { 1111, 925 };
            tabique.LMuy = Tabique_Muv;
            tabique.LMux = Tabique_Muh;
            tabique.LVu = Tabique_Vu;
            tabique.LMsy = Tabique_Msv;
            tabique.LMsx = Tabique_Msh;

            // Canidad de barras a aumentar para cumplir fisuración
            tabique.Cantt_pv = 7;
            tabique.Cantt_ph = 7;

            List<ResultTabique> resultTabique = desarrollo.CaculoTabique(materiales, tabique, diseñoElementos, refuerzo);

            // -------------------------------------------------------------------------------------------------------------------------------

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class Cargas
    {
        // -------------------------------------------------------------------------------------------------------------------------------
        // CARGAS VERTICALES //

        /// <summary>
        /// Peso propio de la estructura _ [kN]
        /// </summary>
        public double DC;

        /// <summary>
        /// Peso propio capa de rodadura (Asfalto o pavimento) _ [kN]
        /// </summary>
        public double DW;

        /// <summary>
        /// Peso propio tapas de concreto _ [kN/m²]
        /// </summary>
        public double EV;

        /// <summary>
        /// Incremeto carga vehicular _ [%]
        /// </summary>
        public double IM;

        /// <summary>
        /// Carga viva vehicular _ [kN/m²]
        /// </summary>
        public double LL;

        /// <summary>
        /// Carga aplicada para camion de diseño (carga por eje) _ [kN]
        /// </summary>
        public double Pcd;

        /// <summary>
        /// Carga aplicada para tandem de diseño (carga por eje) _ [kN]
        /// </summary>
        public double Ptd;

        /// <summary>
        /// Factor de presencia multiple depende del número de carriles
        /// </summary>
        public double m;

        /// <summary>
        /// Separación entre ruedas (Vista frontal del eje) _ [m]
        /// </summary>
        public double Sw;

        /// <summary>
        /// Ancho de la huella de la rueda _ [m]
        /// </summary>
        public double Wt;

        /// <summary>
        /// Longitud de la huella de la rueda _ [m]
        /// </summary>
        public double lt;

        /// <summary>
        /// Factor de distribución de la carga depende del tipo de estructura.
        /// </summary>
        public double LLDF;

        /// <summary>
        /// Separación entre ejes cuando se considera camion de diseño _ [m]
        /// </summary>
        public double Sac;

        /// <summary>
        /// Separación entre ejes cuando se considera tándem de diseño _ [m]
        /// </summary>
        public double Sat;




        // -------------------------------------------------------------------------------------------------------------------------------
        // EMPUJES //

        /// <summary>
        /// Empuje horizontal del suelo _ [kN/m]
        /// </summary>
        public double EH;

        /// <summary>
        /// Sobrecarga del suelo _ [kN/m]
        /// </summary>
        public double ES;

        /// <summary>
        /// Carga de presión de agua, nivel 0 _ [kN/m]
        /// </summary>
        public double WA0;

        /// <summary>
        /// Carga de presión de agua, nivel igual a H1 _ [kN/m]
        /// </summary>
        public double WA1;

        /// <summary>
        /// Carga de presión de agua, nivel igual a H2 _ [kN/m]
        /// </summary>
        public double WA2;

        /// <summary>
        /// Sobrecarga de carga viva superior _ [kN/m]
        /// </summary>
        public double LSs;

        /// <summary>
        /// Sobrecarga de carga viva inferior _ [kN/m]
        /// </summary>
        public double LSi;

        /// <summary>
        /// Ubicación o sentido del muro con relación al tráfico. 
        /// Muros perpendiculares al tráfico = 0
        /// Muros paralelos al trafico = 1
        /// </summary>
        public int SentidoMuro;

        /// <summary>
        /// Seleciona el tipo de vehículo con el que se va realizar el análisis. 
        /// Camion de diseño = 0
        /// Tándem de diseño = 1
        /// </summary>
        public int Vehiculo;


        // -------------------------------------------------------------------------------------------------------------------------------
        // CARGAS VERTICALES // 
        public double PesoPavimento(double ep, double rp)
        {
            double DW = ep * rp;
            return Math.Round(DW, 2);
        }

        public double PresionVertSuelo(double er, double Bc, double rs)
        {
            double Fe = 1 + 0.2 * (er / Bc);
            double EV = Fe * rs * er;
            return Math.Round(EV, 2);
        }

        public double IncrCargaVehic(double er)
        {
            IM = (33 * (1 - 0.41 * er))/ 100;
            return Math.Round(IM, 4);
        }              
        
        // Metodo para el cálculo de cargas vehiculares y sus correspondientes parámetros        
        // Ancho de la parte de la carga viva a una profundidad H
        public double Ancho_profH(double H, double Wt, double Sw, double Di, double LLDF)
        {
            if (H < 0.60)
            {
                double Ww = Wt + (1.5 * H);
                return Math.Round(Ww, 2);
            }
            else
            {
                double Hint_t = (Sw - Wt - 0.06 * Di) / (LLDF);
                double Ww = AnchoWw(H, Hint_t, Wt, LLDF, Di, Sw);
                return Math.Round(Ww, 2);
            }
        }

        static double AnchoWw(double H, double Hint_t, double Wt, double LLDF, double Di, double Sw)
        {
            if (H <= Hint_t)
            {
                double Ww = Wt + (LLDF * H) + (0.06 * Di);
                return Ww;
            }
            else
            {
                double Ww = Wt + Sw + (LLDF * H) + (0.06 * Di);
                return Ww;
            }
        }

        // Longitud de la parte de la carga viva a una profundidad H
        public double Long_profH(double H,double lt, double LLDF, double Sa)
        {
            if (H < 0.60)
            {                
                double lw = lt + (1.5 * H);
                return Math.Round(lw, 2);
            }
            else
            {                
                double Hint_p = (Sa - lt) / (LLDF);
                double lw = Longlw(H, Hint_p, lt, LLDF, Sa);
                return Math.Round(lw, 2);
            }
        }

        static double Longlw(double H, double Hint_p, double lt, double LLDF, double Sa)
        {
            if (H <= Hint_p)
            {
                double lw = lt + (LLDF * H);
                return lw;
            }
            else
            {
                double lw = lt + Sa + (LLDF * H);
                return lw;
            }
        }

        // Carga vehicular
        public double CargaVehicular(double H, double Wt, double lt, double P, double IM, double m, double Sw, double Di, double LLDF, double Sa)
        {
            if ( H < 0.60 )
            {
                double Ww = Wt + (1.5 * H);
                double lw = lt + (1.5 * H);
                double All = Ww * lw;
                double LL = (P * (1 + IM) * m) / All;
                return Math.Round(LL, 2);
            }
            else
            {
                double Hint_t = (Sw - Wt - 0.06 * Di) / (LLDF);
                double Hint_p = (Sa - lt) / (LLDF);
                double Ww = Ww_Ancho(H, Hint_t, Wt, LLDF, Di, Sw);
                double lw = lw_Long(H, Hint_p, lt, LLDF, Sa);
                double All = Ww * lw;
                double LL = (P * (1 + IM) * m) / All;
                return Math.Round(LL, 2);
            }
        }

        static double Ww_Ancho(double H, double Hint_t, double Wt, double LLDF, double Di, double Sw)
        {
            if (H <= Hint_t)
            {
                double Ww = Wt + (LLDF * H) + (0.06 * Di);
                return Ww;
            }
            else
            {
                double Ww = Wt + Sw + (LLDF * H) + (0.06 * Di);
                return Ww;
            }
        }

        static double lw_Long(double H, double Hint_p, double lt, double LLDF, double Sa)
        {
            if (H <= Hint_p)
            {
                double lw = lt + (LLDF * H);
                return lw;
            }
            else
            {
                double lw = lt + Sa + (LLDF * H);
                return lw;
            }
        }
        // --------------------------------------------------------------------------------------------------------------------------------------------------


        // EMPUJES //
        public double EmpujeHorizontal(double fis, double rs, double HT)
        {
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double EH = Ko * rs * HT;
            return Math.Round(EH, 2);
        }

        public double PresionAgua0(double fis, double rs)
        {
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double WA1 = Ko * rs * 0;
            return Math.Round(WA1, 2);
        }

        public double PresionAgua1(double fis, double rs, double H1)
        {
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double WA1 = Ko * rs * H1;
            return Math.Round(WA1, 2);
        }

        public double PresionAgua2(double HT, double H1, double fis, double rs, double rsat, double rw)
        {
            double H2 = HT - H1;
            double refe = rsat - rw;
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double WA2 = (Ko * (rs * H1 + refe * H2)) + rw * H2;
            return Math.Round(WA2, 2);
        }

        public double SobrecargaSuelo(double fis, double DW, double EV)
        {
            double Ko = 1 - Math.Sin(fis * Math.PI / 180);
            double qs = DW + EV;
            double ES = Ko * qs;
            return Math.Round(ES, 2);
        }

        public double SobrecargaVivaS_per(double H, double fis, double rs)
        {
            if (H < 1.50)
            {
                double Heqs = 1.20;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSs_per = Ko * rs * Heqs;
                return Math.Round(LSs_per, 2);
            }
            else
            {
                double Heqs = -0.433 * Math.Log(H) + 1.3755;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSs_per = Ko * rs * Heqs;
                return Math.Round(LSs_per, 2);
            }
        }

        public double SobrecargaVivaI_per(double HT, double fis, double rs)
        {
            if (HT < 1.50)
            {
                double Heqi = 1.20;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSi_per = Ko * rs * Heqi;
                return Math.Round(LSi_per, 2);
            }
            else
            {
                double Heqi = -0.433 * Math.Log(HT) + 1.3755;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSi_per = Ko * rs * Heqi;
                return Math.Round(LSi_per, 2);
            }
        }

        public double SobrecargaVivaS_par(double H, double fis, double rs)
        {
            if (H < 1.50)
            {
                double Heqs = 1.50;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSs_par = Ko * rs * Heqs;
                return Math.Round(LSs_par, 2);
            }
            else
            {
                double Heqs = -0.649 * Math.Log(H) + 1.7466;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSs_par = Ko * rs * Heqs;
                return Math.Round(LSs_par, 2);
            }
        }

        public double SobrecargaVivaI_par(double HT, double fis, double rs)
        {
            if (HT < 1.50)
            {
                double Heqi = 1.20;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSi_par = Ko * rs * Heqi;
                return Math.Round(LSi_par, 2);
            }
            else
            {
                double Heqi = -0.649 * Math.Log(HT) + 1.7466;
                double Ko = 1 - Math.Sin(fis * Math.PI / 180);
                double LSi_par = Ko * rs * Heqi;
                return Math.Round(LSi_par, 2);
            }
        }
    }
}

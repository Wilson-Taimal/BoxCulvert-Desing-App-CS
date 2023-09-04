using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class LosaSup
    {

        // -------------------------------------------------------------------------------------------------------------------------------
        // DATOS DE ENTRADA //

        /// <summary>
        /// Ancho de la sección _ [cm]
        /// </summary>
        public double b;

        /// <summary>
        /// Espesor del elemento _ [cm]
        /// </summary>
        public double h;

        /// <summary>
        /// Recubrimiento del refuerzo parrilla superior _ [cm]
        /// </summary>
        public double rs;

        /// <summary>
        /// Recubrimiento del refuerzo parrilla inferior _ [cm]
        /// </summary>
        public double ri;

        /// <summary>
        /// Altura efectiva parilla superior _ [cm]
        /// </summary>
        public double ds;

        /// <summary>
        /// Altura efectiva parrilla inferior _ [cm]
        /// </summary>
        public double di;

        /// <summary>
        /// Altura efectiva para analisis de cortante _ [cm]
        /// </summary>
        public double d;

        /// <summary>
        /// Cuantía mínima de refuerzo en losas
        /// </summary>
        public double pmin;

        /// <summary>
        /// Número de la barra a emplear en la parrilla superior
        /// </summary>
        public int Nbs;

        /// <summary>
        /// Número de la barra a emplear en la parrilla inferior
        /// </summary>
        public int Nbi;

        /// <summary>
        /// Aumento de barras para cumplir fisuración en parrila superior
        /// </summary>
        public int Cantt_ps;

        /// <summary>
        /// Aumento de barras para cumplir fisuración en parrila inferior
        /// </summary>
        public int Cantt_pi;

        /// <summary>
        /// Lista o vector de momentos de diseño positivos {Mux(+), Muy(+)} _ [kN.cm]
        /// </summary>
        public double[] LMup;

        /// <summary>
        /// Lista o vector de momentos de diseño negativos {Mux(-), Muy(-)} _ [kN.cm]
        /// </summary>
        public double[] LMun;

        /// <summary>
        /// Lista o vector de fuerzas cortantes {Vux, Vuy} _ [kN]
        /// </summary>
        public double[] LVu;

        /// <summary>
        /// Lista o vector de momentos en servicio positivos {Msx(+), Msy(+)} _ [kN.cm]
        /// </summary>
        public double[] LMsp;

        /// <summary>
        /// Lista o vector de momentos en servicio negativos {Msx(-), Msy(-)} _ [kN.cm]
        /// </summary>
        public double[] LMsn;


        // -------------------------------------------------------------------------------------------------------------------------------
        // METODOS //

        public string ChequeoMomentoNominal(double Mu, double Mn)
        {
            string Opcion1 = "Cumple";
            string Opcion2 = "No cumple";

            if (Mu < Mn)
            { return Opcion1; }
            else
            { return Opcion2; }

        }

        public string ChequeoCortante(double Vu, double Vc)
        {
            string Opcion1 = "Cumple";
            string Opcion2 = "No cumple";

            if (Vc > Vu)
            { return Opcion1; }
            else
            { return Opcion2; }
        }
    }
}

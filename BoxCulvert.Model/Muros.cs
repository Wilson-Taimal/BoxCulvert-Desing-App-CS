using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class Muros
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
        /// Recubrimiento del refuerzo _ [cm]
        /// </summary>
        public double r;

        /// <summary>
        /// Altura efectiva de la sección _ [cm]
        /// </summary>
        public double d;

        /// <summary>
        /// Cuantía mínima de refuerzo vertical en muros
        /// </summary>
        public double pminv;

        /// <summary>
        /// ACuantia mínima de refuerzo horizontal en muros
        /// </summary>
        public double pminh;

        /// <summary>
        /// Número de la barra a emplear en el refuerzo vertical
        /// </summary>
        public int Nbv;

        /// <summary>
        /// Número de la barra a emplear en refuerzo horizontal
        /// </summary>
        public int Nbh;

        /// <summary>
        /// Aumento de barras para cumplir fisuración en el refuerzo vertical
        /// </summary>
        public int Cantt_pv;

        /// <summary>
        /// Aumento de barras para cumplir fisuración en refuerzo horizontal
        /// </summary>
        public int Cantt_ph;

        /// <summary>
        /// Lista o vector de momentos de diseño para refuerzo vertical {Muy(+), Muy(-)} _ [kN.cm]
        /// </summary>
        public double[] LMuy;

        /// <summary>
        /// Lista o vector de momentos de diseño para refuerzo horizontal {Mux(+), Mux(-)} _ [kN.cm]
        /// </summary>
        public double[] LMux;

        /// <summary>
        /// Lista o vector de fuerzas cortantes {Vux, Vuy} _ [kN]
        /// </summary>
        public double[] LVu;

        /// <summary>
        /// Lista o vector de momentos en servicio para refuerzo vertical {Msy(+), Msy(-)} _ [kN.cm]
        /// </summary>
        public double[] LMsy;

        /// <summary>
        /// Lista o vector de momentos en servicio para refuerzo horizontal {Msx(+), Msx(-)} _ [kN.cm]
        /// </summary>
        public double[] LMsx;


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

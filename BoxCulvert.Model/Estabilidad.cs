using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class Estabilidad
    {
        // -------------------------------------------------------------------------------------------------------------------------------
        // DATOS DE ENTRADA //

        /// <summary>
        /// Esfuerzo trasmitido al suelo _ [kN/m²]
        /// </summary>
        public double Qs;

        /// <summary>
        /// Asentamiento obtenido para cargas permanentes _ [cm]
        /// </summary>
        public double Si;

        /// <summary>
        /// Asentamiento admisible esperado _ [cm]
        /// </summary>
        public double Sadm;


        // -------------------------------------------------------------------------------------------------------------------------------
        // METODOS //

        public string Esfuerzo(double Qs, double Qadm)
        {
            string Opcion1 = "Cumple";
            string Opcion2 = "No cumple";

            if (Qs < Qadm)
            { return Opcion1; }
            else
            { return Opcion2; }

        }

        public string Asentamiento(double Si, double Sadm)
        {
            string Opcion1 = "Cumple";
            string Opcion2 = "No cumple";

            if (Si < Sadm)
            { return Opcion1; }
            else
            { return Opcion2; }

        }
    }
}

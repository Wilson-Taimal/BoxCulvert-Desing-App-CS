using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class ResultCargas
    {
        /// <summary>
        /// Empuje horizontal del suelo _ [kN/m]
        /// </summary>
        public double DW { get; set; }

        /// <summary>
        /// Presión vertical del suelo de relleno _ [kN/m]
        /// </summary>
        public double EV { get; set; }

        /// <summary>
        /// Incremento de carga vehicular
        /// </summary>
        public double IM { get; set; }

        /// <summary>
        /// Ancho de la distribución de carga vehicular a un profundidad "H" _ [m]
        /// </summary>
        public double Ww { get; set; }

        /// <summary>
        /// Longitud de la distribución de carga vehicular a un profundidad "H"r _ [m]
        /// </summary>
        public double lw { get; set; }

        /// <summary>
        /// Carga viva vehicular _ [kN/m²]
        /// </summary>
        public double LL { get; set; }

        /// <summary>
        /// Empuje horizontal del suelo _ [kN/m]
        /// </summary>
        public double EH { get; set; }

        /// <summary>
        /// Empuje por carga de presión de agua, nivel cero _ [kN/m]
        /// </summary>
        public double WA0 { get; set; }

        /// <summary>
        /// Empuje por carga de presión de agua, nivel igual a H1 _ [kN/m]
        /// </summary>
        public double WA1 { get; set; }

        /// <summary>
        /// Empuje por carga de presión de agua, nivel igual a H2 _ [kN/m]
        /// </summary>
        public double WA2 { get; set; }

        /// <summary>
        /// Empuje por sobrecarga de suelo que se encuentra en la parte superior de la estructura [kN/m]
        /// </summary>
        public double ES { get; set; }

        /// <summary>
        /// Empuje por sobrecarga de carga viva superior sobre muros perpendiculares al trafico _ [kN/m]
        /// </summary>
        public double LSs_per { get; set; }

        /// <summary>
        /// Empuje por sobrcarga de carga viva inferior sobre muros perpendiculares al trafico _ [kN/m]
        /// </summary>
        public double LSi_per { get; set; }

        /// <summary>
        /// Empuje por sobrcarga de carga viva superior sobre muros paralelos al trafico _ [kN/m]
        /// </summary>
        public double LSs_par { get; set; }

        /// <summary>
        /// Empuje por sobrcarga de carga viva inferior sobre muros paralelos al trafico _ [kN/m]
        /// </summary>
        public double LSi_par { get; set; }
    }
}

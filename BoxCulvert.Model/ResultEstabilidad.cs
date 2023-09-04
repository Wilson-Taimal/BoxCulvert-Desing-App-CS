using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class ResultEstabilidad
    {
        /// <summary>
        /// Chequeo del esfuerzo trasmitido al suelo _ [kN/m]
        /// </summary>
        public string Esfuerzo { get; set; }

        /// <summary>
        /// Chqueo de asentamiento cargas permanentes _ [cm]
        /// </summary>
        public string Asentamiento { get; set; }

    }
}

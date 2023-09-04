using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class Suelo
    {
        /// <summary>
        /// Capacidad admisible neta del suelo _ [kN/m²]
        /// </summary>
        public double Qadm;

        /// <summary>
        /// Peso especifico del material de lleno _ [kN/m³]
        /// </summary>
        public double rs;

        /// <summary>
        /// Angulo de fricción del suelo.
        /// </summary>
        public double fis;

        /// <summary>
        /// Módulo de elasticidad de suelo _ [kN/m²]
        /// </summary>
        public double Es;

        /// <summary>
        /// Coeficiente de Poisson del suelo
        /// </summary>
        public double u;

        /// <summary>
        /// Coeficiente de reación vertical del suelo
        /// </summary>
        public double Kz;

        /// <summary>
        /// Peso específico del suelo saturado _ [kN/m³]
        /// </summary>
        public double rsat;

        /// <summary>
        /// Peso específico efectivo del suelo _ [kN/m³]
        /// </summary>
        public double refe;

        /// <summary>
        /// Peso específico del agua _ [kN/m³]
        /// </summary>
        public double rw;

        /// <summary>
        /// Profundidad a la que se encuentra el nivel freático _ [m]
        /// </summary>
        public double H1;

        /// <summary>
        /// Altura del suelo saturado _ [m]
        /// </summary>
        public double H2;
    }
}

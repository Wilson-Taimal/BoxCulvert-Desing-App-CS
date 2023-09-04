using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class Materiales
    {// -------------------------------------------------------------------------------------------------------------------------------
        // DATOS DE ENTRADA //

        /// <summary>
        /// Resistencia del concreto a compresión _ [MPa]
        /// </summary>
        public double fc;

        /// <summary>
        /// Fluencia del acero _ [MPa]
        /// </summary>
        public double fy;

        /// <summary>
        /// Factor de reducción al cortante
        /// </summary>
        public double fiv;

        /// <summary>
        /// Módulo de elasticidad del concreto _ [MPa]
        /// </summary>
        public double Ec;

        /// <summary>
        /// Peso específico del concreto _ [kN/m³]
        /// </summary>
        public double rc;

        /// <summary>
        /// Peso específico de la capa de rodadura (asfalto o pavimento) _ [kN/m³]
        /// </summary>
        public double rp;

        /// <summary>
        /// Ecuacion para el cálculo de Ec. 
        /// 0: 3900*Sqrt(fc), 
        /// 1: 4500*Sqrt(fc), 
        /// </summary>
        public double Ecuacion;


        // -------------------------------------------------------------------------------------------------------------------------------
        // METODOS //

        public double ElasticidadConcreto(double Ecuacion, double fc)
        {
            if (Ecuacion == 0)
            {
                double Ec = 3900 * Math.Sqrt(fc);
                return Math.Round(Ec, 0);
            }
            else
            {
                double Ec = 4500 * Math.Sqrt(fc);
                return Math.Round(Ec, 0);
            }

        }
    }
}

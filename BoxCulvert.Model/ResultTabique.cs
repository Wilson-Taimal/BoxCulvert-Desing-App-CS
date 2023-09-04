using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class ResultTabique
    {
        /// <summary>
        /// preq: Cuantía de refuerzo requerida para un momento ultimo de diseño dado
        /// </summary>
        public double preq { get; set; }

        /// <summary>
        /// Asreq: Área de acero requerido para una cuantia requerida _ [cm²]
        /// </summary>        
        public double Asreq { get; set; }

        /// <summary>
        /// Asminv: Área de acero mínimo vertical del muro  _ [cm²]
        /// </summary>
        public double Asminv { get; set; }

        /// <summary>
        /// Asminh: Área de acero mínimo horizontal del muro  _ [cm²]
        /// </summary>
        public double Asminh { get; set; }

        /// <summary>
        /// As: Área de acero obtenido por diseño igual al maximo entre Asreq y Asmin _ [cm²]
        /// </summary>
        public double As { get; set; }

        /// <summary>
        /// Ascol: Área de acero colocado que depende de la distribucion final de las barras de refuerzo _ [cm²]
        /// </summary>
        public double Ascol { get; set; }

        /// <summary>
        /// Asb: Área de la sección transversal de la barra selecionada _ [cm²]
        /// </summary>
        public double Asb { get; set; }

        /// <summary>
        /// db: Diámetro de la sección transversal de la barra selecionada _ [cm²]
        /// </summary>
        public double db { get; set; }

        /// <summary>
        /// sep: Separación entre barras de refuerzo en un ancho "b" _ [cm]
        /// </summary>        
        public double Sep { get; set; }

        /// <summary>
        /// Cat: Cantidad de barras empleadas en un ancho "b".
        /// </summary>        
        public double Cant { get; set; }

        /// <summary>
        /// Muy: Momento ultimo de diseño para refuerzo vertical _ [kN.cm]
        /// </summary>        
        public double Muy { get; set; }

        /// <summary>
        /// Mux: Momento ultimo de diseño para refuerzo horizontal _ [kN.cm]
        /// </summary>        
        public double Mux { get; set; }

        /// <summary>
        /// Vu: Fuerza cortante de diseño _ [kN]
        /// </summary>        
        public double Vu { get; set; }

        /// <summary>
        /// Mn: Momento nominal de la sección _ [kN.cm]
        /// </summary>        
        public double Mn { get; set; }

        /// <summary>
        /// Vc: Resistencia del concreto al corte _ [kN]
        /// </summary>        
        public double Vc { get; set; }

        /// <summary>
        /// Msy: Momento en servicio para refuerzo vertical _ [kN.cm]
        /// </summary>        
        public double Msy { get; set; }

        /// <summary>
        /// Msx: Momento en servicio para refuerzo horizontal _ [kN.cm]
        /// </summary>        
        public double Msx { get; set; }

        /// <summary>
        /// fs: Esfuerzo en el acero de refuerzo _ [MPa]
        /// </summary>        
        public double fs { get; set; }

        /// <summary>
        /// fsadm: Esfuerzo máximo admisible en el refuerzo _ [MPa]
        /// </summary>        
        public double fsadm { get; set; }

        /// <summary>
        /// Mureq: Momento último de diseño requerido _ [kN.cm]
        /// </summary>        
        public double Mureq { get; set; }

        /// <summary>
        /// Chequeo de momento nominal de la sección (Mu < øMn)
        /// </summary>
        public string ChequeoMomentoNominal { get; set; }

        /// <summary>
        /// Chequeo resistencia del consreto al cortante (øVc > Vu)
        /// </summary>
        public string ChequeoCortante { get; set; }

        /// <summary>
        /// Chequeo de fisuración (170 MPa < fsadm < 250) y (fs < fsadm)
        /// </summary>
        public string ChequeoFisuracion { get; set; }
    }
}

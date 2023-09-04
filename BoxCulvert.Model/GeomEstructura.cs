using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class GeomEstructura
    {
        // -------------------------------------------------------------------------------------------------------------------------------
        // DATOS DE ENTRADA //

        /// <summary>
        /// Espesor capa de rodadura (pavimento) _ [m]
        /// </summary>
        public double ep;

        /// <summary>
        /// Espesor del material del lleno _ [m]
        /// </summary>
        public double er;

        /// <summary>
        /// Espesor de losa _ [m]
        /// </summary>
        public double el;

        /// <summary>
        /// Altura libre cuerpo 1 _ [m]
        /// </summary>
        public double h2;

        /// <summary>
        /// Altura libre cuerpo 2 _ [m]
        /// </summary>
        public double h3;

        /// <summary>
        /// Altura desde el NPA hasta el inicio de la losa superior _ [m]
        /// </summary>
        public double H;

        /// <summary>
        /// Altura total del Box, medida desde el NPA has la terminación de la losa de fondo _ [m]
        /// </summary>
        public double HT;

        /// <summary>
        /// Ancho, luz libre cuerpo 1 _ [m]
        /// </summary>
        public double a;

        /// <summary>
        /// Ancho, luz libre cuerpo 2 _ [m]
        /// </summary>
        public double b;

        /// <summary>
        /// Espesor muros _ [m]
        /// </summary>
        public double em;

        /// <summary>
        /// Espesor tabique _ [m]
        /// </summary>
        public double et;

        /// <summary>
        /// Luz libre de la galeria _ [m]
        /// </summary>
        public double Di;

        /// <summary>
        /// Ancho total de la galeria _ [m]
        /// </summary>
        public double Bc;

        /// <summary>
        /// Longitud del modulo que conforma el Box _ [m]
        /// </summary>
        public double Li;

        /// <summary>
        /// Longitud total del Box _ [m]
        /// </summary>
        public double L;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCulvert.Model
{
    public class Refuerzo
    {
        double db;
        double Asb;

        public double DiametroBarra(int Nb)
        {
            // -------------------------------------------------------------------------------------------------------------------------------
            // METODOS //

            switch (Nb)
            {
                case (2):
                    db = 0.64; break;
                case (3):
                    db = 0.95; break;
                case (4):
                    db = 1.27; break;
                case (5):
                    db = 1.59; break;
                case (6):
                    db = 1.91; break;
                case (7):
                    db = 2.22; break;
                case (8):
                    db = 2.54; break;
                default:
                    db = 0; break;
            }
            return db;
        }

        public double AreaBarra(int Nb)
        {
            switch (Nb)
            {
                case (2):
                    Asb = 0.32; break;
                case (3):
                    Asb = 0.71; break;
                case (4):
                    Asb = 1.29; break;
                case (5):
                    Asb = 1.99; break;
                case (6):
                    Asb = 2.84; break;
                case (7):
                    Asb = 3.87; break;
                case (8):
                    Asb = 5.10; break;
                default:
                    Asb = 0; break;
            }
            return Asb;
        }

    }
}

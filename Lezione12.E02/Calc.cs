using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezione12.E02
{
    public class Calc
    {
        public delegate double Calculate(int a, int b);

        public double Somma (int a, int b)
        {
            return a + b;
        }
        public double Sottrazione (int a, int b)
        {
            return a - b;
        }
        public double Divisione (int a, int b)
        {
            return a / b;
        }
        public double Moltiplicazione (int a, int b)
        {
            return a * b;
        }

        public Calculate Choose(int Operazione)
        {
            switch(Operazione)
            {
                case 1:
                    return Somma;
                case 2:
                    return Sottrazione;
                case 3:
                    return Divisione;
                case 4:
                    return Moltiplicazione;
                default:
                    return null;
            }
        }

    }
}

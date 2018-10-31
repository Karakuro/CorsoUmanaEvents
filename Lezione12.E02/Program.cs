using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lezione12.E02
{
    class Program
    {
        static void Main(string[] args)
        {
            Calc Calcolatore = new Calc();
            var c = Calcolatore.Choose(1);
            c += Calcolatore.Choose(2);
            c += Calcolatore.Choose(3);
            c += Calcolatore.Choose(4);

            foreach(var x in c.GetInvocationList())
            {
                //Calc.Calculate test = (Calc.Calculate)x;
                Console.WriteLine($"{x.DynamicInvoke(1, 2)}");
            }
            Console.ReadLine();
            //c.Invoke(1, 2);
        }
    }
}

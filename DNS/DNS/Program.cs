using System;

namespace DNS
{
    class Program
    {
        static void Main(string[] args)
        {
            Display display = new Display();
            while (true)//Just so you have the chance to refresh if connection is lost 
            {
                display.Wiki();//Starting out with our beloved wiki :D
            }
        }
    }
}

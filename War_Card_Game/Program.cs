using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War_Card_Game
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Manual Play? y/n");
            string manualPlay = Console.ReadLine();
            Cards cards = new Cards(manualPlay == "y");
            cards.Play();
        }
    }
}

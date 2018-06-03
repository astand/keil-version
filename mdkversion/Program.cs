using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdkversion
{
    class Program
    {
        static Mdkversion mdkver;

        static void Main(string[] args)
        {
            mdkver = new Mdkversion(args.ToList());
            mdkver.UpdateVersion();

            foreach (var str in mdkver.StatusMessages)
            {
                Console.WriteLine(str);
            }

            var path = Directory.GetCurrentDirectory();
        }
    }
}

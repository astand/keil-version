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
        static void Main(string[] args)
        {
            var ret = args.ToList();
            //var ret = list.ToList();
            var mdkver = new Mdkversion(ret);
            mdkver.UpdateVersion();

            foreach (var str in mdkver.StatusMessages)
            {
                Console.WriteLine(str);
            }

            var path = Directory.GetCurrentDirectory();

            foreach (var item in ret)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine(path);
            Console.WriteLine(path);
            Console.WriteLine(path);
            Console.WriteLine("Ended...");
        }

        static string[] list = new string[]
        {
            "/conf/conf.h",
            "major=null",
            "minor=month",
            "rev=day",
            "build"
        };

        static string[] list1 = new string[]
        {
            "?"
        };
    }
}

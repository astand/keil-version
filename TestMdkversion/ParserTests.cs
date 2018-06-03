using System;
using System.Diagnostics;
using System.Linq;
using mdkversion;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TestMdkversion
{
    [TestClass]
    public class ParserTests
    {
        public Mdkversion vers;

        void PrintResult(string[] s)
        {
            foreach (var str in s)
            {
                Debug.WriteLine(str);
            }
        }

        [TestMethod]
        public void TestBaseArguments()
        {
            vers = new Mdkversion(list.ToList());
            vers.UpdateVersion();
            PrintResult(vers.StatusMessages);
        }

        [TestMethod]
        public void TestHelpArguments()
        {
            vers = new Mdkversion(list1.ToList());
            PrintResult(vers.StatusMessages);
        }

        static string[] list = new string[]
        {
            "/conf/conf.txt",
            "ma=inc",
            "mi=month",
            "rev=day",
            "build"
        };

        static string[] list1 = new string[]
        {
            "?",
            "-ma=inc"
        };

    }
}

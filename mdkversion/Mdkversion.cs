using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mdkversion
{
    internal class Mdkversion
    {

        public string[] StatusMessages { get; set; }

        public Mdkversion(List<string> ret)
        {
            vs = ret;

            if (vs.Count() == 0)
            {
                innerfail = true;
                StatusMessages = new string[] { "Please pass at least one parameter. \"mdkversion.exe ?\" will print instruction" };
            }
            else if (vs[0] == "?")
            {
                innerfail = true;
                StatusMessages = HelpMessage;
            }
            else
            {
                StatusMessages = new string[] { "Nothing here " };
            }
        }

        internal void UpdateVersion()
        {
            if (innerfail)
                return;

            CheckParameters();

            if (GetCurrentConfigFile() == false)
                CreateNewConfigFile();

            UpdateConfigFileInfo();
        }

        private void UpdateConfigFileInfo()
        {
            if (innerfail == true) {return;}
        }

        private void CreateNewConfigFile()
        {
            if (innerfail == true) {return;}
        }

        private bool GetCurrentConfigFile()
        {
            if (innerfail == true) {return false;}

            return false;
        }

        private void CheckParameters()
        {
            if (innerfail == true) {return;}
        }

        private readonly List<string> vs;

        private string[] HelpMessage =
        {
            "This utility performs versioning control. Settings passed as parameters of commandline.",
            "",
            "",
            "First parametr - path and name to config (.h) file",
            "Next maybe up to 4 params in key=value format where:",
            "  key - name of version number : [ma, mi, rev, bld]",
            "  value - behavior : [none, inc, y, m, d, h]",
            "  \"none\" - the number won't be changed",
            "  \"inc\" - the number will be incremented after each build",
            "  \"y\",\"m\",\"d\",\"h\" - the number will be equal to current year, month, day, hour",
            "",
            "  Example: mdkversion.exe \"sr/conf/conf.h\" ma=none mi=m rev=d bld=inc",
            ""
        };

        private bool innerfail;
    }
}

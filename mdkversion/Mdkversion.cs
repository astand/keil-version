using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CommandLine;

namespace mdkversion
{
    public class Mdkversion
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

        public void UpdateVersion()
        {
            if (innerfail)
                return;

            CheckParameters();

            if (GetCurrentConfigFile() == false)
                CreateNewConfigFile();

            ReadVersion();
            UpdateConfigFileInfo();
        }

        private void ReadVersion()
        {
            if (innerfail == true) { return; }

            var lines = new string[0];

            try
            {
                lines = File.ReadAllLines(confpath);
            }
            catch (Exception e)
            {
                innerfail = true;
                StatusMessages = new string[]
                {
                    "Config file reading failed!",
                    $"Error: {e.Message}",
                    $"Check path parameter : {confpath}!"
                };
                return;
            }

            var vers = lines.Where(s => s.Contains("MDKVERSION_")).ToList();

            if (vers.Count() != 4)
            {
                innerfail = true;
                StatusMessages = new string[]
                {
                    "Current config file doesn't contain valid 4 version numbers. Drop file and try again."
                };
                return;
            }

            numVersion[0] = GetVersionFromString(vers, "MAJOR");
            numVersion[1] = GetVersionFromString(vers, "MINOR");
            numVersion[2] = GetVersionFromString(vers, "REVISION");
            numVersion[3] = GetVersionFromString(vers, "BUILD");
        }

        private int GetVersionFromString(List<string> vers, string v)
        {
            var ret = 0;
            var str = string.Empty;
            str = vers?.FirstOrDefault(s => s.Contains(v));

            if (str == null)
                return ret;

            int.TryParse(Regex.Split(str, @"\s+")[2], out ret);
            return ret;
        }

        private void UpdateConfigFileInfo()
        {
            if (innerfail == true) {return;}

            var dt = DateTime.Now;

            for (int i = 0; i < 4; i++)
            {
                switch (strategies[i])
                {
                    case "inc":
                    {
                        numVersion[i]++;
                        break;
                    }

                    case "y":
                    {
                        numVersion[i] = dt.Year % 100;
                        break;
                    }

                    case "m":
                    {
                        numVersion[i] = dt.Month;
                        break;
                    }

                    case "d":
                    {
                        numVersion[i] = dt.Day;
                        break;
                    }

                    case "h":
                    {
                        numVersion[i] = dt.Hour;
                        break;
                    }

                    default:
                        // @none - no changes
                        break;
                }
            }

            var lines = File.ReadAllLines(confpath);

            for (int j = 0; j < lines.Length; j++)
            {
                if (lines[j].Contains("MDKVERSION_"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (lines[j].Contains(VersPseudo[i]))
                        {
                            //Regex reg = new Regex(@"\s+");
                            //var pars = reg.Split(line);
                            //lines[j] = lines[j].Replace(pars[2], numVersion[i].ToString());
                            lines[j] = versPreambul + VersPseudo[i].PadRight(12, ' ') + numVersion[i];
                        }
                    }
                }
            }

            File.WriteAllLines(confpath, lines);
            StatusMessages = new string[]
            {
                $">> Version file < {confpath} > was updated.",
                $"Current version:  {numVersion[0]}.{numVersion[1]}.{numVersion[2]}.{numVersion[3]}",
                Environment.NewLine
            };
        }

        private string WriteNewVersionInline(string line, int i)
        {
            throw new NotImplementedException();
        }

        private void CreateNewConfigFile()
        {
            if (innerfail == true) {return;}

            var fulldir = Path.GetDirectoryName(confpath);

            try
            {
                Directory.CreateDirectory(fulldir);
            }
            catch (Exception e)
            {
                innerfail = true;
                StatusMessages = new string[]
                {
                    "Directory cannot be created!",
                    $"Error: {e.Message}",
                    $"Directory creates here: {confpath}!"
                };
                return;
            }

            try
            {
                File.WriteAllLines(confpath, ConfigContent);

                for (int i = 0; i < 4; i++)
                {
                    // file version by default values
                    var str = String.Format(versPreambul + VersPseudo[i].PadRight(12, ' ') + 0 + Environment.NewLine);
                    File.AppendAllText(confpath, str);
                }

                File.AppendAllText(confpath, "");
            }
            catch (Exception e)
            {
                innerfail = true;
                StatusMessages = new string[]
                {
                    "Config file creation failed!",
                    $"Error: {e.Message}",
                    $"Check path parameter: {confpath}!"
                };
            }
        }

        private bool GetCurrentConfigFile()
        {
            if (innerfail == true) {return false;}

            confpath = vs[0];
            var ret = false;
            var fullpath = Directory.GetCurrentDirectory();

            if (confpath.StartsWith("/"))
            {
                fullpath += confpath;
            }
            else
            {
                fullpath += "/" + confpath;
            }

            confpath = fullpath;

            try
            {
                if (File.Exists(confpath))
                {
                    // config already exists. Read it
                    ret = true;
                }
            }
            catch (Exception e)
            {
                innerfail = true;
                StatusMessages = new string[]
                {
                    "Config file was not detected. Reading impossible.",
                    $"Error: {e.Message}",
                    "Check parameters!"
                };
            }

            return ret;
        }

        private void CheckParameters()
        {
            if (innerfail == true) {return;}

            // get config file path from args
            var confpath = vs[0];

            for (int i = 0; i < vs.Count() - 1; i++)
            {
                // fill 4 version numbers strategy
                try
                {
                    var pair = vs[i + 1].Split('=');

                    if (pair[0] == ("ma"))
                    {
                        strategies[0] = pair[1];
                    }
                    else if (pair[0] == ("mi"))
                    {
                        strategies[1] = pair[1];
                    }
                    else if (pair[0] == ("rev"))
                    {
                        strategies[2] = pair[1];
                    }
                    else if (pair[0] == ("bld"))
                    {
                        strategies[3] = pair[1];
                    }
                }
                catch (Exception)
                {
                    strategies[i] = "none";
                }
            }
        }

        private readonly List<string> vs;

        private bool innerfail;

        string[] strategies = new string[4];

        int[] numVersion = new int[4] { 0, 0, 0, 0 };

        string confpath;

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


        private string[] ConfigContent =
        {
            "#pragma once",
            "/*",
            $"This file generated by Mdkversion.exe utility @ {DateTime.Now.ToString()}",
            "*/",
            ""
        };


        private string versPreambul = "#define MDKVERSION_";

        private string[] VersPseudo =
        {
            "MAJOR",
            "MINOR",
            "REVISION",
            "BUILD"
        };

    }
}

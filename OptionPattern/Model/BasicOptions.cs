using System;
using System.Collections.Generic;
using System.Text;

namespace OptionPattern.Model
{
    public class BasicOptions : Base
    {
        public BasicOptions()
        {
            FunctionName = "[Default]";
            Version = 1;
            Enviroment = Enviroment.BuildDefault;
        }

        public Enviroment Enviroment { get; set; }
    }

    public class Enviroment
    {
        public static Enviroment BuildDefault => new Enviroment();

        public Enviroment()
        {
            Name = "[Prerelease deveopment]";
            Standalone = true;
        }

        public string Name { get; set; }
        public bool Standalone { get; set; }
    }

}

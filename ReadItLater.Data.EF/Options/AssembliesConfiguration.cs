using System;
using System.Collections.Generic;
using System.Text;

namespace ReadItLater.Data.EF.Options
{
    public class AssembliesConfiguration
    {
        public static string AssembliesSection = "Assemblies";

        //public string? Dtos { get; set; }
        public string? Entities { get; set; }
        //public string? Infrastructure { get; set; }
    }
}

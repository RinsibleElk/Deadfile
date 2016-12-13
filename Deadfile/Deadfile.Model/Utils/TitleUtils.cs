using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadfile.Model.Utils
{
    /// <summary>
    /// Utilities for handling titles.
    /// </summary>
    public static class TitleUtils
    {
        /// <summary>
        /// All the suggested titles that should show up in combo boxes.
        /// </summary>
        public static readonly string[] AllTitles =
            new string[]
            {
                "Mr",
                "Mrs",
                "Ms",
                "Mr & Mrs",
                "Dr",
                "Dr & Mrs",
                "Mr & Dr",
                "Dr & Dr",
                "Attack Helicopter"
            };
    }
}

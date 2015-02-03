using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace imagesIPM.Helpers
{
    public class SupportedFiles
    {
        public static string[] types = new string[]
        {
            ".bmp",
            ".jpg",
            ".jpeg",
            ".gif",
            ".png"
        };


        public static bool IsSupported(string ext)
        {
            return types.Contains(ext.ToLower());
        }

    }
}

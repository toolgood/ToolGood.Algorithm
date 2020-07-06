using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Antlr4Helper.JavaScriptHelper.Helpers
{
    public class MiniSerializedATN
    {
        public static string MiniAtn(string atn)
        {
            atn = atn.Replace(@"\u00", @"\x");
            atn = Regex.Replace(atn, @"\\x0([1-7])([^01234567])", @"\$1$2");
            atn = Regex.Replace(atn, @"\\x0([1-7])([^01234567])", @"\$1$2");

            for (int i = 0; i < 64; i++)
            {
                var olds = @"\\x(" + Convert.ToString(i, 16).PadLeft(2, '0') + @")([^01234567])";
                var news = @"\" + Convert.ToString(i, 8).PadLeft(2, '0') + @"$2";
                atn = Regex.Replace(atn, olds, news);
                atn = Regex.Replace(atn, olds, news);
            }
            return atn;
        }
    }
}

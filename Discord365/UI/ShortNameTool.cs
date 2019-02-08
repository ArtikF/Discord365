using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord365.UI
{
    public class ShortNameTool
    {
        public static string GetShortName(string source, int len = 3)
        {
            string result = "";

            string[] sp = source.Trim().Split(' ');

            if(sp.Length == 1)
            {
                for(int i = 0; i < len; i++)
                {
                    result += source[i];
                }
                return result;
            }

            for(int i = 0; i < sp.Length; i++)
            {
                if (i == len)
                    break;

                result += sp[i][0];
            }

            return result;
        }
    }
}

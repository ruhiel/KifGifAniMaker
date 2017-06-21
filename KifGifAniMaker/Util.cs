using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
    public static class Util
    {
        public static BlackWhite Reverse(this BlackWhite bw) => bw == BlackWhite.Black ? BlackWhite.White : BlackWhite.Black;

        public static string ToSymbol(this BlackWhite bw) => bw == BlackWhite.Black ? "☗" : "☖";
    }
}

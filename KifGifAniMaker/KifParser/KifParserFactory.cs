using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker.KifParser
{
    public static class KifParserFactory
    {
        public static Parser Create(Options options) => new KifParser(options);
    }
}

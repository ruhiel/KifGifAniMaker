using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker.KifParser
{
    public interface Parser
    {
        Record ReadFile();
    }
}

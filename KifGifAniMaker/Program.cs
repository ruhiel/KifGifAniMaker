using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using CommandLine;

namespace KifGifAniMaker
{
	class Program
	{
		static void Main(string[] args)
		{
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                var board = new Board();
                board.InitBoard();
                board.MakeAnimation(options);
            }
		}
	}
}

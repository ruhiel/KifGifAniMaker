using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace KifGifAniMaker
{
	class Program
	{
		static void Main(string[] args)
		{
			var board = new Board();
			board.InitBoard();
			board.MakeGif(args[0]);
		}
	}
}

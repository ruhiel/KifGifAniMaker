using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
	public class ParseResult
	{
        public string Move => Position + Piece + ActionString;
        public BlackWhite BlackWhite { get; set; }
        public string Position { get; set; }
		public int DestPosX { get; set; }
		public int DestPosY { get; set; }
		public bool Promoted { get; set; }
		public string Piece { get; set; }
		public Action? Action { get; set; }
		public int? SrcPosX { get; set; }
		public int? SrcPosY { get; set; }
        public string ActionString { get; set; }
        public int MoveNum { get; set; }
        public ParseResult(BlackWhite bw, int moveNum)
        {
            BlackWhite = bw;
            MoveNum = moveNum;
        }
    }
}

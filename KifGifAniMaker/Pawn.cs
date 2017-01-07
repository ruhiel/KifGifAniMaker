using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
	public class Pawn : Piece
	{
		public Pawn(BlackWhite bw) : base(bw)
		{
		}

		public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl28.png" : "sgl08.png") : (Promoted ? "sgl58.png" : "sgl38.png");

		public override bool Promotable => true;
	}
}

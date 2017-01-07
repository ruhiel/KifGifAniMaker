using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
	public class Rook : Piece
	{
		public Rook(BlackWhite bw) : base(bw)
		{
		}

		public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl22.png" : "sgl02.png") : (Promoted ? "sgl51.png" : "sgl32.png");

		public override bool Promotable => true;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
	public class King : Piece
	{
		public King(BlackWhite bw) : base(bw)
		{
		}

		public override string ImageFile => BW == BlackWhite.Black ? "sgl11.png" : "sgl31.png";

		public override bool Promotable => false;
	}
}

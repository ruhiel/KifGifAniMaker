using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
	public class Lance : Piece
	{
		public Lance(BlackWhite bw) : base(bw)
		{
		}

		public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl27.png" : "sgl07.png") : (Promoted ? "sgl57.png" : "sgl37.png");

		public override bool Promotable => true;
	}
}

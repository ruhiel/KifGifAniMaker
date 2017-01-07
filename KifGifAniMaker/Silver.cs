using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
	public class Silver : Piece
	{
		public Silver(BlackWhite bw) : base(bw)
		{
		}

		public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl25.png" : "sgl05.png") : (Promoted ? "sgl55.png" : "sgl35.png");

		public override bool Promotable => true;
	}
}

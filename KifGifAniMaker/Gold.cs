using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
	public class Gold : Piece
	{
		public Gold(BlackWhite bw) : base(bw)
		{
		}

		public override string ImageFile => BW == BlackWhite.Black ? "sgl04.png" : "sgl34.png";

		public override bool Promotable => false;
	}
}

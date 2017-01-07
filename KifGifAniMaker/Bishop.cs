using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
	public class Bishop : Piece
	{
		public Bishop(BlackWhite bw) : base(bw)
		{
		}

		public override string ImageFile => BW == BlackWhite.Black ? (Promoted ? "sgl23.png" : "sgl03.png") : (Promoted ? "sgl53.png" :"sgl33.png");

		public override bool Promotable => true;
	}
}

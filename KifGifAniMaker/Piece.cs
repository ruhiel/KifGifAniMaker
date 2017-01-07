using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
	public abstract class Piece
	{
		public BlackWhite BW { get; set; }

		public void Reverse() => BW = BW == BlackWhite.Black ? BlackWhite.White : BlackWhite.Black;

		public abstract string ImageFile { get; }

		public abstract bool Promotable { get; }

		public bool Promoted { get; set; }

		public void Promote() => Promoted = Promotable;

		public Piece(BlackWhite bw)
		{
			BW = bw;
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;

namespace KifGifAniMaker
{
	public class Board : IEnumerable<Piece>
	{
		private Piece[,] _Pieces;

		/// <summary>
		/// 手番
		/// </summary>
		private BlackWhite _Hand;

		/// <summary>
		/// 先手駒台
		/// </summary>
		private List<Piece> _BlackHands;

		/// <summary>
		/// 後手駒台
		/// </summary>
		private List<Piece> _WhiteHands;

		private float _Rate = 1.0f;

		/// <summary>
		/// 駒台
		/// </summary>
		/// <returns></returns>
		private List<Piece> GetHands() => _Hand == BlackWhite.Black ? _BlackHands : _WhiteHands;

		private Tuple<int, int> _OldPosition;

		private Dictionary<string, Type> _PieceDictionary = new Dictionary<string, Type>() {
			{"玉", typeof(King)},
			{"金", typeof(Gold)},
			{"銀", typeof(Silver)},
			{"桂", typeof(Knight)},
			{"香", typeof(Lance)},
			{"角", typeof(Bishop)},
			{"飛", typeof(Rook)},
			{"歩", typeof(Pawn)}
		};

		public Board()
		{
			_Pieces = new Piece[9, 9];
			_Hand = BlackWhite.Black;
		}

		public void InitBoard()
		{
			_BlackHands = new List<Piece>();
			_WhiteHands = new List<Piece>();

			this[5, 9] = new King(BlackWhite.Black);
			this[5, 1] = new King(BlackWhite.White);
			this[4, 9] = new Gold(BlackWhite.Black);
			this[6, 1] = new Gold(BlackWhite.White);
			this[6, 9] = new Gold(BlackWhite.Black);
			this[4, 1] = new Gold(BlackWhite.White);
			this[3, 9] = new Silver(BlackWhite.Black);
			this[7, 1] = new Silver(BlackWhite.White);
			this[7, 9] = new Silver(BlackWhite.Black);
			this[3, 1] = new Silver(BlackWhite.White);
			this[2, 9] = new Knight(BlackWhite.Black);
			this[8, 1] = new Knight(BlackWhite.White);
			this[8, 9] = new Knight(BlackWhite.Black);
			this[2, 1] = new Knight(BlackWhite.White);
			this[1, 9] = new Lance(BlackWhite.Black);
			this[9, 1] = new Lance(BlackWhite.White);
			this[9, 9] = new Lance(BlackWhite.Black);
			this[1, 1] = new Lance(BlackWhite.White);
			this[2, 8] = new Rook(BlackWhite.Black);
			this[8, 2] = new Rook(BlackWhite.White);
			this[8, 8] = new Bishop(BlackWhite.Black);
			this[2, 2] = new Bishop(BlackWhite.White);
			this[5, 7] = new Pawn(BlackWhite.Black);
			this[5, 3] = new Pawn(BlackWhite.White);
			this[4, 7] = new Pawn(BlackWhite.Black);
			this[6, 3] = new Pawn(BlackWhite.White);
			this[6, 7] = new Pawn(BlackWhite.Black);
			this[4, 3] = new Pawn(BlackWhite.White);
			this[3, 7] = new Pawn(BlackWhite.Black);
			this[7, 3] = new Pawn(BlackWhite.White);
			this[7, 7] = new Pawn(BlackWhite.Black);
			this[3, 3] = new Pawn(BlackWhite.White);
			this[2, 7] = new Pawn(BlackWhite.Black);
			this[8, 3] = new Pawn(BlackWhite.White);
			this[8, 7] = new Pawn(BlackWhite.Black);
			this[2, 3] = new Pawn(BlackWhite.White);
			this[1, 7] = new Pawn(BlackWhite.Black);
			this[9, 3] = new Pawn(BlackWhite.White);
			this[9, 7] = new Pawn(BlackWhite.Black);
			this[1, 3] = new Pawn(BlackWhite.White);
		}

        public void Next() => _Hand = _Hand == BlackWhite.Black ? BlackWhite.White : BlackWhite.Black;

        public string Paint(int idx, Record record)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), @"result" + idx + ".png");

			//画像ファイルを読み込んでImageオブジェクトを作成する
			using (var img = new Bitmap(@"img\japanese-chess-b02.png"))
			using (var g = Graphics.FromImage(img))
			{
                const float baseX = 30.0f;
                const float baseY = 130.0f;

				// 盤
				for (var i = 0; i < 9; i++)
				{
					for (var j = 0; j < 9; j++)
					{
						var piece = _Pieces[i, j];
						if (piece != null)
						{
							using (var img2 = new Bitmap(@"img\" + piece.ImageFile))
							{
								g.DrawImage(img2, new PointF(baseX + j * 60, baseY + i * 64));
							}
						}
					}
				}

				// 駒台
				_BlackHands.Sort(ComparePiece);
				var groups1 = _BlackHands.GroupBy(x => x.GetType());
				for(var i = 0; i < groups1.Count(); i++)
				{
					var group = groups1.ElementAt(i);
					using (var img2 = new Bitmap(@"img\" + group.First().ImageFile))
					{
						g.DrawImage(img2, new PointF(i * 60, 750.0f));
					}

					DrawNum(g, group.Count(), i, BlackWhite.Black);
				}

				_WhiteHands.Sort(ComparePiece);
				var groups2 = _WhiteHands.GroupBy(x => x.GetType());
				for (var i = 0; i < groups2.Count(); i++)
				{
					var group = groups2.ElementAt(i);
					using (var img2 = new Bitmap(@"img\" + group.First().ImageFile))
					{
						g.DrawImage(img2, new PointF(i * 60, 0.0f));
					}

					DrawNum(g, group.Count(), i, BlackWhite.White);
				}

                if (record.Moves[idx].ActionString != "投了")
                {
                    g.DrawRectangle(new Pen(Brushes.Red, 2), new Rectangle((int)baseX + (9 - record.Moves[idx].DestPosX) * 60, (int)baseY + (record.Moves[idx].DestPosY - 1) * 64, 60, 64));
                }

                g.DrawString($"▽{record.WhitePlayer}", new Font("MS UI Gothic", 24), Brushes.Black, 600, 40);

                foreach (var element in record.Moves.Skip(idx).Take(10).Select((move , index) => new { move, index }))
                {
                    g.DrawString($"{element.move.MoveNum} {element.move.BlackWhite.ToSymbol()} {element.move.MoveString}", new Font("MS UI Gothic", 24), Brushes.Black, 600, 100 + element.index * 60);
                }

                g.DrawString($"▲{record.BlackPlayer}", new Font("MS UI Gothic", 24), Brushes.Black, 600, 700);

                //作成した画像を保存する
                img.Save(path, ImageFormat.Png);
			}

			var tmp = Path.Combine(Path.GetDirectoryName(path) , Path.GetFileName(path) + ".tmp");

			if (File.Exists(tmp))
			{
				File.Delete(tmp);
			}

			File.Move(path, tmp);

			using (var bmp = new Bitmap(tmp))
			{
				var resizeWidth = (int)(bmp.Width * _Rate);

				var resizeHeight = (int)(bmp.Height * _Rate);

				using (var resizeBmp = new Bitmap(resizeWidth, resizeHeight))
				using (var g = Graphics.FromImage(resizeBmp))
				{
					g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
					g.DrawImage(bmp, 0, 0, resizeWidth, resizeHeight);

					resizeBmp.Save(path, ImageFormat.Png);
				}
			}

			File.Delete(tmp);

			return path;
		}

		private void DrawNum(Graphics g, int count, int index, BlackWhite bw)
		{
			if(count == 1)
			{
				return;
			}
			else if(count > 9)
			{
				var degit10 = count / 10;
				var degit1 = count % 10;
				using (var img10 = new Bitmap(@"img\" + "number3_4" + degit10 + ".png"))
				using (var img1 = new Bitmap(@"img\" + "number3_4" + degit1 + ".png"))
				{
					g.DrawImage(img10, new PointF(index * 60 + 55, bw == BlackWhite.Black ? 750.0f : 0.0f));
					g.DrawImage(img1, new PointF(index * 60 + 65, bw == BlackWhite.Black ? 750.0f : 0.0f));
				}

			}
			else
			{
				using (var img1 = new Bitmap(@"img\" + "number3_4" + count + ".png"))
				{
					g.DrawImage(img1, new PointF(index * 60 + 55, bw == BlackWhite.Black ? 750.0f : 0.0f));
				}
			}
		}

		private int ComparePiece(Piece a, Piece b)
		{
			var array = new List<Type>() { typeof(King), typeof(Rook), typeof(Bishop), typeof(Gold), typeof(Silver), typeof(Knight), typeof(Lance), typeof(Pawn) };
			return array.IndexOf(a.GetType()) - array.IndexOf(b.GetType());
		}

		public Piece this[int x, int y]
		{
			get
			{
				if (x < 1 || x > 9)
                {
                    throw new IndexOutOfRangeException();
                }

                if (y < 1 || y > 9)
                {
                    throw new IndexOutOfRangeException();
                }

                x = 9 - x;
				y -= 1;
				return _Pieces[y, x];
			}
			set
			{
				if (x < 1 || x > 9)
                {
                    throw new IndexOutOfRangeException();
                }

                if (y < 1 || y > 9)
                {
                    throw new IndexOutOfRangeException();
                }

                x = 9 - x;
				y -= 1;
				_Pieces[y, x] = value;
			}
		}

		public Record ReadFile(string filePath)
		{
            var record = new Record();
			var list = new List<Move>();
			var pattern = @"^\s*(?<movenum>[0-9]+)\s(?<pos>同\s*|(?<dstPosX>[１２３４５６７８９])(?<dstPosY>[一二三四五六七八九]))(?<promoted>成)?(?<piece>[玉飛角金銀桂香歩龍馬と])[右左]?[上直寄引]?(?<action>不?成|打)?(?<srcPos>\((?<srcPosX>[1-9])(?<srcPosY>[1-9])\))?";
            var pleyerPattern = @"(?<bw>先手|後手)：(?<name>.+)";
            var regex = new Regex(pattern);
            var playerRegex = new Regex(pleyerPattern);
			var numeric = "１２３４５６７８９";
			var numericKan = "一二三四五六七八九";
            var resignPattern = @"^\s*(?<movenum>[0-9]+)\s*投了";
            var resignRegex = new Regex(resignPattern);

            // ファイルからテキストを読み出し。
            using (var r = new StreamReader(filePath, System.Text.Encoding.GetEncoding("shift-jis")))
			{
				string line;
                var bw = BlackWhite.Black;
                var oldDestPosX = 0;
                var oldDestPosY = 0;
                while ((line = r.ReadLine()) != null) // 1行ずつ読み出し。
                {
                    var match = regex.Match(line);
                    if (match.Success)
                    {
                        var move = new Move(bw, int.Parse(match.Groups["movenum"].Value.Trim()));

                        move.Position = match.Groups["pos"].Value.Trim();
                        if (move.Position != "同")
                        {
                            move.DestPosX = numeric.IndexOf(match.Groups["dstPosX"].Value) + 1;
                            move.DestPosY = numericKan.IndexOf(match.Groups["dstPosY"].Value) + 1;
                            oldDestPosX = move.DestPosX;
                            oldDestPosY = move.DestPosY;
                        }
                        else
                        {
                            move.DestPosX = oldDestPosX;
                            move.DestPosY = oldDestPosY;
                        }
                        move.Promoted = match.Groups["promoted"].Value == "成";
                        move.Piece = match.Groups["piece"].Value;
                        move.ActionString = match.Groups["action"].Value;

                        if (move.ActionString == "不成")
                        {
                            move.Action = Action.NoPromote;
                        }
                        else if (move.ActionString == "成")
                        {
                            move.Action = Action.Promote;
                        }
                        else if (move.ActionString == "打")
                        {
                            move.Action = Action.Drops;
                        }
                        if (!string.IsNullOrEmpty(match.Groups["srcPos"].Value))
                        {
                            move.SrcPosX = int.Parse(match.Groups["srcPosX"].Value);
                            move.SrcPosY = int.Parse(match.Groups["srcPosY"].Value);
                        }

                        list.Add(move);
                        bw = bw.Reverse();
                    }

                    match = playerRegex.Match(line);
                    if (match.Success)
                    {
                        var player = match.Groups["bw"].Value.Trim();
                        if (player == "先手")
                        {
                            record.BlackPlayer = match.Groups["name"].Value.Trim();
                        }
                        else
                        {
                            record.WhitePlayer = match.Groups["name"].Value.Trim();
                        }
                    }

                    match = resignRegex.Match(line);
                    if (match.Success)
                    {
                        var move = new Move(bw, int.Parse(match.Groups["movenum"].Value.Trim()));
                        move.ActionString = "投了";
                        list.Add(move);
                    }
                }
			}

            record.Moves = list;

            return record;
		}

		public void Move(Move move)
		{
			var destPosX = move.DestPosX;
			var destPosY = move.DestPosY;
            if(move.ActionString == "投了")
            {
                return;
            }
			if (move.Action == Action.Drops)
			{
				var piece = GetHands().First(x => x.GetType() == _PieceDictionary[move.Piece]);
				this[destPosX, destPosY] = piece;
				GetHands().Remove(piece);
			}
			else
			{
				if(move.Position == "同")
				{
					destPosX = _OldPosition.Item1;
					destPosY = _OldPosition.Item2;
				}

				var piece = this[destPosX, destPosY];

				if(piece != null)
				{
					// 駒取り
					piece.Reverse();
					piece.Promoted = false;
					GetHands().Add(piece);
				}

				this[destPosX, destPosY] = this[move.SrcPosX.Value, move.SrcPosY.Value];

				if (move.Action == Action.Promote)
				{
					// 成
					this[destPosX, destPosY].Promote();
				}

				this[move.SrcPosX.Value, move.SrcPosY.Value] = null;
			}

			_OldPosition = Tuple.Create(destPosX, destPosY);

			Next();
		}

		public void MakeAnimation(string filePath)
		{
			var images = new List<string>();
			var record = ReadFile(filePath);

			for(var i = 0; i < record.Moves.Count; i++)
			{
				Move(record.Moves[i]);
				images.Add(Paint(i, record));
			}
		}

		public IEnumerator<Piece> GetEnumerator()
		{
			for (var i = 0; i < 9; i++)
			{
				for (var j = 0; j < 9; j++)
				{
					yield return _Pieces[i, j];
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// 複数の画像をGIFアニメーションとして保存する
		/// </summary>
		/// <param name="savePath">保存先のファイルのパス</param>
		/// <param name="imageFiles">GIFに追加する画像ファイルのパス</param>
		public void CreateAnimatedGif(string savePath, IEnumerable<string> imageFiles)
		{
			//GifBitmapEncoderを作成する
			var encoder = new GifBitmapEncoder();

			foreach (var f in imageFiles)
			{
				//画像ファイルからBitmapFrameを作成する
				var bmpFrame =
					BitmapFrame.Create(new Uri(f, UriKind.RelativeOrAbsolute));
				//フレームに追加する
				encoder.Frames.Add(bmpFrame);
			}

            //書き込むファイルを開く
            using (var outputFileStrm = new FileStream(savePath,
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                //保存する
                encoder.Save(outputFileStrm);
            }
		}
	}
}

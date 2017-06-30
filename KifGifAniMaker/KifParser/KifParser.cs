using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KifGifAniMaker.KifParser
{
    public class KifParser : Parser
    {
        private Options options;

        public KifParser(Options options)
        {
            this.options = options;
        }

        public Record ReadFile()
        {
            var fileName = options.InputFile;
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
            using (var r = new StreamReader(fileName, System.Text.Encoding.GetEncoding("shift-jis")))
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
    }
}

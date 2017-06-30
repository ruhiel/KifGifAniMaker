using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KifGifAniMaker
{
    public class Options
    {
        //String型のオプション
        [Option('o', HelpText = "出力動画ファイルパス")]
        public string OutputFile
        {
            get;
            set;
        }

        //Boolean型のオプション
        [Option('i', Required = true, HelpText = "入力棋譜ファイルパス")]
        public string InputFile
        {
            get;
            set;
        }

        //Boolean型のオプション
        [Option("f", DefaultValue = "kif", HelpText = "棋譜ファイル形式(csa|kif|ki2)")]
        public string Format
        {
            get;
            set;
        }

        //(3)HelpOption属性
        [HelpOption]
        public string GetUsage()
        {
            //ヘッダーの設定
            var head = new HeadingInfo("ConsoleSample", "Version 1.0");
            var help = new HelpText(head);
            help.Copyright = new CopyrightInfo("Ruhiel", 2017);
            help.AddPreOptionsLine("KifGifAniMaker");

            //全オプションを表示(1行間隔)
            help.AdditionalNewLineAfterOption = true;
            help.AddOptions(this);

            return help.ToString();
        }
    }
}

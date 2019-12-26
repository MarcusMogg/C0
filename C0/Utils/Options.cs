using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace C0.Utils
{
    public class Options
    {
        [Option('s', Required = false, HelpText = "将输入的 c0 源代码翻译为文本汇编文件")]
        public bool Assembly { get; set; }

        [Option('c', Required = false, HelpText = "将输入的 c0 源代码翻译为二进制目标文件")]
        public bool Binary { get; set; }

        [Value(0, Required = true, HelpText = "源文件")]
        public string InputFile { get; set; }

        [Option('o', Default = @"out", Required = false, HelpText = "输出到指定的文件 file")]
        public String OutFile { get; set; }
    }
}

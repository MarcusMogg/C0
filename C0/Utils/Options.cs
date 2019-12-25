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

        [Option('h', Required = false, HelpText = "显示关于编译器使用的帮助")]
        public bool Help { get; set; }

        [Value(0, Default = "test.c0", Required = false, HelpText = "源文件")]
        public string InputFile { get; set; }

        [Option('o', Default = @"out", Required = false, HelpText = "输出到指定的文件 file")]
        public String OutFile { get; set; }
    }
}

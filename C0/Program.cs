using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using C0.Analyser;
using C0.Instruction;
using C0.Tokenizer;
using C0.Utils;
using CommandLine;

namespace C0
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
        }

        private static void Run(Options option)
        {
            Console.WriteLine(option.Assembly);
            Console.WriteLine(option.Binary);
            Console.WriteLine(option.InputFile);
            //using (StreamWriter w = new StreamWriter(option.OutFile))
            //using (FileStream stream = new FileStream(option.OutFile, FileMode.Create))

            try
            {
                Tokenizer.Tokenizer t = new Tokenizer.Tokenizer(option.InputFile);

                List<Token> tokens = new List<Token>();
                Token token;
                do
                {
                    token = t.NextToken();
                    tokens.Add(token);
                } while (token.Type != TokenType.Eof);

                TokenProvider.GetInstance().SetToken(tokens);

                C0Program c0Program = C0Program.Analyse();
                var res = c0Program.GetIns();
                if (option.Binary)
                {
                    using (FileStream stream = new FileStream(option.OutFile, FileMode.Create))
                    {
                        List<byte> bs = new List<byte>();

                        foreach (var i in res)
                        {
                            //w.WriteLine(i.ToNorString());
                            string tmp = i.ToHexString();
                            int len = tmp.Length;
                            for (int j = 0; j < len; j += 2)
                            {
                                bs.Add(byte.Parse(tmp.Substring(j, 2), NumberStyles.HexNumber));
                            }
                        }

                        byte[] bytes = bs.ToArray();
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                        stream.Close();
                    }
                }

                if (option.Assembly)
                {
                    using (StreamWriter w = new StreamWriter(option.OutFile))
                    {
                        foreach (var i in res)
                        {
                            w.WriteLine(i.ToNorString());
                        }
                    }
                }

            }
            catch (MyC0Exception e)
            {
                Console.WriteLine($"{e.ErrPos.X},{e.ErrPos.Y}");
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }



    }
}

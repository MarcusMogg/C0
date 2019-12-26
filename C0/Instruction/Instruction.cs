using System;
using System.Collections.Generic;
using System.Text;

namespace C0.Instruction
{
    public interface IInstruction
    {
        string ToHexString();
        string ToNorString();
    }

    public class Nop : IInstruction
    {
        public string ToHexString()
        {
            return "00";
        }

        public string ToNorString()
        {
            return "nop";
        }
    }

    public class BiPush : IInstruction
    {
        public byte Param1 { get; set; }

        public BiPush(byte param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"01{Param1:x2}";
        }

        public string ToNorString()
        {
            return $"bipush {Param1}";
        }
    }
    public class IPush : IInstruction
    {
        public int Param1 { get; set; }

        public IPush(int param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"02{Param1:x8}";
        }

        public string ToNorString()
        {
            return $"ipush {Param1}";
        }
    }
    public class Pop : IInstruction
    {
        public string ToHexString()
        {
            return "04";
        }

        public string ToNorString()
        {
            return "pop";
        }
    }
    public class Dup : IInstruction
    {
        public string ToHexString()
        {
            return "07";
        }

        public string ToNorString()
        {
            return "dup";
        }
    }

    public class LoadC : IInstruction
    {
        public ushort Param1 { get; set; }

        public LoadC(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"09{Param1:x4}";
        }

        public string ToNorString()
        {
            return $"loadc {Param1}";
        }

    }

    public class LoadA : IInstruction
    {
        // level_diff
        public ushort Param1 { get; set; }
        // offset
        public int Param2 { get; set; }

        public LoadA(ushort param1, int param2)
        {
            Param1 = param1;
            Param2 = param2;
        }

        public string ToHexString()
        {
            return $"0a{Param1:x4}{Param2:x8}";
        }

        public string ToNorString()
        {
            return $"loada {Param1},{Param2}";
        }

    }
    public class ILoad : IInstruction
    {
        public string ToHexString()
        {
            return "10";
        }

        public string ToNorString()
        {
            return "iload";
        }
    }
    public class Istore : IInstruction
    {
        public string ToHexString()
        {
            return "20";
        }

        public string ToNorString()
        {
            return "istore";
        }
    }

    public class IAdd : IInstruction
    {
        public string ToHexString()
        {
            return "30";
        }

        public string ToNorString()
        {
            return "iadd";
        }
    }
    public class ISub : IInstruction
    {
        public string ToHexString()
        {
            return "34";
        }

        public string ToNorString()
        {
            return "isub";
        }
    }
    public class IMul : IInstruction
    {
        public string ToHexString()
        {
            return "38";
        }

        public string ToNorString()
        {
            return "imul";
        }
    }
    public class IDiv : IInstruction
    {
        public string ToHexString()
        {
            return "3c";
        }

        public string ToNorString()
        {
            return "idiv";
        }
    }
    public class INeg : IInstruction
    {
        public string ToHexString()
        {
            return "40";
        }

        public string ToNorString()
        {
            return "ineg";
        }
    }
    public class ICmp : IInstruction
    {
        public string ToHexString()
        {
            return "44";
        }

        public string ToNorString()
        {
            return "icmp";
        }
    }
    public class Jmp : IInstruction
    {
        public ushort Param1 { get; set; }

        public Jmp(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"70{Param1:x4}";
        }

        public string ToNorString()
        {
            return $"jmp {Param1}";
        }

    }
    public class Je : IInstruction
    {
        public ushort Param1 { get; set; }

        public Je(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"71{Param1:x4}";
        }

        public string ToNorString()
        {
            return $"je {Param1}";
        }

    }
    public class Jne : IInstruction
    {
        public ushort Param1 { get; set; }

        public Jne(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"72{Param1:x4}";
        }

        public string ToNorString()
        {
            return $"jne {Param1}";
        }

    }
    public class Jl : IInstruction
    {
        public ushort Param1 { get; set; }

        public Jl(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"73{Param1:x4}";
        }

        public string ToNorString()
        {
            return $"jl {Param1}";
        }

    }
    public class Jge : IInstruction
    {
        public ushort Param1 { get; set; }

        public Jge(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"74{Param1:x4}";
        }

        public string ToNorString()
        {
            return $"jge {Param1}";
        }

    }
    public class Jg : IInstruction
    {
        public ushort Param1 { get; set; }

        public Jg(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"75{Param1:x4}";
        }

        public string ToNorString()
        {
            return $"jg {Param1}";
        }

    }
    public class Jle : IInstruction
    {
        public ushort Param1 { get; set; }

        public Jle(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"76{Param1:x4}";
        }

        public string ToNorString()
        {
            return $"jle {Param1}";
        }

    }
    public class Call : IInstruction
    {
        public ushort Param1 { get; set; }

        public Call(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"80{Param1:x4}";
        }

        public string ToNorString()
        {
            return $"call {Param1}";
        }

    }
    public class Ret : IInstruction
    {
        public string ToHexString()
        {
            return $"88";
        }

        public string ToNorString()
        {
            return $"ret";
        }

    }
    public class IRet : IInstruction
    {
        public string ToHexString()
        {
            return $"89";
        }

        public string ToNorString()
        {
            return $"iret";
        }

    }
    public class IPrint : IInstruction
    {
        public string ToHexString()
        {
            return $"a0";
        }

        public string ToNorString()
        {
            return $"iprint";
        }

    }
    public class CPrint : IInstruction
    {
        public string ToHexString()
        {
            return $"a2";
        }

        public string ToNorString()
        {
            return $"cprint";
        }

    }
    public class PrintL : IInstruction
    {
        public string ToHexString()
        {
            return $"af";
        }

        public string ToNorString()
        {
            return $"printl";
        }

    }
    public class IScan : IInstruction
    {
        public string ToHexString()
        {
            return $"b0";
        }

        public string ToNorString()
        {
            return $"iscan";
        }

    }
    public class CScan : IInstruction
    {
        public string ToHexString()
        {
            return $"b2";
        }

        public string ToNorString()
        {
            return $"cscan";
        }

    }
    public class I2C : IInstruction
    {
        public string ToHexString()
        {
            return $"62";
        }

        public string ToNorString()
        {
            return $"i2c";
        }

    }
    public class ConstantInfoString : IInstruction
    {
        public string Param1 { get; set; }

        public ConstantInfoString(string param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            string res = $"00{Param1.Length:x4}";
            foreach (var i in Param1)
            {
                res += $"{(ushort)i:x2}";
            }
            return res;
        }

        public string ToNorString()
        {
            return $"S {Param1}";
        }
    }
    public class ConstantInfoInt : IInstruction
    {
        public int Param1 { get; set; }

        public ConstantInfoInt(int param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"01{Param1:x8}";
        }

        public string ToNorString()
        {
            return $"I {Param1}";
        }
    }
    public class Magic : IInstruction
    {
        public string ToHexString()
        {
            return $"43303a29";
        }

        public string ToNorString()
        {
            return $"#magic";
        }

    }
    public class Version : IInstruction
    {
        public string ToHexString()
        {
            return $"00000001";
        }

        public string ToNorString()
        {
            return $"#version";
        }

    }

    public class DotConstant : IInstruction
    {
        public ushort Param1 { get; set; }

        public DotConstant(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"{Param1:x4}";
        }

        public string ToNorString()
        {
            return $".constants:";
        }
    }
    public class DotStart : IInstruction
    {
        public ushort Param1 { get; set; }

        public DotStart(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"{Param1:x4}";
        }

        public string ToNorString()
        {
            return $".start:";
        }
    }
    public class FunctionInfo : IInstruction
    {
        //name_index
        public ushort Param1 { get; set; }
        //params_length
        public ushort Param2 { get; set; }
        //level
        public ushort Param3 { get; set; }
        //instructions_count
        public ushort Param4 { get; set; }

        public FunctionInfo(ushort param1, ushort param2, ushort param3, ushort param4)
        {
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
            Param4 = param4;
        }

        public string ToHexString()
        {
            return $"{Param1:x4}{Param2:x4}{Param3:x4}{Param4:x4}";
        }

        public string ToNorString()
        {
            return $".F{Param1}:\n {Param1} {Param2} {Param3} {Param4}";
        }
    }
    public class DotFunction : IInstruction
    {
        public ushort Param1 { get; set; }

        public DotFunction(ushort param1)
        {
            Param1 = param1;
        }

        public string ToHexString()
        {
            return $"{Param1:x4}";
        }

        public string ToNorString()
        {
            return $".functions:";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using C0.Instruction;

namespace C0.Analyser.Statement
{
    public class EmptyStatement
    {
        public List<IInstruction> GetIns(string par, int offset)
        {
            return new List<IInstruction>() { new Nop() };
        }
    }
}

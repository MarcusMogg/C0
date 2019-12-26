using System;
using System.Collections.Generic;
using System.Text;

namespace C0.Tokenizer
{
    public enum DfaState
    {
        Initial,
        // identifier                
        Identifier,

        // literal                   
        LiteralDecimal,              // 123
        LiteralHexadecimal,          // 0xa

        // operator                  
        OperatorAdd,                 // +
        OperatorMinus,               // -
        OperatorMultiply,            // *
        OperatorDivision,            // /

        OperatorLess,                // <
        OperatorLessEqual,           // <=
        OperatorEqual,               // ==
        OperatorGreaterEqual,        // >=
        OperatorGreater,             // >
        OperatorNotEqual,            // !=                          
        OperatorAssignment,          // =
        // brackets                  
        BracketsLeftRound,           // (
        BracketsRightRound,          // )
        BracketsLeftCurly,           // {
        BracketsRightCurly,          // }
        // semicolon                 
        Semicolon,                   // ;
        // comma
        Comma,                       // ,

        // comment
        SingleLineComment,           // //
        MultiLineCommentLeft,        // /*
        MultiLineCommentRight,       // */

        // char
        Char,
        String,
    }
}

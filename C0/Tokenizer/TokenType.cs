namespace C0.Tokenizer
{
    public enum TokenType
    {
        Eof,
        // keywords
        Const,                       
        Void,                        
        Int,                         
        Char,                        
        Double,                      
        Struct,                      
        If,                          
        Else,                        
        Switch,                      
        Case,                        
        Default,                     
        While,                       
        For,                         
        Do,                          
        Return,                      
        Break,                       
        Continue,                    
        Print,                       
        Scan,                        
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
        OperatorAssignment,          // ==
        // brackets                  
        BracketsLeftRound,           // (
        BracketsRightRound,          // )
        BracketsLeftCurly,           // {
        BracketsRightCurly,          // }
        // semicolon                 
        Semicolon,                   // ;
        // comma
        Comma,                       // ,

    }
}

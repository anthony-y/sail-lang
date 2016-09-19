using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sail.Lexical
{
    internal enum TokenType
    {
        FN,
        IDENTIFIER,
        INT,
        STR,
        FLOAT,
        STRUCT,
        ASSIGNMENT,
        ASSIGNINFER,
        EQUALTO,
        NOTEQUALTO,
        LESSTHAN,
        GREATERTHAN,
        PLUS,
        MINUS,
        MODULO,
        ASTERISK,
        FSLASH,
        INTLITERAL,
        STRLITERAL,
        FLOATLITERAL,
        OPAREN,
        CPAREN,
        OBRACKET,
        CBRACKET,
        OBRACE,
        CBRACE,
        HAT,
        AMPERSAN,
        COLON,
        SEMICOLON,
        RIGHTARROW,
        FETCH,
        HASH,
        VOID,
        NULL,
        FOR,
        TO,
        DOT,
        COMMA,
        AS,
        COMMENT,
        DEFER,
        EXCLAMATION,
        CHAR,
        BOOL,
        BOOLLITERAL,
        STATIC,
        STATICDECL,
        PRINT,
        PUTS,
        RETURN,
        IF,
        ELSEIF,
        ELSE,
        TYPE_OF
    }
}

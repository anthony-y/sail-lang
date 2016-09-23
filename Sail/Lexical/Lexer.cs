using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Sail.Lexical
{
    internal class Lexer
    {
        public TokenStream TokenStream { get; private set; }

        private StreamReader _reader;

        private int _line;
        private int _column;

        internal string PathPrefix { get; private set; }

        private Dictionary<char, TokenType> _symbols = new Dictionary<char, TokenType>()
        {
            { ';', TokenType.SEMICOLON },  { '(', TokenType.OPAREN       },
            { ')', TokenType.CPAREN     }, { '[', TokenType.OBRACKET     },
            { ']', TokenType.CBRACKET   }, { '{', TokenType.OBRACE       },
            { '}', TokenType.CBRACE     }, { '+', TokenType.PLUS         },
            { '^', TokenType.HAT        }, { '#', TokenType.HASH         },
            { '&', TokenType.AMPERSAN   }, { ',', TokenType.COMMA        },
            { '*', TokenType.ASTERISK   }, { '%', TokenType.MODULO       }
        };

        private Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>()
        {
            { "fn",     TokenType.FN     }, { "int",    TokenType.INT    },
            { "str",    TokenType.STR    }, { "float",  TokenType.FLOAT  },
            { "struct", TokenType.STRUCT }, { "fetch",  TokenType.FETCH  },
            { "void",   TokenType.VOID   }, { "null",   TokenType.NULL   },
            { "for",    TokenType.FOR    }, { "to",     TokenType.TO     },
            { "as",     TokenType.AS     }, { "defer",  TokenType.DEFER  },
            { "bool",   TokenType.BOOL   }, { "static", TokenType.STATIC },
            { "print",  TokenType.PRINT  }, { "puts",   TokenType.PUTS   },
            { "return", TokenType.RETURN }, { "if",     TokenType.IF     },
            { "elseif", TokenType.ELSEIF }, { "else",   TokenType.ELSE   },
            { "type_of", TokenType.TYPE_OF }
        };

        internal Lexer(string source)
        {
            TokenStream = new TokenStream();

            _reader = new StreamReader(source);

            int lastSlash = source.Replace('\\', '/').LastIndexOf('/');
            PathPrefix = source.Substring(0, lastSlash + 1);

            _line = 1;
            _column = 1;
        }

        private void CreateToken(TokenType type, string value) => TokenStream.Write(new Token(type, value, _line, _column));

        private char Read()
        {
            if ((char)_reader.Peek() == '\n')
            {
                _column = 1;
                ++_line;
            } else _column++;

            return (char)_reader.Read();
        }

        private string Skip(Predicate<char> predicate)
        {
            var str = new StringBuilder();

            while (!_reader.EndOfStream && predicate((char)_reader.Peek()))
                str.Append(Read());

            return str.ToString();
        }

        private bool IsSymbol(char c) => _symbols.ContainsKey(c);
        private bool IsKeyword(string s) => _keywords.ContainsKey(s);

        public void LexFile()
        {
            while (!_reader.EndOfStream)
            {
                Skip(char.IsWhiteSpace);

                char c = (char)_reader.Peek();

                if (char.IsLetter(c) || c == '_')
                {
                    string id = Skip(ch => char.IsLetterOrDigit(ch) || ch == '_');

                    if (IsKeyword(id))
                        CreateToken(_keywords[id], id);
                    else if (id == "true" || id == "false") CreateToken(TokenType.BOOLLITERAL, id);
                    else CreateToken(TokenType.IDENTIFIER, id);
                }

                if (IsSymbol(c))
                {
                    CreateToken(_symbols[c], c.ToString());
                    Read();
                }

                if (char.IsNumber(c))
                {
                    string num = Skip(ch => char.IsNumber(ch) || ch == '.' || ch == '_');

                    // Remove underscores
                    for (int i = 0; i < num.Length; i++)
                        if (num[i] == '_') num = num.Remove(i, 1);

                    if (num.Contains(".")) CreateToken(TokenType.FLOATLITERAL, num);
                    else CreateToken(TokenType.INTLITERAL, num);
                }

                switch (c)
                {
                    case '>':
                        Read();

                        if ((char)_reader.Peek() == '=')
                            CreateToken(TokenType.GTHANEQUAL, ">=");

                        else CreateToken(TokenType.GREATERTHAN, ">");

                        Read();

                        break;

                    case '<':
                        Read();

                        if ((char)_reader.Peek() == '=')
                            CreateToken(TokenType.LTHANEQUAL, "<=");

                        else CreateToken(TokenType.LESSTHAN, "<");

                        Read();

                         break;

                    case ':':
                        Read();

                        if ((char)_reader.Peek() == '=')
                            CreateToken(TokenType.ASSIGNINFER, ":=");
                        else if ((char)_reader.Peek() == ':')
                            CreateToken(TokenType.STATICDECL, "::");
                        else CreateToken(TokenType.COLON, c.ToString());

                        Read();

                        break;

                    case '=':
                        Read();

                        if ((char)_reader.Peek() == '=')
                            CreateToken(TokenType.EQUALTO, "==");
                        else CreateToken(TokenType.ASSIGNMENT, c.ToString());

                        Read();
                        break;

                    case '\'':

                        Read();
                        CreateToken(TokenType.CHAR, ((char)_reader.Read()).ToString());

                        break;

                    case '!':
                        Read();

                        if ((char)_reader.Peek() == '=')
                            CreateToken(TokenType.NOTEQUALTO, "!=");
                        else CreateToken(TokenType.EXCLAMATION, c.ToString());

                        Read();
                        break;

                    case '-':
                        Read();

                        if ((char)_reader.Peek() == '>')
                            CreateToken(TokenType.RIGHTARROW, "->");

                        else CreateToken(TokenType.MINUS, c.ToString());

                        Read();
                        break;

                    case '/':
                        // Comments are actually ignored but later I might add meta data using them
                        if ((char)_reader.Peek() == '/')
                        {
                            // Read past both forward slashses
                            Read();
                            Read();

                            string comment = Skip(ch => ch != '\n').Trim();
                        } else CreateToken(TokenType.FSLASH, c.ToString());

                        Read();
                        break;

                    case '"':
                        Read();

                        string strlit = Skip(ch => ch != '"');
                        strlit = strlit.Replace("\\n", "\n").Replace("\\t", "\t");

                        Read();

                        CreateToken(TokenType.STRLITERAL, strlit);
                        break;

                    case '.':
                        Read();

                        if ((char)_reader.Peek() == '.')
                            CreateToken(TokenType.TO, "..");
                        else CreateToken(TokenType.DOT, ".");

                        Read();
                        break;
                }
            }

            _reader.Close();
        }
    }
}

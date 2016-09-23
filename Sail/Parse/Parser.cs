using System;
using System.Collections.Generic;

using Sail.Lexical;
using Sail.Parse.Parsers;

namespace Sail.Parse
{
    internal class Parser
    {
        public TokenStream TokenStream { get; private set; }

        private Dictionary<TokenType, IPrefixParser> _prefixParsers;
        private Dictionary<TokenType, IInfixParser> _infixParsers;

        internal Parser(Lexer lexer)
        {
            lexer.LexFile();
            TokenStream = lexer.TokenStream;

            _prefixParsers = new Dictionary<TokenType, IPrefixParser>();
            _infixParsers = new Dictionary<TokenType, IInfixParser>();

            /* Using a dirty hack right now for reassignments of uninitialised variables :( */
            //RegisterInfix(TokenType.ASSIGNMENT, new VarAssignParser());

            RegisterPrefix(TokenType.IDENTIFIER, new IdentifierParser());

            RegisterPrefix(TokenType.STRLITERAL, new LiteralParser());
            RegisterPrefix(TokenType.INTLITERAL, new LiteralParser());
            RegisterPrefix(TokenType.FLOATLITERAL, new LiteralParser());
            RegisterPrefix(TokenType.BOOLLITERAL, new LiteralParser());

            RegisterInfix(TokenType.STRLITERAL, new LiteralParser());
            RegisterInfix(TokenType.INTLITERAL, new LiteralParser());
            RegisterInfix(TokenType.FLOATLITERAL, new LiteralParser());
            RegisterInfix(TokenType.BOOLLITERAL, new LiteralParser());

            RegisterPrefix(TokenType.STR, new TypeNameParser());
            RegisterPrefix(TokenType.INT, new TypeNameParser());
            RegisterPrefix(TokenType.FLOAT, new TypeNameParser());
            RegisterPrefix(TokenType.BOOL, new TypeNameParser());
            RegisterPrefix(TokenType.VOID, new TypeNameParser());

            RegisterInfix(TokenType.STR, new TypeNameParser());
            RegisterInfix(TokenType.INT, new TypeNameParser());
            RegisterInfix(TokenType.FLOAT, new TypeNameParser());
            RegisterInfix(TokenType.BOOL, new TypeNameParser());
            RegisterInfix(TokenType.VOID, new TypeNameParser());

            RegisterInfix(TokenType.ASSIGNINFER, new AssignmentInferParser());
            RegisterInfix(TokenType.COLON, new AssignmentExplicitParser());

            RegisterInfix(TokenType.OPAREN, new FunctionCallParser());

            RegisterPrefix(TokenType.STATICDECL, new FunctionDeclarationParser());
            RegisterInfix(TokenType.STATICDECL, new FunctionDeclarationParser());

            RegisterPrefix(TokenType.OBRACE, new BlockParser());
            RegisterInfix(TokenType.OBRACE, new BlockParser());

            RegisterPrefix(TokenType.PRINT, new PrintParser());
            RegisterPrefix(TokenType.PUTS, new PutsParser());

            RegisterPrefix(TokenType.RETURN, new ReturnParser());

            RegisterInfix(TokenType.PLUS, new MathsParser());
            RegisterInfix(TokenType.MINUS, new MathsParser());
            RegisterInfix(TokenType.ASTERISK, new MathsParser());
            RegisterInfix(TokenType.FSLASH, new MathsParser());

            RegisterPrefix(TokenType.IF, new IfParser());
            RegisterPrefix(TokenType.ELSE, new ElseParser());

            RegisterPrefix(TokenType.ELSEIF, new ElseIfParser());

            RegisterPrefix(TokenType.FOR, new ForParser());
            RegisterInfix(TokenType.TO, new IteratorParser());

            RegisterPrefix(TokenType.TYPE_OF, new TypeOfParser());

            RegisterPrefix(TokenType.FETCH, new FetchParser());

            RegisterInfix(TokenType.EQUALTO, new ComparisonParser());
            RegisterInfix(TokenType.NOTEQUALTO, new ComparisonParser());
            RegisterInfix(TokenType.LESSTHAN, new ComparisonParser());
            RegisterInfix(TokenType.GREATERTHAN, new ComparisonParser());
            RegisterInfix(TokenType.LTHANEQUAL, new ComparisonParser());
            RegisterInfix(TokenType.GTHANEQUAL, new ComparisonParser());
        }

        public Token Expect(TokenType type)
        {
            if (TokenStream.Peek().Type != type)
                throw new Exception($"[Line {TokenStream.Current.Line} Column {TokenStream.Current.Column}] Expected {type.ToString().ToLower()}!");

            return TokenStream.Read();
        }

        private bool IsType(Token token) => token.Type == TokenType.VOID || token.Type == TokenType.INT || token.Type == TokenType.FLOAT || token.Type == TokenType.STR;

        private void RegisterPrefix(TokenType type, IPrefixParser parser) => _prefixParsers.Add(type, parser);
        private void RegisterInfix(TokenType type, IInfixParser parser) => _infixParsers.Add(type, parser);

        private IPrefixParser GetPrefix(TokenType type)
        {
            IPrefixParser prefix;
            _prefixParsers.TryGetValue(type, out prefix);

            return prefix;
        }

        private IInfixParser GetInfix(TokenType type)
        {
            IInfixParser infix;
            _infixParsers.TryGetValue(type, out infix);

            return infix;
        }

        private int GetPrecedence()
        {
            try
            {
                var parser = GetInfix(TokenStream.Peek().Type);
                if (parser != null) return parser.GetPrecedence();
            } catch { }

            return 0;
        }

        public IExpression ParseExpression(int precedence)
        {
            var token = TokenStream.Read();

            var prefix = GetPrefix(token.Type);
            if (prefix == null)
                 throw new Exception($"[Sail] Syntax error on line {TokenStream.Current.Line}, column {TokenStream.Current.Column}!");

            var left = prefix.Parse(this, token);

            while (precedence < GetPrecedence())
            {
                token = TokenStream.Read();

                if (token == null) return left;

                var infix = GetInfix(token.Type);
                left = infix.Parse(this, token, left);
            }

            return left;
        }

        public List<IExpression> Parse()
        {
            var exprs = new List<IExpression>();

            TokenStream.StartStream();

            while (TokenStream.Peek(0) != null)
                exprs.Add(ParseExpression(0));

            return exprs;
        }
    }
}

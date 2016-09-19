using System;
using System.Collections.Generic;
using System.Linq;

namespace Sail.Lexical
{
    internal class TokenStream
    {
        public List<Token> Tokens { get; private set; }
        public Token Current { get; private set; }

        internal int _position;

        internal TokenStream()
        {
            Tokens = new List<Token>();

            _position = 0;
        }

        public void Write(Token token)
        {
            Tokens.Add(token);
            Current = Tokens[_position++];
        }

        public void StartStream()
        {
            _position = 0;
            Current = Tokens[_position];
        }

        public Token Read()
        {
            if (_position >= Tokens.Count)
                Current = null;
            else Current = Tokens[_position++];

            return Current;
        }

        public Token Peek(int offset = 0)
        {
            if (_position + offset >= Tokens.Count)
            {
                Current = null;
                return null;
            }

            return Tokens[_position + offset];
        }

        public List<Token> Skip(Predicate<Token> pred)
        {
            var list = new List<Token>();

            while (Current != null && pred(Peek()))
                list.Add(Read());

            return list;
        }

        public bool PeekAll(params TokenType[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                if (Peek(i).Type != tokens[i]) return false;
            }

            return true;
        }

        public bool PeekAny(params TokenType[] types)
        {
            foreach (var t in types)
                if (PeekAll(t)) return true;

            return false;
        }

        public bool MatchAll(params TokenType[] types)
        {
            if (!PeekAll(types)) return false;

            for (int i = 0; i < types.Length; i++)
                Read();

            return true;
        }

        public bool MatchAny(params TokenType[] types)
        {
            foreach (var t in types)
                if (MatchAll(t)) return true;

            return false;
        }
    }
}

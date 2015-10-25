//
// Parser.cs
//
// Author:
//       Danilo Lekovic <danilo@lekovic.ca)
//
// Copyright (c) 2015 Danilo Lekovic
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

namespace Ferum
{
	public partial class Parser
	{
		public Parser(Lexer lexer)
		{
			this.pos = 0;
			this.lexer = lexer;
			this.line = 1;
			this.totalTokens = lexer.tokens.Count;
		}

		public int pos { get; set; }

		public Lexer lexer { get; set; }

		public int line { get; set; }

		public int totalTokens { get; set; }

		public Token peekToken()
		{
			return lexer.peekToken();
		}

		public bool checkToken(TokenType t)
		{
			return lexer.peekToken().type == t;
		}

		public bool checkTokens(List<TokenType> tokens)
		{
			foreach (TokenType tt in tokens) {
				if (checkToken(tt))
					return true;
			}

			return false;
		}

		public Token peekSpecific(int i)
		{
			return lexer.peekSpecific(i);
		}

		public Token nextToken()
		{
			pos++;

			if (checkToken(TokenType.NEWLINE)) {
				this.line++;
			}

			return this.lexer.nextToken();
		}

		public void skipNewline()
		{
			try {
				if (this.checkToken(TokenType.NEWLINE)) {
					this.nextToken();
				}
			} catch (Exception e) {
			}
		}

		public Expression parse()
		{
			Token tok = peekToken();

			switch (tok.type) {
				case TokenType.VAR:
					return parseVar();
				case TokenType.NEWLINE:
					return parseNewline();
				case TokenType.IDENTIFIER:
					return parseStatement();
				default:
					throw new Exception("Unknown statement/expression found! " + tok.type);
			}
		}

		public Literal parseLiteral()
		{
			Token tok = peekToken();

			switch (tok.type) {
				case TokenType.IDENTIFIER:
					return parseIdentifier();
				case TokenType.BOOL:
					return parseBool();
				case TokenType.STRING:
					return parseString();
				case TokenType.NUMBER:
					return parseNumber();
				default:
					throw new Exception("Unknown literal found! " + tok.type);
			}
		}

		public List<Expression> start()
		{
			List<Expression> exprs = new List<Expression>();

			while (pos < totalTokens) {
				exprs.Add(parse());
			}

			return exprs;
		}
	}
}


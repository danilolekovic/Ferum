//
// Lexer.cs
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
	public class Lexer
	{
		public Lexer(string code)
		{
			this.code = code;
			this.index = -1;
			this.tokens = new List<Token>();
		}

		public string code { get; set; }

		public int index { get; set; }

		public List<Token> tokens { get; set; }

		public Token peekToken()
		{
			return this.tokens[this.index + 1];
		}

		public Token peekSpecific(int i)
		{
			return this.tokens[this.index + i];
		}

		public Token nextToken()
		{
			this.index++;
			return this.tokens[this.index];
		}

		public bool isLetter(char c)
		{
			return Char.IsLetter(c);
		}

		public bool isNumber(char c)
		{
			return Char.IsNumber(c);
		}

		public bool isLetterOrNumber(char c)
		{
			return isLetter(c) || isNumber(c);
		}

		public bool isSymbol(char c)
		{
			switch (c) {
				case '{':
				case '}':
				case '+':
				case '-':
				case '*':
				case '/':
				case '=':
				case '(':
				case ')':
				case ',':
				case '.':
				case '[':
				case ']':
				case '#':
					return true;
				default:
					return false;
			}
		}

		public bool isQuote(char c)
		{
			return c == '"' || c == '\'';
		}

		public bool isNewline(char c)
		{
			return c == '\n';
		}

		public bool isWhitespace(char c)
		{
			return Char.IsWhiteSpace(c);
		}

		public void tokenize()
		{
			int pos = 0;

			while (pos < code.Length) {
				if (isLetter(code[pos])) {
					string str = "";

					while (pos != code.Length && isLetterOrNumber(code[pos])) {
						str += code[pos].ToString();
						pos++;
					}

					if (str == "var") {
						tokens.Add(new Token(TokenType.VAR, str));
					} else if (str == "true" || str == "false") {
						tokens.Add(new Token(TokenType.BOOL, str));
					} else {
						tokens.Add(new Token(TokenType.IDENTIFIER, str));
					}
				} else if (isNumber(code[pos])) {
					string str = "";

					while (pos != code.Length && isNumber(code[pos])) {
						str += code[pos].ToString();
						pos++;
					}

					tokens.Add(new Token(TokenType.NUMBER, str));
				} else if (isQuote(code[pos])) {
					string str = "";
					pos++;

					while (pos != code.Length && !isQuote(code[pos])) {
						str += code[pos];
						pos++;
					}

					pos++;

					tokens.Add(new Token(TokenType.STRING, str));
				} else if (isNewline(code[pos])) {
					pos++;
					tokens.Add(new Token(TokenType.NEWLINE, "\n"));
				} else if (isWhitespace(code[pos])) {
					while (pos != code.Length && isWhitespace(code[pos])) {
						pos++;
					}
				} else {
					switch (code[pos]) {
						case '{':
							tokens.Add(new Token(TokenType.LBRACE, code[pos].ToString()));
							pos++;
							break;
						case '}':
							tokens.Add(new Token(TokenType.RBRACE, code[pos].ToString()));
							pos++;
							break;
						case '+':
							tokens.Add(new Token(TokenType.ADD, code[pos].ToString()));
							pos++;
							break;
						case '-':
							tokens.Add(new Token(TokenType.SUB, code[pos].ToString()));
							pos++;
							break;
						case '*':
							tokens.Add(new Token(TokenType.MUL, code[pos].ToString()));
							pos++;
							break;
						case '/':
							tokens.Add(new Token(TokenType.DIV, code[pos].ToString()));
							pos++;
							break;
						case '=':
							tokens.Add(new Token(TokenType.EQUALS, code[pos].ToString()));
							pos++;
							break;
						case '(':
							tokens.Add(new Token(TokenType.LPAREN, code[pos].ToString()));
							pos++;
							break;
						case ')':
							tokens.Add(new Token(TokenType.RPAREN, code[pos].ToString()));
							pos++;
							break;
						case ',':
							tokens.Add(new Token(TokenType.COMMA, code[pos].ToString()));
							pos++;
							break;
						case '.':
							tokens.Add(new Token(TokenType.DOT, code[pos].ToString()));
							pos++;
							break;
						case '[':
							tokens.Add(new Token(TokenType.LBRACK, code[pos].ToString()));
							pos++;
							break;
						case ']':
							tokens.Add(new Token(TokenType.RBRACK, code[pos].ToString()));
							pos++;
							break;
						case '#':
							pos++;

							while (code[pos] != '#') {
								pos++;
							}

							pos++;
							break;
						default:
							throw new Exception("Unknown symbol found: " + code[pos]);
					}
				}
			}
		}
	}
}
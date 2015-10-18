//
// StatementParser.cs
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
using System.Reflection.Emit;

namespace Ferum
{
	public partial class Parser
	{
		public VarDeclaration parseVar()
		{
			nextToken();
			skipNewline();

			if (checkToken(TokenType.IDENTIFIER)) {
				string name = nextToken().value;
				skipNewline();

				if (checkToken(TokenType.EQUALS)) {
					nextToken();
					skipNewline();
					var value = parseLiteral();

					if (checkToken(TokenType.NEWLINE)) {
						skipNewline();
					} else {
						throw new Exception("Expected a newline at the end of variable declaration.");
					}

					if (value is Literal) {
						return new VarDeclaration(name, (Literal)value);
					} else {
						throw new Exception("Invalid value for variable declaration.");
					}
				} else {
					throw new Exception("Expected an equals sign in variable declaration.");
				}
			} else {
				throw new Exception("Expected a name in variable declaration.");
			}
		}

		public Expression parseStatement()
		{
			var identifier = this.nextToken().value;

			if (identifier == "print") {
				this.skipNewline();
				var toPrint = this.parseLiteral();

				if (this.checkToken(TokenType.NEWLINE)) {
					this.nextToken();
					this.skipNewline();
				} else {
					throw new Exception("Expected a newline");
				}

				return new Print(toPrint);
			} else {
				throw new Exception("Unknown statement: " + identifier);
			}
		}

		public Newline parseNewline()
		{
			this.nextToken();
			return new Newline();
		}
	}
}


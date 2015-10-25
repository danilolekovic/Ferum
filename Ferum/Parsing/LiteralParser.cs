//
// LiteralParser.cs
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
		public Literal parseNumber()
		{
			var number = int.Parse(this.nextToken().value);
			string op = null;
			BinaryOp right = new BinaryOp(new Number(number), "+", new Number(0));

			while (this.checkTokens(new List<TokenType>() { TokenType.ADD, TokenType.SUB, TokenType.DIV, TokenType.MUL })) {
				op = this.nextToken().value;
				this.skipNewline();
				right = new BinaryOp(right, op, new Number(int.Parse(this.nextToken().value)));
			}

			if (op == null) {
				return new Number(number);
			} else {
				return right;
			}
		}

		public Literal parseIdentifier()
		{
			var identifier = this.nextToken().value;
			return new Identifier(identifier);
		}

		public Literal parseString()
		{
			var str = this.nextToken().value;
			return new Str(str);
		}

		public Literal parseBool()
		{
			var str = this.nextToken().value;

			if (str == "true") {
				return new Bool(true);
			} else {
				return new Bool(false);
			}
		}
	}
}


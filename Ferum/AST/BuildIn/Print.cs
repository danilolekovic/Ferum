//
// Print.cs
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
	public class Print : Expression
	{
		public Print(Literal toPrint)
		{
			this.toPrint = toPrint;
		}

		public Literal toPrint { get; set; }

		public override void codeGen(ILGenerator generator)
		{
			if (toPrint is Identifier) {
				bool emitted = false;

				foreach (Variable v in VariableStack.variables) {	
					if (v.name.Trim() == toPrint.val().ToString().Trim()) {
						generator.EmitWriteLine(v.lb);
						emitted = true;
						break;
					}
				}

				if (!emitted) {
					throw new Exception("Undefined variable : " + toPrint.val());
				}
			} else {
				if (toPrint is Number || toPrint is Str || toPrint is Bool) {
					generator.EmitWriteLine(toPrint.val().ToString());
				} else {
					throw new Exception("Illegal argument in 'print' function! typeof: " + toPrint.GetType());				
				}
			}
		}
	}
}


//
// Write.cs
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
	public class Write : Expression
	{
		public Write(Literal toWrite)
		{
			this.toWrite = toWrite;
		}

		public Literal toWrite { get; set; }

		public override void codeGen(ILGenerator generator)
		{
			if (toWrite is Identifier) {
				bool emitted = false;

				foreach (Variable v in VariableStack.variables) {	
					if (v.name.Trim() == toWrite.val().ToString().Trim()) {
						if (v.value.type() == LiteralType.IDENT || v.value.type() == LiteralType.STRING) {

							v.value.visit(generator);
							generator.Emit(OpCodes.Call, typeof(Console).GetMethod("Write", new Type[] { typeof(string) }));

							emitted = true;
							break;
						} else {
							throw new Exception("The function 'write' expects an argument that is a string.");	
						}
					}
				}

				if (!emitted) {
					throw new Exception("Undefined variable : " + toWrite.val());
				}
			} else {
				if (toWrite is Str) {
					toWrite.visit(generator);
					generator.Emit(OpCodes.Call, typeof(Console).GetMethod("Write", new Type[] { typeof(Type) }));
				} else {
					throw new Exception("The function 'write' expects an argument that is a string; instead of a string, " + toWrite.GetType() + " was found.");				
				}
			}
		}
	}
}


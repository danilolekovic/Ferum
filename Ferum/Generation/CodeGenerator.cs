//
// CodeGenerator.cs
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
using System.Collections.Generic;

namespace Ferum
{
	public class CodeGenerator
	{
		public CodeGenerator(List<Expression> exprs)
		{
			this.exprs = exprs;
			this.dm = new DynamicMethod("Ferum", null, null);
			this.il = this.dm.GetILGenerator();
		}

		public List<Expression> exprs { get; set; }

		public DynamicMethod dm { get; set; }

		public ILGenerator il { get; set; }

		public void generate()
		{
			foreach (var e in exprs) {
				e.codeGen(il);

				if (e is VarDeclaration) {
					VarDeclaration id = (VarDeclaration)e;
					VariableStack.variables.Add(new Variable(id.name, id.variable, id.value));
				}
			}

			il.Emit(OpCodes.Ret);
			dm.Invoke(null, null);
		}
	}
}


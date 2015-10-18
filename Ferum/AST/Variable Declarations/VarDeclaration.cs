//
// VarDeclaration.cs
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
	public class VarDeclaration : Expression
	{
		public VarDeclaration(string name, Literal value)
		{
			this.name = name;
			this.value = value;
			this.variable = null;
		}

		public string name { get; set; }

		public Literal value { get; set; }

		public LocalBuilder variable { get; set; }

		public override void codeGen(ILGenerator generator)
		{
			if (value is Number) {
				LocalBuilder a = generator.DeclareLocal(typeof(Int32));
				generator.Emit(OpCodes.Ldc_I4, (int)value.val());
				generator.Emit(OpCodes.Stloc, a);
				variable = a;
			} else if (value is Str) {
				LocalBuilder a = generator.DeclareLocal(typeof(String));
				generator.Emit(OpCodes.Ldstr, value.val().ToString());
				generator.Emit(OpCodes.Stloc, a);
				variable = a;
			} else if (value is Bool) {
				LocalBuilder a = generator.DeclareLocal(typeof(Boolean));

				if ((bool)value.val() == true) {
					generator.Emit(OpCodes.Brtrue);
				} else {
					generator.Emit(OpCodes.Brfalse);
				}

				generator.Emit(OpCodes.Stloc, a);
				variable = a;
			} else if (value is Identifier) {
				foreach (Variable v in VariableStack.variables) {
					if ((string)value.val() == v.name) {
						if (v.value is Number) {
							LocalBuilder a = v.lb;
							generator.Emit(OpCodes.Ldc_I4, (int)v.value.val());
							generator.Emit(OpCodes.Stloc, a);
						} else if (v.value is Str) {
							LocalBuilder a = v.lb;
							generator.Emit(OpCodes.Ldstr, (string)v.value.val());
							generator.Emit(OpCodes.Stloc, a);
						} else if (v.value is Bool) {
							LocalBuilder a = v.lb;

							if ((bool)v.value.val() == true) {
								generator.Emit(OpCodes.Brtrue);
							} else {
								generator.Emit(OpCodes.Brfalse);
							}

							generator.Emit(OpCodes.Stloc, a);
						} else if (v.value is Identifier) {
							// todo: fix this
						}
					}
				}
			}
		}
	}
}


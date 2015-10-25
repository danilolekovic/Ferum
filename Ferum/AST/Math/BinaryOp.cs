//
// BinaryOp.cs
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
using System.Data;

namespace Ferum
{
    public class BinaryOp : Literal
    {
        public BinaryOp(Literal left, string op, Literal right)
        {
			this.left = (int)left.val();
			this.op = op;
			this.right = (int)right.val();
        }

		public int left { get; set; }
		public string op { get; set; }
		public int right { get; set; }

		public override object val()
		{
			return Evaluate(left + op + right);
		}

		public int Evaluate(string expression) {
			using (var loDataTable = new DataTable()) {
				var loDataColumn = new DataColumn("Eval", typeof(double), expression);
				loDataTable.Columns.Add(loDataColumn);
				loDataTable.Rows.Add(0);
				return (int)((double)loDataTable.Rows[0]["Eval"]);
			}
		}

		public override LiteralType type()
		{
			return LiteralType.INT;
		}

		public override void codeGen(ILGenerator generator)
		{
		}

		public override void visit(ILGenerator ilg)
		{
			ilg.Emit(OpCodes.Ldc_I4, left);

			switch (op) {
				case "+":
					ilg.Emit(OpCodes.Add);
					break;
				case "-":
					ilg.Emit(OpCodes.Sub);
					break;
				case "*":
					ilg.Emit(OpCodes.Mul);
					break;
				case "/":
					ilg.Emit(OpCodes.Div);
					break;
				default:
					throw new Exception("Unknown mathematical operator: " + op);
			}

			ilg.Emit(OpCodes.Ldc_I4, right);
		}
    }
}


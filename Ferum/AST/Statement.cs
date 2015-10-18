using System;
using System.Reflection.Emit;

namespace Ferum
{
	public abstract class Statement
	{
		public abstract void codeGen(ILGenerator generator);
	}
}


//
// Program.cs
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
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace Ferum
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length >= 1) {
				int argsDealtWith = 0;

				foreach (string arg in args) {
					switch (arg) {
						case "-v":
						case "--version":
							argsDealtWith++;
							Console.WriteLine("Ferum v1.0.0");
							break;
						case "-h":
						case "--help":
							Usage();
							break;
					}
				}

				if (argsDealtWith < args.Length) {
					List<string> codes = new List<string>();

					foreach (string arg in args) {
						if (File.Exists(arg)) {
							string stuff = File.ReadAllText(arg).Trim();
							codes.Add(stuff);
						}
					}

					foreach (string code in codes) {
						ExecuteCode(code);
					}
				} else {
					return;
				}
			} else {
				Usage();
			}
		}

		protected static void Usage()
		{
			Console.WriteLine("Ferum v1.0.0");
			Console.WriteLine();
			Console.WriteLine("\t$ ferum [options] <files..>");
			Console.WriteLine();
			Console.WriteLine("\tOptions:");
			Console.WriteLine("\t\t-v, --version:");
			Console.WriteLine("\t\t\tGets the current Ferum version.");
			Console.WriteLine("\t\t-h, --help:");
			Console.WriteLine("\t\t\tShows usage help.");
			Console.WriteLine();
			Console.WriteLine("\tExample:");
			Console.WriteLine("\t\t$ ferum example.fe");
			Console.WriteLine("\t\t$ ferum -v");
		}

		protected static void ExecuteCode(string code)
		{
			Lexer lex = new Lexer(code + "\n");
			lex.tokenize();

			Parser parser = new Parser(lex);
			var exprs = parser.start();
			CodeGenerator cg = new CodeGenerator(exprs);
			cg.generate();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Drawing;

namespace PdfParser.Engine.Entities
{
	public class LineBreakToken : IToken
	{
		public LineBreakToken(IToken token)
		{
			TextFont = token.TextFont;
		}

		public string Text => Environment.NewLine;
		public Font TextFont { get; }
		public List<IToken> Subelements => new List<IToken>(0);

		public void Append(string text)
		{
			
		}

		public override string ToString()
		{
			return Text;
		}
	}
}

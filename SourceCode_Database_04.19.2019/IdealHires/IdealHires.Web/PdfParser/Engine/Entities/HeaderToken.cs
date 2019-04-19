using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PdfParser.Engine.Entities
{
	public class HeaderToken : IToken
	{
		public HeaderToken(IToken token)
		{
			Text = token.Text;
			TextFont = token.TextFont;
			Subelements = token.Subelements;
		}

		public string Text { get; private set; }
		public Font TextFont { get; }
		public List<IToken> Subelements { get; }

		public void Append(string text)
		{
			Text += text;
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append($"{Text}");
			foreach (IToken lSubelement in Subelements)
			{
				builder.Append(Environment.NewLine);
				builder.Append("\t");
				builder.Append(lSubelement);
			}
			return builder.ToString();
		}
	}
}

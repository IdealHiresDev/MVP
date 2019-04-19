using System.Collections.Generic;
using System.Drawing;

namespace PdfParser.Engine.Entities
{
	public class TextToken : IToken
	{
		public TextToken(Chunk chunk)
		{
			Text = chunk.Text;
			TextFont = chunk.TextFont;
		}

		public string Text { get; private set; }
		public Font TextFont { get; }
		public List<IToken> Subelements => new List<IToken>(0);

		public void Append(string text)
		{
			Text += text;
		}

		public override string ToString()
		{
			return Text;
		}
	}
}

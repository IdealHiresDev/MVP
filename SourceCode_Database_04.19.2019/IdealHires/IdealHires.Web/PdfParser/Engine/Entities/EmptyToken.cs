using System.Collections.Generic;
using System.Drawing;

namespace PdfParser.Engine.Entities
{
	public class EmptyToken : IToken
	{
		public string Text { get; }
		public Font TextFont { get; }
		public List<IToken> Subelements => new List<IToken>(0);

		public EmptyToken(IToken token)
		{
			Text = token.Text;
			TextFont = token.TextFont;
		}

		public void Append(string text)
		{
			throw new System.NotImplementedException();
		}
	}
}

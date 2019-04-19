using System.Collections.Generic;
using System.Drawing;

namespace PdfParser.Engine.Entities
{
	public interface IToken
	{
		string Text { get; }
		Font TextFont { get; }
		List<IToken> Subelements { get; }
		void Append(string text);
	}
}

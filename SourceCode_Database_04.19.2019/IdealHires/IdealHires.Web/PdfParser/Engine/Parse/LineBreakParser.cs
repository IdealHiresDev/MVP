using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Parse
{
	public class LineBreakParser : Parser
	{
		protected override IToken ParseInternal( IToken token)
		{
			if (token.Text == "\r\n")
				return new LineBreakToken(token);
			return token;
		}
	}
}

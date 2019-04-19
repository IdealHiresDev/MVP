using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Parse
{
	public class HeaderParser : Parser
	{
		public HeaderParser(Parser parser) : base(parser)
		{

		}

		protected override IToken ParseInternal( IToken token)
		{
			if ((!(token is LineBreakToken)) && (token.TextFont.Bold || token.TextFont.Size > 9))
				return new HeaderToken(token);
			return token;
		}
	}
}

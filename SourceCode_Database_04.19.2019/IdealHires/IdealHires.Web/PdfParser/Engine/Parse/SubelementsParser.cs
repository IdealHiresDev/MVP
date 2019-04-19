using System;
using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Parse
{
	public class SubelementsParser : Parser
	{
		private IToken previousChiefHeader;
		private IToken currentHeader;
		private IToken previousToken;

		public SubelementsParser(Parser parser) : base(parser)
		{
		}

		protected override IToken ParseInternal(IToken token)
		{
			if (token is HeaderToken)
			{
				IToken returnToken = token;
				double tokenTextSize = Math.Round(token.TextFont.Size, 2);

				if (previousToken is HeaderToken && Math.Round(previousToken.TextFont.Size, 2) == tokenTextSize)
				{
					currentHeader.Append(token.Text);
				}

				if (currentHeader != null)
				{
					double currentHeaderTextSize = Math.Round(currentHeader.TextFont.Size, 2);
					if (tokenTextSize < currentHeaderTextSize)
					{
						currentHeader.Subelements.Add(token);
						previousChiefHeader = currentHeader;
						returnToken = new EmptyToken(token);
					}

					if (previousChiefHeader != null && tokenTextSize == currentHeaderTextSize)
					{
						previousChiefHeader.Subelements.Add(token);
						returnToken = new EmptyToken(token);
					}

					if (tokenTextSize > currentHeaderTextSize)
					{
						previousChiefHeader = null;
					}
				}

				currentHeader = token;
				previousToken = token;

				return returnToken;
			}

			currentHeader.Subelements.Add(token);

			if (!(token is LineBreakToken))
				previousToken = token;

			return new EmptyToken(token);
		}
	}
}

using System.Collections.Generic;
using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Parse
{
	public abstract class Parser
	{
		private readonly Parser _nested;

		protected Parser(Parser nested)
		{
			_nested = nested;
		}

		protected Parser() : this(null) { }

		protected abstract IToken ParseInternal(IToken token);

		public IEnumerable<IToken> Parse(IEnumerable<IToken> tokens)
		{
			if (_nested != null)
			{
				IEnumerable<IToken> nestedTokens = _nested.Parse(tokens);
				foreach (var token in nestedTokens)
				{
					IToken processedToken = ParseInternal(token);
					yield return processedToken;
				}
			}
			else
			{
				foreach (var token in tokens)
				{
					IToken processedToken = ParseInternal(token);
					yield return processedToken;
				}
			}
		}
	}
}


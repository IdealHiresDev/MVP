using System.Collections.Generic;
using System.Linq;
using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Parse
{
	public class ResumeParser
	{
		private IChunkExtractor _ChunkExtractor;
		private Parser _DecoratedParser;

		public ResumeParser(IChunkExtractor chunkExtractor, Parser decoratedParser)
		{
			_ChunkExtractor = chunkExtractor;
			_DecoratedParser = decoratedParser;
		}

		public IEnumerable<IToken> Parse(string filePath)
		{
			List<Chunk> chunks = _ChunkExtractor.GetChunks(filePath);
			TextToken[] textTokens = chunks.Select(c => new TextToken(c)).ToArray();
			IEnumerable<IToken> tokens = _DecoratedParser.Parse(textTokens);
			return tokens.Where(t => !(t is EmptyToken)).ToArray();
		}
	}
}

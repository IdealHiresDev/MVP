using System.Collections.Generic;
using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Parse
{
	public interface IChunkExtractor
	{
		List<Chunk> GetChunks(string filePath);
	}
}

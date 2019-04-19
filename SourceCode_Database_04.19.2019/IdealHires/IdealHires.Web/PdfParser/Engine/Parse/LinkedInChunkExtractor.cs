using System.Collections.Generic;
using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Parse
{
	public class LinkedInChunkExtractor : IChunkExtractor
	{
		public List<Chunk> GetChunks(string filePath)
		{
			GeneralChunkExtractor chunkExtractor = new GeneralChunkExtractor();
			List<Chunk> chunks = chunkExtractor.GetChunks(filePath);
			int summaryIndex = chunks.FindIndex(c => c.Text == "Summary");
			chunks.RemoveRange(0, summaryIndex);
			return chunks;
		}
	}
}

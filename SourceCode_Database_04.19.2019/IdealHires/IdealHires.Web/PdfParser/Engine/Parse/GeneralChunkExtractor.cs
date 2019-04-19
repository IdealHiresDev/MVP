using System.Collections.Generic;
using System.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using PdfParser.Engine.Decode;
using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Parse
{
	public class GeneralChunkExtractor : IChunkExtractor
	{
		public List<Chunk> GetChunks(string filePath)
		{
			HorizontalTextExtractionStrategy lHorizontalTextExtractionStrategy = new HorizontalTextExtractionStrategy();
			PdfReader reader = new PdfReader(filePath);
			for (int index = 1; index <= reader.NumberOfPages; index++)
			{
				PdfTextExtractor.GetTextFromPage(reader, index, lHorizontalTextExtractionStrategy);
			}

			List<Chunk> tokens = lHorizontalTextExtractionStrategy.DocumentChunks.ToList();
			return tokens;
		}
	}
}

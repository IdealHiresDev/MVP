using System;
using System.Collections.Generic;
using System.Drawing;
using iTextSharp.text.pdf.parser;
using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Decode
{
	public class HorizontalTextExtractionStrategy : LocationTextExtractionStrategy
	{
		private List<Chunk> result = new List<Chunk>();
		private Chunk current = null;
		private Vector _previousBottomLeftCorner = null;

		public HorizontalTextExtractionStrategy()
		{
			locationalResultField = typeof(LocationTextExtractionStrategy).GetField("locationalResult", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
			textLineFinder = new PdfParser.Engine.Decode.TextLineFinder();
		}

		public override void RenderText(TextRenderInfo renderInfo)
		{
			//This code assumes that if the baseline changes then we're on a newline
			Vector bottomLeftCorner = renderInfo.GetBaseline().GetStartPoint();
			Vector topRightCorner = renderInfo.GetAscentLine().GetEndPoint();
			iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(bottomLeftCorner[Vector.I1], bottomLeftCorner[Vector.I2], topRightCorner[Vector.I1], topRightCorner[Vector.I2]);
			Single curFontSize = rect.Height;

			var font = GetFont(renderInfo, curFontSize);

			//See if something has changed, either the baseline, the font or the font size
			if (_previousBottomLeftCorner == null)
			{
				current = new Chunk();
			}
			else if (bottomLeftCorner[Vector.I2] != _previousBottomLeftCorner[Vector.I2])
			{
				result.Add(current);

				var lineBreak = new Chunk();
				lineBreak.Append(Environment.NewLine);
				lineBreak.TextFont = font;

				result.Add(lineBreak);
				current = new Chunk();
			}
			current.TextFont = font;
			current.Append(renderInfo.GetText());
			//Set currently used properties
			_previousBottomLeftCorner = bottomLeftCorner;

			LineSegment lSegment = renderInfo.GetBaseline();
			if (renderInfo.GetRise() != 0)
			{
				// remove the rise from the baseline - we do this because the text from a super/subscript render operations
				// should probably be considered as part of the baseline of the text the super/sub is relative to 
				Matrix lRiseOffsetTransform = new Matrix(0, -renderInfo.GetRise());
				lSegment = lSegment.TransformBy(lRiseOffsetTransform);
			}

			TextChunk lOcation = new HorizontalTextChunk(renderInfo.GetText(), lSegment.GetStartPoint(), lSegment.GetEndPoint(), renderInfo.GetSingleSpaceWidth(), textLineFinder);
			GetLocationalResult().Add(lOcation);
		}

		private Font GetFont(TextRenderInfo renderInfo, float curFontSize)
		{
			FontStyle fontStyle = FontStyle.Regular;
			FontFamily fontFamily;
			string postScriptFontName = renderInfo.GetFont().PostscriptFontName;
			if (postScriptFontName.Contains("Bold"))
			{
				fontStyle = FontStyle.Bold;
			}

			try
			{
				fontFamily = new System.Drawing.FontFamily(renderInfo.GetFont().PostscriptFontName);
			}
			catch
			{
				fontFamily = FontFamily.GenericSansSerif;
			}

			return new System.Drawing.Font(fontFamily, curFontSize, fontStyle);
		}

		public IEnumerable<Chunk> DocumentChunks => result;

		List<TextChunk> GetLocationalResult()
		{
			return (List<TextChunk>)locationalResultField.GetValue(this);
		}

		System.Reflection.FieldInfo locationalResultField;
		TextLineFinder textLineFinder;
	}
}

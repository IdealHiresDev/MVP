using System;
using System.Collections.Generic;
using iTextSharp.text.pdf.parser;

namespace PdfParser.Engine.Decode
{
	public class HorizontalTextChunk : LocationTextExtractionStrategy.TextChunk
	{
		public HorizontalTextChunk(String stringValue, Vector startLocation, Vector endLocation, float charSpaceWidth,
			TextLineFinder textLineFinder)
			: base(stringValue, startLocation, endLocation, charSpaceWidth)
		{
			this.textLineFinder = textLineFinder;
		}

		public override int CompareTo(LocationTextExtractionStrategy.TextChunk rhs)
		{
			HorizontalTextChunk lRhs = rhs as HorizontalTextChunk;
			if (lRhs != null)
			{
				HorizontalTextChunk lHorRhs = lRhs;
				int lRslt = CompareInts(GetLineNumber(), lHorRhs.GetLineNumber());
				if (lRslt != 0) return lRslt;
				return CompareFloats(StartLocation[Vector.I1], lRhs.StartLocation[Vector.I1]);
			}

			return base.CompareTo(rhs);
		}

		public override bool SameLine(LocationTextExtractionStrategy.TextChunk a)
		{
			HorizontalTextChunk lAs = a as HorizontalTextChunk;
			if (lAs != null)
			{
				HorizontalTextChunk lHorAs = lAs;
				return GetLineNumber() == lHorAs.GetLineNumber();
			}

			return base.SameLine(a);
		}

		public int GetLineNumber()
		{
			Vector lStartLocation = StartLocation;
			float y = lStartLocation[Vector.I2];
			List<float> lFlips = textLineFinder.VerticalFlips;
			if (lFlips == null || lFlips.Count == 0)
				return 0;
			if (y < lFlips[0])
				return lFlips.Count / 2 + 1;
			for (int i = 1; i < lFlips.Count; i += 2)
			{
				if (y < lFlips[i])
				{
					return (1 + lFlips.Count - i) / 2;
				}
			}
			return 0;
		}

		private static int CompareInts(int int1, int int2)
		{
			return int1 == int2 ? 0 : int1 < int2 ? -1 : 1;
		}

		private static int CompareFloats(float float1, float float2)
		{
			return float1 == float2 ? 0 : float1 < float2 ? -1 : 1;
		}

		TextLineFinder textLineFinder;
	}
}

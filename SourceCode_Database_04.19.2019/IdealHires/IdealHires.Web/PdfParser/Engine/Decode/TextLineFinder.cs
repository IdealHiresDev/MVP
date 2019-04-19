using System;
using System.Collections.Generic;
using iTextSharp.text.pdf.parser;

namespace PdfParser.Engine.Decode
{
	public class TextLineFinder : IRenderListener
	{
		//Store last used properties
		public void BeginTextBlock() { }
		public void EndTextBlock() { }
		public void RenderImage(ImageRenderInfo renderInfo) { }

		public void RenderText(TextRenderInfo renderInfo)
		{
			LineSegment lAscentLine = renderInfo.GetAscentLine();
			LineSegment lDescentLine = renderInfo.GetDescentLine();
			float[] lYCoords = new float[]{
			lAscentLine.GetStartPoint()[Vector.I2],
			lAscentLine.GetEndPoint()[Vector.I2],
			lDescentLine.GetStartPoint()[Vector.I2],
			lDescentLine.GetEndPoint()[Vector.I2]
		};
			Array.Sort(lYCoords);
			AddVerticalUseSection(lYCoords[0], lYCoords[3]);
		}

		void AddVerticalUseSection(float from, float to)
		{
			if (to < from)
			{
				float lTemp = to;
				to = from;
				from = lTemp;
			}

			int i = 0, j = 0;
			for (; i < VerticalFlips.Count; i++)
			{
				float lFlip = VerticalFlips[i];
				if (lFlip < from)
					continue;

				for (j = i; j < VerticalFlips.Count; j++)
				{
					lFlip = VerticalFlips[j];
					if (lFlip < to)
						continue;
					break;
				}
				break;
			}

			bool lFromOutsideInterval = i % 2 == 0;
			bool lToOutsideInterval = j % 2 == 0;

			while (j-- > i)
				VerticalFlips.RemoveAt(j);
			if (lToOutsideInterval)
				VerticalFlips.Insert(i, to);
			if (lFromOutsideInterval)
				VerticalFlips.Insert(i, from);
		}

		public List<float> VerticalFlips = new List<float>();
	}
}

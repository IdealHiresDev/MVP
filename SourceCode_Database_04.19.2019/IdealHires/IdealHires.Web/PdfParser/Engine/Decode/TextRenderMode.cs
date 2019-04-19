namespace PdfParser.Engine.Decode
{
	internal enum TextRenderMode
	{
		FillText = 0,
		StrokeText = 1,
		FillThenStrokeText = 2,
		Invisible = 3,
		FillTextAndAddToPathForClipping = 4,
		StrokeTextAndAddToPathForClipping = 5,
		FillThenStrokeTextAndAddToPathForClipping = 6,
		AddTextToPaddForClipping = 7
	}
}

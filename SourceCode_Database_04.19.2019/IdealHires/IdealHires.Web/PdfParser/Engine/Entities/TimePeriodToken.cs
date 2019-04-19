using System;
using System.Collections.Generic;
using System.Drawing;

namespace PdfParser.Engine.Entities
{
	public class TimePeriodToken : IToken
	{
		public TimePeriodToken(IToken token)
		{
			Text = token.Text;
			TextFont = token.TextFont;
		}

		public string Text { get; private set; }
		public Font TextFont { get; }
		public List<IToken> Subelements => new List<IToken>(0);
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public Boolean UntilPresent { get; set; }

		public void Append(string text)
		{
			Text += text;
		}

		public override string ToString()
		{
			if (!UntilPresent)
				return $"{Text} {StartDate} - {EndDate}";
			return $"{Text} {StartDate} - Present";
		}
	}
}

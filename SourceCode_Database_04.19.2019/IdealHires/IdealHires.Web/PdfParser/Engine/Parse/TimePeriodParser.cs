using System;
using System.Globalization;
using System.Text.RegularExpressions;
using PdfParser.Engine.Entities;

namespace PdfParser.Engine.Parse
{
	public class TimePeriodParser : Parser
	{
		private static string anyRegex = "(.*?)";
		private static string monthRegex = "((?:Jan(?:uary)?|Feb(?:ruary)?|Mar(?:ch)?|Apr(?:il)?|May|Jun(?:e)?|Jul(?:y)?|Aug(?:ust)?|Sep(?:tember)?|Sept|Oct(?:ober)?|Nov(?:ember)?|Dec(?:ember)?))";
		private static string minusRegex = "(-)";
		private static string yearRegex = "((?:(?:[1]{1}\\d{1}\\d{1}\\d{1})|(?:[2]{1}\\d{3})))(?![\\d])";
		private static string whiteSpace = "(\\s+)";
		private static string presentRegex = "(Present)";

		public TimePeriodParser(Parser parser) : base(parser)
		{

		}

		protected override IToken ParseInternal(IToken token)
		{
			TimePeriodToken timeToken = new TimePeriodToken(token);
			if (CheckForExperienceTimePeriodWithPresent(timeToken) 
				|| CheckForLinkedInExperienceTimePeriod(timeToken) 
				|| CheckForEducationTimePeriodWithPresent(timeToken) 
				|| CheckForLinkedInEducationTimePeriod(timeToken))
				return timeToken;
			return token;
		}

		private bool CheckForEducationTimePeriodWithPresent(TimePeriodToken token)
		{
			Regex exp = new Regex(anyRegex + yearRegex + whiteSpace + minusRegex + whiteSpace + presentRegex + anyRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match match = exp.Match(token.Text);
			if (match.Success)
			{
				string startYear = match.Groups[1].ToString().Trim();
				try
				{
					token.StartDate = DateTime.ParseExact(startYear, "yyyy", CultureInfo.InvariantCulture);
					token.UntilPresent = true;
				}
				catch (Exception exception)
				{
					return false;
				}
			}
			return match.Success;
		}

		private bool CheckForExperienceTimePeriodWithPresent(TimePeriodToken token)
		{
			string monthYearRegex = monthRegex + whiteSpace + yearRegex;
			Regex exp = new Regex(monthYearRegex + whiteSpace + minusRegex + whiteSpace + presentRegex + anyRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match match = exp.Match(token.Text);
			if (match.Success)
			{
				string startMonth = match.Groups[1].ToString().Trim();
				string startYear = match.Groups[3].ToString().Trim();
				try
				{
					token.StartDate = DateTime.ParseExact($"{startMonth} {startYear}", "MMMM yyyy", CultureInfo.InvariantCulture);
					token.UntilPresent = true;
				}
				catch (Exception exception)
				{
					return false;
				}
			}
			return match.Success;
		}

		private bool CheckForLinkedInEducationTimePeriod(TimePeriodToken token)
		{
			Regex exp = new Regex(anyRegex + yearRegex + whiteSpace + minusRegex + whiteSpace + yearRegex + anyRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match match = exp.Match(token.Text);
			if (match.Success)
			{
				string startYear = match.Groups[2].ToString().Trim();
				string endYear = match.Groups[6].ToString().Trim();
				try
				{
					token.StartDate = DateTime.ParseExact(startYear, "yyyy", CultureInfo.InvariantCulture);
					token.EndDate = DateTime.ParseExact(endYear, "yyyy", CultureInfo.InvariantCulture);
				}
				catch (Exception exception)
				{
					return false;
				}
			}
			return match.Success;
		}

		private bool CheckForLinkedInExperienceTimePeriod(TimePeriodToken token)
		{
			string monthYearRegex = monthRegex + whiteSpace + yearRegex;
			Regex exp = new Regex(anyRegex + monthYearRegex + whiteSpace + minusRegex + whiteSpace + monthYearRegex + anyRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
			Match match = exp.Match(token.Text);
			if (match.Success)
			{
				string startMonth = match.Groups[2].ToString().Trim();
				string startYear = match.Groups[4].ToString().Trim();
				string endMonth = match.Groups[8].ToString().Trim();
				string endYear = match.Groups[10].ToString().Trim();
				try
				{
					token.StartDate = DateTime.ParseExact($"{startMonth} {startYear}", "MMMM yyyy", CultureInfo.InvariantCulture);
					token.EndDate = DateTime.ParseExact($"{endMonth} {endYear}", "MMMM yyyy", CultureInfo.InvariantCulture);
				}
				catch (Exception exception)
				{
					return false;
				}
			}
			return match.Success;
		}
	}
}

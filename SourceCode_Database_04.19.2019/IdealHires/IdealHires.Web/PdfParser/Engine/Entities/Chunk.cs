using System.Text;

namespace PdfParser.Engine.Entities
{
	public class Chunk
	{
		private readonly StringBuilder builder = new StringBuilder();

		public void Append(string text) => builder.Append(text);
		public string Text => builder.ToString();
		public System.Drawing.Font TextFont { get; set; }
	}
}

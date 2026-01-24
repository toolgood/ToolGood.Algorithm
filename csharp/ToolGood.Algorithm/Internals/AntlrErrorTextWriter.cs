using System;
using System.IO;
using System.Text;

namespace ToolGood.Algorithm.Internals
{
	internal class AntlrErrorTextWriter : TextWriter
	{
		public bool IsError { get; set; }
		public string ErrorMsg { get; private set; }
		public AntlrErrorTextWriter()
		{
		}

		public AntlrErrorTextWriter(IFormatProvider formatProvider) : base(formatProvider)
		{
		}

		public override Encoding Encoding => Encoding.UTF8;

		public override void WriteLine(string value)
		{
			IsError = true;
			ErrorMsg = value;
		}
		 

	}
}

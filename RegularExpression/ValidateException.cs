using System;

namespace RegularExpression
{
	public class ValidateException : Exception
	{
		public ValidateException(string message = "") : base(message) { }
	}
}

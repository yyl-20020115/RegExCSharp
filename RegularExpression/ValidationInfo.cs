using System;

namespace RegularExpression
{
	public class ValidationInfo
    {
        public ErrorCode ErrorCode = ErrorCode.ERR_SUCCESS;
        public int ErrorStartAt = -1;
        public int ErrorLength = -1;
        public string FormattedString = String.Empty;
        public bool MatchAtStart = false;
        public bool MatchAtEnd = false;
    }
}

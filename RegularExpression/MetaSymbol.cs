namespace RegularExpression
{
	public class MetaSymbol
    {
        public const char CONCANATE = '.';
        public const char ALTERNATE = '|';
        public const char ZERO_OR_MORE = '*';
        public const char ONE_OR_MORE = '+';
        public const char ZERO_OR_ONE = '?';
        public const char OPEN_PREN = '(';
        public const char CLOSE_PREN = ')';
        public const char COMPLEMENT = '^';
        public const char ANY_ONE_CHAR = '_';  // this used in the syntax i.e., A_B => AEB or AcB
        public const string ANY_ONE_CHAR_TRANS = "AnyChar";  // this is the actual transitional symbol
        public const char ESCAPE = '\\';
        public const string EPSILON = "epsilon";
        public const char CHARHashSet_START = '[';
        public const char CHARHashSet_END = ']';
        public const char RANGE = '-';
        public const string DUMMY = "Dummy";  // if you draw the model on paper, you should not draw this transition
        public const char MATCH_START = '^';  // token to specify to match at the beginning of the string
        public const char MATCH_END = '$';  // token to specify to match at the end of string
        public const char NEW_LINE = '\n';
        public const char TAB = '\t';
    }
}

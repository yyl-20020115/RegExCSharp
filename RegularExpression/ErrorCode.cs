namespace RegularExpression
{
	public enum ErrorCode
    {
        ERR_SUCCESS,        // the patten is in correct format
        ERR_PREN_MISMATCH,  // "(A(D*)"
        ERR_EMPTY_PREN,     // "()"
        ERR_EMPTY_BRACKET,  // "[]"
        ERR_BRACKET_MISMATCH,  // "["
        ERR_OPERAND_MISSING,  // "A|"
        ERR_INVALID_ESCAPE,   // "\A"
        ERR_INVALID_RANGE,  // "[C-A]" 
        ERR_EMPTY_STRING,   // ""
    }
}

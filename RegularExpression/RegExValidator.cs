using System;
using System.Text;

namespace RegularExpression
{
	/// <summary>
	/// This class is used to validate a pattern.  Validation is done using Recursive Descent Parsing.
	/// Beside validating the pattern, it does two other tasks: insertion of implicit tokens and expanding character classes.
	/// i.e., "AB"    -> "A.B"        (inserting the concatenating quantifier)
	///       "A.B"   -> "A\.B"       (inserting the escape)
	///       "[A-C]" -> "(A|B|C)"    (expanding the range)
	///       "(AB"   -> Reports error with mismatch parenthesis
	/// </summary>
	public class RegExValidator
    {
        protected bool m_bConcatenate = false;
		protected bool m_bAlternate = false;

		protected const char m_chNull = char.MinValue; // null symbol;
		protected char m_chSym = m_chNull;

		protected int m_nPatternLength = -1;
		protected string m_sPattern = String.Empty;
		protected int m_nCurrPos = -1;
		protected StringBuilder m_sb = null;

		protected ValidationInfo m_validationInfo = null;

        public RegExValidator()
        {

        }

        public virtual ValidationInfo Validate(string sPattern)
        {
            m_chSym = m_chNull;
            m_bConcatenate = false;
            m_bAlternate = false;
            m_sb = new StringBuilder();
            m_nCurrPos = -1;
            m_sPattern = sPattern;
            m_nPatternLength = m_sPattern.Length;

            m_validationInfo = new ValidationInfo();

            if (sPattern.Length == 0)
            {
                m_validationInfo.ErrorCode = ErrorCode.ERR_EMPTY_STRING;
                return m_validationInfo;
            }

            GetNextSymbol();

            string sLit1 = MetaSymbol.MATCH_START.ToString();
            string sLit2 = MetaSymbol.MATCH_END.ToString();
            string sLit3 = sLit1 + sLit2;

            if (!(sPattern.CompareTo(sLit1) == 0 || sPattern.CompareTo(sLit2) == 0 || sPattern.CompareTo(sLit3) == 0))
            {
                if (sPattern[0] == MetaSymbol.MATCH_START)
                {
                    m_validationInfo.MatchAtStart = true;
                    Accept(MetaSymbol.MATCH_START);
                }
                if (m_sPattern[m_nPatternLength - 1] == MetaSymbol.MATCH_END)
                {
                    m_validationInfo.MatchAtEnd = true;
                    m_nPatternLength--;
                }
            }

            try
            {
                while (m_nCurrPos < m_nPatternLength)
                {
                    switch (m_chSym)
                    {
                        case MetaSymbol.ALTERNATE:
                        case MetaSymbol.ONE_OR_MORE:
                        case MetaSymbol.ZERO_OR_MORE:
                        case MetaSymbol.ZERO_OR_ONE:
                            Abort(ErrorCode.ERR_OPERAND_MISSING, m_nCurrPos, 1);
                            break;
                        case MetaSymbol.CLOSE_PREN:
                            Abort(ErrorCode.ERR_PREN_MISMATCH, m_nCurrPos, 1);
                            break;
                        default:
                            ValidateExpression();
                            break;
                    }
                }
            }
            catch (ValidateException)
            {
               
            }

            m_validationInfo.FormattedString = m_sb.ToString();

            return m_validationInfo;
        }
        protected virtual void GetNextSymbol()
        {
            m_nCurrPos++;
            if (m_nCurrPos < m_nPatternLength)
            {
                m_chSym = m_sPattern[m_nCurrPos];
            }
            else
            {
                m_chSym = m_chNull;
            }
        }
		protected virtual bool Accept(char ch)
        {
            if (m_chSym == ch)
            {
                GetNextSymbol();
                return true;
            }
            return false;
        }
		protected virtual bool AcceptPostfixOperator()
        {
            switch (m_chSym)
            {
                case MetaSymbol.ONE_OR_MORE:
                case MetaSymbol.ZERO_OR_MORE:
                case MetaSymbol.ZERO_OR_ONE:
                    m_sb.Append(m_chSym);
                    return Accept(m_chSym);
                default:
                    return false;
            }
        }
		protected virtual bool AcceptNonEscapeChar()
        {
            switch (m_chSym)
            {
                case MetaSymbol.ALTERNATE:
                case MetaSymbol.CHARHashSet_START:
                case MetaSymbol.CLOSE_PREN:
                case MetaSymbol.ESCAPE:
                case MetaSymbol.ONE_OR_MORE:
                case MetaSymbol.OPEN_PREN:
                case MetaSymbol.ZERO_OR_MORE:
                case MetaSymbol.ZERO_OR_ONE:
                case MetaSymbol.CONCANATE:
                case m_chNull:
                    return false;
                default:
                    AppendConcatenate();
                    m_sb.Append(m_chSym);
                    Accept(m_chSym);
                    break;
            }
            return true;
        }
		protected virtual bool Expect(char ch)
        {
            if (Accept(ch))
            {
                return true;
            }
            return false;
        }
		protected virtual bool ExpectEscapeChar()
        {
            switch (m_chSym)
            {
                case MetaSymbol.ALTERNATE:
                case MetaSymbol.ANY_ONE_CHAR:
                case MetaSymbol.CHARHashSet_START:
                case MetaSymbol.CLOSE_PREN:
                case MetaSymbol.COMPLEMENT:
                case MetaSymbol.ESCAPE:
                case MetaSymbol.ONE_OR_MORE:
                case MetaSymbol.OPEN_PREN:
                case MetaSymbol.ZERO_OR_MORE:
                case MetaSymbol.ZERO_OR_ONE:
                    m_sb.Append(MetaSymbol.ESCAPE);
                    m_sb.Append(m_chSym);
                    Accept(m_chSym);
                    break;
                case MetaSymbol.NEW_LINE:
                    m_sb.Append('\n');
                    Accept(m_chSym);
                    break;
                case MetaSymbol.TAB:
                    m_sb.Append('\t');
                    Accept(m_chSym);
                    break;
                default:
                    return false;
            }
            return true;
        }
		protected virtual void Abort(ErrorCode errCode, int nErrPosition, int nErrLen)
        {
            m_validationInfo.ErrorCode = errCode;
            m_validationInfo.ErrorStartAt = nErrPosition;
            m_validationInfo.ErrorLength = nErrLen;

            throw new ValidateException("Syntax error.");
        }
		protected virtual void AppendConcatenate()
        {
            if (m_bConcatenate)
            {
                m_sb.Append(MetaSymbol.CONCANATE);
                m_bConcatenate = false;
            }
        }
		protected virtual void AppendAlternate()
        {
            if (m_bAlternate)
            {
                m_sb.Append(MetaSymbol.ALTERNATE);
                m_bAlternate = false;
            }
        }
		protected virtual void ValidateExpression()
        {
            while (Accept(MetaSymbol.ESCAPE))
            {
                AppendConcatenate();
                if (!ExpectEscapeChar())
                {
                    Abort(ErrorCode.ERR_INVALID_ESCAPE, m_nCurrPos - 1, 1);
                }
                AcceptPostfixOperator();
                m_bConcatenate = true;
            }

            while (Accept(MetaSymbol.CONCANATE))
            {
                AppendConcatenate();
                m_sb.Append(MetaSymbol.ESCAPE);
                m_sb.Append(MetaSymbol.CONCANATE);
                AcceptPostfixOperator();
                m_bConcatenate = true;
            }

            while (Accept(MetaSymbol.COMPLEMENT))
            {
                AppendConcatenate();
                m_sb.Append(MetaSymbol.ESCAPE);
                m_sb.Append(MetaSymbol.COMPLEMENT);
                AcceptPostfixOperator();
                m_bConcatenate = true;
            }

            while (AcceptNonEscapeChar())
            {
                AcceptPostfixOperator();
                m_bConcatenate = true;
                ValidateExpression();
            }

            if (Accept(MetaSymbol.OPEN_PREN))
            {
                int nEntryPos = m_nCurrPos - 1;
                AppendConcatenate();
                m_sb.Append(MetaSymbol.OPEN_PREN);
                ValidateExpression();
                if (!Expect(MetaSymbol.CLOSE_PREN))
                {
                    Abort(ErrorCode.ERR_PREN_MISMATCH, nEntryPos, m_nCurrPos - nEntryPos);
                }
                m_sb.Append(MetaSymbol.CLOSE_PREN);

                int nLen = m_nCurrPos - nEntryPos;
                if (nLen == 2)
                {
                    Abort(ErrorCode.ERR_EMPTY_PREN, nEntryPos, m_nCurrPos - nEntryPos);
                }

                AcceptPostfixOperator();
                m_bConcatenate = true;
                ValidateExpression();
            }


            if (Accept(MetaSymbol.CHARHashSet_START))
            {
                int nEntryPos = m_nCurrPos - 1;
                bool bComplement = false;

                AppendConcatenate();

                if (Accept(MetaSymbol.COMPLEMENT))
                {
                    bComplement = true;
                }

                string sTmp = m_sb.ToString();

                m_sb = new StringBuilder();
                m_bAlternate = false;
                CharacterHashSet();

                if (!Expect(MetaSymbol.CHARHashSet_END))
                {
                    Abort(ErrorCode.ERR_BRACKET_MISMATCH, nEntryPos, m_nCurrPos - nEntryPos);
                }

                int nLen = m_nCurrPos - nEntryPos;

                if (nLen == 2)  // "[]"
                {
                    Abort(ErrorCode.ERR_EMPTY_BRACKET, nEntryPos, m_nCurrPos - nEntryPos);
                }
                else if (nLen == 3 && bComplement) // "[^]"  - treat the complement as literal
                {
                    m_sb = new StringBuilder();
                    m_sb.Append(sTmp);
                    m_sb.Append(MetaSymbol.OPEN_PREN);
                    m_sb.Append(MetaSymbol.ESCAPE);
                    m_sb.Append(MetaSymbol.COMPLEMENT);
                    m_sb.Append(MetaSymbol.CLOSE_PREN);
                }
                else
                {
                    string sCharHashSet = m_sb.ToString();
                    m_sb = new StringBuilder();
                    m_sb.Append(sTmp);
                    if (bComplement)
                    {
                        m_sb.Append(MetaSymbol.COMPLEMENT);
                    }
                    m_sb.Append(MetaSymbol.OPEN_PREN);
                    m_sb.Append(sCharHashSet /*ExpandRange(sCharHashSet, nEntryPos) */   );
                    m_sb.Append(MetaSymbol.CLOSE_PREN);
                }

                AcceptPostfixOperator();

                m_bConcatenate = true;

                ValidateExpression();
            }


            if (Accept(MetaSymbol.ALTERNATE))
            {
                int nEntryPos = m_nCurrPos - 1;
                m_bConcatenate = false;
                m_sb.Append(MetaSymbol.ALTERNATE);
                ValidateExpression();
                int nLen = m_nCurrPos - nEntryPos;
                if (nLen == 1)
                {
                    Abort(ErrorCode.ERR_OPERAND_MISSING, nEntryPos, m_nCurrPos - nEntryPos);
                }
                ValidateExpression();
            }
        }
		protected virtual string ExpectEscapeCharInBracket()
        {
            char ch = m_chSym;

            switch (m_chSym)
            {
                case MetaSymbol.CHARHashSet_END:
                case MetaSymbol.ESCAPE:
                    //AppendAlternate();
                    //m_sb.Append(MetaSymbol.ESCAPE);
                    //m_sb.Append(m_chSym);
                    //return Accept(m_chSym);
                    Accept(m_chSym);
                    return MetaSymbol.ESCAPE.ToString() + ch.ToString();
                case MetaSymbol.NEW_LINE:
                    //AppendAlternate();
                    //m_sb.Append(Environment.NewLine);
                    //return Accept(m_chSym);
                    Accept(m_chSym);
                    //return Environment.NewLine;
                    return ('\n').ToString();
                case MetaSymbol.TAB:
                    //AppendAlternate();
                    //m_sb.Append('\t');
                    //return Accept(m_chSym);
                    Accept(m_chSym);
                    return ('\t').ToString();
                default:
                    return String.Empty;
            }
        }
		protected virtual string AcceptNonEscapeCharInBracket()
        {
            char ch = m_chSym;

            switch (ch)
            {
                case MetaSymbol.CHARHashSet_END:
                case MetaSymbol.ESCAPE:
                case m_chNull:
                    return String.Empty;
                case MetaSymbol.ALTERNATE:
                case MetaSymbol.ANY_ONE_CHAR:
                case MetaSymbol.CLOSE_PREN:
                case MetaSymbol.COMPLEMENT:
                case MetaSymbol.ONE_OR_MORE:
                case MetaSymbol.OPEN_PREN:
                case MetaSymbol.ZERO_OR_MORE:
                case MetaSymbol.ZERO_OR_ONE:
                case MetaSymbol.CONCANATE:
                    Accept(m_chSym);
                    return MetaSymbol.ESCAPE.ToString() + ch.ToString();
                default:
                    Accept(m_chSym);
                    return ch.ToString();
            }
        }
		protected virtual void CharacterHashSet()
        {
            int nRangeFormStartAt = -1;
            int nStartAt = -1;
            int nLength = -1;

            // xx-xx form
            string sLeft = String.Empty;
            string sRange = String.Empty;
            string sRight = String.Empty;

            string sTmp = String.Empty;

            while (true)
            {
                sTmp = String.Empty;

                nStartAt = m_nCurrPos;

                if (Accept(MetaSymbol.ESCAPE))
                {
                    if ((sTmp = ExpectEscapeCharInBracket()) == String.Empty)
                    {
                        Abort(ErrorCode.ERR_INVALID_ESCAPE, m_nCurrPos - 1, 1);
                    }
                    nLength = 2;
                }

                if (sTmp == String.Empty)
                {
                    sTmp = AcceptNonEscapeCharInBracket();
                    nLength = 1;
                }

                if (sTmp == String.Empty)
                {
                    break;
                }

                if (sLeft == String.Empty)
                {
                    nRangeFormStartAt = nStartAt;
                    sLeft = sTmp;
                    AppendAlternate();
                    m_sb.Append(sTmp);
                    m_bAlternate = true;
                    continue;
                }

                if (sRange == String.Empty)
                {
                    if (sTmp != MetaSymbol.RANGE.ToString())
                    {
                        nRangeFormStartAt = nStartAt;
                        sLeft = sTmp;
                        AppendAlternate();
                        m_sb.Append(sTmp);
                        m_bAlternate = true;
                        continue;
                    }
                    else
                    {
                        sRange = sTmp;
                    }
                    continue;
                }

                sRight = sTmp;

                bool bOk = ExpandRange(sLeft, sRight);

                if (!bOk)
                {
                    int nSubstringLen = (nStartAt + nLength) - nRangeFormStartAt;

                    Abort(ErrorCode.ERR_INVALID_RANGE, nRangeFormStartAt, nSubstringLen);
                }
                sLeft = String.Empty;
                sRange = String.Empty;
                sRange = String.Empty;
            }

            if (sRange != String.Empty)
            {
                AppendAlternate();
                m_sb.Append(sRange);
                m_bAlternate = true;
            }

        }
		protected virtual bool ExpandRange(string sLeft, string sRight)
        {
            char chLeft = (sLeft.Length > 1 ? sLeft[1] : sLeft[0]);
            char chRight = (sRight.Length > 1 ? sRight[1] : sRight[0]);

            if (chLeft > chRight)
            {
                return false;
            }

            chLeft++;
            while (chLeft <= chRight)
            {
                AppendAlternate();

                switch (chLeft)
                {
                    case MetaSymbol.ALTERNATE:
                    case MetaSymbol.ANY_ONE_CHAR:
                    case MetaSymbol.CLOSE_PREN:
                    case MetaSymbol.COMPLEMENT:
                    case MetaSymbol.CONCANATE:
                    case MetaSymbol.ESCAPE:
                    case MetaSymbol.ONE_OR_MORE:
                    case MetaSymbol.ZERO_OR_MORE:
                    case MetaSymbol.ZERO_OR_ONE:
                    case MetaSymbol.OPEN_PREN:
                        m_sb.Append(MetaSymbol.ESCAPE);
                        break;
                    default:
                        break;
                }

                m_sb.Append(chLeft);
                m_bAlternate = true;
                chLeft++;
            }

            return true;
        }
    }
}

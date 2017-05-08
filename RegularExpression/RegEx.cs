using System;
using System.Text;

namespace RegularExpression
{
	/// <summary>
	/// the regular expression recognizer class
	/// </summary>
	public class RegEx
	{
		/// <summary>
		/// Used to validate the pattern string and make sure it is in correct form
		/// </summary>
		static private RegExValidator m_reValidator = new RegExValidator();

		/// <summary>
		/// Position of the last error in the pattern string.
		/// 
		/// </summary>
		private int m_nLastErrorIndex = -1;

		/// <summary>
		/// Length of the error substring in the pattern string
		/// </summary>
		private int m_nLastErrorLength = -1;

		/// <summary>
		/// ErrorCode indicating what if the last compilation succeeded.
		/// </summary>
		private ErrorCode m_LastErrorCode = ErrorCode.ERR_SUCCESS;

		/// <summary>
		/// Specifies match must occur at the beginning of the input string (^)
		/// </summary>
		private bool m_bMatchAtStart = false;

		/// <summary>
		///  Specifies match must occur at the end of the input string ($)
		/// </summary>
		private bool m_bMatchAtEnd = false;

		/// <summary>
		/// Behave greedy, default is true.
		/// If true, Find will stop at the first accepting state, otherwise it will try process more character
		/// </summary>
		private bool m_bGreedy = true;

		/// <summary>
		/// Start state of the DFA M'.
		/// </summary>
		private State m_stateStartDfaM = null;

		/// <summary>
		/// public default constructor
		/// </summary>
		public RegEx() {}

		/// <summary>
		/// Compiles a pattern string produces a Minimum DFA model.
		/// </summary>
		/// <param name="pattern">Actual pattern string in the correct format</param>
		/// <param name="status">This will receive the statistics. Can be null.</param>
		/// <returns>ErrorCode indicating how the compilation went.</returns>
		public ErrorCode Compile(string pattern, StringBuilder status = null)
		{
			ValidationInfo vi = m_reValidator.Validate(pattern ?? throw new ArgumentNullException(nameof(pattern)));

			State.ResetId();

			int nLineLength = 0;

			UpdateValidationInfo(vi);

			if (vi.ErrorCode != ErrorCode.ERR_SUCCESS)
				return vi.ErrorCode;

			string sRegExPostfix = FixConverter.ConvertToPostfix(vi.FormattedString);

			if (status != null)
			{
				status.AppendLine("Original pattern:\t\t" + pattern);
				status.AppendLine("Pattern after formatting:\t" + vi.FormattedString);
				status.AppendLine("Pattern after postfix:\t\t" + sRegExPostfix);
				status.AppendLine();
			}
			State stateStartNfa = Fsa.CreateNfa(sRegExPostfix);

			if (status != null)
			{
				status.AppendLine();
				status.AppendLine("NFA Table:");
				nLineLength = Fsa.GetSerializedFsa(stateStartNfa, status);
				status.AppendFormat((string.Empty).PadRight(nLineLength, '*'));
				status.AppendLine();
			}

			State.ResetId();
			State stateStartDfa = Fsa.ConvertToDfa(stateStartNfa);

			if (status != null)
			{
				status.AppendLine();
				status.AppendLine("DFA Table:");
				nLineLength = Fsa.GetSerializedFsa(stateStartDfa, status);
				status.AppendFormat((string.Empty).PadRight(nLineLength, '*'));
				status.AppendLine();
			}

			State stateStartDfaM = Fsa.ReduceDfa(stateStartDfa);
			m_stateStartDfaM = stateStartDfaM;

			if (status != null)
			{
				status.AppendLine();
				status.AppendLine("Reduced DFA Table:");
				nLineLength = Fsa.GetSerializedFsa(stateStartDfaM, status);
				status.AppendFormat((string.Empty).PadRight(nLineLength, '*'));
				status.AppendLine();
			}
			return ErrorCode.ERR_SUCCESS;
		}

		/// <summary>
		/// Search and finds a match for the compiled pattern.
		/// One must call Compile method before calling this method.
		/// </summary>
		/// <param name="sSearchIn">String to search in.</param>
		/// <param name="nSearchStartAt">Index at which to begin the search.</param>
		/// <param name="nSearchEndAt">Index at which to end the search.</param>
		/// <param name="nFoundBeginAt">If match found, receives the index where the match started, otherwise -1</param>
		/// <param name="nFoundEndAt">If match found, receives the index where the match ended, otherwise -1</param>
		/// <returns>true if match found, otherwise false.</returns>
		public bool FindMatch(string sSearchIn,
							  int nSearchStartAt,
							  int nSearchEndAt,
							  ref int nFoundBeginAt,
							  ref int nFoundEndAt)
		{

			if (m_stateStartDfaM == null || nSearchStartAt < 0) return false;

			State stateStart = m_stateStartDfaM;

			nFoundBeginAt = -1;
			nFoundEndAt = -1;

			bool bAccepted = false;
			State toState = null;
			State stateCurr = stateStart;
			int nIndex = nSearchStartAt;
			int nSearchUpTo = nSearchEndAt;

			while (nIndex <= nSearchUpTo)
			{
				if (m_bGreedy && IsWildCard(stateCurr) == true)
				{
					if (nFoundBeginAt == -1)
					{
						nFoundBeginAt = nIndex;
					}
					
					ProcessWildCard(stateCurr, sSearchIn, ref nIndex, nSearchUpTo);
				}

				char chInputSymbol = sSearchIn[nIndex];

				toState = stateCurr.GetSingleTransition(chInputSymbol.ToString());

				if (toState == null)
				{
					toState = stateCurr.GetSingleTransition(MetaSymbol.ANY_ONE_CHAR_TRANS);
				}
				if (toState != null)
				{
					if (nFoundBeginAt == -1)
					{
						nFoundBeginAt = nIndex;
					}
					if (toState.AcceptingState)
					{
						if (m_bMatchAtEnd && nIndex != nSearchUpTo)  // then we ignore the accepting state
						{
							//toState = stateStart ;
						}
						else
						{
							bAccepted = true;
							nFoundEndAt = nIndex;
							if (m_bGreedy == false)
								break;
						}
					}
					stateCurr = toState;
					nIndex++;
				}
				else if (!m_bMatchAtStart && !bAccepted)  // we reset everything
				{
					nIndex = (nFoundBeginAt != -1 ? nFoundBeginAt + 1 : nIndex + 1);

					nFoundBeginAt = -1;
					nFoundEndAt = -1;
					//nIndex++;
					stateCurr = stateStart;  // start from beginning
				}
				else
				{
					break;
				}
			}  // end of while..loop 

			if (!bAccepted)
			{
				if (!stateStart.AcceptingState)
				{
					return false;
				}
				else // matched an empty string
				{
					nFoundBeginAt = nSearchStartAt;
					nFoundEndAt = nFoundBeginAt - 1;
					return true;
				}
			}
			return true;
		}

		/// <summary>
		/// Determines if a state contains a wildcard transition.
		/// i.e., A_*B
		/// </summary>
		/// <param name="state">State to check</param>
		/// <returns>true if the state contains wildcard transition, otherwise false</returns>
		private bool IsWildCard(State state)
		{
			return (state == state.GetSingleTransition(MetaSymbol.ANY_ONE_CHAR_TRANS));
		}

		/// <summary>
		/// Process state that has wildcard transition.
		/// </summary>
		/// <param name="state">State with wildcard transition</param>
		/// <param name="sSearchIn">String to search in.</param>
		/// <param name="nCurrIndex">Current index of the search</param>
		/// <param name="nSearchUpTo">Index where to stop the search.</param>
		private void ProcessWildCard(State state, string sSearchIn, ref int nCurrIndex, int nSearchUpTo)
		{
			State toState = null;

			int nIndex = nCurrIndex;

			while (nIndex <= nSearchUpTo)
			{
				char ch = sSearchIn[nIndex];

				toState = state.GetSingleTransition(ch.ToString());

				if (toState != null)
				{
					nCurrIndex = nIndex;
				}
				nIndex++;
			}
		}

		/// <summary>
		/// Get the ready state of the parser.
		/// </summary>
		/// <returns>true if a Compile method had been called successfully, otherwise false.</returns>
		public bool IsReady => (this.m_stateStartDfaM != null);

		/// <summary>
		/// Position where error occurred during the last compilation
		/// </summary>
		/// <returns>-1 if there was no compilation</returns>
		public int LastErrorPosition => m_nLastErrorIndex;

		/// <summary>
		/// Indicate if the last compilation was successful or error
		/// </summary>
		/// <returns></returns>
		public ErrorCode LastErrorCode => m_LastErrorCode;

		/// <summary>
		/// Get last error length.
		/// </summary>
		/// <returns>Length</returns>
		public int LastErrorLength => m_nLastErrorLength;

		/// <summary>
		/// Gets/Sets to indicate whether Find should stop at the first accepting state, 
		/// or should continue see if further match is possible (greedy).
		/// </summary>
		public bool UseGreedy
		{
			get => m_bGreedy;
			set => m_bGreedy = value;
		}

		/// <summary>
		/// Helper function. Updates the local variables once the validation returns.
		/// </summary>
		/// <param name="vi"></param>
		private void UpdateValidationInfo(ValidationInfo vi)
		{
			if (vi.ErrorCode == ErrorCode.ERR_SUCCESS)
			{
				m_bMatchAtEnd = vi.MatchAtEnd;
				m_bMatchAtStart = vi.MatchAtStart;
			}

			m_LastErrorCode = vi.ErrorCode;
			m_nLastErrorIndex = vi.ErrorStartAt;
			m_nLastErrorLength = vi.ErrorLength;
		}
	}
}



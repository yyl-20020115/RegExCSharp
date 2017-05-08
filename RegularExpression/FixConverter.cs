using System.Collections.Generic;
using System.Text;

namespace RegularExpression
{
	public class FixConverter
	{
		/// <summary>
		/// Converts a regular expression from infix form to postfix form
		/// </summary>
		/// <param name="sInfixPattern">regular expression in infix form</param>
		/// <returns>regular expression in postfix form</returns>
		public static string ConvertToPostfix(string sInfixPattern)
		{
			Stack<char> stackOperator = new Stack<char>();
			Queue<char> queuePostfix = new Queue<char>();

			bool bEscape = false;

			for (int i = 0; i < sInfixPattern.Length; i++)
			{
				char ch = sInfixPattern[i];

				if (bEscape == false && ch == MetaSymbol.ESCAPE)
				{
					queuePostfix.Enqueue(ch);
					bEscape = true;
					continue;
				}

				if (bEscape == true)
				{
					queuePostfix.Enqueue(ch);
					bEscape = false;
					continue;
				}
				switch (ch)
				{
					case MetaSymbol.OPEN_PREN:
						stackOperator.Push(ch);
						break;
					case MetaSymbol.CLOSE_PREN:
						while (stackOperator.Peek() != MetaSymbol.OPEN_PREN)
							queuePostfix.Enqueue(stackOperator.Pop());
						stackOperator.Pop();  // pop the '('

						break;
					default:
						while (stackOperator.Count > 0)
						{
							char chPeeked = stackOperator.Peek();

							int nPriorityPeek = GetOperatorPriority(chPeeked);
							int nPriorityCurr = GetOperatorPriority(ch);

							if (nPriorityPeek >= nPriorityCurr)
								queuePostfix.Enqueue(stackOperator.Pop());
							else
								break;
						}
						stackOperator.Push(ch);
						break;
				}

			}  // end of for..loop

			while (stackOperator.Count > 0)
			{
				queuePostfix.Enqueue(stackOperator.Pop());
			}

			StringBuilder sb = new StringBuilder();

			while (queuePostfix.Count > 0)
			{
				sb.Append(queuePostfix.Dequeue());
			}
			return sb.ToString();
		}

		/// <summary>
		/// helper function. needed for postfix conversion
		/// </summary>
		/// <param name="chOpt">literal symbol</param>
		/// <returns>priority</returns>
		public static int GetOperatorPriority(char chOpt)
		{
			switch (chOpt)
			{
				case MetaSymbol.OPEN_PREN:
					return 0;
				case MetaSymbol.ALTERNATE:
					return 1;
				case MetaSymbol.CONCANATE:
					return 2;
				case MetaSymbol.ZERO_OR_ONE:
				case MetaSymbol.ZERO_OR_MORE:
				case MetaSymbol.ONE_OR_MORE:
					return 3;
				case MetaSymbol.COMPLEMENT:
					return 4;
				default:
					return 5;
			}
		}
	}
}

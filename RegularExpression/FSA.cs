using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RegularExpression
{
	public class Fsa
	{
		/// <summary>
		/// Reruns a string format of NFA.Set type.
		/// Only used for debugging.
		/// </summary>
		/// <param name="set">Set of state.</param>
		/// <returns>Formatted string</returns>
		public static string SetToString(HashSet<State> set)
		{
			return "{"
				+ (set.Count == 0
				? "Empty"
				: string.Join(", ", set.Select(s => s == null ? string.Empty : s.Id.ToString())))
				+ "}";
		}
		/// <summary>
		/// Converts a regular expression in postfix form to NFA using "Thompson抯 Construction"
		/// </summary>
		/// <param name="sRegExPosfix">Regular expression in postfix form (pattern)</param>
		/// <returns>Start state of the NFA</returns>
		public static State CreateNfa(string sRegExPosfix)
		{
			Stack<NfaLink> stackNfa = new Stack<NfaLink>();
			NfaLink expr = null;
			NfaLink exprA = null;
			NfaLink exprB = null;
			NfaLink exprNew = null;
			bool bEscape = false;

			foreach (char ch in sRegExPosfix)
			{
				if (bEscape == false && ch == MetaSymbol.ESCAPE)
				{
					bEscape = true;
					continue;
				}

				if (bEscape == true)
				{
					exprNew = new NfaLink();

					exprNew.StartState.AddTransition(ch.ToString(), exprNew.FinalState);

					stackNfa.Push(exprNew);

					bEscape = false;
					continue;
				}

				switch (ch)
				{
					case MetaSymbol.ZERO_OR_MORE:  // A*  Kleene closure

						exprA = stackNfa.Pop();
						exprNew = new NfaLink();

						exprA.FinalState.AddTransition(MetaSymbol.EPSILON, exprA.StartState);
						exprA.FinalState.AddTransition(MetaSymbol.EPSILON, exprNew.FinalState);

						exprNew.StartState.AddTransition(MetaSymbol.EPSILON, exprA.StartState);
						exprNew.StartState.AddTransition(MetaSymbol.EPSILON, exprNew.FinalState);
						 
						stackNfa.Push(exprNew);

						break;
					case MetaSymbol.ALTERNATE:  // A|B
						exprB = stackNfa.Pop();
						exprA = stackNfa.Pop();

						exprNew = new NfaLink();

						exprA.FinalState.AddTransition(MetaSymbol.EPSILON, exprNew.FinalState);
						exprB.FinalState.AddTransition(MetaSymbol.EPSILON, exprNew.FinalState);

						exprNew.StartState.AddTransition(MetaSymbol.EPSILON, exprA.StartState);
						exprNew.StartState.AddTransition(MetaSymbol.EPSILON, exprB.StartState);

						stackNfa.Push(exprNew);

						break;

					case MetaSymbol.CONCANATE:  // AB
						exprB = stackNfa.Pop();
						exprA = stackNfa.Pop();

						exprA.FinalState.AddTransition(MetaSymbol.EPSILON, exprB.StartState);

						exprNew = new NfaLink(exprA.StartState, exprB.FinalState);

						stackNfa.Push(exprNew);

						break;

					case MetaSymbol.ONE_OR_MORE:  // A+ => AA* => A.A*

						exprA = stackNfa.Pop();
						exprNew = new NfaLink();

						exprNew.StartState.AddTransition(MetaSymbol.EPSILON, exprA.StartState);
						exprNew.FinalState.AddTransition(MetaSymbol.EPSILON, exprA.StartState);
						exprA.FinalState.AddTransition(MetaSymbol.EPSILON, exprNew.FinalState);

						stackNfa.Push(exprNew);

						break;
					case MetaSymbol.ZERO_OR_ONE:  // A? => A|empty  
						exprA = stackNfa.Pop();
						exprNew = new NfaLink();

						exprNew.StartState.AddTransition(MetaSymbol.EPSILON, exprA.StartState);
						exprNew.StartState.AddTransition(MetaSymbol.EPSILON, exprNew.FinalState);
						exprA.FinalState.AddTransition(MetaSymbol.EPSILON, exprNew.FinalState);

						stackNfa.Push(exprNew);

						break;
					case MetaSymbol.ANY_ONE_CHAR:
						exprNew = new NfaLink();
						exprNew.StartState.AddTransition(MetaSymbol.ANY_ONE_CHAR_TRANS, exprNew.FinalState);
						stackNfa.Push(exprNew);
						break;

					case MetaSymbol.COMPLEMENT:  // ^ 

						exprA = stackNfa.Pop();

						NfaLink exprDummy = new NfaLink();

						exprDummy.StartState.AddTransition(MetaSymbol.DUMMY, exprDummy.FinalState);

						exprA.FinalState.AddTransition(MetaSymbol.EPSILON, exprDummy.StartState);

						NfaLink exprAny = new NfaLink();

						exprAny.StartState.AddTransition(MetaSymbol.ANY_ONE_CHAR_TRANS, exprAny.FinalState);


						exprNew = new NfaLink();
						exprNew.StartState.AddTransition(MetaSymbol.EPSILON, exprA.StartState);
						exprNew.StartState.AddTransition(MetaSymbol.EPSILON, exprAny.StartState);

						exprAny.FinalState.AddTransition(MetaSymbol.EPSILON, exprNew.FinalState);
						exprDummy.FinalState.AddTransition(MetaSymbol.EPSILON, exprNew.FinalState);

						stackNfa.Push(exprNew);

						break;
					default:
						exprNew = new NfaLink();
						exprNew.StartState.AddTransition(ch.ToString(), exprNew.FinalState);

						stackNfa.Push(exprNew);

						break;

				} // end of switch statement
			}  // end of for..each loop

			Debug.Assert(stackNfa.Count == 1);
			expr = stackNfa.Pop();  // pop the very last one.  YES, THERE SHOULD ONLY BE ONE LEFT AT THIS POINT
			expr.FinalState.AcceptingState = true;  // the very last state is the accepting state of the NFA

			return expr.StartState;  // return the start state of NFA

		}  // end of CreateNfa method


		/// <summary>
		/// Finds all state reachable from the specific state on Epsilon transition
		/// </summary>
		/// <param name="stateStart">State from which search begins</param>
		/// <returns>A set of all state reachable from teh startState on Epsilon transition</returns>
		public static HashSet<State> E_Closure(State stateStart)
		{
			HashSet<State> setProcessed = new HashSet<State>();
			HashSet<State> setUnprocessed = new HashSet<State>
			{
				stateStart
			};
			while (setUnprocessed.Count > 0)
			{
				State state = setUnprocessed.FirstOrDefault();

				setProcessed.Add(state);
				setUnprocessed.Remove(state);

				foreach (State stateEpsilon in
					state.GetTransitions(MetaSymbol.EPSILON))
				{
					if (!setProcessed.Contains(stateEpsilon))
					{
						setUnprocessed.Add(stateEpsilon);
					}
				}
			}
			return setProcessed;
		}

		/// <summary>
		/// Finds all state reachable from the set of states on Epsilon transition
		/// </summary>
		/// <param name="setState">Set of states to search from</param>
		/// <returns></returns>
		public static HashSet<State> E_Closure(HashSet<State> setState)
		{
			HashSet<State> setAllEnclosure = new HashSet<State>();

			foreach (State state in setState)
			{
				setAllEnclosure.UnionWith(E_Closure(state));
			}
			return setAllEnclosure;
		}

		/// <summary>
		/// Gets Move of a set states.
		/// </summary>
		/// <param name="setState">Set of state for which to get Move </param>
		/// <param name="chInputSymbol">Input symbol</param>
		/// <returns>Set of Move</returns>
		public static HashSet<State> Move(HashSet<State> setState, string sInputSymbol)
		{
			HashSet<State> set = new HashSet<State>();

			foreach (State state in setState)
			{
				set.UnionWith(Move(state, sInputSymbol));
			}
			return set;
		}

		/// <summary>
		/// Gets Move of a state.
		/// </summary>
		/// <param name="state">state for which to get Move</param>
		/// <param name="chInputSymbol">Input symbol</param>
		/// <returns>Set of Move</returns>
		public static HashSet<State> Move(State state, string sInputSymbol)
		{
			return state.GetTransitions(sInputSymbol);
		}

		/// <summary>
		/// Converts NFA to DFA using "Subset Construction"
		/// </summary>
		/// <param name="stateStartNfa">Starting state of NFA</param>
		/// <param name="setMasterDfa">Contains set of all DFA states when this function returns</param>
		/// <returns>Starting state of DFA</returns>
		public static State ConvertToDfa(State stateStartNfa)
		{
			HashSet<string> setAllInput = new HashSet<string>();
			HashSet<State> setAllState = new HashSet<State>();

			GetAllStateAndInput(stateStartNfa, setAllState, setAllInput);
			setAllInput.Remove(MetaSymbol.EPSILON);

			NfaToDfaHelper helper = new NfaToDfaHelper();
			HashSet<State> setMove = null;
			HashSet<State> setE_closure = null;

			// first, we get E_Closure of the start state of NFA ( just following the algorithm)
			setE_closure = E_Closure(stateStartNfa);

			State stateStartDfa = new State();  // create a new DFA state to represent the above E_Closure

			// NOTE: 
			// we keep track of the NFA E_Closure and the DFA state that represent the E_Closure.
			// we maintain a relationship between the NFA E_Closure and DFA state that represents the NFA E_Closure.
			// all these are done in the NfaToDfaHelper class.

			if (IsAcceptingGroup(setE_closure))
			{
				stateStartDfa.AcceptingState = true;
			}

			helper.AddDfaState(stateStartDfa, setE_closure);
			// please see "subset construction" algorithm
			// for clear understanding

			State stateT = null;
			HashSet<State> setT = null;
			State stateU = null;

			while ((stateT = helper.GetNextUnmarkedDfaState()) != null)
			{
				helper.Mark(stateT);   // flag it to indicate that we have processed this state.

				// the DFA state stateT represents a set of NFA E_Closure.
				// so, we retrieve the E_Closure.
				setT = helper.GetE_ClosureByDfaState(stateT);

				foreach (string str in setAllInput)
				{
					setMove = Move(setT, str);

					if (setMove.Count > 0)
					{
						setE_closure = E_Closure(setMove);

						stateU = helper.FindDfaStateByE_Closure(setE_closure);

						if (stateU == null) // so set setEnclosure must be a new one and we should crate a new DFA state
						{
							stateU = new State();
							if (IsAcceptingGroup(setE_closure))
							{
								stateU.AcceptingState = true;
							}
							helper.AddDfaState(stateU, setE_closure);  // add new state (as unmarked by default)
						}

						stateT.AddTransition(str, stateU);
					}

				}  // end of foreach..loop

			}  // end of while..loop

			return stateStartDfa;

		}  // end of ConvertToDfa method


		/// <summary>
		/// Converts DFA to Minimum DFA or DFA M'.
		/// </summary>
		/// <param name="stateStartDfa">Starting state of DFA</param>
		/// <param name="setMasterDfa">Set of all DFA state (including the starting one)</param>
		/// <param name="setInputSymbol">Set of all input symbol</param>
		/// <returns>Starting state of DFA M'</returns>
		public static State ReduceDfa(State stateStartDfa)
		{
			HashSet<string> setInputSymbol = new HashSet<string>();
			HashSet<State> setAllDfaState = new HashSet<State>();

			GetAllStateAndInput(stateStartDfa, setAllDfaState, setInputSymbol);

			State stateStartReducedDfa = null;   // start state of the Reduced DFA

			List<HashSet<State>> arrGroup = null;  // master array of all possible partitions/groups

			// STEP 1: partition the DFS states.
			// we do this by calling another method 
			arrGroup = PartitionDfaGroups(setAllDfaState, setInputSymbol);

			// NOTE: arrGroup now contains all possible groups for all the DFA state.


			// STEP 2: now we go through all the groups and select a group representative for each group.
			// eventually the representative becomes one of the state in DFA M'.  All other members of the groups get eliminated (deleted).
			foreach (HashSet<State> setGroup in arrGroup)
			{
				bool bAcceptingGroup = IsAcceptingGroup(setGroup);  // see if the group contains any accepting state
				bool bStartingGroup = setGroup.Contains(stateStartDfa); // check if the group contains the starting DFA state

				// choose group representative
				State stateRepresentative = setGroup.FirstOrDefault(); // just choose the first one as group representative

				// should the representative be start state of DFA M'
				if (bStartingGroup)
				{
					stateStartReducedDfa = stateRepresentative;
				}
				// should the representative be an accepting state of DFA M'
				if (bAcceptingGroup)
				{
					stateRepresentative.AcceptingState = true;
				}
				if (setGroup.Count == 1)
				{
					continue;  // no need for further processing
				}
				// STEP 3: remove the representative from its group
				// and replace all the references of the remaining member of the group with the representative
				setGroup.Remove(stateRepresentative);

				// state to be replaced with the group representative
				int nReplecementCount = 0;
				foreach (State stateToBeReplaced in setGroup)
				{
					setAllDfaState.Remove(stateToBeReplaced);  // remove this member from the master set as well

					foreach (State state in setAllDfaState)
					{
						nReplecementCount += state.ReplaceTransitionState(stateToBeReplaced, stateRepresentative);
					}
					// here, in C++, you would actually delete the stateA object by calling:
					// delete stateToBeRemoved;
				}
			}  // end of outer foreach..loop

			//  STEP 4: now remove all "dead states"
			setAllDfaState.RemoveWhere(state => state.IsDeadState());

			return stateStartReducedDfa;
		}

		/// <summary>
		/// Partitions set of all DFA states into smaller groups (according to the partition rules).
		/// Please see notes for detail of partitioning DFA.
		/// </summary>
		/// <param name="setMasterDfa">Set of all DFA states</param>
		/// <param name="setInputSymbol">Set of all input symbol</param>
		/// <returns>Array of DFA groups</returns>
		public static List<HashSet<State>> PartitionDfaGroups(HashSet<State> setMasterDfa, HashSet<string> setInputSymbol)
		{
			List<HashSet<State>> arrGroup = new List<HashSet<State>>();  // array of all set (group) of DFA states.
			Map<HashSet<State>, State> map = new Map<HashSet<State>, State>();   // to keep track of which member transition into which group
			HashSet<State> setEmpty = new HashSet<State>();

			// first we need to create two partition of the setMasterDfa:
			// one with all the accepting states and the other one with all the non-accepting states
			HashSet<State> setAccepting = new HashSet<State>();  // group of all accepting state
			HashSet<State> setNonAccepting = new HashSet<State>();  // group of all non-accepting state

			foreach (State state in setMasterDfa)
			{
				if (state.AcceptingState)
				{
					setAccepting.Add(state);
				}
				else
				{
					setNonAccepting.Add(state);
				}
			}
			if (setNonAccepting.Count > 0)
			{
				arrGroup.Add(setNonAccepting);  // add this newly created partition to the master list
			}
			// for accepting state, there should always be at least one state, if NOT then there must be something wrong somewhere
			arrGroup.Add(setAccepting);   // add this newly created partition to the master list
	
			// now we iterate through these two partitions and see if they can be further partitioned.
			// we continue the iteration until no further partitioning is possible.

			IEnumerator<string> iterInput = setInputSymbol.GetEnumerator();

			iterInput.Reset();

			while (iterInput.MoveNext())
			{
				string sInputSymbol = iterInput.Current;

				int nPartionIndex = 0;

				while (nPartionIndex < arrGroup.Count)
				{
					HashSet<State> setToBePartitioned = arrGroup[nPartionIndex];

					nPartionIndex++;

					if (setToBePartitioned.Count <= 1)
					{
						continue;   // because we can't partition a set with zero or one member in it
					}
					foreach (State state in setToBePartitioned.ToList())
					{
						State[] arrState = state.GetTransitions(sInputSymbol).ToArray();

						if (arrState != null)
						{
							Debug.Assert(arrState.Length == 1);

							// since the state is DFA state,
							// this array should contain only ONE state

							map.Add(FindGroup(arrGroup, arrState[0]), state);
						}
						else
						{
							// no transition exists, so transition to empty set
							//setEmpty = new Set();
							map.Add(setEmpty, state);  // keep a map of which states transition into which group
						}
					}  // end of foreach (object objState in setToBePartitioned)
					if (map.Count > 1)  // means some states transition into different groups
					{
						arrGroup.Remove(setToBePartitioned);
						foreach (HashSet<State> set in map.Values)
						{
							arrGroup.Add(set);
						}
						nPartionIndex = 0;  // we want to start from the beginning again
						iterInput.Reset();  // we want to start from the beginning again
					}
					map.Clear();
				}  // end of while..loop


			}  // end of foreach (object objString in setInputSymbol)

			return arrGroup;
		}  // end of PartitionDfaSet method

		/// <summary>
		/// Helper function.  Finds a set in a array of set for a particular state.
		/// </summary>
		/// <param name="arrGroup">Array of set of states</param>
		/// <param name="state">State to search for</param>
		/// <returns>Set the state belongs to</returns>
		public static HashSet<State> FindGroup(List<HashSet<State>> arrGroup, State state)
		{
			return arrGroup.Where(set => set.Contains(state)).FirstOrDefault();
		}

		/// <summary>
		/// Helper function. Check to see if a set contains any accepting state.
		/// </summary>
		/// <param name="setGroup">Set of state</param>
		/// <returns>true if set contains any accepting states, otherwise false</returns>
		public static bool IsAcceptingGroup(HashSet<State> setGroup)
		{
			return setGroup.Any(s => s.AcceptingState);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="stateStart">start state of the model</param>
		/// <param name="setProcessed">when function returns, this set contains all the states</param>
		/// <param name="setAllState">when function returns, this set contains all the input symbols</param>
		public static void GetAllStateAndInput(State stateStart, HashSet<State> setAllState, HashSet<string> setInputSymbols)
		{
			HashSet<State> setUnprocessed = new HashSet<State>
			{
				stateStart
			};
			while (setUnprocessed.Count > 0)
			{
				State state = setUnprocessed.FirstOrDefault();

				setAllState.Add(state);
				setUnprocessed.Remove(state);

				foreach (string sSymbol in state.GetAllKeys())
				{
					setInputSymbols.Add(sSymbol);

					foreach (State stateEpsilon in state.GetTransitions(sSymbol))
					{
						if (!setAllState.Contains(stateEpsilon))
						{
							setUnprocessed.Add(stateEpsilon);
						}
					}
					// end of inner foreach..loop
				}  // end of outer foreach..loop
			}  // end of outer while..loop      
		}

		public static int GetSerializedFsa(State stateStart, StringBuilder sb)
		{
			HashSet<State> setAllState = new HashSet<State>();
			HashSet<string> setAllInput = new HashSet<string>();
			GetAllStateAndInput(stateStart, setAllState, setAllInput);
			return GetSerializedFsa(stateStart, setAllState, setAllInput, sb);
		}
		public static int GetSerializedFsa(State stateStart, HashSet<State> setAllState, HashSet<string> setAllSymbols, StringBuilder sb)
		{
			int nLineLength = 0;
			int nMinWidth = 6;
			string sLine = String.Empty;
			string sFormat = String.Empty;
			setAllSymbols.Remove(MetaSymbol.EPSILON);
			setAllSymbols.Add(MetaSymbol.EPSILON); // adds it at the end;

			// construct header row and format string
			string[] arrObj = new string[setAllSymbols.Count + 1];// the extra one because of the first State column
			arrObj[0] = "State";
			sFormat = "{0,-8}";
			int i = 0;

			foreach (string sSymbol in setAllSymbols)
			{
				arrObj[i++ + 1] = sSymbol;

				sFormat += " | ";
				sFormat += "{" + (i + 1).ToString() + ",-" + Math.Max(Math.Max(sSymbol.Length, nMinWidth), sSymbol.ToString().Length) + "}";
			}
			sLine = String.Format(sFormat, arrObj);
			nLineLength = Math.Max(nLineLength, sLine.Length);
			sb.AppendLine((string.Empty).PadRight(nLineLength, '-'));
			sb.AppendLine(sLine);
			sb.AppendLine((string.Empty).PadRight(nLineLength, '-'));

			// construct the rows for transition
			int nTransCount = 0;
			foreach (State state in setAllState)
			{
				arrObj[0] = (state.Equals(stateStart) ? ">" + state.ToString() : state.ToString());

				i = 0;
				foreach (string sSymbol in setAllSymbols)
				{
					State[] arrStateTo = state.GetTransitions(sSymbol).ToArray();

					string sTo = String.Empty;
					if (arrStateTo != null && arrStateTo.Length > 0)
					{
						nTransCount += arrStateTo.Length;

						sTo = arrStateTo[0].ToString();

						for (int j = 1; j < arrStateTo.Length; j++)
							sTo += ", " + arrStateTo[j].ToString();
					}
					else
						sTo = "--";
					arrObj[i++ + 1] = sTo;
				}

				sLine = String.Format(sFormat, arrObj);
				sb.AppendLine(sLine);
				nLineLength = Math.Max(nLineLength, sLine.Length);
			}

			sFormat = "State Count: {0}, Input Symbol Count: {1}, Transition Count: {2}";
			sLine = String.Format(sFormat, setAllState.Count, setAllSymbols.Count, nTransCount);
			nLineLength = Math.Max(nLineLength, sLine.Length);
			sb.AppendLine((string.Empty).PadRight(nLineLength, '-'));
			sb.AppendLine(sLine);
			nLineLength = Math.Max(nLineLength, sLine.Length);
			setAllSymbols.Remove(MetaSymbol.EPSILON);

			return nLineLength;
		}
	}
}

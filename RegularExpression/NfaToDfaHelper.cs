using System.Collections.Generic;
using System.Linq;

namespace RegularExpression
{
    /// <summary>
    /// this class simply the conversion process from the NFA to DFA.
    /// Many other implementation of NFA->DFA does this in the same method.
    /// But I thought having a helper class will make the code clearer.
    /// </summary>
    public class NfaToDfaHelper
    {
        /// <summary>
        /// array of of DfaStateRecord
        /// </summary>
        protected Dictionary<State, DfaStateRecord> states = new Dictionary<State, DfaStateRecord>();

        /// <summary>
        /// A nested class.
        /// A row with three fields. to store DFA states with two other attributes.
        /// </summary>
        public class DfaStateRecord
        {
            /// <summary>
            /// HashSet of NFA state from which the DFA state was created
            /// </summary>
            public HashSet<State> SetE_Closure = null;
            /// <summary>
            /// a flag to indicate whether or not this DFA state has been processed.
            /// See the SubHashSet Construction algorithm for detail
            /// </summary>
            public bool Marked = false;
        }

        public NfaToDfaHelper()
        {

        }
        /// <summary>
        /// Simply adds newly created DFA state to the table
        /// </summary>
        /// <param name="stateDfa">the newly created DFA state</param>
        /// <param name="setE_Closure">HashSet of Enclosure that was used to create the DFA state</param>
        public void AddDfaState(State stateDfa, HashSet<State> setE_Closure)
        {
            this.states[stateDfa] = new DfaStateRecord()
            {
                SetE_Closure = setE_Closure
            };
        }

		/// <summary>
		/// finds a DFA state using a HashSet of Enclosure state as search criteria.
		/// because all DFAs are constructed from a HashSet of NFA state
		/// </summary>
		/// <param name="setE_closure">HashSet of Enclosure state as search criteria</param>
		/// <returns>if found, returns the DFA state record, or returns null</returns>
		public State FindDfaStateByE_Closure(HashSet<State> setE_closure)
        {
            return (from state in this.states where state.Value.SetE_Closure.SetEquals(setE_closure) select state.Key).FirstOrDefault();
        }  // end of FindDfaStateByEnclosure method

        public HashSet<State> GetE_ClosureByDfaState(State state)
        {
            return this.states.TryGetValue(state, out var value) ? value?.SetE_Closure : null;
        }
        public State GetNextUnmarkedDfaState()
        {
            return (from state in this.states where !state.Value.Marked select state.Key).FirstOrDefault();
        }
        public void Mark(State state)
        {
            this.states[state].Marked = true ;
        }

        /// <summary>
        /// checks to see if a HashSet contains any state that is an accepting state.
        /// used in NFA to DFA conversion
        /// </summary>
        /// <param name="HashSetState">HashSet of state</param>
        /// <returns>true if HashSet contains at least one accepting state, else false</returns>

        public ICollection<State> GetAllDfaState()
        {
            return this.states.Keys;
        }
    }
}

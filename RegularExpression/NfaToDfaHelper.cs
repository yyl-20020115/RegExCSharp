using System;
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
        protected class DfaStateRecord
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
		public virtual ICollection<State> AllDfaState => this.states.Keys;

		public NfaToDfaHelper() { }

        /// <summary>
        /// Simply adds newly created DFA state to the table
        /// </summary>
        /// <param name="stateDfa">the newly created DFA state</param>
        /// <param name="setE_Closure">HashSet of Enclosure that was used to create the DFA state</param>
        public virtual void AddDfaState(State stateDfa, HashSet<State> setE_Closure)
        {
			if (stateDfa == null) throw new ArgumentNullException(nameof(stateDfa));
			if (setE_Closure == null) throw new ArgumentNullException(nameof(setE_Closure));

			this.states[stateDfa] = new DfaStateRecord()
            {
                SetE_Closure = setE_Closure
            };
        }

		/// <summary>
		/// finds a DFA state using a HashSet of Enclosure state as search criteria.
		/// because all DFAs are constructed from a HashSet of NFA state
		/// </summary>
		/// <param name="setE_Closure">HashSet of Enclosure state as search criteria</param>
		/// <returns>if found, returns the DFA state record, or returns null</returns>
		public virtual State FindDfaStateByE_Closure(HashSet<State> setE_Closure)
        {
			if (setE_Closure == null) throw new ArgumentNullException(nameof(setE_Closure));

			return (from state in this.states where state.Value.SetE_Closure.SetEquals(setE_Closure) select state.Key).FirstOrDefault();
        }  // end of FindDfaStateByEnclosure method

        public virtual HashSet<State> GetE_ClosureByDfaState(State state)
        {
			if (state == null) throw new ArgumentNullException(nameof(state));

			return this.states.TryGetValue(state, out var value) ? value?.SetE_Closure : null;
        }
        public virtual State GetNextUnmarkedDfaState()
        {
            return (from state in this.states where !state.Value.Marked select state.Key).FirstOrDefault();
        }
        public virtual void Mark(State state)
        {
			if (state == null) throw new ArgumentNullException(nameof(state));

			this.states[state].Marked = true ;
        }
	}
}

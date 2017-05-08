using System;
using System.Collections.Generic;
using System.Linq;

namespace RegularExpression
{
	public class State
    {
        // the one and only Id tracker
        protected static int stateId = 0;

        public static void ResetId()
        {
            stateId = 0;
        }

        // HashSet of transition objects
        protected Map<string,State> states = new Map<string,State>();

        public readonly int Id = stateId++;

        public virtual bool AcceptingState { get; set; } = false;

		public virtual IEnumerable<string> AllKeys => this.states.Keys;

		public virtual bool IsDeadState
		{
			get
			{
				if (this.AcceptingState)
				{
					return false;
				}
				if (this.states.Count == 0)
				{
					return false;
				}
				foreach (HashSet<State> setToState in this.states.Values)
				{
					foreach (State state in setToState)  // in a DFA, it should only iterate once
					{
						if (!state.Equals(this))
						{
							return false;
						}
					}
				}

				return true;
			}
		}

		public State() { }
        
        public virtual void AddTransition(string sInputSymbol, State stateTo)
        {
			if (sInputSymbol == null) throw new ArgumentNullException(nameof(sInputSymbol));

			this.states.Add(sInputSymbol, stateTo);
        }

        public virtual HashSet<State> GetTransitions(string sInputSymbol)
        {
			if (sInputSymbol == null) throw new ArgumentNullException(nameof(sInputSymbol));

			return this.states.TryGetValue(sInputSymbol, out var value)
                ? value : new HashSet<State>();
        }

        public virtual State GetSingleTransition(string sInputSymbol)
        {
			if (sInputSymbol == null) throw new ArgumentNullException(nameof(sInputSymbol));

			return this.states.TryGetValue(sInputSymbol, out var value)
                ? value.FirstOrDefault() : null;
        }

        public virtual void RemoveTransition(string sInputSymbol)
        {
			if (sInputSymbol == null) throw new ArgumentNullException(nameof(sInputSymbol));

			this.states.Remove(sInputSymbol);
        }

        public virtual int ReplaceTransitionState(State stateOld, State stateNew)
        {
			if (stateOld == null) throw new ArgumentNullException(nameof(stateOld));

			if (stateNew == null) throw new ArgumentNullException(nameof(stateNew));

			int nReplacementCount = 0;

            foreach (HashSet<State> HashSetTrans in states.Values)
            {
                if (HashSetTrans.Contains(stateOld))
                {
                    HashSetTrans.Remove(stateOld);
                    HashSetTrans.Add(stateNew);
                    nReplacementCount++;
                }
            }
            return nReplacementCount;
        }

		public override string ToString()
        {
            return this.AcceptingState ? "{s"+this.Id+"}" : "s"+this.Id;
        }
    }
}

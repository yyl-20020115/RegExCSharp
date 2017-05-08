using System.Collections;
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
        public bool AcceptingState { get; set; } = false;
        public bool IsDeadState()
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
                    if (state.Equals(this) == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        public State()
        {
        }
        
        public void AddTransition(string sInputSymbol, State stateTo)
        {
            this.states.Add(sInputSymbol, stateTo);
        }

        public HashSet<State> GetTransitions(string sInputSymbol)
        {
            return this.states.TryGetValue(sInputSymbol, out var value)
                ? value : new HashSet<State>();
        }

        public State GetSingleTransition(string sInputSymbol)
        {
            return this.states.TryGetValue(sInputSymbol, out var value)
                ? value.ToArray()?[0] : null;
        }
        public void RemoveTransition(string sInputSymbol)
        {
            this.states.Remove(sInputSymbol);
        }

        public int ReplaceTransitionState(State stateOld, State stateNew)
        {
            int nReplacementCount = 0;
            foreach (HashSet<State> HashSetTrans in states.Values)
            {
                if (HashSetTrans.Contains(stateOld) == true)
                {
                    HashSetTrans.Remove(stateOld);
                    HashSetTrans.Add(stateNew);
                    nReplacementCount++;
                }
            }
            return nReplacementCount;
        }
        public ICollection GetAllKeys()
        {
            return this.states.Keys;
        }
        public override string ToString()
        {
            string s = "s" + this.Id.ToString();
            if (this.AcceptingState)
            {
                s = "{" + s + "}";
            }
            return s;
        }
    }
}

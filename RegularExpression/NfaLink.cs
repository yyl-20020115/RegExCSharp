namespace RegularExpression
{
	/// <summary>
	/// see NfaDiagram.txt file.
	/// this class represent a box in that diagram.
	/// this class helps constructing the NFA from the regular expression
	/// </summary>
	public class NfaLink
    {
        public virtual State StartState { get; protected set; }

        public virtual State FinalState { get; protected set; }

		public NfaLink(State stateFrom = null, State stateTo = null)
        {
            this.StartState = stateFrom ?? new State();
            this.FinalState = stateTo ?? new State();
        }
    }
}

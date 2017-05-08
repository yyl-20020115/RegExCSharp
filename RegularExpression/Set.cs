using System.Collections.Generic;

namespace RegularExpression
{
    /// <summary>
    /// this class represent a Set in Mathematics, nothing else.
    /// Since .NET does not have any built-in class for Set, I decided to create one myself.
    /// Since this program does many Set operations, this class will help writing clean code
    /// Many of the methods of this class have not been used in this program.
    /// </summary>
    public class Set<T> : HashSet<T> //: List<T>
    {
        public Set()
        {

        }
        public Set(IEnumerable<T> collection)
            :base(collection)
        {

        }
    }
}

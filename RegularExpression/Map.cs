using System.Collections.Generic;

namespace RegularExpression
{
	/// <summary>
	/// this class Map many Value object to one Key object  (one-to-many).
	/// Example:
	/// private Map map = new Map();
	/// map.Add(".txt", "notepad.exe");
	/// map.Add(".txt", "wordpad.exe");
	/// map.Add(".jpg", "ie.exe");
	/// map.Add(".jpg", "paint.exe");
	/// 
	/// NFA.HashSet HashSetProgram = map[".txt"];   // get value is always a NFA.HashSet
	/// 
	/// </summary>
	public class Map<TKey, T> : Dictionary<TKey, HashSet<T>>
	{
		public virtual void Add(TKey key, T mapTo)
		{
			if (!this.TryGetValue(key, out HashSet<T> set))
				if (key is HashSet<T>)
					set = (key as HashSet<T>);
				else
					set = new HashSet<T>();
			set.Add(mapTo);

			base[key] = set;
		}
	}
}

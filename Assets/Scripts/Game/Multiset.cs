using System.Collections;
using System.Collections.Generic;

public class Multiset<T> : IEnumerable<T> {

	private Dictionary<T, int> dict;

	public Multiset() {
		dict = new Dictionary<T, int>();
	}

	public Multiset(IEnumerable<T> elements) : this() {
		Add(elements);
	}

	public int Count {
		get {
			int count = 0;
			foreach (KeyValuePair<T, int> kv in dict) {
				count += kv.Value;
			}

			return count;
		}
	}

	public bool Contains(T element) {
		return dict.ContainsKey(element);
	}

	public void Add(T element) {
		if (!dict.ContainsKey(element)) {
			dict[element] = 0;
		}
		dict[element]++;
	}

	public void Add(T element, int amount) {
		for (int i = 0; i < amount; i++) {
			Add(element);
		}
	}

	public void Add(IEnumerable<T> elements) {
		foreach (T element in elements) {
			Add(element);
		}
	}

	public void Remove(T element) {
		if (dict.ContainsKey(element)) {
			if (--dict[element] == 0) {
				dict.Remove(element);
			}
		}
	}

	public T Pop() {
		if (dict.Count == 0) {
			return default(T);
		}

		T poppedElement = default(T);
		foreach (KeyValuePair<T, int> kv in dict) {
			poppedElement = kv.Key;
			break;
		}
		Remove(poppedElement);

		return poppedElement;
	}

	public Multiset<T> ExceptWith(IEnumerable<T> other) {
		Multiset<T> resultSet = new Multiset<T>(this);
		foreach (T element in other) {
			resultSet.Remove(element);
		}

		return resultSet;
	}

	public IEnumerator<T> GetEnumerator() {
		foreach (KeyValuePair<T, int> kv in dict) {
			for (int i = 0; i < kv.Value; i++) {
				yield return kv.Key;
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

}

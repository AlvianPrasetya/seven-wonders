using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stock represents a pile of cards to be dealt.
public abstract class Stock<T> : Pile<T>, ILoadable, IShuffleable, IDealable where T : MonoBehaviour, IMoveable {

	public abstract IEnumerator Load();
	public abstract IEnumerator RandomLoad(int randomSeed);
	public Pile<T>[] rifflePiles;

	public IEnumerator Shuffle(int numIterations, int randomSeed) {
		if (Elements.Count < 2) {
			// Less than 2 cards, no point in shuffling
			yield break;
		}

		System.Random random = new System.Random(randomSeed);
		int numElements = Elements.Count;
		Queue<Coroutine> loadRifflePiles = new Queue<Coroutine>();
		for (int i = 0; i < numIterations; i++) {
			// Move half of the stock to the first riffle pile
			while (Elements.Count != numElements / 2) {
				loadRifflePiles.Enqueue(StartCoroutine(rifflePiles[0].Push(Pop())));
			}
			// Move the other half to the second riffle pile
			while (Elements.Count != 0) {
				loadRifflePiles.Enqueue(StartCoroutine(rifflePiles[1].Push(Pop())));
			}
			while (loadRifflePiles.Count != 0) {
				yield return loadRifflePiles.Dequeue();
			}

			while (rifflePiles[0].Count != 0 && rifflePiles[1].Count != 0) {
				yield return Push(rifflePiles[random.Next(0, rifflePiles.Length)].Pop());
			}
			while (rifflePiles[0].Count != 0) {
				yield return Push(rifflePiles[0].Pop());
			}
			while (rifflePiles[1].Count != 0) {
				yield return Push(rifflePiles[1].Pop());
			}
		}
	}

	public abstract IEnumerator Deal();

}

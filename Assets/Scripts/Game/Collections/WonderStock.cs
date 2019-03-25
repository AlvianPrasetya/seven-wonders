using System.Collections;
using UnityEngine;

public class WonderStock : WonderPile, IRandomLoadable, IShuffleable {

	[System.Serializable]
	public class WonderEntry {
		
		public Wonder[] prefabVariants;

	}

	public WonderEntry[] wonderEntries;
	public WonderPile[] shuffleWonderPiles;

	public IEnumerator Load(int randomSeed) {
		System.Random random = new System.Random(randomSeed);
		foreach (WonderEntry wonderEntry in wonderEntries) {
			Wonder wonder = Instantiate(
				wonderEntry.prefabVariants[random.Next(0, wonderEntry.prefabVariants.Length)],
				transform.position,
				transform.rotation
			);
			yield return Push(wonder);
		}
	}

	public IEnumerator Shuffle(int numIterations, int randomSeed) {
		if (Elements.Count < 2) {
			// Less than 2 wonders, no point in shuffling
			yield break;
		}

		System.Random random = new System.Random(randomSeed);

		for (int i = 0; i < numIterations; i++) {
			// Move each wonder to a random shuffle stock
			while (Elements.Count != 0) {
				yield return shuffleWonderPiles[random.Next(0, shuffleWonderPiles.Length)].Push(Pop());
			}

			// Merge all shuffle stocks
			foreach (WonderPile shuffleWonderPile in shuffleWonderPiles) {
				yield return PushMany(shuffleWonderPile.PopMany(shuffleWonderPile.Count));
			}
		}
	}

	public IEnumerator Deal() {
		foreach (Player player in GameManager.Instance.Players) {
			Wonder wonder = Pop();
			yield return player.wonderSlot.Push(wonder);
			foreach (WonderStage wonderStage in wonder.wonderStages) {
				wonderStage.buryDropArea.onDropEvent.AddListener(player.DecideBury);
			}
		}
	}

	public IEnumerator Dump() {
		while (Elements.Count != 0) {
			Wonder wonder = Elements.Pop();
			Destroy(wonder.gameObject);
			yield return new WaitForSeconds(pushDuration);
		}
	}

}

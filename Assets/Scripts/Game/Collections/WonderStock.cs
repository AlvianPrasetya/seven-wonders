using System.Collections;
using UnityEngine;

public class WonderStock : Stock<Wonder>, IDealable {

	[System.Serializable]
	public class WonderEntry {
		
		public Wonder[] prefabVariants;

	}

	public WonderEntry[] wonderEntries;
	public WonderPile[] wonderRifflePiles;

	protected override void Awake() {
		base.Awake();
		rifflePiles = wonderRifflePiles;
	}

	public override IEnumerator Load() {
		yield return new System.NotImplementedException();
	}

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

	public IEnumerator Deal() {
		foreach (Player player in GameManager.Instance.Players) {
			Wonder wonder = Pop();
			yield return player.SetWonder(wonder);
		}
	}

}

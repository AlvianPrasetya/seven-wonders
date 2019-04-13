using System.Collections;

public class CardStock : Stock<Card> {

	[System.Serializable]
	public class CardEntry {

		public Card cardPrefab;
		public int minPlayers;

	}

	public CardEntry[] initialCardEntries;
	public CardPile[] cardRifflePiles;

	protected override void Awake() {
		base.Awake();
		rifflePiles = cardRifflePiles;
	}

	public override IEnumerator Load() {
		foreach (CardEntry cardEntry in initialCardEntries) {
			if (cardEntry.minPlayers > GameManager.Instance.Players.Count) {
				// Not enough players to put this card into play
				continue;
			}

			Card card = Instantiate(cardEntry.cardPrefab, transform.position, transform.rotation);
			yield return Push(card);
		}
	}

}

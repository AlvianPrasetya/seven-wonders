using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraftAction : IActionable {

	private LeaderCard leaderCard;

	public DraftAction(Card card) {
		leaderCard = (LeaderCard)card;
	}

	public IEnumerator Perform(Player player) {
		yield return player.Decks[DeckType.Leader].Push(leaderCard);
		// Set leader card as drafted
		leaderCard.drafted = true;
	}

	public void Effect(Player player) {
	}

}

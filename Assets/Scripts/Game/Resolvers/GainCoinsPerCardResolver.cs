using System.Collections;
using UnityEngine;

public class GainCoinsPerCardResolver : IResolvable {

	private Player player;
	public int amountPerCard;
	public CardType cardType;
	public Target countTarget;

	public GainCoinsPerCardResolver(Player player, int amountPerCard, CardType cardType, Target countTarget) {
		this.player = player;
		this.amountPerCard = amountPerCard;
		this.cardType = cardType;
		this.countTarget = countTarget;
	}

	public IEnumerator Resolve() {
		int amount = 0;
		switch (countTarget) {
			case Target.Self:
				amount = amountPerCard * player.BuiltCardsByType[cardType].Count;
				break;
			case Target.Neighbours:
				amount = amountPerCard * 
					(player.Neighbours[Direction.West].BuiltCardsByType[cardType].Count +
					player.Neighbours[Direction.East].BuiltCardsByType[cardType].Count);
				break;
			case Target.Neighbourhood:
				amount = amountPerCard *
					(player.Neighbours[Direction.West].BuiltCardsByType[cardType].Count +
					player.BuiltCardsByType[cardType].Count +
					player.Neighbours[Direction.East].BuiltCardsByType[cardType].Count);
				break;
			case Target.Others:
				break;
			case Target.Everyone:
				break;
		}

		Debug.LogFormat("Gain {0} coins", amount);
		yield return player.GainCoin(amount);
	}

}

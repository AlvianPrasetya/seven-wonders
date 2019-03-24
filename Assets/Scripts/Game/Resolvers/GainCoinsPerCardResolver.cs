using System.Collections;

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
		yield return GameManager.Instance.gameCamera.Focus(player);
		switch (countTarget) {
			case Target.Self:
				yield return player.GainCoin(
					amountPerCard * player.BuiltCardsByType[cardType].Count
				);
				break;
			case Target.Neighbours:
				yield return player.GainCoin(
					amountPerCard * 
						(player.Neighbours[Direction.West].BuiltCardsByType[cardType].Count +
						player.Neighbours[Direction.East].BuiltCardsByType[cardType].Count)
				);
				break;
			case Target.Neighbourhood:
				yield return player.GainCoin(
					amountPerCard *
						(player.Neighbours[Direction.West].BuiltCardsByType[cardType].Count +
						player.BuiltCardsByType[cardType].Count +
						player.Neighbours[Direction.East].BuiltCardsByType[cardType].Count)
				);
				break;
			case Target.Others:
				break;
			case Target.Everyone:
				break;
		}
	}

}

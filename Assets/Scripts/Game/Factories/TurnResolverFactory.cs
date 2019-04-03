using System;

public class TurnResolverFactory : Factory {

	public static TurnResolverFactory Instance {
		get {
			if (instance == null) {
				instance = new TurnResolverFactory();
			}
			return instance;
		}
	}
	public Player DoubleTurnPlayer { private get; set; }

	private static TurnResolverFactory instance;

	public override IResolvable[] Create(Player player) {
		throw new NotImplementedException();
	}

	public IResolvable[] Create(DeckType sourceDeck, DeckType targetDeck, Direction direction) {
		if (targetDeck == DeckType.Discard) {
			// Last turn, check if any player can play both cards
			if (DoubleTurnPlayer != null) {
				return new IResolvable[]{
					new DoubleTurnResolver(sourceDeck, targetDeck, direction, DoubleTurnPlayer)
				};
			}
		}

		return new IResolvable[]{
			new TurnResolver(sourceDeck, targetDeck, direction)
		};
	}

}

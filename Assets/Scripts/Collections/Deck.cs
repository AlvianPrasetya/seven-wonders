using System.Collections;

public class Deck : Pile<Card> {

	public IEnumerator Unload(Hand targetHand, Direction pushDirection) {
		yield return targetHand.PushMany(PopMany(Elements.Count), pushDirection);
	}

}

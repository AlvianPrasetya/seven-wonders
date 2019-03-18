using System.Collections;

public class Deck : Pile<Card>, IUnloadable<Card> {

	public IEnumerator Unload(IPushable<Card> targetContainer) {
		yield return targetContainer.PushMany(PopMany(Elements.Count));
	}

}

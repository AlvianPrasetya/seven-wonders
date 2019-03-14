using System.Collections;
using UnityEngine;

public abstract class CardPile : MonoBehaviour {

	public Card[] cardPrefabs;

	public abstract void Push(Card card);
	public abstract Card Pop();
	protected abstract IEnumerator LoadCards();

}

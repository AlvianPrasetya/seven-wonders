using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryTokenDisplay : MonoBehaviour, IPushable<MilitaryToken> {

	public MilitaryTokenSlot[] militaryTokenSlots;

	public IEnumerator Push(MilitaryToken token) {
		foreach (MilitaryTokenSlot slot in militaryTokenSlots) {
			if (slot.Element == null) {
				yield return slot.Push(token);
				break;
			}
		}
	}

	public IEnumerator PushMany(MilitaryToken[] tokens) {
		foreach (MilitaryToken token in tokens) {
			yield return Push(token);
		}
	}

}

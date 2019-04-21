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

	public IEnumerator Remove(MilitaryToken token) {
		foreach (MilitaryTokenSlot slot in militaryTokenSlots) {
			if (slot.Element.type == token.type) {
				yield return slot.Pop();
				break;
			}
		}
	}

	public IEnumerator PushMany(MilitaryToken[] tokens) {
		foreach (MilitaryToken token in tokens) {
			yield return Push(token);
		}
	}

	public int Count(MilitaryTokenType type) {
		int count = 0;
		foreach (MilitaryTokenSlot slot in militaryTokenSlots) {
			if (slot.Element == null) {
				continue;
			}

			if (slot.Element.type == type) {
				count++;
			}
		}
		
		return count;
	}

}

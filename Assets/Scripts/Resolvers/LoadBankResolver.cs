using System.Collections;
using UnityEngine;

public class LoadBankResolver : IResolvable {
	
	public IEnumerator Resolve() {
		yield return GameManager.Instance.bank.Load();
	}

}

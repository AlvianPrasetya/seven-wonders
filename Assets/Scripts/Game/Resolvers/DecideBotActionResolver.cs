using System.Collections;
using UnityEngine;

public class DecideBotActionResolver : IResolvable {

	private Bot bot;

	public DecideBotActionResolver(Bot bot) {
		this.bot = bot;
	}

	public IEnumerator Resolve() {
		bot.IsPlayable = true;
		bot.IsPlayable = false;

		yield return null;
	}

}

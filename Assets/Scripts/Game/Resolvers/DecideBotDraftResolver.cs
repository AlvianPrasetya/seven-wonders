using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecideBotDraftResolver : IResolvable {

	private Bot bot;

	public DecideBotDraftResolver(Bot bot) {
		this.bot = bot;
	}

	public IEnumerator Resolve() {
		bot.IsPlayable = true;

		bot.DecideDraft(bot.hand.GetRandom());

		bot.IsPlayable = false;

		yield return null;
	}

}

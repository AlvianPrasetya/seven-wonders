using System.Collections;

public class GainCoinResolver : IResolvable {

	private Player player;
	private int amount;

	public GainCoinResolver(Player player, int amount) {
		this.player = player;
		this.amount = amount;
	}

	public IEnumerator Resolve() {
		yield return GameManager.Instance.gameCamera.Focus(player);
		yield return player.GainCoin(amount);
	}

}

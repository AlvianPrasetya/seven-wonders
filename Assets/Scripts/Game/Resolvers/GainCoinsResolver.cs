using System.Collections;

public class GainCoinsResolver : IResolvable {

	private Player player;
	private int amount;

	public GainCoinsResolver(Player player, int amount) {
		this.player = player;
		this.amount = amount;
	}

	public IEnumerator Resolve() {
		yield return GameManager.Instance.gameCamera.Focus(player);
		yield return player.GainCoin(amount);
	}

}

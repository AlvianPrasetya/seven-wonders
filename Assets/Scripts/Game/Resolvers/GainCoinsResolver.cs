using System.Collections;

public class GainCoinsResolver : IResolvable {

	private Player player;
	private int amount;

	public GainCoinsResolver(Player player, int amount) {
		this.player = player;
		this.amount = amount;
	}

	public IEnumerator Resolve() {
		IEnumerator gainCoin = player.GainCoin(amount);
		if (player == GameManager.Instance.Player) {
			yield return GameManager.Instance.gameCamera.Focus(player);
			yield return gainCoin;
		} else {
			GameManager.Instance.StartCoroutine(gainCoin);
		}
	}

}

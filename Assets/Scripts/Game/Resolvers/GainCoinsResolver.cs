using System.Collections;

public class GainCoinsResolver : IResolvable {

	public delegate int Count();

	private Player player;
	private Count count;

	public GainCoinsResolver(Player player, Count count) {
		this.player = player;
		this.count = count;
	}

	public IEnumerator Resolve() {
		yield return player.GainCoins(count.Invoke());
	}

}

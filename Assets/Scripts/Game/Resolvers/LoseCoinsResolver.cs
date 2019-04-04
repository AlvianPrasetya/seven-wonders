using System.Collections;

public class LoseCoinsResolver : IResolvable {

	public delegate int Count();

	private Player player;
	private Count count;

	public LoseCoinsResolver(Player player, Count count) {
		this.player = player;
		this.count = count;
	}

	public IEnumerator Resolve() {
		yield return player.LoseCoins(count.Invoke());
	}

}

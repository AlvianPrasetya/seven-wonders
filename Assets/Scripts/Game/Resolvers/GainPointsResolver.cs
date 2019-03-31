using System.Collections;

public class GainPointsResolver : IResolvable {

	private Player player;
	private PointType pointType;
	private int amount;

	public GainPointsResolver(Player player, PointType pointType, int amount) {
		this.player = player;
		this.pointType = pointType;
		this.amount = amount;
	}

	public IEnumerator Resolve() {
		yield return player.GainPoints(pointType, amount);
	}

}

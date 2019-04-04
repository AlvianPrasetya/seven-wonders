using System.Collections;

public class GainPointsResolver : IResolvable {
	
	public delegate int Count();

	private Player player;
	private PointType pointType;
	private Count count;

	public GainPointsResolver(Player player, PointType pointType, Count count) {
		this.player = player;
		this.pointType = pointType;
		this.count = count;
	}

	public IEnumerator Resolve() {
		yield return player.GainPoints(pointType, count.Invoke());
	}

}

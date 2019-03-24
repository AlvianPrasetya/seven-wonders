public class GainFlatCoinsOnBuild : OnBuildEffect {

	public int amount;

	public override void Effect(Player player) {
		GameManager.Instance.EnqueueResolver(new GainCoinsResolver(player, amount), 5);
	}

}

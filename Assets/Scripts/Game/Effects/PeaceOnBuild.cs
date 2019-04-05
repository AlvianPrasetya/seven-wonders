public class PeaceOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		player.IsPeaceful = true;
	}

}

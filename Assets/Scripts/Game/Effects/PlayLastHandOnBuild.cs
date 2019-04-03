using Photon.Pun;

public class PlayLastHandOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		TurnResolver.DoubleTurnPlayer = player;
	}

}

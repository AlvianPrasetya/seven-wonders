using Photon.Pun;

public class ExtraTurnOnBuild : OnBuildEffect {

	public override void Effect(Player player) {
		LastTurnResolver.AddExtraTurnPlayer(player);
	}

}

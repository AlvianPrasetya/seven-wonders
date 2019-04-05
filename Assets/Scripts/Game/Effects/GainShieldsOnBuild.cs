public class GainShieldsOnBuild : OnBuildEffect {

	public int shieldsToGain;

	public override void Effect(Player player) {
		player.AddShield(shieldsToGain);
	}

}

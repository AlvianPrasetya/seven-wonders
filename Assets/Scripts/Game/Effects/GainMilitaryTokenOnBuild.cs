public class GainMilitaryTokenOnBuild : OnBuildEffect {
	
	public MilitaryTokenType militaryTokenType;
	public Target target;
	
	public override void Effect(Player player) {
		// Determine military token to gain
		MilitaryToken militaryToken = null;
		switch (militaryTokenType) {
			case MilitaryTokenType.Victory:
				switch (GameManager.Instance.CurrentAge) {
					case Age.Age1:
						militaryToken = GameManager.Instance.victoryTokenAge1Prefab;
						break;
					case Age.Age2:
						militaryToken = GameManager.Instance.victoryTokenAge2Prefab;
						break;
					case Age.Age3:
						militaryToken = GameManager.Instance.victoryTokenAge3Prefab;
						break;
				}
				break;
			case MilitaryTokenType.Defeat:
				militaryToken = GameManager.Instance.defeatTokenPrefab;
				break;
			case MilitaryTokenType.Draw:
				militaryToken = GameManager.Instance.drawTokenPrefab;
				break;
		}

		switch (target) {
			case Target.Self:
				player.GainMilitaryToken(militaryToken);
				break;
			case Target.Neighbours:
				player.Neighbours[Direction.East].GainMilitaryToken(militaryToken);
				player.Neighbours[Direction.West].GainMilitaryToken(militaryToken);
				break;
			case Target.Neighbourhood:
				player.GainMilitaryToken(militaryToken);
				player.Neighbours[Direction.East].GainMilitaryToken(militaryToken);
				player.Neighbours[Direction.West].GainMilitaryToken(militaryToken);
				break;
			case Target.Others:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					if (targetPlayer == player) {
						continue;
					}

					targetPlayer.GainMilitaryToken(militaryToken);
				}
				break;
			case Target.Everyone:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					targetPlayer.GainMilitaryToken(militaryToken);
				}
				break;
		}
	}
}

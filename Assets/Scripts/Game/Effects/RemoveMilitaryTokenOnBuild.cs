public class RemoveMilitaryTokenOnBuild : OnBuildEffect {
    public MilitaryTokenType militaryTokenType;
	public Target target;
	public int tokenCount;	// -1 means all
	private MilitaryToken militaryToken;

	public override void Effect(Player player) {
		// Determines military token to remove
		switch (militaryTokenType) {
			case MilitaryTokenType.Defeat:
				militaryToken = GameManager.Instance.defeatTokenPrefab;
				break;
			case MilitaryTokenType.Draw:
				militaryToken = GameManager.Instance.drawTokenPrefab;
				break;
		}

		int count = tokenCount == -1 ? player.militaryTokenDisplay.Count(militaryTokenType) : tokenCount;
		
		for (int i = 0; i < count; i++) {
			switch (target) {
				case Target.Self:
					RemoveMilitaryTokenOf(player);	
					break;
				case Target.Neighbours:
					RemoveMilitaryTokenOf(player.Neighbours[Direction.East]);
					RemoveMilitaryTokenOf(player.Neighbours[Direction.West]);
					break;
				case Target.Neighbourhood:
					RemoveMilitaryTokenOf(player);
					RemoveMilitaryTokenOf(player.Neighbours[Direction.East]);
					RemoveMilitaryTokenOf(player.Neighbours[Direction.West]);
					break;
				case Target.Others:
					foreach (Player targetPlayer in GameManager.Instance.Players) {
						if (targetPlayer == player) { continue; }
						RemoveMilitaryTokenOf(targetPlayer);	
					}
					break;
				case Target.Everyone:
					foreach (Player targetPlayer in GameManager.Instance.Players) {
						RemoveMilitaryTokenOf(targetPlayer);
					}
					break;
			}
		}
	}

	private void RemoveMilitaryTokenOf(Player player) {
		if (militaryToken == null) {
			militaryToken = GetLowestPointVictoryTokenOf(player);
		}

		if (militaryToken != null) {
			player.RemoveMilitaryToken(militaryToken);
		}
	}

	private MilitaryToken GetLowestPointVictoryTokenOf(Player player) {
		MilitaryTokenSlot[] slots = player.militaryTokenDisplay.militaryTokenSlots;

		bool isAge1VictoryTokenPresent = false;
		bool isAge2VictoryTokenPresent = false;
		bool isAge3VictoryTokenPresent = false;

		foreach (MilitaryTokenSlot slot in slots) {
			switch (slot.Element.points) {
				case 1:
					isAge1VictoryTokenPresent = true;
					break;
				case 3:
					isAge1VictoryTokenPresent = true;
					break;
				case 5:
					isAge1VictoryTokenPresent = true;
					break;
			}
		}

		if (isAge1VictoryTokenPresent) { return GameManager.Instance.victoryTokenAge1Prefab; }
		if (isAge2VictoryTokenPresent) { return GameManager.Instance.victoryTokenAge2Prefab; }
		if (isAge3VictoryTokenPresent) { return GameManager.Instance.victoryTokenAge3Prefab; }
		return null;
	}
}

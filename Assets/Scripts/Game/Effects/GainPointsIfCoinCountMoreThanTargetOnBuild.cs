using System.Collections.Generic;

public class GainPointsIfCoinCountMoreThanTargetOnBuild : OnBuildEffect {

	public PointType pointType;
	public int amount;
	public Target compareTarget;

	public override void Effect(Player player) {
		GainPointsResolver.Count count = () => {
			List<Player> comparedPlayers = new List<Player>();
			switch (compareTarget) {
				case Target.Self:
					comparedPlayers.Add(player);
					break;
				case Target.Neighbours:
					comparedPlayers.Add(player.Neighbours[Direction.West]);
					comparedPlayers.Add(player.Neighbours[Direction.East]);
					break;
				case Target.Neighbourhood:
					comparedPlayers.Add(player);
					comparedPlayers.Add(player.Neighbours[Direction.West]);
					comparedPlayers.Add(player.Neighbours[Direction.East]);
					break;
				case Target.Others:
					foreach (Player targetPlayer in GameManager.Instance.Players) {
						if (targetPlayer == player) {
							continue;
						}

						comparedPlayers.Add(targetPlayer);
					}
					break;
				case Target.Everyone:
					foreach (Player targetPlayer in GameManager.Instance.Players) {
						comparedPlayers.Add(targetPlayer);
					}
					break;
			}

			foreach (Player comparedPlayer in comparedPlayers) {
				if (player.bank.Count <= comparedPlayer.bank.Count) {
					return 0;
				}
			}

			return amount;
		};

		GameManager.Instance.EnqueueResolver(
			new GainPointsResolver(player, pointType, count),
			Priority.GainPoints
		);
	}

}
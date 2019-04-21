using System.Collections.Generic;

public class LoseCoinsPerWonderStageOnBuild : OnBuildEffect {

	public Target target;
	public int amountPerWonderStage;

	public override void Effect(Player player) {
		Queue<Player> targetPlayers = new Queue<Player>();
		switch (target) {
			case Target.Self:
				targetPlayers.Enqueue(player);
				break;
			case Target.Neighbours:
				targetPlayers.Enqueue(player.Neighbours[Direction.West]);
				targetPlayers.Enqueue(player.Neighbours[Direction.East]);
				break;
			case Target.Neighbourhood:
				targetPlayers.Enqueue(player);
				targetPlayers.Enqueue(player.Neighbours[Direction.West]);
				targetPlayers.Enqueue(player.Neighbours[Direction.East]);
				break;
			case Target.Others:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					if (targetPlayer == player) {
						continue;
					}

					targetPlayers.Enqueue(targetPlayer);
				}
				break;
			case Target.Everyone:
				foreach (Player targetPlayer in GameManager.Instance.Players) {
					targetPlayers.Enqueue(targetPlayer);
				}
				break;
		}
		
		while (targetPlayers.Count != 0) {
			Player targetPlayer = targetPlayers.Dequeue();
			GameManager.Instance.EnqueueResolver(
				new LoseCoinsResolver(
					targetPlayer,
					() => {
						return amountPerWonderStage * targetPlayer.Wonder.BuiltStagesCount;
					}
				),
				Priority.LoseCoins
			);
		}
	}

}

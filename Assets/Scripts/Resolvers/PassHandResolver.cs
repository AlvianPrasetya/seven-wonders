using UnityEngine;

public enum PassDirection { Left, Right };

public class PassHandResolver : IResolver {

	private PassDirection passDirection;

	public PassHandResolver(PassDirection passDirection) {
		this.passDirection = passDirection;
	}

	public void Resolve() {
	}

}

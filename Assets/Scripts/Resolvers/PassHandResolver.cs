using UnityEngine;

public class PassHandResolver : IResolver {

	public enum Direction { Left, Right };

	private Direction direction;

	public PassHandResolver(Direction direction) {
		this.direction = direction;
	}

	public void Resolve() {
	}

}

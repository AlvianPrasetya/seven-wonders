using UnityEngine;

public class AgeResolver : IResolver {

	private int age;
	
	public AgeResolver(int age) {
		this.age = age;
	}
	
	public void Resolve() {
	}

}

public struct ResourceOptions {

	public bool IsProduced {
		get; private set;
	}
	public Resource[] Resources {
		get; private set;
	}

	public ResourceOptions(bool produced, Resource[] resources) {
		IsProduced = produced;
		Resources = resources;
	}

}

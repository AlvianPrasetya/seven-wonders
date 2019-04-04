[System.Serializable]
public class ResourceOptions {

	public bool IsProduced {
		get; private set;
	}
	public Resource[] Resources {
		get; private set;
	}

	public ResourceOptions(bool produced, Resource[] resources) {
		this.IsProduced = produced;
		this.Resources = resources;
	}

}

[System.Serializable]
public class ResourceOptions {

	public bool produced {
		get; private set;
	}
	public Resource[] resources {
		get; private set;
	}

	public ResourceOptions(bool produced, Resource[] resources) {
		this.produced = produced;
		this.resources = resources;
	}

}

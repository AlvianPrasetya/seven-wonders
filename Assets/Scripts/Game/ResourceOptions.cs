[System.Serializable]
public class ResourceOptions {

	public Resource[] resources {
		get; private set;
	}

	public ResourceOptions(Resource[] resources) {
		this.resources = resources;
	}

}

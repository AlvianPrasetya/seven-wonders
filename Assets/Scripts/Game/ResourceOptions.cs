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

	public override bool Equals(object obj) {
		ResourceOptions other = (ResourceOptions)obj;
		return IsProduced.Equals(other.IsProduced) && Resources.Equals(other.Resources);
	}

	public override int GetHashCode() {
		int hash = 0;
		hash = hash * 23 + IsProduced.GetHashCode();
		hash = hash * 23 + Resources.GetHashCode();
		return hash;
	}

}

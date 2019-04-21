public struct Resource {

	public bool IsProduced {
		get; private set;
	}
	public ResourceType[] ResourceTypes {
		get; private set;
	}

	public Resource(bool produced, ResourceType[] resourceTypes) {
		IsProduced = produced;
		ResourceTypes = resourceTypes;
	}

	public override bool Equals(object obj) {
		Resource other = (Resource)obj;
		return IsProduced.Equals(other.IsProduced) && ResourceTypes.Equals(other.ResourceTypes);
	}

	public override int GetHashCode() {
		int hash = 0;
		hash = hash * 23 + IsProduced.GetHashCode();
		hash = hash * 23 + ResourceTypes.GetHashCode();
		return hash;
	}

}

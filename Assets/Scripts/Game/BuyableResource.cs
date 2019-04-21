public struct BuyableResource {

	public Resource Resource {
		get; private set;
	}
	public Payment Cost {
		get; private set;
	}

	public BuyableResource(Resource resource, Payment cost) {
		Resource = resource;
		Cost = cost;
	}

	public override bool Equals(object obj) {
		BuyableResource other = (BuyableResource)obj;
		return Resource.Equals(other.Resource) && Cost.Equals(other.Cost);
	}

	public override int GetHashCode() {
		int hash = 0;
		hash = hash * 23 + Resource.GetHashCode();
		hash = hash * 23 + Cost.GetHashCode();
		return hash;
	}

}

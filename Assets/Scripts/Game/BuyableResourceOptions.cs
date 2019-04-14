public struct BuyableResourceOptions {

	public ResourceOptions ResourceOptions {
		get; private set;
	}
	public Payment Cost {
		get; private set;
	}

	public BuyableResourceOptions(ResourceOptions resourceOptions, Payment cost) {
		ResourceOptions = resourceOptions;
		Cost = cost;
	}

	public override bool Equals(object obj) {
		BuyableResourceOptions other = (BuyableResourceOptions)obj;
		return ResourceOptions.Equals(other.ResourceOptions) && Cost.Equals(other.Cost);
	}

	public override int GetHashCode() {
		int hash = 0;
		hash = hash * 23 + ResourceOptions.GetHashCode();
		hash = hash * 23 + Cost.GetHashCode();
		return hash;
	}

}

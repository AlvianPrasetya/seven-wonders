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

}

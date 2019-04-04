[System.Serializable]
public class ScienceOptions {

	public bool IsProduced {
		get; private set;
	}
	public Science[] Sciences {
		get; private set;
	}

	public ScienceOptions(bool produced, Science[] sciences) {
		this.IsProduced = produced;
		this.Sciences = sciences;
	}

}

[System.Serializable]
public class Science {

	public bool IsProduced {
		get; private set;
	}
	public ScienceType[] ScienceTypes {
		get; private set;
	}

	public Science(bool produced, ScienceType[] scienceTypes) {
		this.IsProduced = produced;
		this.ScienceTypes = scienceTypes;
	}

}

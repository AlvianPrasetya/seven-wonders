public enum Direction { West, East };
public enum StockType {
	RawMaterial, ManufacturedGoods, Civilian, Scientific, Commercial, Military, Guild, City, Leader, Wonder,
	Age1, Age2, Age3
}
public enum DeckType {
	Wonder, Age1, Age2, Age3, WestDeck, EastDeck
}
public enum Structure { RawMaterial, ManufacturedGoods, Civilian, Scientific, Commercial, Military, Guild, City, Leader, Wonder };
public enum Age { Age1, Age2, Age3 };
public enum Facing { Up, Down };

public class Constant {

	public const float DistanceEpsilon = 0.01f;
	public const float AngleEpsilon = 0.1f;

}

using System;

public enum Direction { West, East };
public enum StockType {
	RawMaterial, ManufacturedGoods, Civilian, Scientific, Commercial, Military, Guild, City, Leader, Wonder,
	Age1, Age2, Age3
}
public enum DeckType {
	Wonder, Age1, Age2, Age3, West, East, Discard, Swap
}
public enum Structure { RawMaterial, ManufacturedGoods, Civilian, Scientific, Commercial, Military, Guild, City, Leader, Wonder };
public enum Age { Age1, Age2, Age3 };
public enum Facing { Up, Down };
public enum DisplayType { Resource, Point, OneOff, Military, Scientific, Leader };
public enum CardType { RawMaterial, ManufacturedGoods, Civilian, Scientific, Commercial, Military, Guild, City, Leader };
public enum Target { Self, Neighbours, Neighbourhood, Others, Everyone };
public enum Resource { Lumber, Ore, Clay, Stone, Loom, Glassworks, Press };
public enum PointType { Military, Treasury, Wonders, Civilian, Scientific, Commercial, Guilds, Leaders };
public enum Science { Compass, Tablet, Gear }
public enum MilitaryTokenType { Victory, Draw, Defeat }
public enum PaymentType { Normal, Chained }

public class Constant {
	
	public const string GameVersion = "v0.0.2";
	public const float DistanceEpsilon = 0.01f;
	public const float AngleEpsilon = 0.1f;
	public const int MaxCost = Int16.MaxValue;

}

public class LevelName {

	public const string Lobby = "lobby";
	public const string Room = "room";
	public const string Game = "game";

}

public class LayerName {

	public const string Table = "Table";
	public const string DropArea = "Drop Area";

}

public class RoomProperty {

	public const string MatchSeed = "match_seed";

}

public class PlayerProperty {

	public const string Ready = "ready";
	public const string Pos = "pos";
	
}

public class GameOptions {

	public const int InitialCoinAmount = 3;
	public const int DiscardCoinAmount = 3;
	public const int InitialBuyCost = 2;
	public const int DiscountedBuyCost = 1;
	public const int DecideTime = 30;
	public const int TurnsPerAge = 6;
	public const int PointsPerScienceSet = 7;
	public const int CoinsPerTreasuryPoint = 3;

}

public class Priority {

	public const int GainCoins = 8;
	public const int LoseCoins = 7;
	public const int PlayHand = 6;
	public const int DiscardLastHand = 5;
	public const int DigDiscardPile = 4;
	public const int ResolveTurn = 3;
	public const int ResolvePhase = 2;
	public const int GainPoints = 2;
	public const int ResolveMatch = 1;

}

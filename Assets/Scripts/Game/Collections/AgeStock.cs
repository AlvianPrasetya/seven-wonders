using System.Collections;

public class AgeStock : Stock {

	public Age age;

	public override IEnumerator Load() {
		Stock rawMaterialStock = GameManager.Instance.Stocks[StockType.RawMaterial];
		Stock manufacturedGoodsStock = GameManager.Instance.Stocks[StockType.ManufacturedGoods];
		Stock civilianStock = GameManager.Instance.Stocks[StockType.Civilian];
		Stock scientificStock = GameManager.Instance.Stocks[StockType.Scientific];
		Stock commercialStock = GameManager.Instance.Stocks[StockType.Commercial];
		Stock militaryStock = GameManager.Instance.Stocks[StockType.Military];
		Stock guildStock = GameManager.Instance.Stocks[StockType.Guild];

		while (rawMaterialStock.Count != 0) {
			Card card = rawMaterialStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return rawMaterialStock.Push(card);
				break;
			}
		}

		while (manufacturedGoodsStock.Count != 0) {
			Card card = manufacturedGoodsStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return manufacturedGoodsStock.Push(card);
				break;
			}
		}

		while (civilianStock.Count != 0) {
			Card card = civilianStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return civilianStock.Push(card);
				break;
			}
		}

		while (scientificStock.Count != 0) {
			Card card = scientificStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return scientificStock.Push(card);
				break;
			}
		}

		while (commercialStock.Count != 0) {
			Card card = commercialStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return commercialStock.Push(card);
				break;
			}
		}

		while (militaryStock.Count != 0) {
			Card card = militaryStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return militaryStock.Push(card);
				break;
			}
		}

		// N + 2 guild cards
		int guildsCount = GameManager.Instance.players.Length + 2;
		for (int i = 0; i < guildsCount; i++) {
			Card card = guildStock.Pop();
			if (((StructureCard) card).age == age) {
				yield return Push(card);
			} else {
				yield return guildStock.Push(card);
				break;
			}
		}
	}

}

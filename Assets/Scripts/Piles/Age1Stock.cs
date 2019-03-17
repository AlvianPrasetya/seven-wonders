using System.Collections;

public class Age1Stock : Stock {

	public override IEnumerator Load() {
		Stock rawMaterialStock = GameManager.Instance.Stocks[StockType.RawMaterial];
		Stock manufacturedGoodsStock = GameManager.Instance.Stocks[StockType.ManufacturedGoods];

		while (rawMaterialStock.Count != 0) {
			Card card = rawMaterialStock.Pop();
			if (((StructureCard) card).age == Age.Age1) {
				yield return Push(card);
			} else {
				yield return rawMaterialStock.Push(card);
				break;
			}
		}

		while (manufacturedGoodsStock.Count != 0) {
			Card card = manufacturedGoodsStock.Pop();
			if (((StructureCard) card).age == Age.Age1) {
				yield return Push(card);
			} else {
				yield return manufacturedGoodsStock.Push(card);
				break;
			}
		}
	}

}

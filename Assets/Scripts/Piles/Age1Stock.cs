using System.Collections;

public class Age1Stock : Stock {

	public override IEnumerator Load() {
		Stock rawMaterialStock = GameManager.Instance.Stocks[StockType.RawMaterial];
		Stock manufacturedGoodsStock = GameManager.Instance.Stocks[StockType.ManufacturedGoods];

		while (rawMaterialStock.Count != 0) {
			Card card = rawMaterialStock.PeekBottom();
			if (((StructureCard) card).age == Age.Age1) {
				yield return Push(rawMaterialStock.PopBottom());
			} else {
				break;
			}
		}

		while (manufacturedGoodsStock.Count != 0) {
			Card card = manufacturedGoodsStock.PeekBottom();
			if (((StructureCard) card).age == Age.Age1) {
				yield return Push(manufacturedGoodsStock.PopBottom());
			} else {
				break;
			}
		}
	}

}

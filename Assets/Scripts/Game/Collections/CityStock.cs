using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CityStock : CardStock {

	[System.Serializable]
	public class CityCardEntry {

		public Card cardPrefab;
		public Age age;

	}
	
	public CityCardEntry[] cityCardEntries;

	public override IEnumerator RandomLoad(int randomSeed) {
		Dictionary<Age, List<Card>> cityCardPrefabsByAge = new Dictionary<Age, List<Card>>();
		foreach (CityCardEntry entry in cityCardEntries) {
			if (!cityCardPrefabsByAge.ContainsKey(entry.age)) {
				cityCardPrefabsByAge[entry.age] = new List<Card>();
			}
			cityCardPrefabsByAge[entry.age].Add(entry.cardPrefab);
		}

		System.Random random = new System.Random(randomSeed);
		foreach (Age age in Enum.GetValues(typeof(Age))) {
			List<Card> cityCardPrefabs;
			if (!cityCardPrefabsByAge.TryGetValue(age, out cityCardPrefabs)) {
				continue;
			}

			cityCardPrefabs = cityCardPrefabs.OrderBy(x => random.Next()).ToList();

			for (int i = 0; i < GameManager.Instance.Players.Count; i++) {
				Card card = Instantiate(cityCardPrefabs[i], transform.position, transform.rotation);
				yield return Push(card);
			}
		}
	}

}

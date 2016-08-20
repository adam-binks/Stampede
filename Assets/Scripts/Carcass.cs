using UnityEngine;
using System.Collections.Generic;

public class Carcass : MonoBehaviour {

	public static List<List<Carcass>> AllCarcassesByTrophicLevel;
	public bool isPlant = false;
	public float remainingFood = 100f;
	public string species = "UNDEFINED";

	private int trophicLevel = 0; // assume plant


	void Start() {
		// plants set themselves up rather than being set up by the dying animal
		if (isPlant) {
			Setup(trophicLevel, remainingFood, transform.position, species);
		}
	}


	/// Called by the dying animal (or Start() if this is a plant)
	public void Setup(int trophLvl, float foodAmount, Vector2 animalPosition, string thisSpecies) {
		// setup lists if this is the first carcass
		if (AllCarcassesByTrophicLevel == null) {
			AllCarcassesByTrophicLevel = new List<List<Carcass>>();
			int numTrophicLevels = 10; // this can easily be changed if there are more big creatures
			for (int i = 0; i < numTrophicLevels; i++) {
				AllCarcassesByTrophicLevel.Add(new List<Carcass>());
			}
		}

		// setup this carcass
		trophicLevel = trophLvl;
		AllCarcassesByTrophicLevel[trophicLevel].Add(this);

		remainingFood = foodAmount;
		transform.position = animalPosition;
		species = thisSpecies;
	}


	public void TakeBite(float foodToSubtract) {
		remainingFood -= foodToSubtract;
		if (remainingFood <= 0) {
			Destroy(this.gameObject);
			AllCarcassesByTrophicLevel[trophicLevel].Remove(this);
			// TODO: spawn bones?
		}
	}
}

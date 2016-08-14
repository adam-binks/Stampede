using UnityEngine;
using System.Collections.Generic;

public class Carcass : MonoBehaviour {

	public static List<List<Carcass>> AllCarcassesByTrophicLevel;
	public bool isPlant = false;
	private float remainingFood;

	private int trophicLevel = 0; // assume plant


	void Start() {
		// plants set themselves up rather than being set up by the dying animal
		if (isPlant) {
			Setup(trophicLevel, remainingFood, transform.position);
		}
	}


	/// Called by the dying animal (or Start() if this is a plant)
	public void Setup(int trophLvl, float foodAmount, Vector2 animalPosition) {
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
	}


	public void TakeBite(float foodToSubtract) {
		remainingFood -= foodToSubtract;
		if (remainingFood <= 0) {
			Destroy(this);
			// TODO: spawn bones?
		}
	}
}

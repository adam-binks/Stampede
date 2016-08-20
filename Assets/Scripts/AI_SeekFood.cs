using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Creature))]
[RequireComponent(typeof(AI_Queen))]

/// Only queens seek food probably?
public class AI_SeekFood : MonoBehaviour {

	public float foodPerBite = 25;
	public float maxSeekDistance = 15;
	public float eatDistance = 1.2f;
	public float searchFrequency = 2; // seconds
	public float AIBehaviourWeight = 5;
	public bool runToFood = true; // herbivores probably don't need to run to their food

	private Carcass target;
	private AI_Queen queen;
	private Creature thisCreature;


	void Start () {
		thisCreature = GetComponent<Creature>();

		if (thisCreature.isQueen == false) {
			this.enabled = false;
			return;
		}

		queen = GetComponent<AI_Queen>();
		StartCoroutine(GetNearestFood(0.1f));
	}


	void DoAIBehaviour() {
		if (target != null) {
			if (target.remainingFood <= 0) {
				target = null;
				return;
			} else {
				float dist = Vector2.Distance(transform.position, target.transform.position);
				if (dist <= eatDistance) {
					Eat();
				} else if (dist <= maxSeekDistance) {
					thisCreature.MoveTowards(target.transform.position, AIBehaviourWeight, runToFood);
				} else {
					target = null;
				}
			}
		}
	}


	void Eat() {
		target.TakeBite(foodPerBite);
		queen.EatFood();
	}


	/// Called every SearchFrequency seconds. 
	IEnumerator GetNearestFood(float delay) {
		yield return new WaitForSeconds(delay);

		if (thisCreature.isAlive) {
			target = null;
			float targetDist = float.PositiveInfinity;

			// look for carcasses in each trophic level that this creature hunts
			for (int tropLvl = queen.minPreyTrophic; tropLvl <= queen.maxPreyTrophic; tropLvl++) {
				foreach (Carcass carcass in Carcass.AllCarcassesByTrophicLevel[tropLvl]) {
					// no cannibalism!
					if (carcass.species == thisCreature.species) {
						continue;
					}
					
					// check if closer than previous target
					float dist = Vector2.Distance(transform.position, carcass.transform.position);
					if ((target == null || dist < targetDist) && dist < maxSeekDistance) {
						target = carcass;
						targetDist = dist;
					}
				}
			}

			
			StartCoroutine(GetNearestFood(searchFrequency));
		}
	}
}

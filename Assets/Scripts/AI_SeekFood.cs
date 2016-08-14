using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Creature))]
[RequireComponent(typeof(AI_Queen))]

/// Only queens seek food probably?
public class AI_SeekFood : MonoBehaviour {

	public float maxDistance = 15;
	public float searchFrequency = 2; // seconds
	public float AIBehaviourWeight = 5;
	public bool runToFood = true; // herbivores probably don't need to run to their food

	private Creature target;
	private AI_Queen queen;
	private Creature thisCreature;


	void Start () {
		queen = GetComponent<AI_Queen>();
		thisCreature = GetComponent<Creature>();

		StartCoroutine(GetNearestPrey(0.1f));
	}


	void DoAIBehaviour() {
		if (target != null) {
			if (target.isAlive == false) {
				target = null;
				return;
			} else {
				thisCreature.MoveTowards(target.transform.position, AIBehaviourWeight, runToFood);
			}
		}
	}


	/// Called every SearchFrequency seconds. 
	IEnumerator GetNearestPrey(float delay) {
		yield return new WaitForSeconds(delay);

		if (thisCreature.isAlive) {
			target = null;
			float targetDist = float.PositiveInfinity;
			foreach (Creature creature in Creature.AllCreatures)
			{	
				// check if trophic level is correct
				if (creature.trophicLevel <= queen.maxPreyTrophic && creature.trophicLevel >= queen.minPreyTrophic) {
					// check if closer than previous target
					float dist = Vector2.Distance(transform.position, creature.transform.position);
					if ((target == null || dist < targetDist) && dist < maxDistance) {
						target = creature;
						targetDist = dist;
					}
				}
			}

			
			StartCoroutine(GetNearestPrey(searchFrequency));
		}
	}
}

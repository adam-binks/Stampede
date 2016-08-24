using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Creature))]
[RequireComponent(typeof(AI_Queen))]

/// Only queens seek food probably?
public class AI_Flee : MonoBehaviour {

	public float fleeDistance = 15;
	public float searchFrequency = 2; // seconds
	public float AIBehaviourWeight = 30;

	private Creature target;
	private Creature thisCreature;


	void Start () {
		thisCreature = GetComponent<Creature>();
		StartCoroutine(GetNearestPredator(0.1f));
	}


	void DoAIBehaviour() {
		if (target != null) {
			float dist = Vector2.Distance(transform.position, target.transform.position);
			if (dist <= fleeDistance) {
				thisCreature.MoveAwayFrom(target.transform.position, AIBehaviourWeight, true);
			} else {
				target = null;
			}
		}
	}


	/// Called every SearchFrequency seconds. 
	IEnumerator GetNearestPredator(float delay) {
		yield return new WaitForSeconds(delay);

		if (thisCreature.isAlive) {
			target = null;
			float targetDist = float.PositiveInfinity;

			// target the nearest creature with greater trophic level than this one
			foreach (Creature creature in Creature.AllCreatures) {
				// ignore creatures with equal or lower trophic level
				if (creature.trophicLevel <= thisCreature.trophicLevel) {
					continue;
				}
				
				// check if closer than previous target
				float dist = Vector2.Distance(transform.position, creature.transform.position);
				if ((target == null || dist < targetDist) && dist < fleeDistance) {
					target = creature;
					targetDist = dist;
				}
			}

			
			StartCoroutine(GetNearestPredator(searchFrequency));
		}
	}
}


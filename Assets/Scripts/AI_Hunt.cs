using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Creature))]
[RequireComponent(typeof(AI_Queen))]

public class AI_Hunt : MonoBehaviour {
	public float maxChaseDistance = 15;
	public float maxAttackRadius = 1.5f;
	public float searchFrequency = 2; // seconds
	public float chaseWeight = 5;
	public float attackWeight = 40;
	public bool runToFood = true;
	[HideInInspector]
	public Creature target;

	private AI_Queen queen;
	private AI_Hunt queenHunt;
	private Creature thisCreature;


	void Start () {
		queen = GetComponent<AI_Queen>();
		thisCreature = GetComponent<Creature>();

		// offspring hunt their queen's target (reduce search overheads and simplify action)
		// ^ may be an issue if there are too many offspring?
		if (thisCreature.isQueen == false) {
			queenHunt = thisCreature.queen.GetComponent<AI_Hunt>();
		}
		if (thisCreature.isQueen) {
			StartCoroutine(GetNearestPrey(0.1f));
		}
	}


	void DoAIBehaviour() {
		if (thisCreature.isQueen == false && target == null) {
			// no target? check if the queen has one and use that. If it's null, we'll hunt nothing
			target = queenHunt.target;
		}

		if (target != null) {
			if (target.isAlive == false) {
				target = null;
				return;
			} else {
				float dist = DistanceToCurrentTarget();
				if (dist <= maxAttackRadius) {
					AttackTarget();
				} else if (thisCreature.isQueen && dist > maxChaseDistance) {
					target = null;
				} else {
					thisCreature.MoveTowards(target.transform.position, chaseWeight, runToFood);
				}
			}
		}
	}


	void AttackTarget() {
		WeightedDirection wd = new WeightedDirection(Vector2.zero, attackWeight, true, Intention.Attack, target);
		thisCreature.desiredDirections.Add(wd);
	}


	float DistanceToCurrentTarget() {
		return Vector2.Distance(transform.position, target.transform.position);
	}


	/// Called every SearchFrequency seconds IF this creature is a queen. 
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
					if ((target == null || dist < targetDist) && dist < maxChaseDistance) {
						target = creature;
						targetDist = dist;
					}
				}
			}

			
			StartCoroutine(GetNearestPrey(searchFrequency));
		}
	}
}

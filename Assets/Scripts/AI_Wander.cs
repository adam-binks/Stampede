using UnityEngine;
using System.Collections;

public class AI_Wander : MonoBehaviour {

	public float medianWanderTime;
	public float wanderTimeStdDev;
	
	private Vector2 wanderDirection;
	private Creature thisCreature;


	void Start() {
		thisCreature = GetComponent<Creature>();
		StartCoroutine(GetNewWanderDirection(0));
	}

	void DoAIBehaviour() {
		// very small weight - almost anything should override this
		WeightedDirection wd = new WeightedDirection(wanderDirection, 0.01f, false, Intention.Move);
		thisCreature.desiredDirections.Add(wd);
	}

	IEnumerator GetNewWanderDirection(float delay) {
		yield return new WaitForSeconds(delay);

		if (thisCreature.isQueen == true || thisCreature.queen.IsInRoamRadius(transform.position)) {
			// if inside roam radius or is queen, go in a completely random direction
			wanderDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		} else {
			// if outside roam radius, wander back towards queen
			wanderDirection = thisCreature.queen.transform.position - transform.position;
		}

		float timeTillNextNewDir = RandomPlus.Gaussian(medianWanderTime, wanderTimeStdDev, true);

		StartCoroutine(GetNewWanderDirection(timeTillNextNewDir));

	}
}

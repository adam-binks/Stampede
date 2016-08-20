using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour {

	public string species;
	[RangeAttribute(0, 10)]
	public int trophicLevel; // position on the food chain: used by other creatures to determine predator or prey
	public static List<Creature> AllCreatures;
	// these variables should be set up by other scripts (usually AI_Queen.cs)
	[HideInInspector]
	public List<WeightedDirection> desiredDirections;
	[HideInInspector]
	public float walkSpeed;
	[HideInInspector]
	public float runSpeed;
	[HideInInspector]
	public float acceleration; // higher is slower acceleration I think
	public bool isQueen = false;
	[HideInInspector]
	public AI_Queen queen;
	public bool canMove = true;
	public bool isAlive = true;
	public float maxHealth = 100;
	public float health;
	public float damagePerAttack;
	[HeaderAttribute("Carcass")]
	public GameObject carcassPrefab;
	public float foodOnCarcass = 100;
	public Sprite carcassSprite;

	private Move move;


	void Start () {
		if (canMove) {
			move = GetComponent<Move>();
			move.acceleration = acceleration;
		}

		if (AllCreatures == null) {
			AllCreatures = new List<Creature>();
		}
		AllCreatures.Add(this);

		if (isQueen) {
			GetComponent<AI_Queen>().enabled = true;
			GetComponent<AI_Offspring>().enabled = false;
		}

		health = maxHealth;

		if (species == null || species == "") {
			Debug.LogError("Must set species", this);
		} 
	}
	
	
	void Update () {
		// plants don't need to move obviously
		if (canMove) {
			UpdateAI();
		}
	}


	void UpdateAI() {
		// Ask all AI scripts to tell us in which direction we should move
		desiredDirections = new List<WeightedDirection>();
		BroadcastMessage("DoAIBehaviour", SendMessageOptions.DontRequireReceiver);

		// Add up all the desired directions by weight in case they are blended moves
		Vector2 dir = Vector2.zero;
		bool urgent = false;
		// Also work out the weightiest intention in case it's not a move, then do that exclusively
		Intention weightiestWD = Intention.Move;
		float highestWeight = -1;
		object actionTarget = null; 
		foreach(WeightedDirection wd in desiredDirections) {
			if (wd.intention == Intention.Move) {
				dir += wd.direction * wd.weight;
			}
			if (wd.isUrgent) {
				urgent = true;
			}
			if (wd.weight > highestWeight) {
				weightiestWD = wd.intention;
				highestWeight = wd.weight;
				actionTarget = wd.target;
			}
		}

		switch (weightiestWD) {
			case Intention.Move:
				// only go fast if move order is urgent
				move.targetSpeed = urgent ? runSpeed : walkSpeed;
				move.direction = dir.normalized;
				move.UpdateMovement();
				break;
			case Intention.Eat:
				Debug.Log("Nom nom nom");
				break;
			case Intention.Attack:
				Attack((Creature)actionTarget);
				break;
		}
	}


	void Attack(Creature target) {
		Debug.Log("Attack attack attack!");
		target.TakeDamage(damagePerAttack);
	}


	public void TakeDamage(float damageAmount) {
		health -= damageAmount;
		if (health <= 0) {
			Die();
		}
	}


	void Die() {
		isAlive = false;
		if (isQueen) {
			// do something to offspring. just kill them all?
		}
		Debug.Log("RIP", this);
		AllCreatures.Remove(this);
		Destroy(this.gameObject);

		// spawn a carcass
		GameObject carcassGO = (GameObject)Instantiate(carcassPrefab);
		float carcassScale = isQueen ? 1 : queen.offspringScale; // offspring carcasses are smaller
		carcassGO.GetComponent<Carcass>().Setup(trophicLevel, foodOnCarcass, transform.position, species, carcassScale);
		carcassGO.GetComponent<SpriteRenderer>().sprite = carcassSprite;
	}

	
	public void MoveTowards(Vector3 targetPos, float weight, bool urgency) {
		Vector2 direction = targetPos - transform.position;
		WeightedDirection wd =  new WeightedDirection(direction, weight, urgency, Intention.Move);
		desiredDirections.Add(wd);
	}


	public void MoveAwayFrom(Vector3 targetPos, float weight, bool urgency) {
		Vector2 direction = transform.position - targetPos;
		WeightedDirection wd =  new WeightedDirection(direction, weight, urgency, Intention.Move);
		desiredDirections.Add(wd);
	}
}

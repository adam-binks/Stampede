using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Creature))]

public class AI_Queen : MonoBehaviour {

	[HeaderAttribute("Food chain")]
	[RangeAttribute(0, 10)]
	public int maxPreyTrophic;
	[RangeAttribute(0, 10)]
	public int minPreyTrophic;
	[HeaderAttribute("Movement")]
	public float walkSpeed;
	public float runSpeed;
	public float acceleration;
	[HeaderAttribute("Offspring")]
	public int maxOffspring;
	public int foodPerOffspring;
	public GameObject eggPrefab;
	public GameObject offspringPrefab;
	public float eggHatchTime;
	[RangeAttribute(0, 1)]
	public float offspringScale;
	public float offspringRoamRadius;

	private List<AI_Offspring> offspring;
	private List<Egg> eggs;
	private Creature thisCreature;
	private int currentFood = 0;


	void Start () {
		thisCreature = GetComponent<Creature>();
		if (thisCreature.isQueen) {
			SetupQueen();
		}
	}

	void SetupQueen() {
		thisCreature.isQueen = true;
		thisCreature.walkSpeed = walkSpeed;
		thisCreature.runSpeed = runSpeed;
		thisCreature.acceleration = acceleration;

		offspring = new List<AI_Offspring>();
		eggs = new List<Egg>();
	}


	public void EatFood() {
		currentFood ++;
		if (currentFood >= foodPerOffspring) {
			LayEgg();
		}
	}


	void LayEgg() {
		if ((offspring.Count + eggs.Count) >= maxOffspring) {
			// can't lay any more eggs
			return;
		}
		
		GameObject eggGO = (GameObject)Instantiate(eggPrefab, transform.position, Quaternion.identity);
		Egg egg = eggGO.GetComponent<Egg>();
		eggs.Add(egg);
		egg.queen = this;
		egg.hatchTime = eggHatchTime;

		currentFood = 0;
	}


	/// Called by egg on hatch. Create a new offspring
	public void CreateOffspring(Vector3 pos) {
		GameObject offspringGO = (GameObject)Instantiate(offspringPrefab, pos, Quaternion.identity);
		offspringGO.transform.localScale = new Vector2(offspringScale, offspringScale);
		Creature offspringCreature = offspringGO.GetComponent<Creature>();
		offspringCreature.queen = this;
		offspringCreature.isQueen = false;
	}


	public bool IsInRoamRadius(Vector2 offspringPos) {
		if (Vector2.Distance(offspringPos, transform.position) < offspringRoamRadius) {
			return true;
		}
		return false;
	}
}

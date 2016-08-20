using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {

	public float hatchTime;
	public AI_Queen queen;

	void Start () {
		if (hatchTime == 0 || queen == null) {
			Debug.LogError("Egg properties not passed by queen", queen);
		}

		Invoke("Hatch", hatchTime);
	}
	
	void Hatch() {
		// if the queen is dead, just break the egg. (does this make sense tho?)
		if (queen != null) {
			queen.CreateOffspring(transform.position);
		}
		Destroy(this.gameObject);
	}
}

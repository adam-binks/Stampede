using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour {

	public float hatchTime;
	public AI_Queen queen;

	void Start () {
		if (hatchTime == 0 || queen == null) {
			Debug.LogError("Egg properties not passed by queen, ya dingus!", queen);
		}

		Invoke("Hatch", hatchTime);
	}
	
	void Hatch() {
		queen.CreateOffspring(transform.position);
		Destroy(this.gameObject);
	}
}

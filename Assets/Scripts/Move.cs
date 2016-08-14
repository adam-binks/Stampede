using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

	[HideInInspector]
	public Vector2 direction = new Vector2(0, 0);
	[HideInInspector]
	public float targetSpeed;
	[HideInInspector]
	public float acceleration;

	private Vector2 velocity;
	private SpriteRenderer spriteRenderer;


	void Start() {
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();
		spriteRenderer.sortingOrder = -(int)transform.position.y;
	}
	
	public void UpdateMovement () {
		// Lerp velocity towards the target direction and speed
		velocity = Vector2.Lerp(velocity, direction * targetSpeed, acceleration);
		// Update position
		transform.Translate(velocity * Time.deltaTime);

		// display sprites further forwards on top of ones further back
		spriteRenderer.sortingOrder = -(int)transform.position.y;
	}
}

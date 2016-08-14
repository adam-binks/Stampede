using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Move))]

public class PlayerMoveInput : MonoBehaviour {

	public float moveSpeed;
	public float acceleration;

	private Move move;


	void Start () {
		move = GetComponent<Move>();
		move.targetSpeed = moveSpeed;
		move.acceleration = acceleration;
	}
	
	
	void Update () {
		move.direction = new Vector2(Input.GetAxis("Horizontal"),  Input.GetAxis("Vertical"));
		move.UpdateMovement();
	}
}

using UnityEngine;
using System.Collections;

public enum Intention {
	Move,
	Eat,
	Attack
}

public class WeightedDirection {

	public readonly Vector2 direction;
	public readonly float weight;
	public readonly bool isUrgent;
	public readonly Intention intention;
	public object target;

	public WeightedDirection(Vector2 dir, float wgt, bool urgent, Intention intentionType, object actionTarget=null) {
		direction = dir.normalized;
		weight = wgt;
		isUrgent = urgent;
		intention = intentionType;
		target = actionTarget;
	}
}

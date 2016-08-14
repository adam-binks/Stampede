using UnityEngine;
using System;

public class RandomPlus : MonoBehaviour {

	public static float Gaussian(float mean, float stdDev, bool mustBePositive) {
		// thanks yoyoyoyosef https://stackoverflow.com/questions/218060/random-gaussian-variables/218600#218600

		double u1 = UnityEngine.Random.value; //these are uniform(0,1) random doubles
		double u2 = UnityEngine.Random.value;
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
		double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

		if (mustBePositive) {
			randNormal = Math.Abs(randNormal);
		}

		return (float)randNormal;
	}
}

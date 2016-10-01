using UnityEngine;
using System.Collections;

public class DieAfterTime : MonoBehaviour {

	public float lifeSpan = 1f;
	private float deathTime = float.MaxValue;

	// Use this for initialization
	void Start () {
		deathTime = Time.time + lifeSpan;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > deathTime)
			Destroy (gameObject);
	}
}

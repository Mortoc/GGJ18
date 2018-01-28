using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instrument : MonoBehaviour {
	public beetDropper player;
	public BoxCollider instrumentCollider;
	public float orbitDistance = 10.0f;
	public float orbitSpeed = 180f;

	// Use this for initialization
	void Awake () {
		
	}

	public void Setup(beetDropper player) {
		this.player = player;
	}

	void Update() {
		if (player) {
			transform.position = player.transform.position + (transform.position - player.transform.position).normalized * orbitDistance;
			transform.RotateAround(player.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
		}
	}
}	

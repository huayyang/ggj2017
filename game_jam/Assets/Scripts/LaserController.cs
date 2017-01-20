using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (!collider.CompareTag("Player")) {
			return;
		}

		//TODO(Huayu): Fade in, Fade out
		collider.SendMessageUpwards("death");
	}
}

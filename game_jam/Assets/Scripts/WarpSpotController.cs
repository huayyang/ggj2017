using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpSpotController : MonoBehaviour {
	public Transform warpDestination;
	private GameObject mPlayer;
	// Use this for initialization
	void Start () {
		mPlayer = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (!collider.CompareTag("Player")) {
			return;
		}

		//TODO(Huayu): Fade in, Fade out
		mPlayer.transform.position = warpDestination.transform.position;
	}
}

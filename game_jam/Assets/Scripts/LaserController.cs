using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {
	public GameObject laserEffect;
	private Animator mAnimator;
	// Use this for initialization
	void Start () {
		mAnimator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		mAnimator.SetBool("laserEnabled", laserEffect.activeSelf);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (!collider.CompareTag("Player")) {
			return;
		}

		//TODO(Huayu): Fade in, Fade out
		collider.SendMessageUpwards("death");
	}
}

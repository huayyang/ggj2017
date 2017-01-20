using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

	private PlayerController mPlayerController;
	private CircleCollider2D mCircleCollider;
	private float waveCastCoolDown = 3.0f;
	private float waveCastTimer;
	public float waveSpeed = 0.5f;
	public enum WaveType {North, East, South, West};
	public WaveType mWaveType;
	public float waveMaxRadius = 10.0f;

	private float waveInitialRadius = 0.5f;
	// Use this for initialization
	void Start () {
		mPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		mCircleCollider = this.GetComponent<CircleCollider2D>();
		mCircleCollider.radius = waveInitialRadius;
	}
	
	// Update is called once per frame
	void Update () {
		if (waveCastTimer > 0) {
			waveCastTimer -= Time.deltaTime;
		}
	}
	public bool canCastWave() {
		return waveCastTimer <= 0;
	}
	public void castWave(WaveType waveType) {
		mWaveType = waveType;
		waveCastTimer = waveCastCoolDown;
		StartCoroutine(waveStart());
	}

	IEnumerator waveStart() {
		while (mCircleCollider.radius < waveMaxRadius) {
			mCircleCollider.radius += waveSpeed * Time.deltaTime;
			//Debug.Log("wave radius: " + mCircleCollider.radius);
			yield return null;
		}

		mCircleCollider.radius = waveInitialRadius;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (!collider.CompareTag("hitObject")) {
			return;
		}

		Debug.Log("Wave OnTriggerEnter");
		mPlayerController.onWaveHitObject(collider);
	}

	
}

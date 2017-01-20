using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

	private GameObject mPlayerWaveCollider;
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
		mPlayerWaveCollider = GameObject.FindGameObjectWithTag("waveCollider");
		mCircleCollider = this.GetComponent<CircleCollider2D>();
		mCircleCollider.radius = waveInitialRadius;
		waveCastTimer = 0.0f;
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

	bool canWaveTriggerObject(Vector3 playerPosition, Vector3 targetPosition) {
		RaycastHit2D hit = Physics2D.Raycast(playerPosition, targetPosition - playerPosition);
		if (hit != null && hit.collider.CompareTag("waveBlocker")) {
			return false;
		}
		return true;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (!collider.CompareTag("waveTrigger")) {
			return;
		}

		if (!canWaveTriggerObject(mPlayerWaveCollider.transform.position, collider.transform.position)) {
			return;
		}

		collider.SendMessageUpwards("handleWaveAction");

		Debug.Log("Wave OnTriggerEnter");
	}


}

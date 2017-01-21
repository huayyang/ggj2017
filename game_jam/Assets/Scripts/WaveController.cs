﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour {

	private GameObject mPlayerWaveCollider;
	private CircleCollider2D mCircleCollider;
	private PhotonWaveEffectsController mWaveEffectController;
	public float waveCastCoolDown = 3.0f;
	private float waveCastTimer;
	public float waveSpeed = 0.5f;
	public enum WaveType {Long, Mid, Short};
	public WaveType mWaveType;
	public float waveMaxRadius = 10.0f;
	public const float LongWaveSpeed = 0.5f;
	public const float LongWaveMaximumRadius = 10.0f;
	public const float MidWaveSpeed = 0.75f;
	public const float MidWaveMaximumRadius = 7.5f;
	public const float ShortWaveSpeed = 1.0f;
	public const float ShortWaveMaximumRadius = 5.0f;
	private Vector3 castPosition;

	private float waveInitialRadius = 0.5f;
	// Use this for initialization
	void Start () {
		mPlayerWaveCollider = GameObject.FindGameObjectWithTag("waveCollider");
		mCircleCollider = this.GetComponent<CircleCollider2D>();
		mCircleCollider.radius = waveInitialRadius;
		waveCastTimer = 0.0f;
		mWaveEffectController = this.GetComponent<PhotonWaveEffectsController>();
		mCircleCollider.enabled = false;
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
		if (waveType == WaveType.Long) {
			waveSpeed = LongWaveSpeed;
			waveMaxRadius = LongWaveMaximumRadius;
		} else if (waveType == WaveType.Mid) {
			waveSpeed = MidWaveSpeed;
			waveMaxRadius = MidWaveMaximumRadius;
		} else if (waveType == WaveType.Short) {
			waveSpeed = ShortWaveSpeed;
			waveMaxRadius = ShortWaveMaximumRadius;
		}
		mCircleCollider.enabled = true;
		castPosition = mPlayerWaveCollider.transform.position;
		StartCoroutine(waveStart(waveType));
	}

	IEnumerator waveStart(WaveType waveType) {
		mWaveEffectController.PlayEffect(waveType);
		while (mCircleCollider.radius < waveMaxRadius) {
			mCircleCollider.radius += waveSpeed * Time.deltaTime;
			//Debug.Log("wave radius: " + mCircleCollider.radius);
			yield return null;
		}

		mCircleCollider.radius = waveInitialRadius;
		mCircleCollider.enabled = false;
	}

	bool canWaveTriggerObject(Vector3 playerPosition, Vector3 targetPosition) {
		float distance = Vector2.Distance(playerPosition, targetPosition);
		RaycastHit2D[] hits = Physics2D.RaycastAll(playerPosition, targetPosition - playerPosition, distance);
		for (int i = 0; i < hits.Length; ++i) {
			if (hits[i].collider != null && hits[i].collider.CompareTag("waveBlocker")) {
				return false;
			}
		}
		
		return true;
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (!collider.CompareTag("waveTrigger")) {
			return;
		}

		if (!canWaveTriggerObject(castPosition, collider.transform.position)) {
			return;
		}

		collider.SendMessageUpwards("handleWaveAction");
	}


}

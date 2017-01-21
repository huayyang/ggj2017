﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	// Components
	private Rigidbody2D mRigidbody;
	private WaveController mWaveController;
	private GameObject mWaveCollider;
	private Animator mAnimator;
	private MusicManager mMusicManager;

	// Movements
	public float mMoveForce = 5f;
	public float mJumpForce = 8f;
	private float distanceToGround;

	public float horizontalMaximumSpeed = 2.5f;

	// Ability
	// Used as initial amount
	public int currentWaveAmount = 2;
	private float wavePressedTime = 0;
	public float WaveMidPressTimeThreshold = 1.0f;
	public float WaveLongPressTimeThreshold = 3.0f;
	private bool isCastingWave = false;
	
	// Use this for initialization
	void Start () {
		mRigidbody = this.GetComponent<Rigidbody2D>();
		mWaveController = GameObject.FindGameObjectWithTag("waveCollider").GetComponent<WaveController>();
		mWaveCollider = GameObject.FindGameObjectWithTag("waveCollider");
		isCastingWave = false;
		distanceToGround = this.GetComponent<BoxCollider2D>().bounds.max.y - mRigidbody.transform.position.y;
		mAnimator = this.GetComponent<Animator>();
		mMusicManager = GameObject.FindGameObjectWithTag("musicManager").GetComponent<MusicManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		handlePlayerMovement(Time.deltaTime);
		handlePlayerRestart();
	}

	void Update() {
		handleCastWaves();
		checkDeath();
	}

	void handlePlayerMovement(float deltaTime) {
		bool isWalking = false;
		bool isJumping = false;
		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		Vector2 movementForce = new Vector2(movement.x * mMoveForce, movement.y * mJumpForce);
		
		if (movementForce != Vector2.zero) {
			if (Mathf.Abs(mRigidbody.velocity.x) >= horizontalMaximumSpeed) {
				movementForce.x = 0;
			}
			if (isGrounded()) {
				mRigidbody.AddForce(movementForce * Time.deltaTime);
				isWalking = true;
				if (movement.y != 0) {
					isJumping = true;
				}
			} else if (movementForce.x != 0){
				mRigidbody.AddForce(new Vector2(movementForce.x * Time.deltaTime, 0));
			}

			if (isJumping && movement.y > 0) {
				mMusicManager.PlayJumpSound();
			}
		}
		// TODO(Huayu): jump animation
		mAnimator.SetBool("isWalking", isWalking);
		mAnimator.SetFloat("inputX", movement.x);
		mAnimator.SetFloat("inputY", movement.y);
	}

	public bool isGrounded() {
		RaycastHit2D hit = Physics2D.Raycast(mRigidbody.transform.position, Vector2.down, distanceToGround + 0.1f);
		if (hit.collider != null && hit.collider.CompareTag("photon")) {
			return false;
		}
		return hit.collider != null;
	}

	void handlePlayerRestart() {
		if (Input.GetKeyDown(KeyCode.R)) {
			death();
		}
	}
	private bool canCastWave() {
		if (isCastingWave) {
			return false;
		}

		if (currentWaveAmount <= 0) {
			return false;
		}

		if (!mWaveController.canCastWave()) {
			return false;
		}

		return true;
	}
	public void handleCastWaves() {
		if (Input.GetKeyDown(KeyCode.Space) && canCastWave()) {
			isCastingWave = true;
		}

		if (!isCastingWave) {
			return;
		}

		if (Input.GetKey(KeyCode.Space)) {
			wavePressedTime += Time.deltaTime;
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			if (wavePressedTime < WaveMidPressTimeThreshold) {
				mWaveController.castWave(WaveController.WaveType.Short);
			} else if (wavePressedTime < WaveLongPressTimeThreshold) {
				mWaveController.castWave(WaveController.WaveType.Mid);
			} else {
				mWaveController.castWave(WaveController.WaveType.Long);
			}

			mMusicManager.PlayAbilitySound();
			wavePressedTime = 0.0f;
			isCastingWave = false;
		}
	}
	// Deprecated
	public void increaseWaveAmount(int amount) {
		currentWaveAmount += amount;
	}

	private void checkDeath() {
		if (mRigidbody.transform.position.y <= -100) {
			death();
		}
	}

	public void death() {
		enabled = false;
		mMusicManager.PlayDeathSound();
		StartCoroutine(reloadAfterTime(3.0f));
	}

	 IEnumerator reloadAfterTime(float time) {
		 yield return new WaitForSeconds(time);

		 mMusicManager.PlayRespwanSound();
     	 Scene loadedLevel = SceneManager.GetActiveScene();
    	 SceneManager.LoadScene (loadedLevel.buildIndex);
	}
}

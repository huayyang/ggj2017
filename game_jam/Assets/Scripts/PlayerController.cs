using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Components
	private Rigidbody2D mRigidbody;
	private WaveController mWaveController;
	private GameObject mWaveCollider;

	// Movements
	public float mMoveSpeed = 5f;
	public float mJumpSpeed = 8f;
	public float mClimbSpeed = 3f;

	// Ability
	private WaveController.WaveType currentWaveType;
	private int currentWaveAmount;
	
	// Use this for initialization
	void Start () {
		mRigidbody = this.GetComponent<Rigidbody2D>();
		mWaveController = GameObject.FindGameObjectWithTag("waveCollider").GetComponent<WaveController>();
		mWaveCollider = GameObject.FindGameObjectWithTag("waveCollider");
		currentWaveType = WaveController.WaveType.North;
	}
	
	// Update is called once per frame
	void Update () {
		handlePlayerMovement(Time.deltaTime);
		handleCastWaves();
	}

	void handlePlayerMovement(float deltaTime) {
		bool isWalking = false;
		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal") * mMoveSpeed, Input.GetAxisRaw("Vertical") * mJumpSpeed);
		if (movement != Vector2.zero) {
			mRigidbody.AddForce(movement);
		}
	}

	private bool canCastWave() {
		if (currentWaveAmount <= 0) {
			return false;
		}

		if (mWaveController.canCastWave()) {
			return false;
		}

		return true;
	}
	public void handleCastWaves() {
		if (Input.GetKeyDown(KeyCode.A) && canCastWave()) {
			mWaveController.castWave(currentWaveType);
		}
	}
	bool canWaveHitObject(Vector3 playerPosition, Vector3 targetPosition) {
		RaycastHit2D hit = Physics2D.Raycast(playerPosition, targetPosition - playerPosition);
		if (hit != null && hit.collider.CompareTag("waveBlocker")) {
			return false;
		}
		return true;
	}
	public void onWaveHitObject(Collider2D collider) {
		if (!canWaveHitObject(mWaveCollider.transform.position, collider.transform.position)) {
			return;
		}
		Debug.Log("Player OnWaveHit");
	}

	public void increaseWaveAmount(int amount) {
		currentWaveAmount += amount;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	private int currentWaveAmount = 2;
	
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
		handlePlayerRestart();
		checkDeath();
	}

	void handlePlayerMovement(float deltaTime) {
		bool isWalking = false;
		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal") * mMoveSpeed, Input.GetAxisRaw("Vertical") * mJumpSpeed);
		if (movement != Vector2.zero) {
			mRigidbody.AddForce(movement);
		}
	}

	void handlePlayerRestart() {
		if (Input.GetKeyDown(KeyCode.R)) {
			death();
		}
	}

	private bool canCastWave() {
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
			mWaveController.castWave(currentWaveType);
		}
	}

	public void increaseWaveAmount(int amount) {
		currentWaveAmount += amount;
	}

	private void checkDeath() {
		if (mRigidbody.transform.position.y <= -100) {
			death();
		}
	}

	public void death() {
		Scene loadedLevel = SceneManager.GetActiveScene();
     	SceneManager.LoadScene (loadedLevel.buildIndex);
	}
}

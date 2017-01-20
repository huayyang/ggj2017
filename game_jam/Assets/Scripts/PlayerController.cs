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
		Vector2 movement = new Vector2(Input.GetAxisRaw("Horizontal") * mMoveForce, Input.GetAxisRaw("Vertical") * mJumpForce);
		
		if (movement != Vector2.zero) {
			if (Mathf.Abs(mRigidbody.velocity.x) >= horizontalMaximumSpeed) {
				movement.x = 0;
			}
			if (isGrounded()) {
				mRigidbody.AddForce(movement * Time.deltaTime);
			} else if (movement.x != 0){
				mRigidbody.AddForce(new Vector2(movement.x * Time.deltaTime, 0));
			}
		}
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
		Scene loadedLevel = SceneManager.GetActiveScene();
     	SceneManager.LoadScene (loadedLevel.buildIndex);
	}
}

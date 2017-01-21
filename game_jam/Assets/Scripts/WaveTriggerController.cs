using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTriggerController : MonoBehaviour {
	public GameObject triggerActionHandler;
	public bool isEnabledOnStart;
	public float resetTime = 0.0f;
	private MusicManager mMusicManager;
	// Use this for initialization
	void Start () {
		mMusicManager = GameObject.FindGameObjectWithTag("musicManager").GetComponent<MusicManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator setEnable(bool rEnabled) {
		float statesChangeTimer = 0.0f;
		triggerActionHandler.SetActive(rEnabled);
		while (statesChangeTimer < resetTime) {
			statesChangeTimer += Time.deltaTime;
			yield return null;
		}

		triggerActionHandler.SetActive(!rEnabled);
	}

	void handleWaveAction() {
		mMusicManager.PlayWaveTriggerSound();
		if (resetTime > 0) {
			StartCoroutine(setEnable(!triggerActionHandler.activeSelf));
		} else {
			triggerActionHandler.SetActive(!triggerActionHandler.activeSelf);
		}
	}
}

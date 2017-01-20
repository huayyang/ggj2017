using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTriggerController : MonoBehaviour {
	public GameObject triggerActionHandler;
	public bool isEnabledOnStart;
	private SpriteRenderer handlerSpriteRender;
	// Use this for initialization
	void Start () {
		handlerSpriteRender = triggerActionHandler.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setEnable(bool rEnabled) {
		triggerActionHandler.SetActive(rEnabled);
		//handlerSpriteRender.enabled = rEnabled;
	}

	void handleWaveAction() {
		Debug.Log("handleWaveAction");
		setEnable(!triggerActionHandler.activeSelf);
	}
}

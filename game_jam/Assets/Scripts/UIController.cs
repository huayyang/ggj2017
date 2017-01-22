using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Text playerWaveAmount;
	public Text playerWaveCD;
	public Text playerWaveType;
	private PlayerController mPlayerController;
	// Use this for initialization
	void Start () {
		mPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (mPlayerController == null) {
			mPlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		}
		playerWaveAmount.text = "Waves Left: " + mPlayerController.currentWaveAmount;
		playerWaveCD.text = "Cast CD: " + mPlayerController.waveCastCDLeft();
		WaveController.WaveType waveType = mPlayerController.waveTypeMessageForUI();
		if (waveType == WaveController.WaveType.Short) {
			playerWaveType.text = "Short Wave";
			playerWaveType.color = Color.green;
		} else if (waveType == WaveController.WaveType.Mid) {
			playerWaveType.text = "Mid Wave";
			playerWaveType.color = Color.yellow;
		} else if (waveType == WaveController.WaveType.Long) {
			playerWaveType.text = "Long Wave";
			playerWaveType.color = Color.magenta;
		}
		
	}
}

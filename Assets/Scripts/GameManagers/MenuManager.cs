﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InControl;

[RequireComponent (typeof (SpriteRenderer))]
public class MenuManager : MonoBehaviour
{
	public static bool starting = false;

	public AudioClip menuMusic;
	public AudioClip startSound;
	public Image fadeoutOverlay;

	private Color color;
	private float startTimer = 2f;

	public void Awake()
	{
		if(InputManager.Devices.Count < PlayerControl.NumberOfPlayers) {
			InputManager.AttachDevice(new UnityInputDevice(new KeyboardProfileIJKL()));
			Debug.Log("Attaching Keyboard IJKL");
		}

		
		if(InputManager.Devices.Count < PlayerControl.NumberOfPlayers) {
			InputManager.AttachDevice(new UnityInputDevice(new KeyboardProfileWASD()));
			Debug.Log("Attaching Keyboard WASD");
		}


		// Look for device changes
		InputManager.OnDeviceAttached += inputDevice => Application.LoadLevel(0);
		InputManager.OnDeviceDetached += inputDevice => Application.LoadLevel(0);

		starting = false;
		Sound_Manager.Instance.PlayMusicLoop(menuMusic);
	}


	public void Update()
	{
		// Fade out this scene and load the Game when done
		if(starting) {
			startTimer -= Time.deltaTime;
			float alpha = 1f - (startTimer / 2f);
			fadeoutOverlay.color = new Color(0.0f, 0.0f, 0.0f, alpha);

			// if we're done fading out
			if(startTimer <= 0f) {
				Application.LoadLevel("Game");
			}
		}

		else {
			for(int i = 0; i < InputManager.Devices.Count && i < PlayerControl.NumberOfPlayers; i++) {
				if(Input.GetKeyUp(KeyCode.Space) && MenuPlayerController.allPlayersHaveChosenACharacter()) {
					starting = true;
					Sound_Manager.Instance.PlayEffectOnce(startSound);
				}
			}
		}
	}

}

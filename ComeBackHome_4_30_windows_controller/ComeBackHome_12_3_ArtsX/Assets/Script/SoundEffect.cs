using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

	private const float MIN_LENGTH = 5f;
	private const float MAX_LENGTH = 7f;

	public AudioClip[] villageSounds;
	public AudioClip[] caveSounds;
	public AudioClip[] forestSounds;
	public AudioClip[] lakeSounds;
	private AudioClip[][] soundEffects = new AudioClip[4][];
	private AudioClip nextAudioClip;
	private AudioSource audio;
	private int soundIndex;
	private float timeLength;
	private float timer;
	private bool hasAudio;

	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> ();
		soundEffects [0] = villageSounds;
		soundEffects [1] = forestSounds;
		soundEffects [2] = caveSounds;
		soundEffects [3] = lakeSounds;

		hasAudio = false;
		resetAudio ();

	}
	
	// Update is called once per frame
	void Update () {

		if (hasAudio && !audio.isPlaying) {
			timer += Time.deltaTime;
			if (timer > timeLength) {
				audio.clip = nextAudioClip;
				audio.Play ();
				resetAudio ();
			}
		}
	}

	public void resetAudio(){
		if (SceneManager.GetActiveScene ().buildIndex >= 1 && SceneManager.GetActiveScene ().buildIndex <= 4) {
			timer = 0;
			if (soundEffects [SceneManager.GetActiveScene ().buildIndex - 1].Length != 0) {
				soundIndex = Random.Range (0, soundEffects [SceneManager.GetActiveScene ().buildIndex - 1].Length);
				nextAudioClip = soundEffects [SceneManager.GetActiveScene ().buildIndex - 1] [soundIndex];
				timeLength = Random.Range (MIN_LENGTH, MAX_LENGTH);
				hasAudio = true;
			} else
				hasAudio = false;
		} else
			hasAudio = false;
	}

}

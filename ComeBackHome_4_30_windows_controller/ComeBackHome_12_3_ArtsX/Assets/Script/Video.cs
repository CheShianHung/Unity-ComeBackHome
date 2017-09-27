using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]

public class Video : MonoBehaviour {

	public MovieTexture movie;
	public int saveID;
	public bool ending; 

	private bool triggerOnce;
	private SceneM sceneM;

	void Start(){
		GetComponent<Renderer> ().material.mainTexture = movie as MovieTexture;
		GetComponent<AudioSource> ().clip = movie.audioClip;
		sceneM = GameObject.Find ("Scene Manager").GetComponent<SceneM> ();
		movie.Play ();
		GetComponent<AudioSource> ().Play ();
		Cursor.visible = false;
		triggerOnce = false;
	}

	//Menu index: 0
	//Village index: 1
	//Forest index: 2
	//Cave index: 3
	//Lake index: 4
	//Ending index: 5
	//Intro Image: 6
	//Cutscene Village: 7
	//Cutscene Opening: 8
	//Cutscene Cave: 9
	//Cutscene Forest: 10

	// Update is called once per frame
	void Update () {
		if(movie.isPlaying && Input.GetButton("Jump") && !triggerOnce){
			triggerOnce = true; 
			SceneTransition (); 
		}

		if (!movie.isPlaying && !triggerOnce) {
			triggerOnce = true;
			movie.Stop ();
			GetComponent<AudioSource> ().Stop ();
			SceneTransition ();
		}
	}

	void SceneTransition(){
		if (ending) {
			sceneM.saveID = saveID;
		} else {
			sceneM.areaID = saveID;
		}
	}
}

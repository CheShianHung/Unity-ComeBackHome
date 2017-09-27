using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroImages : MonoBehaviour {

	public Material[] images;

	private bool triggerOnce;
	private int index;
	private SceneM sceneM;

	void Start(){
		triggerOnce = false;
		index = 0;
		GetComponent<Renderer> ().material = images [index];
		sceneM = GameObject.Find ("Scene Manager").GetComponent<SceneM> ();
	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown("Submit") && index < images.Length - 1) {
			index++;
			GetComponent<Renderer> ().material = images [index];
		} else if (Input.GetButtonDown ("Submit") && index == images.Length - 1 && !triggerOnce) {
			triggerOnce = true; 
			GameObject.Find ("Scene Manager").GetComponent<SceneCollider> ().InitializeSceneObjects ();
			sceneM.unlockCave = false;
			sceneM.unlockForest = false;
			sceneM.areaID = 1;
		}
	}
}

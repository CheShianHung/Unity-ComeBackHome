using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageEnding : MonoBehaviour {

	private bool triggerOnce;

	void Start(){
		triggerOnce = false;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape) && !triggerOnce){
			triggerOnce = true; 
			GameObject.Find("Scene Manager").GetComponent<SceneM>().saveID = 0;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AutoRestart : MonoBehaviour {

	public float timeToRestart;
	private float timer;
	private bool restarting;


	// Use this for initialization
	void Start () {
		resetTimer ();
	}
	
	// Update is called once per frame
	void Update () {

		//not the first scene
		if (SceneManager.GetActiveScene ().buildIndex != 0) {
			
			timer += Time.fixedDeltaTime; //counting

			if (timer > timeToRestart && !restarting) { //when over the restart time and not restarted yet
				
				if (!restarting) { //if not restarted yet

					Time.timeScale = 1; //unpause the game
					restarting = true; //set bool to locked
					//restart
//					File.Delete(Application.persistentDataPath + "/save.dat");
					GameObject.Find("Scene Manager").GetComponent<SceneM>().saveID = 0;

				}

			}

			//reset timer if is playing
			checkInput ();

		} else { //when in the first scene
			
			resetTimer ();

		}
			
	}

	void resetTimer(){
		timer = 0f; //Set timer to 0
		restarting = false; //set restarting to false
	}

	void checkInput(){
		if (Input.GetAxisRaw ("Horizontal") != 0 || 
			Input.GetAxisRaw ("Vertical") != 0 || 
			Input.GetButton("Jump") ||
			Input.GetButton("Fire2") ||
			Input.GetAxisRaw("Fire2") == 1 ||
			Input.GetAxis("MouseRS X") > 0.15f ||
			Input.GetAxis("MouseRS X") > 0.15f ||
			Input.GetAxis("Mouse X") != 0 ||
			Input.GetAxis("Mouse Y") != 0 ||
			Input.GetButtonDown("Journal") ||
			Input.GetButtonDown("Cancel") ||
			Input.GetButtonDown("Submit") ||
			Input.GetAxisRaw ("Journal Control") != 0 ||
			Input.GetAxis("Mouse ScrollWheel") != 0 ||
			Input.GetAxis("Joy ScrollWheel") != 0) {

			resetTimer ();
		}
	}
}

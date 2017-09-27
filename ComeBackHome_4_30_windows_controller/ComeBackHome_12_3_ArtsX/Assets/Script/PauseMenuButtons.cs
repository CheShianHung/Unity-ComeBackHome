using UnityEngine;
using System.Collections;

public class PauseMenuButtons : MonoBehaviour { 

	public GameObject resumeBtn;
	public GameObject loadBtn;
	public GameObject controlBtn;
	public GameObject quitBtn;
	public GameObject backBtn;
	public GameObject line;
	public GameObject controlImage;

    public void returnButton(){
        PauseMenu pm = GameObject.Find("Scene Manager").GetComponent<PauseMenu>();
		SceneM sceneM = GameObject.Find ("Scene Manager").GetComponent<SceneM> ();
	
        pm.show = false;
		sceneM.pauseMenuIsOn = false;
		Cursor.visible = false; //Hide the cursor
        pm.Menu.SetActive(pm.show);
        Time.timeScale = 1;
    }

	public void controlButton(){
		resumeBtn.SetActive (false);
		loadBtn.SetActive (false);
		controlBtn.SetActive (false);
		quitBtn.SetActive (false);
		line.SetActive (false);

		backBtn.SetActive (true);
		controlImage.SetActive (true);
	}

	public void loadButton(){
        SaveAndLoad load = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
        load.Load();
		returnButton();
	}

	public void quitButton(){
        returnButton();
        GameObject.Find("Scene Manager").GetComponent<SceneM>().saveID = 0;
    }

	public void resetPauseMenu(){
		resumeBtn.SetActive (true);
		loadBtn.SetActive (true);
		controlBtn.SetActive (true);
		quitBtn.SetActive (true);
		line.SetActive (true);

		backBtn.SetActive (false);
		controlImage.SetActive (false);
	}
}

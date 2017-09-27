using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;

public class PauseMenu : MonoBehaviour {

    public bool show = false;
    public GameObject Menu;
    public GameObject loadButton;

	private SceneM sceneM;
	private JournalScript journal;

    // Use this for initialization
    void Start() {
        Menu.SetActive(show);
		sceneM = GameObject.Find("Scene Manager").GetComponent<SceneM>();
		journal = GameObject.Find("Scene Manager").GetComponent<JournalScript>();
    }

    // Update is called once per frame
    void Update() {
        MenuFunction();
    }

    void MenuFunction() {
		if (Input.GetButtonDown("Cancel") && SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 5 && SceneManager.GetActiveScene().buildIndex != 6) {
            if (show == false) {
				
				//If the journal is open, close it and then open the pause menu
				if (sceneM.journalIsOn)
					journal.forceToClose = true;

				show = true;
				sceneM.pauseMenuIsOn = true;
				Cursor.visible = true; //Hide the cursor
                if (File.Exists(Application.persistentDataPath + "/save.dat"))
                {
                    loadButton.GetComponent<Button>().interactable = true;
                }
                else
                {
                    loadButton.GetComponent<Button>().interactable = false;
                }
                Menu.SetActive(show);
				Menu.GetComponent<PauseMenuButtons> ().resetPauseMenu ();
                Time.timeScale = 0;
            }
            else {
				show = false;
				sceneM.pauseMenuIsOn = false;
				Cursor.visible = false; //Hide the cursor
                Menu.SetActive(show);
                Time.timeScale = 1;
            }
        }
    }
}

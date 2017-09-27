using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Collections;

public class StartMenu : MonoBehaviour {

    private SceneM sceneManager;
    public GameObject loadButton;

    // Use this for initialization
    void Awake()
    {
        sceneManager = GameObject.Find("Scene Manager").GetComponent<SceneM>();
    }

    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            loadButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            loadButton.GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    public void StartNewGame()
    {
        File.Delete(Application.persistentDataPath + "/save.dat");
		GameObject.Find ("Scene Manager").GetComponent<JournalScript> ().resetJournal ();
        sceneManager.areaID = 11;
    }

    public void LoadGame()
    {
        SaveAndLoad load = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
        load.Load();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif 
    }
}


using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;


public class SaveAndLoad : MonoBehaviour
{

    public int sceneIndex;
    public Transform spawnPosition;
	public float yRotation;
    public GUIText saveText;
//	public GUITexture checkJournalImage;
	public int journalSaveIndex;

	private JournalScript journalScript;
	private TextTransformUp textTransformUp;
    private double timer;

    void Start()
    {
		journalScript = GameObject.Find ("Scene Manager").GetComponent<JournalScript> ();
//		textTransformUp = GameObject.Find ("DialogText").GetComponent<TextTransformUp> ();
        saveText.enabled = false;
//		checkJournalImage.enabled = false;
    }

//    void Update()
//    {
//		if(checkJournalImage.enabled == true)
//        {
//            if (Input.GetKeyDown(KeyCode.J))
//				checkJournalImage.enabled = false;
//        }
//    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
        Transform girl = GameObject.Find("Girl_OBJ").transform;
		bool[] activePages = journalScript.activePages;

        GameData data = new GameData();
        data.spawnPositionX = girl.position.x;
        data.spawnPositionY = girl.position.y;
        data.spawnPositionZ = girl.position.z;
		data.spawnRotationY = girl.transform.eulerAngles.y;
		data.activeJournal = activePages;
        data.sceneIndex = SceneManager.GetActiveScene().buildIndex;
		data.unlockCave = GameObject.Find ("Scene Manager").GetComponent<SceneM> ().unlockCave;
		data.unlockForest = GameObject.Find ("Scene Manager").GetComponent<SceneM> ().unlockForest;
		data.saveTextAry = GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().currentTextAry;

        bf.Serialize(file, data);
        file.Close();
        StartCoroutine(ShowSaveText());
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            //Load the save file
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);

            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            spawnPosition.position = new Vector3(data.spawnPositionX, data.spawnPositionY, data.spawnPositionZ);
			yRotation = data.spawnRotationY;
			journalScript.activePages = data.activeJournal;
			journalScript.SetPageImage ();
			GameObject.Find ("Scene Manager").GetComponent<SceneM> ().unlockCave = data.unlockCave;
			GameObject.Find ("Scene Manager").GetComponent<SceneM> ().unlockForest = data.unlockForest;
			GameObject.Find("Scene Manager").GetComponent<SceneM>().textArray = data.saveTextAry;
            GameObject.Find("Scene Manager").GetComponent<SceneM>().saveID = data.sceneIndex;
        }
    }

	IEnumerator ShowSaveText()
	{
		saveText.enabled = true;

		//The the journal has not been activated
//		if(!journalScript.activePages[journalSaveIndex])
//			checkJournalImage.enabled = true;

		yield return new WaitForSeconds(4);
		saveText.enabled = false;
	}


    [Serializable]
    class GameData
    {
        public int sceneIndex;
        public float spawnPositionX;
        public float spawnPositionY;
        public float spawnPositionZ;
		public float spawnRotationY;
		public bool[] activeJournal;
		public bool unlockCave;
		public bool unlockForest;
		public string[] saveTextAry;
    }
}

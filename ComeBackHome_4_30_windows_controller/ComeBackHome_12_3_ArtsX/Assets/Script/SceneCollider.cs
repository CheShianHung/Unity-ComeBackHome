using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//This script enables the collider we need and disables the collider we don't need for the current scene
public class SceneCollider : MonoBehaviour {

	public GameObject villageSceneTrigger;
	public GameObject villageTriggerBlock;
	public GameObject villageIndicators;
	public GameObject caveSceneTrigger;
	public GameObject caveTriggerBlock;
	public GameObject caveIndicators;
	public GameObject forestSceneTrigger;
	public GameObject forestTriggerBlock;
	public GameObject forestIndicators;
	public GameObject lakeSceneTrigger;
	public GameObject lakeTriggerBlock;

	private SaveAndLoad saveAndLoad;
	private SceneM sceneM;

    void Awake()
    {
		InitializeSceneObjects ();
    }

	public void InitializeSceneObjects() 
	{
//		journalScript = gameObject.GetComponent<JournalScript> ();
		saveAndLoad = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
		sceneM = GameObject.Find ("Scene Manager").GetComponent<SceneM> ();

		villageSceneTrigger.SetActive (false);
		villageTriggerBlock.SetActive (false);
		villageIndicators.SetActive (false);
		caveSceneTrigger.SetActive (false);
		caveTriggerBlock.SetActive (false);
		caveIndicators.SetActive (false);
		forestSceneTrigger.SetActive (false);
		forestTriggerBlock.SetActive (false);
		forestIndicators.SetActive (false);
		lakeSceneTrigger.SetActive (false);
		lakeTriggerBlock.SetActive (false);

		//If in village
		if (SceneManager.GetActiveScene ().buildIndex == 1) {

			//objects
			villageSceneTrigger.SetActive (true);
			villageTriggerBlock.SetActive (true);
			villageIndicators.SetActive (true);

			//entrance
			villageSceneTrigger.transform.Find("Village to Cave Entrance").GetComponent<Collider> ().enabled = false;
			villageSceneTrigger.transform.Find("Village to Forest Entrance").GetComponent<Collider> ().enabled = false;
			villageSceneTrigger.transform.Find("Village to Lake Entrance").GetComponent<Collider> ().enabled = false;

			//blocks
			villageTriggerBlock.transform.Find ("Cave Block").GetComponent<Collider> ().enabled = true;
			villageTriggerBlock.transform.Find ("Forest Block").GetComponent<Collider> ().enabled = true;
			villageTriggerBlock.transform.Find ("Lake Block").GetComponent<Collider> ().enabled = true;

			//indicators
			villageIndicators.transform.Find ("Cave Entrance").gameObject.SetActive (false);
			villageIndicators.transform.Find ("Forest Entrance").gameObject.SetActive (false);
			villageIndicators.transform.Find ("Lake Entrance").gameObject.SetActive (false);

			//if after cave and forest
			if (sceneM.unlockCave && sceneM.unlockForest) {

				//activate lake scene
				villageSceneTrigger.transform.Find("Village to Lake Entrance").GetComponent<Collider> ().enabled = true;
				villageTriggerBlock.transform.Find ("Lake Block").GetComponent<Collider> ().enabled = false;
				villageIndicators.transform.Find ("Lake Entrance").gameObject.SetActive (true);
			}
			//if after cave
			else if (sceneM.unlockCave) {

				//activate forest scene
				villageSceneTrigger.transform.Find("Village to Forest Entrance").GetComponent<Collider> ().enabled = true;
				villageTriggerBlock.transform.Find ("Forest Block").GetComponent<Collider> ().enabled = false;
				villageIndicators.transform.Find ("Forest Entrance").gameObject.SetActive (true);
			}


		}

		//If in forest
		if (SceneManager.GetActiveScene ().buildIndex == 2) {
			forestSceneTrigger.SetActive (true);
			forestTriggerBlock.SetActive (true);
//			forestSceneTrigger.transform.Find ("Forest to Village Entrance").GetComponent<EndingTrigger> ().collideOnce = false;
		}

		//If in cave
		if (SceneManager.GetActiveScene ().buildIndex == 3) {
			caveSceneTrigger.SetActive (true);
			caveTriggerBlock.SetActive (true);
		}
		//If in lake
		if (SceneManager.GetActiveScene ().buildIndex == 4) {
			lakeTriggerBlock.SetActive (true);
		}
	}

	//Village index: 1
	//Forest index: 2
	//Cave index: 3
	//Lake index: 4
	//Ending index: 5
	//Intro Image: 6

	public void AfterJournalCollect (int index) {
		TextTransformUp textTransformUp = GameObject.Find ("DialogText").GetComponent<TextTransformUp>();

		//in village scene (before cave), unlock cave scene
		if (index == 1) {
			InitializeSceneObjects ();

			villageTriggerBlock.transform.Find ("Cave Block").GetComponent<Collider> ().enabled = false;
			villageIndicators.transform.Find ("Cave Entrance").gameObject.SetActive (true);
			villageSceneTrigger.transform.Find("Village to Cave Entrance").GetComponent<Collider> ().enabled = true;
		}

		//in forest scene, unlock village entrance
		if (index == 2) {
			forestTriggerBlock.transform.Find ("Village Block").GetComponent<Collider> ().enabled = false;
			forestSceneTrigger.transform.Find ("Forest to Village Entrance").GetComponent<Collider> ().enabled = true;
			forestIndicators.SetActive (true);
		}

		//in cave scene, unlock cave back entrance
		if (index == 3) {
			caveTriggerBlock.transform.Find ("Village Back Block").GetComponent<Collider> ().enabled = false;
			caveSceneTrigger.transform.Find ("Cave to Village Entrance").GetComponent<Collider> ().enabled = true;
			caveIndicators.SetActive (true);
		}

		//in lake scene, unlock ending trigger
		if (index == 4) {
			lakeTriggerBlock.SetActive (false);
		}

		textTransformUp.currentTextAry = textTransformUp.afterTriggerText;
	}

	public void AfterSceneLoad(int transNum) {

		//If not the ending scene and main menu scene
		if (SceneManager.GetActiveScene ().buildIndex >= 1 && SceneManager.GetActiveScene ().buildIndex <= 4) {
			TextTransformUp textTransformUp = GameObject.Find ("DialogText").GetComponent<TextTransformUp> ();

			//when transit from village to cave from entrance
			if (transNum == 2) {
				InitializeSceneObjects ();
				textTransformUp.currentTextAry = textTransformUp.idlingText;
			}

			//when transit from cave to village
			if (transNum == 3) {
				sceneM.unlockCave = true;
				InitializeSceneObjects ();
				textTransformUp.currentTextAry = textTransformUp.afterTriggerText; //Change text
			}

			//when transit from village to forest
			if (transNum == 4) {
				InitializeSceneObjects ();
				textTransformUp.currentTextAry = textTransformUp.idlingText;
			}

			//when transit from forest to village
			if (transNum == 5) {
				sceneM.unlockForest = true;
				InitializeSceneObjects ();
				textTransformUp.currentTextAry = textTransformUp.afterTriggerText; //Change text
			}

			//when transit from village to lake
			if (transNum == 6) {
				InitializeSceneObjects ();
			}

			saveAndLoad.Save ();
		}
	}
}

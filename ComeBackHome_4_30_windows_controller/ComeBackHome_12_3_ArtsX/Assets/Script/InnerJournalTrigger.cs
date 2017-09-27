using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerJournalTrigger : MonoBehaviour {

	public GameObject particles;
	public GameObject afterTriggerParticles;
	public GameObject textObject;
	public int journalIndex;
	public float clueTextInterval;

	private float timer;
	private bool showText;
	private int textIndexCounter;
	private string[] textAry;
	JournalScript journalScript;

	private void Start()
	{
		textIndexCounter = 0;
		textAry = textObject.GetComponent<ClueText> ().texts;
		journalScript = GameObject.Find ("Scene Manager").GetComponent<JournalScript> ();
	}

	void Update() {
		if (showText) {
			if (timer < clueTextInterval) {
				timer += Time.deltaTime;
			} else {
				CreateNextText ();
			}
		}
	}

	private void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Player")
		{
			if (particles != null) {
				Destroy (particles);
				//Activate second particles
				afterTriggerParticles.SetActive (true);
			}

			//Text starts floating
			timer = clueTextInterval - 0.5f;
			GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().floatingTextIsOn = false;
			showText = true;

			if (!journalScript.activePages [journalIndex]) 
			{
				journalScript.activePages [journalIndex] = true;
				journalScript.setPage (journalIndex / 2 + 1);
				GameObject.Find ("Check Journal Image").GetComponent<GUITexture>().enabled = true;
				journalScript.SetPageImage ();

				//Save the game
				SaveAndLoad load = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
				load.journalSaveIndex = journalIndex;
				load.Save();
			}
		} 
	}

	private void OnTriggerExit(Collider coll)
	{
		if(coll.tag == "Player")
		{
			//Text stop floating
			showText = false;
			GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().ResetText ();
			GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().floatingTextIsOn = true;
		}
	}

	private void CreateNextText()
	{
		var newText = Instantiate (textObject);
		newText.transform.parent = gameObject.transform.parent.transform;
		newText.GetComponent<TextMesh>().text = textAry [textIndexCounter].Replace ("\\n", "\n");
		newText.SetActive (true);
		if (textIndexCounter == textAry.Length - 1) {
			textIndexCounter = 0;
		} else {
			textIndexCounter++;
		}
		timer = 0;
	}
}

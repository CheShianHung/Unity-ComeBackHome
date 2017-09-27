using UnityEngine;
using System.Collections;

public class InnerBoxTextTrigger : MonoBehaviour
{
	//public GameObject text;
	public GameObject particles;
	public int journalIndex;

	JournalScript journalScript;

	private void Start()
	{
		journalScript = GameObject.Find ("Scene Manager").GetComponent<JournalScript> ();
	}

	private void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Player")
		{
			//text.SetActive(true);
			if (particles != null) {
				Destroy (particles);
			}

			//Save the game
			SaveAndLoad load = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
			load.journalSaveIndex = journalIndex;
			load.Save();

			if (!journalScript.activePages [journalIndex]) 
			{
				journalScript.activePages [journalIndex] = true;
				GameObject.Find ("Check Journal Image").GetComponent<GUITexture>().enabled = true;
				journalScript.SetPageImage ();
			}
		} 
	}
//	private void OnTriggerExit()
//	{
//		text.SetActive(false);
//	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterJournalTrigger : MonoBehaviour {

	public GameObject particles;
	public GameObject afterTriggerParticles;
	public int journalIndex;

	private void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Player" && particles != null && !GameObject.Find("Scene Manager").GetComponent<JournalScript>().activePages[journalIndex]) {
			particles.SetActive (true);
		} else if (coll.tag == "Player") {
			afterTriggerParticles.SetActive (true);
		}
	}

	private void OnTriggerExit(Collider coll)
	{
		if (coll.tag == "Player" && particles != null){
			particles.SetActive (false);
		} else if (coll.tag == "Player") {
			afterTriggerParticles.SetActive (false);
		}
	}
}

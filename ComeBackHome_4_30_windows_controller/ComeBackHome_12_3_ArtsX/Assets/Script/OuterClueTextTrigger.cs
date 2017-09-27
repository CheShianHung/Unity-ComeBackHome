using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterClueTextTrigger : MonoBehaviour {

	public GameObject particles;

	private void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Player")
		{
			particles.SetActive (true);
		} 
	}

	private void OnTriggerExit(Collider coll)
	{
		if (coll.tag == "Player")
		{
			particles.SetActive (false);
		} 
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndingTrigger : MonoBehaviour {

	public bool collideOnce;

	void Start()
	{
		collideOnce = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player" && !collideOnce)
		{
			collideOnce = true;
//			print ("load ending scene.");
			GameObject.Find("Scene Manager").GetComponent<SceneM>().areaID = 8;
//			GameObject.Find("Scene Manager").GetComponent<SceneM>().saveID = 0;

		}
	}
}

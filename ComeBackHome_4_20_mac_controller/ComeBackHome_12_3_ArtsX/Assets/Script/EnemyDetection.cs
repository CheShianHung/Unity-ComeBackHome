using UnityEngine;
using System.Collections;

public class EnemyDetection : MonoBehaviour {

	private bool loading;

	void Start()
	{
		loading = false;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "enemy" && !loading) 
		{
			GetComponent<Animator>().SetBool ("deathByWolf", true);
			loading = true;
			SaveAndLoad load = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
			load.Load();
		}

		if (collision.gameObject.tag == "caveFalling") {
			GetComponent<Animator>().SetBool ("deathFalling", true);
		}
			
	}

}

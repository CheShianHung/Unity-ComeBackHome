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
		if (!GameObject.Find ("Girl_OBJ").GetComponent<BeginPosition> ().collideOnce) {

			if (collision.gameObject.tag == "enemy" && !loading) {
				print ("wolf load");
				GetComponent<Animator> ().SetBool ("deathByWolf", true);
				loading = true;
				SaveAndLoad load = GameObject.Find ("Scene Manager").GetComponent<SaveAndLoad> ();
				load.Load ();
			}

			if (collision.gameObject.tag == "caveFalling") {
				GetComponent<Animator> ().SetBool ("deathFalling", true);
			}
		}
			
	}

}

using UnityEngine;
using System.Collections;

public class DeadLoading : MonoBehaviour {

	private bool loadOnce;

	void Start() {
		loadOnce = false;
	}

	void OnTriggerEnter(Collider other)
    {
		if (!loadOnce && other.tag == "Player") {
//			StartCoroutine (WaitAndLoad ());
			loadOnce = true;
			SaveAndLoad saveAndLoad = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
			saveAndLoad.Load();
		}
    }

//    IEnumerator WaitAndLoad()
//    {
//        yield return new WaitForSeconds(1);
//		loadOnce = true;
//        SaveAndLoad saveAndLoad = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
//        saveAndLoad.Load();
//    }
}

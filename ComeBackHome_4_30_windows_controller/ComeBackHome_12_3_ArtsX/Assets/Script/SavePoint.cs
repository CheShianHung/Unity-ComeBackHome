using UnityEngine;
using System.Collections;

public class SavePoint : MonoBehaviour {

    void OnTriggerEnter()
    {
        SaveAndLoad saveAndLoad = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
        saveAndLoad.Save();
    }
}

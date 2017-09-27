using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {

    private static bool created = false;
    
    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(transform.gameObject);
            created = true;
        }
        else
        {
            Destroy(transform.gameObject);
        }
    }
}

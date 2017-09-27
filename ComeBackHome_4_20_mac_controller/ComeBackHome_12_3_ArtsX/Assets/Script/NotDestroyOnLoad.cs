using UnityEngine;
using System.Collections;

public class NotDestroyOnLoad : MonoBehaviour {

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
    
   /* 
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    */
}

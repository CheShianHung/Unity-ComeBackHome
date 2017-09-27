using UnityEngine;
using System.Collections;

public class JournalNotDestroy : MonoBehaviour {

	private static bool created = false;

    void Awake()
    {
        if (!created)
        {
			print ("Not created yet");
            DontDestroyOnLoad(transform.gameObject);
            created = true;
        }
        else
        {
			print ("alrealy there");
            Destroy(transform.gameObject);
        }
    }
}

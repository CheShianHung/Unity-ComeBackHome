using UnityEngine;
using System.Collections;

public class LookAtCameraBehaviour : MonoBehaviour
{
	// Use this for initialization
	void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () 
    {
		if (Camera.main == null) return;
        transform.LookAt( new Vector3( Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z ) ); 
	}
}

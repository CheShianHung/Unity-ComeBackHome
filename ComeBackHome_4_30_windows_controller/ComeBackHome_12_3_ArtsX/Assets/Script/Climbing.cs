using UnityEngine;
using System.Collections;

public class Climbing : MonoBehaviour {

    private GameObject girlObj;

    void OnTriggerEnter(Collider other)
    {
        girlObj = GameObject.Find("Girl_OBJ");

        if (other.gameObject.tag == "climbDetect")
        {
            girlObj = GameObject.Find("Girl_OBJ");
            girlObj.GetComponent<PlayerMovement>().climbing = true;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

//This script detects the sceneTrigger collider, getting corresponding areaID and set the girls position and rotation after loading scene
public class BeginPosition : MonoBehaviour {

	private SceneM sceneM;
	private SaveAndLoad saveAndLoad;
    private int tempIndex;
	public bool collideOnce;

    void Awake()
    {
        sceneM = GameObject.Find("Scene Manager").GetComponent<SceneM>();
		saveAndLoad = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>();
		//If no loading
        if (!sceneM.loadGirl)
        {
			//If the first scene is not the start menu (debugging)
            if (sceneM.transitionNumber == 0 && SceneManager.GetActiveScene().buildIndex != 0)
            {
                tempIndex = SceneManager.GetActiveScene().buildIndex;
                transform.position = sceneM.debugPosition[tempIndex].position;
            }
			//If scene changes
            else
            {
                tempIndex = sceneM.transitionNumber; //Get areaID
                transform.position = sceneM.array[tempIndex].position; //Set transform position

            }
            transform.rotation = Quaternion.Euler(0, sceneM.rotationArray[tempIndex], 0); //Set transform rotation
        }
		//If loading
        else
        {
            transform.position = sceneM.savePosition.position;
			transform.eulerAngles = new Vector3 (0f, saveAndLoad.yRotation, 0f);
        }
        sceneM.loadGirl = false;
		collideOnce = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SceneTrigger" && !collideOnce)
        {
			collideOnce = true;

			//without cut scenes after each map
			if (other.gameObject.name == "Village to Cave Entrance")
				sceneM.areaID = 2;
			if (other.gameObject.name == "Cave to Village Entrance")
				sceneM.areaID = 3;
			if (other.gameObject.name == "Village to Forest Entrance")
				sceneM.areaID = 4;
			if (other.gameObject.name == "Forest to Village Entrance")
				sceneM.areaID = 5;
			if (other.gameObject.name == "Village to Lake Entrance")
				sceneM.areaID = 6;
			if (other.gameObject.name == "Lake to Village Entrance")
				sceneM.areaID = 7;


			//with cut scenes after each map
//			if (other.gameObject.name == "Village to Cave Entrance")
//				sceneM.areaID = 10;
//			if (other.gameObject.name == "Cave to Village Entrance")
//				sceneM.areaID = 12;
//			if (other.gameObject.name == "Village to Forest Entrance")
//				sceneM.areaID = 4;
//			if (other.gameObject.name == "Forest to Village Entrance")
//				sceneM.areaID = 13;
//			if (other.gameObject.name == "Village to Lake Entrance")
//				sceneM.areaID = 6;
//			if (other.gameObject.name == "Lake to Village Entrance")
//				sceneM.areaID = 7;
			
        }
    }
}

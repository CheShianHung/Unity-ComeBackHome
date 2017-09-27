using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectOnInput : MonoBehaviour {

    public EventSystem eventSystem;
    public GameObject selectedObject;

	private SceneM sceneM;
    private bool buttonSelected;

	void Start()
	{
		sceneM = GameObject.Find ("Scene Manager").GetComponent<SceneM>();
	}

	// Update is called once per frame
	void Update ()
    { 
		if(Input.GetAxisRaw("Vertical")!=0 && buttonSelected == false && !sceneM.journalIsOn)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
	}

    private void OnDisable()
    {
        buttonSelected = false;
    }
}

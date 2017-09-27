using UnityEngine;
using System.Collections;

//This script controlls the camera movement and camera wall collision
public class CameraMovement : MonoBehaviour
{

	public Transform target; //1st Cam Position
	public Transform camCollision; //Cam Collision Point
	public Transform mainCamPosition; //Main Camera
	public Transform thirdPerson; //3rd Cam Position
	public float mouseSpeed = 0.5f;
	public float maxCamHeight = 1.7f;
	public float minCamHeight = -2.0f;
	public float maxZoomLength;
	public float minZoomLength;
	public float zoomSpeed;

	private Vector3 offset;

	void Start()
	{
		offset = transform.position - target.position;
		maxZoomLength = 8f;
		minZoomLength = 1.5f;
		zoomSpeed = 0.05f;

	}

	void LateUpdate()
	{

		if (Time.deltaTime != 0) {
			CameraRotation ();
			Zooming ();
			CameraWallCollision ();
		}


	}

	void CameraRotation()
	{
		if(Input.GetAxis("MouseRS X") > 0.15f || Input.GetAxis("MouseRS X") < -0.15f || Input.GetAxis("MouseRS Y") > 0.15f || Input.GetAxis("MouseRS Y") < -0.15f){
			//Get new asset with mouse drag
			offset = Quaternion.AngleAxis(Input.GetAxis("MouseRS X") * 5, Vector3.up) * offset;
			if (transform.position.y - camCollision.position.y >= minCamHeight && transform.position.y - camCollision.position.y <= maxCamHeight)
				offset = Quaternion.AngleAxis(-Input.GetAxis("MouseRS Y") * 3, mainCamPosition.right) * offset;
			if (transform.position.y - camCollision.position.y < minCamHeight)
			if (Input.GetAxis("MouseRS Y") < 0)
				offset = Quaternion.AngleAxis(-Input.GetAxis("MouseRS Y") * 3, mainCamPosition.right) * offset;
			if (transform.position.y - camCollision.position.y > maxCamHeight)
				if (Input.GetAxis("MouseRS Y") > 0)
				offset = Quaternion.AngleAxis(-Input.GetAxis("MouseRS Y") * 3, mainCamPosition.right) * offset;
			transform.position = target.position + offset;
			mainCamPosition.LookAt(camCollision.position); //Camera face the player
		}

		//Get new asset with mouse drag
		offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * mouseSpeed, Vector3.up) * offset;
		if (transform.position.y - camCollision.position.y >= minCamHeight && transform.position.y - camCollision.position.y <= maxCamHeight)
			offset = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * 5, mainCamPosition.right) * offset;
		if (transform.position.y - camCollision.position.y < minCamHeight)
		if (Input.GetAxis("Mouse Y") < 0)
			offset = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * 5, mainCamPosition.right) * offset;
		if (transform.position.y - camCollision.position.y > maxCamHeight)
		if (Input.GetAxis("Mouse Y") > 0)
			offset = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * 5, mainCamPosition.right) * offset;
		transform.position = target.position + offset;
		mainCamPosition.LookAt(camCollision.position); //Camera face the player

	}

	void Zooming()
	{
		float mouseScroll = 0;
		if(Input.GetAxis("Mouse ScrollWheel") != 0)
			mouseScroll = Input.GetAxis ("Mouse ScrollWheel");
		if(Input.GetAxis("Joy ScrollWheel") != 0)
			mouseScroll = Input.GetAxisRaw("Joy ScrollWheel") * 0.15f;
		
		float length = offset.magnitude;

		if (length > maxZoomLength && mouseScroll > 0f) {
			offset = (1 - zoomSpeed) * offset;
		}
		if (length < minZoomLength && mouseScroll < 0f) {
			offset = (1 + zoomSpeed) * offset;
		}
		if (length < maxZoomLength && length > minZoomLength && mouseScroll > 0f) {
			offset = (1 - zoomSpeed) * offset;
		}
		if (length < maxZoomLength && length > minZoomLength && mouseScroll < 0f) {
			offset = (1 + zoomSpeed) * offset;
		}

		transform.position = target.position + offset;

	}

	void ThirdPersonCamera()
	{
		//Get offset vector
		offset = transform.position - target.position;
		transform.position = thirdPerson.position;//cam container goes to third cam position
		mainCamPosition.LookAt(camCollision.position);//Camera looks at girl
	}

	void CameraWallCollision()
	{
		RaycastHit wallHit = new RaycastHit();

		if (Physics.Linecast(camCollision.position, transform.position, out wallHit) && !wallHit.collider.isTrigger)
		{
			//print(wallHit.collider.gameObject.name);
			mainCamPosition.position = wallHit.point;
			mainCamPosition.LookAt(camCollision.position);
		}
		else
		{
			mainCamPosition.position = Vector3.Lerp(mainCamPosition.position, transform.position, 500 * Time.deltaTime);
			mainCamPosition.LookAt(camCollision.position);
		}

	}

}

//using UnityEngine;
//using System.Collections;
//
////This script controlls the camera movement and camera wall collision
//public class CameraMovement : MonoBehaviour
//{
//
//    public Transform target; //1st Cam Position
//    public Transform camCollision; //Cam Collision Point
//    public Transform mainCamPosition; //Main Camera
//    public Transform thirdPerson; //3rd Cam Position
//    public Transform thirdPersonTent; //3rd Cam Position inside tent 
//    public float mouseSpeed = 6.0f;
//    public float firstPersonSmoothing = 30f;
//    public bool rightClickMode;
//    public float maxCamHeight = 1.7f;
//    public float minCamHeight = -2.0f;
//    public bool insideTent;
//    public bool lerpToThirdPosition; //camera lerp back to third person position
//
//    private Vector3 offset;
//    private bool thirdCamSwitch; //Third person cam
//    private bool reachCam; //If the camera reach third person cam position
//    private float lerpTimer;
//    private float rightClickLerpTimer;
//    private Transform thirdPersonCam;
//    private bool tentTransition;
//    private Transform camTemp;
//    private bool insideTentSwitch;
//    private bool outsideTentSwitch;
//    private bool lerping;
//    private PlayerMovement playerMovement;
////	private SceneM sceneM;
//
//    void Start()
//    {
//        GameObject girl = GameObject.Find("Girl_OBJ");
//		playerMovement = girl.GetComponent<PlayerMovement>();
//        //Get offset vector
//        offset = transform.position - target.position;
//        thirdCamSwitch = true;
//        reachCam = true;
//        lerpToThirdPosition = false;
//        rightClickMode = false;
//        lerpTimer = 0;
//        rightClickLerpTimer = 0;
//        thirdPersonCam = thirdPerson;
//        tentTransition = false;
//
////		sceneM = GameObject.Find("Scene Manager").GetComponent<SceneM>();
//    }
//
//    void Update()
//    {
//        //If the player pressed the key "c"
//        if (Input.GetButtonUp("Fire3"))
//        {
//            if (thirdCamSwitch == true)
//                thirdCamSwitch = false;//Change to first person cam
//            else
//                thirdCamSwitch = true;//Change to thrid person cam
//        }
//
//    }
//
//    void LateUpdate()
//    {
//        Debugging();
//        //For third person cam
//        if (thirdCamSwitch && !tentTransition)
//        {
//            //If the camera is back to cam container and the player press right click
//			if (reachCam == true && Input.GetMouseButton(1) && Time.timeScale != 0)
//            {
//                rightClickMode = true; //Go into right click mode
//            }
//            //If the right click does not press/hold and in right click mode and the palyer presses any arrow key
//			if (!Input.GetMouseButton(1) && rightClickMode && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && !playerMovement.climbingRightClickCam && !playerMovement.isIcing)
//            {
//                rightClickMode = false; //Exit right click mode
//                lerpToThirdPosition = true; //Camera starts lerp back to cam container
//            }
//            //When in right click mode
//            if (rightClickMode)
//            {
//                CameraRotation();
//                reachCam = false;
//            }
//
//            //When the camera goes back to cam container
//            if (reachCam)
//            {
//                ThirdPersonCamera(); //Camera sticks to player
//            }
//
//            //Once the right click mode is canceled, reset the camera position and cam container position
//            if (lerpToThirdPosition)
//            {
//                lerping = true;
//                transform.position = Vector3.Lerp(transform.position, thirdPersonCam.position, rightClickLerpTimer += Time.deltaTime / 2);
//                mainCamPosition.position = Vector3.Lerp(mainCamPosition.position, transform.position, rightClickLerpTimer += Time.deltaTime / 2);
//                mainCamPosition.LookAt(camCollision.position);
//                if (mainCamPosition.position == transform.position && transform.position == thirdPersonCam.position)
//                {
//                    //print("back to original");
//                    rightClickLerpTimer = 0;
//                    reachCam = true;
//                    lerping = false;
//                    lerpToThirdPosition = false;
//                }
//            }
//            CameraWallCollision();
//        }
//        else if (!tentTransition)
//        {
//            FirstPersonCamera();
//            if (Input.GetButtonUp("Fire3"))
//            {
//                reachCam = false;
//                lerpToThirdPosition = true;
//            }
//        }
//
//        //For Camera transition inside/outside the tent
//        if (insideTent && thirdCamSwitch && !rightClickMode && !lerping)
//        {
//            insideTentSwitch = true;
//
//            camTemp = thirdPersonTent;
//            TentTransition(camTemp, "inside");
//        }
//        else if (thirdCamSwitch && !rightClickMode && !lerping)
//        {
//            outsideTentSwitch = true;
//
//            camTemp = thirdPerson;
//            TentTransition(camTemp, "outside");
//        }
//
//
//    }
//
//    void CameraRotation()
//    {
//        //Get new asset with mouse drag
//        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * mouseSpeed, Vector3.up) * offset;
//        if (transform.position.y - camCollision.position.y >= minCamHeight && transform.position.y - camCollision.position.y <= maxCamHeight)
//            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 5, mainCamPosition.right) * offset;
//        if (transform.position.y - camCollision.position.y < minCamHeight)
//            if (Input.GetAxis("Mouse Y") > 0)
//                offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 5, mainCamPosition.right) * offset;
//        if (transform.position.y - camCollision.position.y > maxCamHeight)
//            if (Input.GetAxis("Mouse Y") < 0)
//                offset = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * 5, mainCamPosition.right) * offset;
//        transform.position = target.position + offset;
//        mainCamPosition.LookAt(camCollision.position); //Camera face the player
//    }
//
//    void ThirdPersonCamera()
//    {
//        //Get offset vector
//        offset = transform.position - target.position;
//        transform.position = thirdPersonCam.position;//cam container goes to third cam position
//        mainCamPosition.LookAt(camCollision.position);//Camera looks at girl
//    }
//
//    void CameraWallCollision()
//    {
//        RaycastHit wallHit = new RaycastHit();
//
//        if (Physics.Linecast(camCollision.position, transform.position, out wallHit) && !wallHit.collider.isTrigger)
//        {
//            //print(wallHit.collider.gameObject.name);
//            mainCamPosition.position = wallHit.point;
//            mainCamPosition.LookAt(camCollision.position);
//        }
//        else
//        {
//            mainCamPosition.position = Vector3.Lerp(mainCamPosition.position, transform.position, 500 * Time.deltaTime);
//            mainCamPosition.LookAt(camCollision.position);
//        }
//
//    }
//
//    void FirstPersonCamera()
//    {
//        mainCamPosition.position = Vector3.Lerp(mainCamPosition.position, target.position, firstPersonSmoothing * Time.deltaTime);
//        mainCamPosition.rotation = Quaternion.Lerp(mainCamPosition.rotation, target.rotation, firstPersonSmoothing * Time.deltaTime);
//		float mouseInput = Input.GetAxis("Mouse Y");
//		Vector3 lookhere = new Vector3(-mouseInput * 5f, 0, 0);
//		target.Rotate(lookhere);
//    }
//
//    void Debugging()
//    {
//        //Draw line from cam container to girl's head
//        Debug.DrawLine(camCollision.position, transform.position, Color.blue);
//        //Indicate main camera's position
//        Debug.DrawRay(mainCamPosition.position, mainCamPosition.right, Color.red);
//        //Indicate cam container's position
//        Debug.DrawRay(transform.position, -transform.forward, Color.green);
//        //Indicate third cam position to camCollision
//        Debug.DrawLine(thirdPersonCam.position, camCollision.position, Color.yellow);
//    }
//
//    void TentTransition(Transform temp, string status)
//    {
//        //print("inside: " + insideTentSwitch + "; outside: " + outsideTentSwitch + "; status: " + status);
//        if (insideTentSwitch && outsideTentSwitch)
//        {
//            lerpTimer = 0;
//        }
//
//        if (status == "inside")
//        {
//            outsideTentSwitch = false;
//        }
//        else
//        {
//            insideTentSwitch = false;
//        }
//
//        if (transform.position != temp.position)
//        {
//            tentTransition = true;
//            transform.position = Vector3.Lerp(transform.position, temp.position, lerpTimer += Time.deltaTime / 4);
//            mainCamPosition.position = Vector3.Lerp(mainCamPosition.position, temp.position, lerpTimer += Time.deltaTime / 4);
//            mainCamPosition.LookAt(camCollision.position);
//        }
//        else
//        {
//            lerpTimer = 0;
//            thirdPersonCam = temp;
//
//            if (temp = thirdPersonTent)
//            {
//                insideTentSwitch = false;
//            }
//            else
//            {
//                outsideTentSwitch = false;
//            }
//
//            tentTransition = false;
//
//        }
//
//        //lerping
//        RaycastHit wallHit = new RaycastHit();
//
//        if (Physics.Linecast(camCollision.position, mainCamPosition.position, out wallHit) && !wallHit.collider.isTrigger)
//        {
//            mainCamPosition.position = wallHit.point;
//            mainCamPosition.LookAt(camCollision.position);
//        }
//
//    }
//}

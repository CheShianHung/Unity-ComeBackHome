using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public GameObject mainCam;
	public float moveSpeed = 3f;
	public float turnSpeed = 6f;
	public float firstPersonTurnSpeed = 5f;
	public float jumpSpeed = 5f;
	public float gravityForce = 1.2f;
	public float gravity = 19f;

	Animator anim;
	private Vector3 movement;
	private Rigidbody playerRigidbody;
	private CameraMovement cameraMovement;
	private float speed;
	//private bool crawling;

	//for Jumping
	private bool onGround;
	private float timer;
	private float timer2;
	private bool jumping;
	private float forceY = 0;
	private float invertGrav;
	private float airTime = 1.5f;
	private Vector3 moveDirection = Vector3.zero;
	private SceneM sceneM;

	//for Climbing
	public float vOffset = 0f;
	public float hOffset = 0f;
	public bool climbing;
	public bool climbingRightClickCam;
	private bool climbingUp;
	private Vector3 tempVerticalPosition;
	private Vector3 tempHorizontalPosition;
	private bool goUp;
	public bool hanging;
	private bool climbStraightUp;
	private Vector3 hangPosition;

	//for Icing
	public bool isIcing;
	private bool icingDirection;
	private float icingSpeed;
	private float hVal;
	private float vVal;


	void Awake()
	{
		anim = GetComponent<Animator>();
		cameraMovement = GameObject.Find("Camera Container").GetComponent<CameraMovement>();
		playerRigidbody = GetComponent<Rigidbody>();
		playerRigidbody.freezeRotation = true;
		//running = false;
		//crawling = false;
		climbing = false;
		invertGrav = gravity * airTime;
		onGround = true;
		climbing = false;
		climbingUp = false;
		goUp = false;
		hanging = false;
		climbStraightUp = false;
		climbingRightClickCam = false;
		isIcing = false;
		icingDirection = false;
		hVal = 0;
		vVal = 0;

		sceneM = GameObject.Find("Scene Manager").GetComponent<SceneM>();
		anim.SetBool ("deathFalling", false);
		anim.SetBool ("deathByWolf", false);
	}

	void Update()
	{
		if (!sceneM.journalIsOn && !sceneM.pauseMenuIsOn) {
			if (isIcing) {
				IcingMovement ();
//				if (!cameraMovement.rightClickMode)
//					cameraMovement.rightClickMode = true;
			} else {
				icingDirection = false;
				hVal = 0;
				vVal = 0;
			}


			//Jumping Animation
			//			if (jumping) {
			//				//anim.SetBool ("IsJumping", true);
			//			} else {
			//				anim.SetBool ("IsJumping", false);
			//			}

			if (!jumping) {
				anim.SetBool ("IsJumping", false);
				anim.SetBool ("IsJumpingDown", false);
			}

			if (!climbing && !hanging && !isIcing) {
				Movement ();
				//Jumping();
				Running ();
				//Crawling();
				DustTrails (onGround);
			}

			if (!isIcing) {
				Jumping ();
			}
			Debugging ();
		}
	}

	void FixedUpdate()
	{

		if(climbing)
		{
			LerpToClimbPos();
		}

		if(hanging)
		{
			Hanging();
		}

		if(climbingUp)
		{
			ClimbingUp();
		}

	}

	void IcingMovement()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		if ((h != 0 || v != 0) && !icingDirection) {
			icingDirection = true;
			hVal = h;
			vVal = v;
		}

		//Get moving direction from input
		movement = transform.right * hVal + transform.forward * vVal;
		movement.y = 0;
		movement = movement.normalized * icingSpeed * Time.deltaTime;

		//Move the player
		playerRigidbody.MovePosition(transform.position + movement);
	}

	//for jumping and icing
	void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag == "ground")
		{
			timer = 0;
			onGround = true;
			jumping = false;
			isIcing = false;
			anim.SetBool ("isSliding", false);
		}

		if (other.collider.tag == "ice" && !isIcing)
		{
			icingSpeed = moveSpeed + 7;
			isIcing = true;
			anim.SetBool ("isSliding", true);
		}
	}

	void OnCollisionExit(Collision other)
	{
		//if in lake scene
		if (SceneManager.GetActiveScene ().buildIndex == 4) {
//			if (other.collider.tag == "ground" && !isIcing) {
//				icingSpeed = speed + 2;
//				isIcing = true;
//			}

			if (other.collider.tag == "ice" && isIcing) {
				isIcing = false;
				anim.SetBool ("isSliding", false);
			}
		}
	}

	void OnCollisionStay(Collision other){
		if (other.collider.tag == "ice" && !isIcing) {
			isIcing = true;
			anim.SetBool ("isSliding", true);
		}
		if (other.collider.tag == "ground" && isIcing) {
			isIcing = false;
			anim.SetBool ("isSliding", false);
		}
	}

	void Jumping()
	{
		moveDirection = new Vector3(0, 0, 0);
		if (onGround)
		{
			timer = 0;
			forceY = 0;
			invertGrav = gravity * airTime;
			if (Input.GetButton("Jump"))
			{
				jumping = true;
				forceY = jumpSpeed;
				onGround = false;
			}
		}

		if (Input.GetButton("Jump") && forceY != 0 && timer < 0.2 && Time.timeScale != 0)
		{
			timer += Time.deltaTime;
			invertGrav -= Time.deltaTime;
			forceY += invertGrav * Time.deltaTime;
		}

		if (jumping && !onGround)
		{
			if (forceY >= 0) {
				anim.SetBool ("IsJumping", true);
				anim.SetBool ("IsJumpingDown", false);
			} else {
				anim.SetBool ("IsJumping", false);
				anim.SetBool ("IsJumpingDown", true);
			}
			// Here we apply the gravity
			forceY -= gravity * Time.deltaTime * gravityForce;
			moveDirection.y = forceY;
			playerRigidbody.velocity = moveDirection;
		}
	}

	void Movement()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
		anim.SetFloat("moveX", v * speed);
		anim.SetFloat("moveY", h * speed);

//		if (h > 0.15f || h < -0.15f || v == 1f || v == -1f) {
//			Vector3 rotation = new Vector3 (0, mainCam.transform.eulerAngles.y, 0);
//			transform.eulerAngles = rotation;
//		}
//
//		if (h > 0.25f || h < -0.25f) {
//			anim.SetBool ("isMovingHorizontal", true);
//		}
//		else{
//			anim.SetBool ("isMovingHorizontal", false);
//		}
//
//		if (v > 0.75f || v < -0.75f){
//			anim.SetBool ("isMovingVertical", true);
//		}
//		else{
//			anim.SetBool("isMovingVertical", false);
//		}

		if (h != 0 || v != 0) {
			Vector3 rotation = new Vector3 (0, mainCam.transform.eulerAngles.y, 0);
			transform.eulerAngles = rotation;
			anim.SetBool ("isMoving", true);
		} else {
			anim.SetBool ("isMoving", false);
		}

//		if (h != 0) {
//			anim.SetBool ("isMovingHorizontal", true);
//		}
//		else{
//			anim.SetBool ("isMovingHorizontal", false);
//		}
//
//		if (v != 0){
//			anim.SetBool ("isMovingVertical", true);
//		}
//		else{
//			anim.SetBool("isMovingVertical", false);
//		}

		//Get moving direction from input
		movement = transform.right * h + transform.forward * v;
		movement.y = 0;
		movement = movement.normalized * speed * Time.deltaTime;

		//Move the player
		playerRigidbody.MovePosition(transform.position + movement);


		/*Slope detection
        RaycastHit hit;
        if (Physics.Raycast(point.position, transform.forward, out hit, 1.0f))
        {
            if (Vector3.Dot(Vector3.up, hit.normal) > 0.7)
            {
            }
        }
        */
	}

	void WalkingAnim(float h, float v)
	{
		bool forward = h == 0 && v > 0 && !Input.GetButton("Fire2");
		bool backward = h == 0 && v < 0 && !Input.GetButton("Fire2");
		bool left = h < 0 && !Input.GetButton("Fire2");
		bool right = h > 0 && !Input.GetButton("Fire2");

		anim.SetBool("IsWalkingForward", forward);
		anim.SetBool("IsWalkingBackward", backward);
		anim.SetBool("IsSlidingLeft", left);
		anim.SetBool("IsSlidingRight", right);
	}

	void Running()
	{
		float runningSpeed = moveSpeed + 3;
		//If left shift is held, run
		if (Input.GetButton("Fire2") || Input.GetAxisRaw("Fire2") == 1)
		{
			if (speed < runningSpeed) {
				speed += Time.deltaTime * 5f;
			} else if (speed > runningSpeed) {
				speed = runningSpeed;
			}
			//speed = moveSpeed + 1;
		}
		else
		{
			if (speed < moveSpeed) {
				speed = moveSpeed;
			} else if (speed > moveSpeed) {
				speed -= Time.deltaTime * 5f;
			}
			//			speed = moveSpeed;
		}
	}

	void Crawling(){
		//If left ctrl is held, crawl
		if (Input.GetButton("Fire1"))
		{
			//crawling = true;
			speed = moveSpeed - 2;
		}
		else
		{
			//crawling = false;
			speed = moveSpeed;
		}
	}

	void DustTrails(bool onGround)
	{
		GameObject left = transform.Find("Left DustTrail").gameObject;
		GameObject right = transform.Find("Right DustTrail").gameObject;
		//print(onGround);
		if (!onGround)
		{
			left.GetComponent<ParticleSystem>().Play();
			right.GetComponent<ParticleSystem>().Play();
		}
	}

	void LerpToClimbPos()
	{
		RaycastHit verticalHit = new RaycastHit();
		RaycastHit horizontalHit = new RaycastHit();

		if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(0.0f, 0.5f, 0.0f)), transform.forward, out horizontalHit, 1f))
		{
			//Debug.DrawLine(horizontalHit.point, horizontalHit.transform.right, Color.red);
		}
		if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.0f, 0.7f)), -transform.up, out verticalHit, 1f) && verticalHit.collider.gameObject.tag != "climb")
		{
			//Debug.DrawLine(verticalHit.point, verticalHit.transform.forward, Color.blue);
			anim.SetBool("IsJumping", false);
			anim.SetBool("IsJumpingDown", false);
			playerRigidbody.isKinematic = true;
//			cameraMovement.rightClickMode = true;
			climbingRightClickCam = true;
			//onGround = false;
			Vector3 temp = horizontalHit.point + transform.TransformDirection(new Vector3(0.0f, 0.0f, -0.25f));
			hangPosition = new Vector3(temp.x, verticalHit.point.y + vOffset, temp.z);
			transform.position = hangPosition;
			timer = 0;
			anim.SetBool("Hanging", true);
			hanging = true;
			climbing = false;
		}
		else
		{
			climbing = false;
		}

	}

	void Hanging()
	{
		float v = Input.GetAxisRaw("Vertical");
		timer += Time.deltaTime;
		forceY = 0;
		onGround = false;
		//jumping = true;
		//print("hanging");
		//print(timer);
		if (v != 0 && timer > 0.5)
		{
			//print("press up or down");
			if (v < 0)
			{
				//cameraMovement.rightClickMode = false;
				//cameraMovement.lerpToThirdPosition = true;
				jumping = true;
				playerRigidbody.isKinematic = false;
				anim.SetBool("Hanging", false);
				//anim.SetBool("IsJumping", true);
				anim.SetBool("IsJumping", false);
				anim.SetBool("IsJumpingDown", true);
			}
			else if (v > 0)
			{
				tempVerticalPosition = transform.position;
				//tempVerticalPosition += transform.TransformDirection(new Vector3(0.0f, 0.8f, 0.3f));
				tempVerticalPosition += transform.TransformDirection(new Vector3(0.0f, 0.9f, 0.0f));
				tempHorizontalPosition = tempVerticalPosition;
				tempHorizontalPosition += transform.TransformDirection(new Vector3(0.0f, 0.0f, 0.3f));
				timer = 0;
				timer2 = 0;
				goUp = false;
				anim.SetBool("Hanging", false);
				anim.SetBool("ClimbingUp", true);
				climbingUp = true;
			}
			hanging = false;
		}
		else
		{
			transform.position = hangPosition;
		}
	}

	void ClimbingUp()
	{
		//print("transform: " + transform.position + "; temp: " + tempPosition);
		//If haven't climbed up
		if (!goUp)
		{
			//print("climbing up");
			if (transform.position == tempVerticalPosition && !climbStraightUp)
				climbStraightUp = true;
			//Climbup vertically
			if (!climbStraightUp)
			{
				transform.position = Vector3.Lerp (transform.position, tempVerticalPosition, timer += 0.005f);
			}
			//Shift forward
			else
			{
				transform.position = Vector3.Lerp (transform.position, tempHorizontalPosition, timer2 += 0.03f);
			}
			forceY = 0;
			if(transform.position == tempHorizontalPosition)
				goUp = true;
		}
		//If the girl finished climbing
		else
		{
			//print("finish climbing");
			anim.SetBool("ClimbingUp", false);
			transform.position = tempHorizontalPosition;
			jumping = false;
			onGround = true;
			playerRigidbody.isKinematic = false;
			climbStraightUp = false;
			climbingRightClickCam = false;
			climbingUp = false;
		}
	}
	void Debugging()
	{
		Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.0f, 0.7f)), -transform.up, Color.green);
		Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0.0f, 0.5f, 0.0f)), transform.forward, Color.black);
	}
}


//using UnityEngine;
//using System.Collections;
//
//public class PlayerMovement : MonoBehaviour {
//
//	public float moveSpeed = 3f;
//	public float turnSpeed = 6f;
//	public float firstPersonTurnSpeed = 5f;
//	public float jumpSpeed = 5f;
//	public float gravityForce = 1.2f;
//	public float gravity = 19f;
//
//	Animator anim;
//	private Vector3 movement;
//	private Rigidbody playerRigidbody;
//	private CameraMovement cameraMovement;
//	private float speed;
//	//private bool crawling;
//
//	//for Jumping
//	private bool onGround;
//	private float timer;
//	private float timer2;
//	private bool jumping;
//	private float forceY = 0;
//	private float invertGrav;
//	private float airTime = 1.5f;
//	private Vector3 moveDirection = Vector3.zero;
//	private SceneM sceneM;
//
//	//for Climbing
//	public float vOffset = 0f;
//	public float hOffset = 0f;
//	public bool climbing;
//	public bool climbingRightClickCam;
//	private bool climbingUp;
//	private Vector3 tempVerticalPosition;
//	private Vector3 tempHorizontalPosition;
//	private bool goUp;
//	public bool hanging;
//	private bool climbStraightUp;
//	private Vector3 hangPosition;
//
//	//for Icing
//	public bool isIcing;
//	private float icingSpeed;
//
//
//	void Awake()
//	{
//		GameObject mainCam = GameObject.Find("Camera Container");
//		anim = GetComponent<Animator>();
//		cameraMovement = mainCam.GetComponent<CameraMovement>();
//		playerRigidbody = GetComponent<Rigidbody>();
//		playerRigidbody.freezeRotation = true;
//		//running = false;
//		//crawling = false;
//		climbing = false;
//		invertGrav = gravity * airTime;
//		onGround = true;
//		climbing = false;
//		climbingUp = false;
//		goUp = false;
//		hanging = false;
//		climbStraightUp = false;
//		climbingRightClickCam = false;
//		isIcing = false;
//
//		sceneM = GameObject.Find("Scene Manager").GetComponent<SceneM>();
//	}
//
//	void Update()
//	{
//		if (!sceneM.journalIsOn) {
//			if (isIcing) {
//				IcingMovement ();
//				if (!cameraMovement.rightClickMode)
//					cameraMovement.rightClickMode = true;
//			}
//
//
//			//Jumping Animation
////			if (jumping) {
////				//anim.SetBool ("IsJumping", true);
////			} else {
////				anim.SetBool ("IsJumping", false);
////			}
//
//			if (!jumping) {
//				anim.SetBool ("IsJumping", false);
//				anim.SetBool ("IsJumpingDown", false);
//			}
//
//			if (!climbing && !hanging && !isIcing) {
//				Movement ();
//				//Jumping();
//				Running ();
//				//Crawling();
//				DustTrails (onGround);
//			}
//
//			if (!isIcing) {
//				Jumping ();
//			}
//			Debugging ();
//		}
//	}
//
//	void FixedUpdate()
//	{
//
//		if (!cameraMovement.rightClickMode)
//			CharacterRotation();
//
//
//		if(climbing)
//		{
//			LerpToClimbPos();
//		}
//
//		if(hanging)
//		{
//			Hanging();
//		}
//
//		if(climbingUp)
//		{
//			ClimbingUp();
//		}
//
//	}
//
//	void IcingMovement()
//	{
//		//Get moving direction from input
//		movement = transform.forward;
//		movement.x = 0;
//		movement.y = 0;
//		movement = movement.normalized * icingSpeed * Time.deltaTime;
//
//		//Move the player
//		playerRigidbody.MovePosition(transform.position + movement);
//		//
//		//		float h = Input.GetAxisRaw("Horizontal");
//		//		float v = Input.GetAxisRaw("Vertical");
//
//	}
//
//	//for jumping and icing
//	void OnCollisionEnter(Collision other)
//	{
//		if (other.collider.tag == "ground")
//		{
//			if (isIcing == true && cameraMovement.rightClickMode) {
//				cameraMovement.rightClickMode = false;
//				cameraMovement.lerpToThirdPosition = true;
//			}
//
//			timer = 0;
//			onGround = true;
//			jumping = false;
//			isIcing = false;
//		}
//
//		if (other.collider.tag == "ice" && !isIcing)
//		{
//			icingSpeed = speed + 2;
//			isIcing = true;
//		}
//	}
//
//	void Jumping()
//	{
//		moveDirection = new Vector3(0, 0, 0);
//		if (onGround)
//		{
//			timer = 0;
//			forceY = 0;
//			invertGrav = gravity * airTime;
//			if (Input.GetButton("Jump"))
//			{
//				jumping = true;
//				forceY = jumpSpeed;
//				onGround = false;
//			}
//		}
//
//		if (Input.GetButton("Jump") && forceY != 0 && timer < 0.2 && Time.timeScale != 0)
//		{
//			timer += Time.deltaTime;
//			invertGrav -= Time.deltaTime;
//			forceY += invertGrav * Time.deltaTime;
//		}
//
//		if (jumping && !onGround)
//		{
//			if (forceY >= 0) {
//				anim.SetBool ("IsJumping", true);
//				anim.SetBool ("IsJumpingDown", false);
//			} else {
//				anim.SetBool ("IsJumping", false);
//				anim.SetBool ("IsJumpingDown", true);
//			}
//			// Here we apply the gravity
//			forceY -= gravity * Time.deltaTime * gravityForce;
//			moveDirection.y = forceY;
//			playerRigidbody.velocity = moveDirection;
//		}
//	}
//
//	void Movement()
//	{
//		float h = Input.GetAxisRaw("Horizontal");
//		float v = Input.GetAxisRaw("Vertical");
//
//		anim.SetFloat("moveX", v * speed);
//		anim.SetFloat("moveY", h * speed);
//
//
//		if (h != 0)
//			anim.SetBool ("isMovingHorizontal", true);
//		else
//			anim.SetBool ("isMovingHorizontal", false);
//
//		if (v != 0)
//			anim.SetBool ("isMovingVertical", true);
//		else
//			anim.SetBool("isMovingVertical", false);
//
//		//Get moving direction from input
//		movement = transform.right * h + transform.forward * v;
//		movement.y = 0;
//		movement = movement.normalized * speed * Time.deltaTime;
//
//		//Move the player
//		playerRigidbody.MovePosition(transform.position + movement);
//
//
//		/*Slope detection
//        RaycastHit hit;
//        if (Physics.Raycast(point.position, transform.forward, out hit, 1.0f))
//        {
//            if (Vector3.Dot(Vector3.up, hit.normal) > 0.7)
//            {
//            }
//        }
//        */
//	}
//
//	void CharacterRotation()
//	{
//		float mouseInput = Input.GetAxis("Mouse X");
//		Vector3 lookhere = new Vector3(0, mouseInput * firstPersonTurnSpeed, 0);
//		transform.Rotate(lookhere);
//	}
//
//	void WalkingAnim(float h, float v)
//	{
//		bool forward = h == 0 && v > 0 && !Input.GetButton("Fire2");
//		bool backward = h == 0 && v < 0 && !Input.GetButton("Fire2");
//		bool left = h < 0 && !Input.GetButton("Fire2");
//		bool right = h > 0 && !Input.GetButton("Fire2");
//
//		anim.SetBool("IsWalkingForward", forward);
//		anim.SetBool("IsWalkingBackward", backward);
//		anim.SetBool("IsSlidingLeft", left);
//		anim.SetBool("IsSlidingRight", right);
//	}
//
//	void Running()
//	{
//		float runningSpeed = moveSpeed + 3;
//
//		//If left shift is held, run
//		if (Input.GetButton("Fire2"))
//		{
//			if (speed < runningSpeed) {
//				speed += Time.deltaTime * 2f;
//			} else if (speed > runningSpeed) {
//				speed = runningSpeed;
//			}
//			//speed = moveSpeed + 1;
//		}
//		else
//		{
//			if (speed < moveSpeed) {
//				speed = moveSpeed;
//			} else if (speed > moveSpeed) {
//				speed -= Time.deltaTime * 2f;
//			}
////			speed = moveSpeed;
//		}
//	}
//
//	void Crawling(){
//		//If left ctrl is held, crawl
//		if (Input.GetButton("Fire1"))
//		{
//			//crawling = true;
//			speed = moveSpeed - 2;
//		}
//		else
//		{
//			//crawling = false;
//			speed = moveSpeed;
//		}
//	}
//
//	void DustTrails(bool onGround)
//	{
//		GameObject left = transform.Find("Left DustTrail").gameObject;
//		GameObject right = transform.Find("Right DustTrail").gameObject;
//		//print(onGround);
//		if (!onGround)
//		{
//			left.GetComponent<ParticleSystem>().Play();
//			right.GetComponent<ParticleSystem>().Play();
//		}
//	}
//
//	void LerpToClimbPos()
//	{
//		RaycastHit verticalHit = new RaycastHit();
//		RaycastHit horizontalHit = new RaycastHit();
//
//		if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(0.0f, 0.5f, 0.0f)), transform.forward, out horizontalHit, 1f))
//		{
//			//Debug.DrawLine(horizontalHit.point, horizontalHit.transform.right, Color.red);
//		}
//		if (Physics.Raycast(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.0f, 0.7f)), -transform.up, out verticalHit, 1f) && verticalHit.collider.gameObject.tag != "climb")
//		{
//			//Debug.DrawLine(verticalHit.point, verticalHit.transform.forward, Color.blue);
//			anim.SetBool("IsJumping", false);
//			anim.SetBool("IsJumpingDown", false);
//			playerRigidbody.isKinematic = true;
//			cameraMovement.rightClickMode = true;
//			climbingRightClickCam = true;
//			//onGround = false;
//			Vector3 temp = horizontalHit.point + transform.TransformDirection(new Vector3(0.0f, 0.0f, -0.25f));
//			hangPosition = new Vector3(temp.x, verticalHit.point.y + vOffset, temp.z);
//			transform.position = hangPosition;
//			timer = 0;
//			anim.SetBool("Hanging", true);
//			hanging = true;
//			climbing = false;
//		}
//		else
//		{
//			climbing = false;
//		}
//
//	}
//
//	void Hanging()
//	{
//		float v = Input.GetAxisRaw("Vertical");
//		timer += Time.deltaTime;
//		forceY = 0;
//		onGround = false;
//		//jumping = true;
//		//print("hanging");
//		//print(timer);
//		if (v != 0 && timer > 0.5)
//		{
//			//print("press up or down");
//			if (v < 0)
//			{
//				//cameraMovement.rightClickMode = false;
//				//cameraMovement.lerpToThirdPosition = true;
//				jumping = true;
//				playerRigidbody.isKinematic = false;
//				anim.SetBool("Hanging", false);
//				//anim.SetBool("IsJumping", true);
//				anim.SetBool("IsJumping", false);
//				anim.SetBool("IsJumpingDown", true);
//			}
//			else if (v > 0)
//			{
//				tempVerticalPosition = transform.position;
//				//tempVerticalPosition += transform.TransformDirection(new Vector3(0.0f, 0.8f, 0.3f));
//				tempVerticalPosition += transform.TransformDirection(new Vector3(0.0f, 0.9f, 0.0f));
//				tempHorizontalPosition = tempVerticalPosition;
//				tempHorizontalPosition += transform.TransformDirection(new Vector3(0.0f, 0.0f, 0.3f));
//				timer = 0;
//				timer2 = 0;
//				goUp = false;
//				anim.SetBool("Hanging", false);
//				anim.SetBool("ClimbingUp", true);
//				climbingUp = true;
//			}
//			hanging = false;
//		}
//		else
//		{
//			transform.position = hangPosition;
//		}
//	}
//
//	void ClimbingUp()
//	{
//		//print("transform: " + transform.position + "; temp: " + tempPosition);
//		//If haven't climbed up
//		if (!goUp)
//		{
//			//print("climbing up");
//			if (transform.position == tempVerticalPosition && !climbStraightUp)
//				climbStraightUp = true;
//			//Climbup vertically
//			if (!climbStraightUp)
//			{
//				transform.position = Vector3.Lerp (transform.position, tempVerticalPosition, timer += 0.005f);
//			}
//			//Shift forward
//			else
//			{
//				transform.position = Vector3.Lerp (transform.position, tempHorizontalPosition, timer2 += 0.03f);
//			}
//			forceY = 0;
//			if(transform.position == tempHorizontalPosition)
//				goUp = true;
//		}
//		//If the girl finished climbing
//		else
//		{
//			//print("finish climbing");
//			anim.SetBool("ClimbingUp", false);
//			transform.position = tempHorizontalPosition;
//			jumping = false;
//			onGround = true;
//			playerRigidbody.isKinematic = false;
//			climbStraightUp = false;
//			//cameraMovement.rightClickMode = false;
//			//cameraMovement.lerpToThirdPosition = true;
//			climbingRightClickCam = false;
//			climbingUp = false;
//		}
//	}
//	void Debugging()
//	{
//		Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0.0f, 2.0f, 0.7f)), -transform.up, Color.green);
//		Debug.DrawRay(transform.position + transform.TransformDirection(new Vector3(0.0f, 0.5f, 0.0f)), transform.forward, Color.black);
//	}
//}

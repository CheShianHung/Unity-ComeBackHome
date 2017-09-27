using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

//This script controls fading, scene names, transit positions and the girl's rotation
public class SceneM : MonoBehaviour {


    public Transform[] array = new Transform[10]; //Array of starting position
    public Transform[] debugPosition = new Transform[10];
    public AudioClip[] audioBetweenScene = new AudioClip[6];
    public GameObject pauseMenu;
    public GameObject journal;
    public GameObject mainMenu;
    public GameObject fadingCanvas;
    public Transform savePosition;
    public Image fadeImage; //Canvas Image
    public string[] levelArray = new string[10]; //Array of strings of scenes
	public string[] textArray;
    public float[] rotationArray = new float[10]; //Array of rotation.y
    public int areaID;
    public int saveID;
    public int transitionNumber; //Buffer for areaID
    public float fadeSpeed = 1.0f; //Duration of fading
    public bool loadGirl;
	public bool journalIsOn;
	public bool pauseMenuIsOn;
	public bool unlockCave; //if the player finish the cave scene
	public bool unlockForest; // if the player finish the forest scene

    private GameObject girlObj;
    private Transform girl;
    private AudioSource audioS;
	private AudioSource soundEffectAudio;
    private float transition;
    private float duration;
    private static bool created;
    private bool isInTransition;
    private bool isShowing;
    private bool volumnFadeOut;
    private bool sceneTransition;
    private bool sceneLoading;
    private bool loadSceneOnce;
	private SceneCollider sceneCollider;

    void Awake()
    {
        //Not destroy on load
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }

        audioS = GetComponent<AudioSource>();
		soundEffectAudio = GameObject.Find ("SoundEffects").GetComponent<AudioSource> ();
		if (!IsCutscene()) {
			SetAndPlayAudioClip ();
		}

		if (HasGirlObject()) {
			//We need the girl object to control it's rotation
			girlObj = GameObject.Find ("Girl_OBJ");
			girl = girlObj.GetComponent<Transform> ();
			//Freeze the rotation x, y, z (default)
			girl.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeRotation;
			textArray = GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().idlingText;
			Cursor.visible = false; //Hide the cursor
		}
			
        areaID = 0; //reset area id
        saveID = -1; //reset save id
        sceneTransition = false;
        sceneLoading = false;
        volumnFadeOut = false;
        loadGirl = false;
        loadSceneOnce = false;
		journalIsOn = false;
		pauseMenuIsOn = false;
		unlockCave = false;
		unlockForest = false;

		sceneCollider = gameObject.GetComponent<SceneCollider> ();

        setCanvasOrder();
    }

    void FixedUpdate()
    {
        //print(AudioListener.volume);
        //If area id changes
        if(areaID != 0)
        {
            sceneTransition = true;
            loadSceneOnce = true;
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                fadingCanvas.GetComponent<Canvas>().sortingOrder = mainMenu.GetComponent<Canvas>().sortingOrder + 1;
            }
            else
                fadingCanvas.GetComponent<Canvas>().sortingOrder = pauseMenu.GetComponent<Canvas>().sortingOrder + 1;
            FreezeAndFade(); //freeze the character and fade out
        }

        if(saveID != -1)
        {
            sceneLoading = true;
            loadSceneOnce = true;
            if(saveID != 0)
                loadGirl = true;

			//If loading from main menu
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                fadingCanvas.GetComponent<Canvas>().sortingOrder = mainMenu.GetComponent<Canvas>().sortingOrder + 1;
            }
            else
                fadingCanvas.GetComponent<Canvas>().sortingOrder = pauseMenu.GetComponent<Canvas>().sortingOrder + 1;
            SavingFade();
            
        }

        //volumn fade out
        if(volumnFadeOut)
        {
            VolumnFadeOut();
        }

    }

    private void Update()
    {
        //If not fading, do nothing
        if (!isInTransition)
            return;

        //Increase transition if isShowing (fade in), otherwise decrease transition (fade out)
        transition += (isShowing) ? Time.deltaTime * (1 / duration) : -Time.deltaTime * (1 / duration);
        //Lerp the image from transparent to black
        fadeImage.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);
        
        //Once the imgae is completely dark, load map
        if (fadeImage.color == Color.black && sceneTransition)
        {
            LoadMap(transitionNumber);
        }

        if(fadeImage.color == Color.black && sceneLoading)
        {
            LoadSavingMap(transitionNumber);
        }

        //Find the girl object after load
		if(HasGirlObject())
            FindAndSetGirl();

        //Once transition is beyond 0 to 1, stop transition
        if (transition < 0)
        {
            isInTransition = false;
        }

        //if(transition > 1)
        //{
          //  FindAndSetGirl();
        //}
    }

    public void Fade(bool showing, float duration)
    {
        isShowing = showing; //True when fading in, false when fading out
        isInTransition = true;
        this.duration = duration;
        transition = (isShowing) ? 0 : 1; //If true, starting from clear scene (fade in), if false, starting from black scene (fade out)
    }

    private void FreezeAndFade()
    {
        //Freeze movement and rotation
		if(HasGirlObject())
        {
            girl.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
        volumnFadeOut = true;
        Fade(true, fadeSpeed);//Fade out
        transitionNumber = areaID;
        areaID = 0;
    }

    private void SavingFade()
    {
		if(HasGirlObject())
        {
            girl.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        }
        volumnFadeOut = true;
        Fade(true, fadeSpeed);
        transitionNumber = saveID;
        saveID = -1;
    }

    private void LoadMap(int transNum)
    {
        if (loadSceneOnce)
        {
            SceneManager.LoadScene(levelArray[transNum]);
            loadSceneOnce = false;
        }
        if(SceneManager.GetActiveScene().name == levelArray[transNum])
        {
			print ("transnum: " + transNum);
			sceneCollider.AfterSceneLoad (transNum);
			Cursor.visible = false; // Hide the cursor
            Fade(false, fadeSpeed);

			//If not ending
			if (HasGirlObject()) {
				GameObject.Find ("SoundEffects").GetComponent<SoundEffect> ().resetAudio ();
				SetAndPlayAudioClip();
				FindAndSetGirl();
				textArray = GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().idlingText;
				GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().currentTextAry = textArray;
			} 

            setCanvasOrder();//*
            sceneTransition = false;
        }
    }

    private void LoadSavingMap(int transNum)
    {
		//Load the scene once
        if(loadSceneOnce)
        {
            SceneManager.LoadScene(transNum);
            loadSceneOnce = false;
        }

		//When the scene has finished loading
        if(SceneManager.GetActiveScene().buildIndex == transNum)
        {
			print ("Load transNum:" + transNum);
            Fade(false, fadeSpeed);
            SetAndPlayAudioClip();
			//If the scene is not start menu
			if (transNum != 0) 
			{
				GameObject.Find ("SoundEffects").GetComponent<SoundEffect> ().resetAudio ();
				FindAndSetGirl ();
				SetGirlPosition ();
				Cursor.visible = false; //Hid the cursor
				GameObject.Find ("Scene Manager").GetComponent<SceneCollider> ().InitializeSceneObjects ();//Set scene objects
			} 
			//If the scene is start menu
			else 
			{
				Cursor.visible = true; //Show the cursor
			}
            setCanvasOrder();
            sceneLoading = false;
        }
    }

    private void SetGirlPosition()
    {
        girlObj.transform.position = GameObject.Find("Scene Manager").GetComponent<SaveAndLoad>().spawnPosition.position;
    }

    private void FindAndSetGirl()
    {
        //If cannot find girl object
        if (girlObj == null)
        {
            girlObj = GameObject.Find("Girl_OBJ"); //Find new girl object
            girl = girlObj.GetComponent<Transform>();
            girl.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation; //Freeze its rotation
        }
    }

    private void SetAndPlayAudioClip()
    {
        volumnFadeOut = false;
        audioS.Pause();
		soundEffectAudio.Pause ();
        audioS.enabled = false;
        audioS.clip = audioBetweenScene[SceneManager.GetActiveScene().buildIndex];
        audioS.enabled = true;
        audioS.Play();
        VolumnFadeIn();
    }

    private void VolumnFadeOut()
    {
        audioS.volume = Mathf.Lerp(audioS.volume, 0.01f, Time.deltaTime * 5);
		if (soundEffectAudio.isPlaying)
			soundEffectAudio.volume = Mathf.Lerp (soundEffectAudio.volume, 0.01f, Time.deltaTime * 5);
    }

    private void VolumnFadeIn()
    {
        audioS.volume = Mathf.Lerp(audioS.volume, 1f, Time.deltaTime * 10);
		soundEffectAudio.volume = Mathf.Lerp (soundEffectAudio.volume, 0.5f, Time.deltaTime * 5);

    }

    private void setCanvasOrder()
    {
		//In start menu
		if (SceneManager.GetActiveScene ().buildIndex == 0) {
			mainMenu.SetActive (true);
			journal.SetActive (false);
			pauseMenu.SetActive (false);

			mainMenu.GetComponent<Canvas> ().sortingOrder = 3;
			fadingCanvas.GetComponent<Canvas> ().sortingOrder = 2;
			journal.GetComponent<Canvas> ().sortingOrder = 1;
			pauseMenu.GetComponent<Canvas> ().sortingOrder = 2;

			//When the scene loads back to main menu
			if (sceneLoading) {
				fadingCanvas.GetComponent<Canvas> ().sortingOrder = 4;
				StartCoroutine (BackToMenu ());
			}

			//GameObject.Find("Main Menu").GetComponent<Canvas>().sortingOrder = fadingCanvas.GetComponent<Canvas>().sortingOrder + 1;
			//if(Time.timeScale == 0)
			//	pauseMenu.GetComponent<Canvas>().sortingOrder = 0;
			//journal.GetComponent<Canvas>().sortingOrder = 0;

		} 
        //In other scene
        else
        {
            mainMenu.SetActive(false);
            journal.SetActive(false);
            pauseMenu.SetActive(false);

            mainMenu.GetComponent<Canvas>().sortingOrder = 0;
            fadingCanvas.GetComponent<Canvas>().sortingOrder = 1;
            journal.GetComponent<Canvas>().sortingOrder = 1;
            pauseMenu.GetComponent<Canvas>().sortingOrder = 1;

            //if (Time.timeScale == 0)
            //	pauseMenu.GetComponent<Canvas> ().sortingOrder = fadingCanvas.GetComponent<Canvas> ().sortingOrder + 1;
            //journal.GetComponent<Canvas> ().sortingOrder = fadingCanvas.GetComponent<Canvas>().sortingOrder + 1;

        }
    }

	private IEnumerator BackToMenu() 
	{
		yield return new WaitForSeconds (1.5f);
		fadingCanvas.GetComponent<Canvas>().sortingOrder = 2;
	}

	private bool HasGirlObject(){
		return SceneManager.GetActiveScene ().buildIndex == 1 ||
			   SceneManager.GetActiveScene ().buildIndex == 2 || 
			   SceneManager.GetActiveScene ().buildIndex == 3 ||
			   SceneManager.GetActiveScene ().buildIndex == 4;
	}
	private bool IsCutscene(){
		return !HasGirlObject() && SceneManager.GetActiveScene().buildIndex != 0;
	}
} 

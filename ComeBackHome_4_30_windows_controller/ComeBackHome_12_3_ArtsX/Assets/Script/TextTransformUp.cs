using UnityEngine;
using System.Collections;

public class TextTransformUp : MonoBehaviour {

	public string[] idlingText;
	public string[] beforeTriggerText;
	public string[] afterTriggerText;
	public Vector3 startingPosition;
	public GameObject mainCamera;
	public float waitTime;
	public string[] currentTextAry;
    public float speed = 1.3f;
    public float fadeIn = 5f;
	public bool floatingTextIsOn;

    private float fadeOut;
    private float textTime;
	private float fadeOutTime;
    private Color clone;
	private Color myColor;
	private float counter;
	private bool transition;
	private bool countingTime;
	//    private Vector3 initialPos;
	//    private float hold;

	void Start () {
		clone = GetComponent<TextMesh>().color;
		myColor = GetComponent<TextMesh>().color;
		transform.localPosition = startingPosition;
		mainCamera = GameObject.Find ("Main Camera");
        fadeOut = fadeIn + 2;
        clone.a = 1;
		myColor.a = 0;
		GetComponent<TextMesh> ().color = myColor;
        textTime = 0;
		counter = 0;
		currentTextAry = GameObject.Find("Scene Manager").GetComponent<SceneM>().textArray;
		transition = false;
		countingTime = true;
		floatingTextIsOn = true;

		//        initialPos = transform.position;
		//        hold = fadeIn + 2;
    }

    // Update is called once per frame
    void Update () {
		if (floatingTextIsOn) {
			if (countingTime) {
				Counter ();
			}

			if (transition) {
				TextTransition ();
			}
		} else if (!floatingTextIsOn && GetComponent<TextMesh> ().color.a > 0.15f) {
			TextFadeOut ();
		} else {
			ResetText ();
		}

//        if (time >= fadeIn && Time.time < hold)
//        {
//            myColor.a = 1;
//        }
//        this.transform.LookAt(girl.position);
    }

	void Counter()
	{
		counter += Time.deltaTime;
		if (counter >= waitTime)
		{
			countingTime = false;
			SetRandomText (currentTextAry);
			textTime = 0;
			transition = true;
		}
	}

	void TextFadeOut() {
		myColor = GetComponent<TextMesh>().color;
		fadeOutTime = 0;
		fadeOutTime += Time.deltaTime;
		myColor.a = Mathf.Lerp(myColor.a, 0, fadeOutTime);
		GetComponent<TextMesh>().color = myColor;
	}

	void TextTransition()
	{
		myColor = GetComponent<TextMesh>().color;
		textTime += Time.deltaTime;

		if (textTime < fadeIn)
		{
			myColor.a = 0;
			float ratio = textTime / fadeIn;
			myColor.a = Mathf.Lerp(0,1,ratio);
		}

		if (textTime >= fadeIn)
		{
			float ratiodos = (textTime - fadeIn) / fadeOut;
			myColor.a = Mathf.Lerp(1, 0, ratiodos);
		}

		if (textTime >= fadeIn && myColor.a == 0f) {
			transition = false;
			transform.localPosition = startingPosition;
			counter = 0f;
			countingTime = true;
		}

		if(Time.deltaTime != 0)
			transform.Translate(0.01f * Mathf.Sin (textTime * 3), speed * Time.deltaTime, 0); //Moving horizontally
		else
			transform.Translate(0, 0, 0);

		GetComponent<TextMesh>().color = myColor;
	}

	public void ResetText() {
		transform.localPosition = startingPosition;
		countingTime = true;
		transition = false;
		SetRandomText (currentTextAry);
		counter = waitTime / 2;
		myColor.a = 0;
		GetComponent<TextMesh>().color = myColor;
	}

	public void SetRandomText(string[] textAry)
	{
		if (textAry.Length > 0) {
			int randomNumber = (int)Random.Range (0f, textAry.Length);
			GetComponent<TextMesh> ().text = textAry [randomNumber].Replace ("\\n", "\n");
		}
	}



//	void OnEnable ()
//	{
//		myColor = clone;
//		myColor.a = 1;
//		textTime = 0;
//		initialPos = transform.position;
//	}
//		
//    void OnDisable ()
//    {
//		print ("in disable");
//        myColor = clone;
//        myColor.a = clone.a;
//        GetComponent<TextMesh>().color = myColor;
//        transform.position = initialPos;
//    }
}

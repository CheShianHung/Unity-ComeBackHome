using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueText : MonoBehaviour {

	public string[] texts;
	public Vector3[] startingPosition;
	public float speed = 0.5f;
	public float fadeIn = 0.5f;
	public float textHeight;

	private GameObject mainCamera;
	private float fadeOut;
	private float textTime;
	private float xRange;
	private float zRange;
	private Color myColor;
	//    private Vector3 initialPos;
	//    private float hold;

	void Start () {
		xRange = 1f;
		zRange = 1f;
		transform.localPosition = new Vector3(Random.Range(-xRange, xRange), textHeight, Random.Range(-zRange, zRange));
		myColor = GetComponent<TextMesh>().color;
		mainCamera = GameObject.Find ("Main Camera");
		fadeOut = fadeIn + 1;
		myColor.a = 0;
		GetComponent<TextMesh> ().color = myColor;
		textTime = 0;

		//        initialPos = transform.position;
		//        hold = fadeIn + 2;
	}

	// Update is called once per frame
	void Update () {

		TextTransition ();
		transform.LookAt (2 * transform.position - mainCamera.transform.position);

		//        if (time >= fadeIn && Time.time < hold)
		//        {
		//            myColor.a = 1;
		//        }
		//        this.transform.LookAt(girl.position);
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
			Destroy (this.gameObject);
		}

		if(Time.deltaTime != 0)
			transform.Translate(0.01f * Mathf.Sin (textTime * 3), speed * Time.deltaTime, 0); //Moving horizontally
		else
			transform.Translate(0, 0, 0);

		GetComponent<TextMesh>().color = myColor;
	}

//	private void SetRandomText()
//	{
//		int randomNumber = (int) Random.Range(0f, texts.Length);
//		GetComponent<TextMesh> ().text = texts[randomNumber].Replace("\\n","\n");
//	}

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

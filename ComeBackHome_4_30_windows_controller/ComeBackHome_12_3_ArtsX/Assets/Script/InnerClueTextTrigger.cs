using UnityEngine;
using System.Collections;

public class InnerClueTextTrigger : MonoBehaviour
{

	public GameObject textObject;
	public float clueTextInterval;

	private float timer;
	private bool showText;
	private int textIndexCounter;
	private string[] textAry;

	void Start() {
		textIndexCounter = 0;
		textAry = textObject.GetComponent<ClueText> ().texts;
	}

	void Update() {
		if (showText) {
			if (timer < clueTextInterval) {
				timer += Time.deltaTime;
			} else {
				CreateNextText ();
			}
		}
	}


	private void OnTriggerEnter(Collider coll)
	{
		if(coll.tag == "Player")
		{
			timer = clueTextInterval - 0.5f;
			GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().floatingTextIsOn = false;
			showText = true;
		}
	}

	private void OnTriggerExit(Collider coll)
	{
		if(coll.tag == "Player")
		{
			showText = false;
			GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().ResetText ();
			GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().floatingTextIsOn = true;
		}
	}

	private void CreateNextText()
	{
		var newText = Instantiate (textObject);
		newText.transform.parent = gameObject.transform.parent.transform;
		newText.GetComponent<TextMesh>().text = textAry [textIndexCounter].Replace ("\\n", "\n");
		newText.SetActive (true);
		if (textIndexCounter == textAry.Length - 1) {
			textIndexCounter = 0;
		} else {
			textIndexCounter++;
		}
		timer = 0;
	}
}
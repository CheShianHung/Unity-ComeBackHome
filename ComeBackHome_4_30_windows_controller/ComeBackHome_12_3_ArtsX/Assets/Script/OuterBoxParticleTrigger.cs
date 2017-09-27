using UnityEngine;
using System.Collections;

public class OuterBoxParticleTrigger : MonoBehaviour
{

	public GameObject particle;
	public GameObject textObject;
	public float clueTextInterval;
	public bool useParticle;
//	public bool useText;

	private float timer;
	private bool showText;
//	private bool activateOnce;
	private int textIndexCounter;
	private string[] textAry;

	void Start() {
//		activateOnce = false;
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
			if (useParticle && particle != null) {
				particle.SetActive (true);
			}
			timer = clueTextInterval - 0.5f;
			showText = true;
			GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().floatingTextIsOn = false;
		}
	}

	private void OnTriggerExit(Collider coll)
	{
		if(coll.tag == "Player")
		{
			if (useParticle && particle != null) {
				particle.SetActive (false);
			}
			showText = false;
			GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().floatingTextIsOn = true;
			GameObject.Find ("DialogText").GetComponent<TextTransformUp> ().ResetText ();
		}
//		if (coll.tag == "clueText") 
//		{
//			print ("text destroy");
//			Destroy (coll.gameObject);
//		}
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

//	private void OnTriggerEnter(Collider coll) 
//	{
//		if(coll.tag == "Player" && !activateOnce)
//		{
//			activateOnce = true;
//			Particle.SetActive(true);
//			timer = 2.9f;
//			showText = true;
//			GameObject.Find ("Camera Container").GetComponent<TextTransformUp> ().floatingTextIsOn = false;
//		}
//	}
}
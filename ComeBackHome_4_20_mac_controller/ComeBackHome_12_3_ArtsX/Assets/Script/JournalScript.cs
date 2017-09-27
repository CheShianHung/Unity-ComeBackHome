using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class JournalScript : MonoBehaviour {
	const int PAGE_NUM = 12;
	public Sprite[] clearPages = new Sprite[PAGE_NUM];
	public Sprite[] blurPages = new Sprite[PAGE_NUM];
	public bool[] activePages;
	public GUIText pageText;
	public GameObject Journal;
	public GameObject leftPage;
	public GameObject rightPage;
	public GameObject leftArrow;
	public GameObject rightArrow;
	public GUITexture checkJournalImage;
	public bool forceToClose;

	private SceneM sceneM;
	private Image leftImage;
	private Image rightImage;
	private int currentPage;
	private int pageSize;
	private bool show = false;
//	private bool pageShow = true;
//	private bool active = true;
	private bool arrowActive;
	private bool switchForArrowChange; // So the coroutine will only run once

	private SceneCollider sceneCollider;

	void Awake () {

		//Get page size
		if (clearPages.Length % 2 == 0)
			pageSize = clearPages.Length / 2;
		else
			pageSize = clearPages.Length / 2 + 1;
		currentPage = 1;

		//Set page text and arrow image
		SetPageText();
		SetArrowImage();

		//hide journal in the beginning
		show = false;
		Journal.SetActive(show);
		if(pageText != null)
			pageText.GetComponent<GUIText> ().enabled = false;

		//Set left and right pages
		leftImage = leftPage.GetComponent<Image>();
		rightImage = rightPage.GetComponent<Image>();
		leftImage.sprite = blurPages[0];
		rightImage.sprite = blurPages[1];

		//Set the arrow keys bool
		forceToClose = false;
		arrowActive = false;
		switchForArrowChange = false;

		sceneM = GameObject.Find("Scene Manager").GetComponent<SceneM>();
		sceneCollider = GameObject.Find ("Scene Manager").GetComponent<SceneCollider> ();

		//Set all page inactivated
		activePages = new bool[PAGE_NUM]{false, false, false, false, false, false, false, false, false, false, false, false};

		checkJournalImage.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		JournalFunction();

		if (show) {
			float h = Input.GetAxisRaw ("Journal Control");
			if (h == 1 && !arrowActive) {
				rightClick ();
				arrowActive = true;
			}
			if (h == -1 && !arrowActive) {
				leftClick ();
				arrowActive = true;
			}

			if (arrowActive && !switchForArrowChange) {
				StartCoroutine ("ArrowReactive");
				switchForArrowChange = true;
			}
		} 
//		else {
//			pageText.GetComponent<GUIText> ().enabled = false;
//		}

		if(checkJournalImage.enabled == true)
		{
			if (Input.GetButtonDown("Journal"))
				checkJournalImage.enabled = false;
		}
	}

	private IEnumerator ArrowReactive()
	{
		yield return new WaitForSecondsRealtime (0.2f);
		arrowActive = false;
		switchForArrowChange = false;

	}

	public void rightClick()
	{
		if(currentPage < pageSize)
		{
			currentPage++;		//Increase the page number
			SetPageText();		//Reset the page number text
			SetArrowImage();	//Reset the arrow images
			SetPageImage();		//Reset the page images
		}
	}

	public void leftClick()
	{
		if(currentPage > 1)
		{
			currentPage--;
			SetPageText();
			SetArrowImage();
			SetPageImage();
		}
	}

	public void setPage(int pageNum)
	{
		currentPage = pageNum;
		SetPageText ();
		SetArrowImage ();
		SetPageImage ();
	}

	//Manipulate hiding and showing the journal
	public void JournalFunction(){
		if (Input.GetButtonDown("Journal") && SceneManager.GetActiveScene().buildIndex != 0  && SceneManager.GetActiveScene().buildIndex != 5 && SceneManager.GetActiveScene().buildIndex != 6 && !sceneM.pauseMenuIsOn || forceToClose)
		{
			if (show == false)
			{
				CheckSceneUnlock ();
				show = true;
				sceneM.journalIsOn = true;
				Cursor.visible = true; //Show the cursor
				Journal.SetActive(show);
				pageText.GetComponent<GUIText> ().enabled = true;
				Time.timeScale = 0;
			}
			else
			{
				show = false;
				sceneM.journalIsOn = false;
				Journal.SetActive(show);
				pageText.GetComponent<GUIText> ().enabled = false;

				if (!forceToClose) 
				{
					Time.timeScale = 1;
					Cursor.visible = false; //Hide the cursor
				}

				forceToClose = false;
			}
		}
	}

	//Set the page text
	private void SetPageText()
	{
		if(pageText != null)
			pageText.text = currentPage + " / " + pageSize;
	}

	//Set the page images
	public void SetPageImage()
	{
		if (activePages [(currentPage - 1) * 2])
			leftImage.sprite = clearPages[(currentPage - 1) * 2];
		else
			leftImage.sprite = blurPages[(currentPage - 1) * 2];

		//If there is only one image in the last page
		if ((currentPage - 1) * 2 + 1 == clearPages.Length)
			rightImage.sprite = null;
		//If not
		else 
		{
			if(activePages[(currentPage - 1) * 2 + 1])
				rightImage.sprite = clearPages[(currentPage - 1) * 2 + 1];
			else
				rightImage.sprite = blurPages[(currentPage - 1) * 2 + 1];
		}
	}

	private void SetArrowImage()
	{
		Image leftArrowImage = leftArrow.GetComponent<Image>(); 
		Image rightArrowImage = rightArrow.GetComponent<Image>(); 

		Color leftColor = leftArrowImage.color;
		Color rightColor = rightArrowImage.color;

		//If on the first page
		if (currentPage == 1) 
		{
			leftColor.a = 0;
			rightColor.a = 1;
		} 
		//If on the last page
		else if (currentPage == pageSize) 
		{
			leftColor.a = 1;
			rightColor.a = 0;
		} 
		//If in between
		else 
		{
			rightColor.a = 1;
			leftColor.a = 1;
		}

		leftArrowImage.color = leftColor;
		rightArrowImage.color = rightColor;
	}

	private void CheckSceneUnlock()
	{
		bool unlock = false;
		//In village scene
		if (SceneManager.GetActiveScene ().buildIndex == 1) {
			if (activePages [0] && activePages [1] && activePages [2]  && !sceneM.unlockCave) {
				sceneCollider.AfterJournalCollect (1);
				unlock = true;
			}
		}

		//In forest scene
		if (SceneManager.GetActiveScene ().buildIndex == 2) {
			if (activePages [5] && activePages [6] && activePages [7] && activePages [8]) {
				sceneCollider.AfterJournalCollect (2);
				unlock = true;
			}
		}

		//In cave scene
		if (SceneManager.GetActiveScene ().buildIndex == 3) {
			if (activePages [3] && activePages [4]) {
				sceneCollider.AfterJournalCollect (3);
				unlock = true;
			}
		}

		if (SceneManager.GetActiveScene ().buildIndex == 4) {
			if (activePages [9] && activePages [10] && activePages [11]) {
				sceneCollider.AfterJournalCollect (4);
				unlock = true;
			}
		}

		if (!unlock) {
			TextTransformUp textTransformUp = GameObject.Find ("DialogText").GetComponent<TextTransformUp>();
			textTransformUp.currentTextAry = textTransformUp.beforeTriggerText;
		}
	}

	public void resetJournal(){
		checkJournalImage.enabled = false;
		currentPage = 1;

		for (int i = 0; i < activePages.Length; i++) {
			activePages [i] = false;
		}
		setPage (1);
	}
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonScr : MonoBehaviour 
{
	private StartManagerScr startManager;
	private SpriteRenderer sprite;
	private Color buttonColor1;
	private Color buttonColor2;
	[SerializeField] private AudioSource click;
	[SerializeField] private SpriteRenderer icon;
	[SerializeField] private Sprite mute1;
	[SerializeField] private Sprite mute2;

	void Start()
	{
		buttonColor1 = DB.ButtonColor1;
		buttonColor2 = DB.ButtonColor2;
		startManager = StartManagerScr.StartManager;
		if (name != "Record")
		{
			sprite = GetComponent<SpriteRenderer>();
			sprite.color = buttonColor1;
		}
		if (name == "Mute")
			icon.sprite = ProgressScr.IsMute ? mute2 : mute1;
	}

	void Update()
	{
		if (name == "Mute")
			icon.sprite = ProgressScr.IsMute ? mute2 : mute1;
	}

	void OnMouseEnter()
	{
		sprite.color = buttonColor2;
	}

	void OnMouseExit()
	{
		sprite.color = buttonColor1;
	}

	void OnMouseUpAsButton()
	{
		/*if (!ProgressScr.IsMute)*/
			click.Play();
		switch (name)
		{
			case "Play":
				ProgressScr.GameMode = 0;
				SceneManager.LoadScene("MainGame");
				break;
			case "Mute":
				SwapMuteSprite();
				break;
			case "Star":

				break;
			case "Question":
				SceneManager.LoadScene("Question");
				break;
			case "Record":

				break;
			case "Swarms":
				ProgressScr.GameMode = 2;
				SceneManager.LoadScene("MainGame");
				break;
			case "Divers":
				ProgressScr.GameMode = 1;
				SceneManager.LoadScene("MainGame");
				break;
			case "Reset":
				ProgressScr.ResetGameData();
				break;
			case "Exit":
				Debug.Log("exit");
				Application.Quit();
				break;
		}
	}

	void SwapMuteSprite ()
	{
		if (icon.sprite == mute1)
		{
			ProgressScr.IsMute = true;
			icon.sprite = mute2;
		}
		else 
		{
			ProgressScr.IsMute = false;
			icon.sprite = mute1;
		}
	}
}

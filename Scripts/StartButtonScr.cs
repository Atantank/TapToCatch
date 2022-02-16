using UnityEngine;

public class StartButton : MonoBehaviour 
{
	StartManager startManager;
	SpriteRenderer sprite;
	SpriteRenderer icon;
	public AudioSource click;

	void Start()
	{
		startManager = StartManager.SManager;
		if (name != "Record")
		{
			sprite = GetComponent<SpriteRenderer>();
			sprite.color = startManager.buttonColor1;
		}
		icon = startManager.icon;
		if (name == "Mute")
			icon.sprite = ProgressScr.IsMute ? startManager.mute2 : startManager.mute1;
	}

	void Update()
	{
		if (name == "Mute")
			icon.sprite = ProgressScr.IsMute ? startManager.mute2 : startManager.mute1;
	}

	void OnMouseEnter()
	{
		sprite.color = startManager.buttonColor2;
	}

	void OnMouseExit()
	{
		sprite.color = startManager.buttonColor1;
	}
	void OnMouseUpAsButton()
	{
		if (!ProgressScr.IsMute)
			click.Play();
		switch (name)
		{
			case "Play":
				startManager.Play();
				break;
			case "Mute":
				SwapSprite();
				break;
			case "Star":

				break;
			case "Question":
				startManager.Question();
				break;
			case "Record":

				break;
			case "Swarms":
				ProgressScr.GameMode = 2;
				startManager.Play();
				break;
			case "Divers":
				ProgressScr.GameMode = 1;
				startManager.Play();
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

	void SwapSprite ()
	{
		if (icon.sprite == startManager.mute1)
		{
			ProgressScr.IsMute = true;
			icon.sprite = startManager.mute2;
		}
		else 
		{
			ProgressScr.IsMute = false;
			icon.sprite = startManager.mute1;
		}
	}
}

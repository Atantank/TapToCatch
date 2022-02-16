using UnityEngine;

public class Button : MonoBehaviour 
{
	GameManager gameManager;
	SpriteRenderer sprite;
	public AudioSource click;

	void Start()
	{
		gameManager = GameManager.GM;
		sprite = GetComponent<SpriteRenderer>();
		sprite.color = gameManager.buttonColor1;
	}

	void OnMouseEnter()
	{
		sprite.color = gameManager.buttonColor2;
	}

	void OnMouseExit()
	{
		sprite.color = gameManager.buttonColor1;
	}
	void OnMouseUpAsButton()
	{
		if (!ProgressScr.IsMute)
			click.Play();
		switch (name)
		{
			case "Resume":
				gameManager.GamePause();
				break;
			case "NextLevel":
				gameManager.NextLevel();
				break;
			case "Restart":
				gameManager.Restart();
				break;
			case "Home":
				gameManager.GoHome();
				break;
			case "Menu":
				gameManager.GamePause();
				break;
		}
	}
}

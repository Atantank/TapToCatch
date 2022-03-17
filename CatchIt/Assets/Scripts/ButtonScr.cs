using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScr : MonoBehaviour 
{
	private GameManager gameManager;
	private SpriteRenderer sprite;
	[SerializeField] private AudioSource click;
	private Color buttonColor1;
	private Color buttonColor2;

	void Start()
	{
		buttonColor1 = DB.ButtonColor1;
		buttonColor2 = DB.ButtonColor2;
		gameManager = GameManager.GM;
		sprite = GetComponent<SpriteRenderer>();
		sprite.color = buttonColor1;
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
		click.Play();
		switch (name)
		{
			case "Resume":
				gameManager.GamePause();
				break;
			case "NextLevel":
				gameManager.ExitLevel();
				SceneManager.LoadScene("MainGame");
				break;
			case "Restart":
				gameManager.ExitLevel();
				SceneManager.LoadScene("MainGame");
				break;
			case "Home":
				gameManager.ExitLevel();
				SceneManager.LoadScene("Start");
				break;
			case "Menu":
				gameManager.GamePause();
				break;
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionHelperScr : MonoBehaviour 
{
	[SerializeField] private Text text;
	[SerializeField] private AudioSource click;
	private SpriteRenderer sprite;
	private Color color1;
	private Color color2;

	void Start () 
	{
		color1 = DB.ButtonColor1;
		color2 = DB.ButtonColor2;

		sprite = GetComponent<SpriteRenderer>();
		sprite.color = color1;
		text.text = "Blah....";
	}

	void OnMouseEnter()
	{
		sprite.color = color2;
	}

	void OnMouseExit()
	{
		sprite.color = color1;
	}

	void OnMouseUpAsButton ()
	{
		/*if (!ProgressScr.IsMute)*/
			click.Play();
		SceneManager.LoadScene("Start");
	}
}

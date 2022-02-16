using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuestionHelper : MonoBehaviour 
{
	public Text text;
	public Color color1;
	public Color color2;
	SpriteRenderer sprite;
	public AudioSource click;
	void Start () 
	{
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
		if (!ProgressScr.IsMute)
			click.Play();
		SceneManager.LoadScene("Start");
	}
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour 
{
	public Color buttonColor1;
	public Color buttonColor2;
	public SpriteRenderer icon;
	public Sprite mute1;
	public Sprite mute2;
	public Text score;
	public static StartManager SManager;

	void Awake()
	{
		SManager = this;
	}

	void Start()
	{
		score.text = ProgressScr.GameScore.ToString();
	}

	public void Play ()
	{
		SceneManager.LoadScene("MainGame");
	}

	public void Question ()
	{
		SceneManager.LoadScene("Question");
	}
}

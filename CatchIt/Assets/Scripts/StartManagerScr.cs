using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManagerScr : MonoBehaviour 
{
	[SerializeField] private Text Score; // !
	public static StartManagerScr StartManager;

	void Awake()
	{
		StartManager = this;
	}

	void Start()
	{
		Score.text = ProgressScr.GameScore.ToString();
	}
}

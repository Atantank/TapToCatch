using UnityEngine;

public class PlayerBehaviour : MonoBehaviour 
{
	GameManager gameManager;
	int multyCast;

	void Start () 
	{
		gameManager = GameManager.GM;
		name = "Step";
		multyCast = 0;
	}

	public void addCast()
	{
		multyCast++;
		if (multyCast > 1)
			gameManager.Multycast(multyCast);
	}

	void OnDestroy ()
	{
		gameManager.stepCount--;
	}
}
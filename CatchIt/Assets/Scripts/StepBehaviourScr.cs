using UnityEngine;

public class StepBehaviourScr : MonoBehaviour 
{
	public int number;
	private GameManager gameManager;
	private int catchInRow;
	private float oldTime;
	private float newTime;

	void Start ()
	{
		gameManager = GameManager.GM;
		name = "Step";
		catchInRow = 1;
		oldTime = 0;
	}

	public void Catch()
	{
		newTime = Time.timeSinceLevelLoad;
		if (oldTime != 0 && newTime - oldTime < DB.MultycatchTime)
		{
			catchInRow++;
			gameManager.Multycatch(catchInRow);
		}
		else
		{
			catchInRow = 1;
		}
		oldTime = newTime;
	}

	void OnDestroy ()
	{
		gameManager.StepCount--;
	}
}
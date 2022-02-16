using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB : MonoBehaviour
{
	public static int MaxCountDivers { get; private set; }
	public static float TimeVisible { get; private set; }
	public static float TimeRepeat { get; private set; }
	public static float DiverSpeed { get; private set; }
	public static int MaxCountMines { get; private set; }
	public static int CountRespawn { get; private set; }
	public static int MaxCountSwarms { get; private set; }
	public static float FlyTimeMin { get; private set; }
	public static float FlyTimeMax { get; private set; }
	public static float CatchTime { get; private set; }
	public static int CountToCatch { get; private set; }

    public static void SetDiversSettings()
    {
		CountRespawn = 0;
		switch (ProgressScr.LevelDiver)
		{
			case 0:
				Debug.Log("Wrong levelDiver");
				break;

			case 1:
				MaxCountDivers = 1;
				TimeVisible = 1f;
				TimeRepeat = 4f;
				DiverSpeed = 5;

				MaxCountMines = 0;
				break;
			case 2:
				MaxCountDivers = 2;
				TimeVisible = 1f;
				TimeRepeat = 4f;
				DiverSpeed = 5;

				MaxCountMines = 1;
				break;
			case 3:
				MaxCountDivers = 4;
				TimeVisible = 1f;
				TimeRepeat = 3f;
				DiverSpeed = 5;

				MaxCountMines = 1;
				break;
			case 4:
				MaxCountDivers = 5;
				TimeVisible = 1f;
				TimeRepeat = 3f;
				DiverSpeed = 6;

				MaxCountMines = 2;
				break;
			case 5:
				MaxCountDivers = 7;
				TimeVisible = 1f;
				TimeRepeat = 3f;
				DiverSpeed = 6;

				MaxCountMines = 2;
				break;
			case 6:
				MaxCountDivers = 7;
				TimeVisible = 1f;
				TimeRepeat = 3f;
				DiverSpeed = 7;

				MaxCountMines = 3;
				break;
			case 7:
				MaxCountDivers = 8;
				TimeVisible = 1f;
				TimeRepeat = 3f;
				DiverSpeed = 7;

				MaxCountMines = 3;
				break;
			case 8:
				MaxCountDivers = 9;
				TimeVisible = 1f;
				TimeRepeat = 3f;
				DiverSpeed = 7;

				MaxCountMines = 3;
				break;
			case 9:
				MaxCountDivers = 10;
				TimeVisible = 1f;
				TimeRepeat = 3f;
				DiverSpeed = 7;

				MaxCountMines = 4;
				break;
			case 10:
				MaxCountDivers = 10;
				TimeVisible = 1f;
				TimeRepeat = 3f;
				DiverSpeed = 7;

				MaxCountMines = 5;
				break;

			case 11 - 100:
				MaxCountDivers = 10;
				TimeVisible = 1f;
				TimeRepeat = 3f;
				DiverSpeed = 7;
				CountRespawn = ProgressScr.LevelDiver - 10;

				MaxCountMines = 5;
				break;
		}
    }

	public static void SetSwarmsSettings()
	{
		switch (ProgressScr.LevelSwarm)
		{
			case 0:
				Debug.Log("Wrong levelSwarm");
				break;

			case 1:
				MaxCountSwarms = 1;
				FlyTimeMin = 2f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
			case 2:
				MaxCountSwarms = 2;
				FlyTimeMin = 2f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
			case 3:
				MaxCountSwarms = 2;
				FlyTimeMin = 1.8f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
			case 4:
				MaxCountSwarms = 3;
				FlyTimeMin = 1.6f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
			case 5:
				MaxCountSwarms = 4;
				FlyTimeMin = 1.5f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
			case 6:
				MaxCountSwarms = 6;
				FlyTimeMin = 1.4f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
			case 7:
				MaxCountSwarms = 7;
				FlyTimeMin = 1.2f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
			case 8:
				MaxCountSwarms = 8;
				FlyTimeMin = 1.2f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
			case 9:
				MaxCountSwarms = 10;
				FlyTimeMin = 1f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
			case 10:
				MaxCountSwarms = 10;
				FlyTimeMin = 1f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;

			case 11 - 100:
				MaxCountSwarms = 10;
				FlyTimeMin = 2f;
				FlyTimeMax = 4f;
				CatchTime = 0.2f;
				CountToCatch = MaxCountSwarms;
				break;
		}
	}
}

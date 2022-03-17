using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB : MonoBehaviour
{
	// * Настройки Цвета в игре //
	[SerializeField] private Color buttonColor1;
	public static Color ButtonColor1 { get; private set; }
	[SerializeField] private Color buttonColor2;
	public static Color ButtonColor2 { get; private set; }
	[SerializeField] private Color diverColor;
	public static Color DiverColor { get; private set; }
	[SerializeField] private Color mineColor;
	public static Color MineColor { get; private set; }
	[SerializeField] private Color swarmColor1;
	public static Color SwarmColor1 { get; private set; }
	[SerializeField] private Color swarmColor2;
	public static Color SwarmColor2 { get; private set; }
	// * Настройка размеров //
	public static float SwarmRadius;
	public static float DiverRadius;
	public static float StepRadius; // TODO Внедрить

	// * Параметры уровней //
	public static int MaxCountDivers { get => diversLevelSettings.CountDivers; }
	public static int MaxCountMines { get => diversLevelSettings.CountMines; }
	public static int MaxCountRespawns { get => diversLevelSettings.RespawnCount; }
	public static float DiversSpeed { get => diversLevelSettings.Speed; }
	public static float TimeDirectionChange { get => diversLevelSettings.TimeDirectionChange; }
	public static float TimeVisible { get => diversLevelSettings.TimeVisible; }

	public static int MaxCountSwarms { get => swarmsLevelSettings.CountSwarms; }
	public static int[] GroupSize { get => swarmsLevelSettings.GroupSize; }
	public static float FlyTimeMin { get => swarmsLevelSettings.FlyTimeMin; }
	public static float FlyTimeMax { get => swarmsLevelSettings.FlyTimeMax; }
	public static float CatchTime { get => swarmsLevelSettings.CatchTime; }
	public static int CountToCatch { get => swarmsLevelSettings.CountToCatch; }

	private static DiversLevelSettings diversLevelSettings;
	private static SwarmsLevelSettings swarmsLevelSettings;

	// * Параметры игрока //
	public static float StepLifeTime;
	public static float MultycatchTime;
	public static int MaxStepCount;

	public void Awake()
	{
		ButtonColor1 = buttonColor1;
		ButtonColor2 = buttonColor2;
		DiverColor = diverColor;
		MineColor = mineColor;
		SwarmColor1 = swarmColor1;
		SwarmColor2 = swarmColor2;
		StepLifeTime = 1f;
		MaxStepCount = 1;
		SwarmRadius = 1f;
		DiverRadius = 0.5f;
		StepRadius = 1f;
		MultycatchTime = 1f;
	}

	public static void SetZero()
	{
		diversLevelSettings = new DiversLevelSettings(0, 0, 0, 0f, 0f, 0f);
		swarmsLevelSettings = new SwarmsLevelSettings(0, 0, 0, 0);
	}

	public static void SetLevelSettings()
	{
		diversLevelSettings = new DiversLevelSettings(0, 0, 0, 0f, 0f, 0f);
		swarmsLevelSettings = new SwarmsLevelSettings(0, 0, 0, 0);

		switch (ProgressScr.LevelDiver)
		{
			case 0:
				Debug.Log("Wrong levelDiver");
				break;
			case 1:
				diversLevelSettings = new DiversLevelSettings(1, 0, 0, 5f, 4f, 1f);
				break;
			case 2:
				diversLevelSettings = new DiversLevelSettings(2, 1, 0, 5f, 4f, 1f);
				break;
			case 3:
				diversLevelSettings = new DiversLevelSettings(4, 1, 0, 5f, 3f, 1f);
				break;
			case 4:
				diversLevelSettings = new DiversLevelSettings(5, 2, 0, 6f, 3f, 1f);
				break;
			case 5:
				diversLevelSettings = new DiversLevelSettings(5, 2, 0, 6f, 3f, 1f);
				swarmsLevelSettings = new SwarmsLevelSettings(1, 2f, 4f, 0.2f);
				break;
			case 6:
				diversLevelSettings = new DiversLevelSettings(5, 2, 0, 6f, 3f, 1f);
				swarmsLevelSettings = new SwarmsLevelSettings(1, 2f, 4f, 0.2f);
				break;
			case 7:
				diversLevelSettings = new DiversLevelSettings(5, 2, 0, 6f, 3f, 1f);
				swarmsLevelSettings = new SwarmsLevelSettings(1, 2f, 4f, 0.2f);
				break;
			case 8:
				diversLevelSettings = new DiversLevelSettings(5, 2, 0, 6f, 3f, 1f);
				swarmsLevelSettings = new SwarmsLevelSettings(1, 2f, 4f, 0.2f);
				break;
			case 9:
				diversLevelSettings = new DiversLevelSettings(5, 2, 0, 6f, 3f, 1f);
				swarmsLevelSettings = new SwarmsLevelSettings(1, 2f, 4f, 0.2f);
				break;
			case 10:
				diversLevelSettings = new DiversLevelSettings(5, 2, 0, 6f, 3f, 1f);
				swarmsLevelSettings = new SwarmsLevelSettings(1, 2f, 4f, 0.2f);
				break;
			case 11 - 100:
				diversLevelSettings = new DiversLevelSettings(10, 5, ProgressScr.LevelDiver - 10, 7f, 3f, 1f);
				swarmsLevelSettings = new SwarmsLevelSettings(10, 1f, 4f, 0.2f);
				break;
		}
	}

    public static void SetDiversSettings()
    {
		switch (ProgressScr.LevelDiver)
		{
			case 0:
				Debug.Log("Wrong levelDiver");
				break;

			case 1:
				diversLevelSettings = new DiversLevelSettings(1, 0, 0, 5f, 4f, 1f);
				break;
			case 2:
				diversLevelSettings = new DiversLevelSettings(2, 1, 0, 5f, 4f, 1f);
				break;
			case 3:
				diversLevelSettings = new DiversLevelSettings(4, 1, 0, 5f, 3f, 1f);
				break;
			case 4:
				diversLevelSettings = new DiversLevelSettings(5, 2, 0, 6f, 3f, 1f);
				break;
			case 5:
				diversLevelSettings = new DiversLevelSettings(7, 2, 0, 6f, 3f, 1f);
				break;
			case 6:
				diversLevelSettings = new DiversLevelSettings(7, 3, 0, 7f, 3f, 1f);
				break;
			case 7:
				diversLevelSettings = new DiversLevelSettings(8, 3, 0, 7f, 3f, 1f);
				break;
			case 8:
				diversLevelSettings = new DiversLevelSettings(9, 3, 0, 7f, 3f, 1f);
				break;
			case 9:
				diversLevelSettings = new DiversLevelSettings(10, 4, 0, 7f, 3f, 1f);
				break;
			case 10:
				diversLevelSettings = new DiversLevelSettings(10, 5, 0, 7f, 3f, 1f);
				break;

			case 11 - 100:
				diversLevelSettings = new DiversLevelSettings(10, 5, ProgressScr.LevelDiver - 10, 7f, 3f, 1f);
				break;
		}

		StepLifeTime = TimeVisible;
		MaxStepCount = 1;
    }

	public static void SetSwarmsSettings()
	{
		StepLifeTime = 0.8f;
		MaxStepCount = 2;

		switch (ProgressScr.LevelSwarm)
		{
			case 0:
				Debug.Log("Wrong levelSwarm");
				break;

			case 1:
				swarmsLevelSettings = new SwarmsLevelSettings(1, 2f, 4f, 0.2f);
				break;
			case 2:
				swarmsLevelSettings = new SwarmsLevelSettings(2, 2f, 4f, 0.2f);
				break;
			case 3:
				swarmsLevelSettings = new SwarmsLevelSettings(2, 1.8f, 4f, 0.2f);
				break;
			case 4:
				swarmsLevelSettings = new SwarmsLevelSettings(3, 1.6f, 4f, 0.2f);
				break;
			case 5:
				swarmsLevelSettings = new SwarmsLevelSettings(4, 0.5f, 4f, 0.2f);
				break;
			case 6:
				swarmsLevelSettings = new SwarmsLevelSettings(6, 1.4f, 4f, 0.2f);
				break;
			case 7:
				swarmsLevelSettings = new SwarmsLevelSettings(7, 1.2f, 4f, 0.2f);
				break;
			case 8:
				swarmsLevelSettings = new SwarmsLevelSettings(8, 1.2f, 4f, 0.2f);
				break;
			case 9:
				swarmsLevelSettings = new SwarmsLevelSettings(10, 1f, 4f, 0.2f);
				break;
			case 10:
				swarmsLevelSettings = new SwarmsLevelSettings(10, 1f, 4f, 0.2f);
				break;

			case 11 - 100:
				swarmsLevelSettings = new SwarmsLevelSettings(10, 1f, 4f, 0.2f);
				break;
		}
	}

	[Serializable]
	class DiversLevelSettings
	{
		//public int Level;
		public int CountDivers;
		public int CountMines;
		public int RespawnCount;
		public float Speed;
		public float TimeDirectionChange;
		public float TimeVisible;
		public DiversLevelSettings(int _countDivers, int _countMines, int _respawnCount, float _speed, float _timeDirectionChange, float _timeVisible)
		{
			CountDivers = _countDivers;
			CountMines = _countMines;
			RespawnCount = _respawnCount;
			Speed = _speed;
			TimeDirectionChange = _timeDirectionChange;
			TimeVisible = _timeVisible;
		}
	}

	[Serializable]
	class SwarmsLevelSettings
	{
		//public int Level;
		public int CountSwarms;
		public int[] GroupSize;
		public float FlyTimeMin;
		public float FlyTimeMax;
		public float CatchTime;
		public int CountToCatch;

		public SwarmsLevelSettings(int _countSwarms, float _flyTimeMin, float _flyTimeMax, float _catchTime)
		{
			CountSwarms = _countSwarms;
			GroupSize = new int[] { 2, 2, 4, 3, 2, 3, 2, 4, 3, 2, 3 };
			FlyTimeMin = _flyTimeMin;
			FlyTimeMax = _flyTimeMax;
			CatchTime = _catchTime;
			CountToCatch = _countSwarms;
		}
	}
}

using System;
using UnityEngine;

public class ProgressScr : MonoBehaviour 
{
	[SerializeField] private static ProgressData progress;
	public static int LevelDiver
	{
		get => progress.LevelDiver;
		set
		{
			progress.LevelDiver = value;
			SaveData();
		}
	}
	public static int LevelSwarm
	{
		get => progress.LevelSwarm;
		set
		{
			progress.LevelSwarm = value;
			SaveData();
		}
	}
	public static int GameScore
	{
		get => progress.GameScore;
		set
		{
			progress.GameScore = value;
			SaveData();
		}
	}
	public static bool IsMute // TODO Переделать на выключение компонента UNITY, чтобы не надо было везде проверять, просто не должно работать
	{
		get => progress.IsMute;
		set
		{
			progress.IsMute = value;
			SaveData();
		}
	}

	public static int GameMode;//0 - null; 1 - diver; 2 - swarm; // TODO Необходимо избавиться от этого
	
	private static bool created = false;

	void Awake() 
	{
		progress = new ProgressData(1, 1, 0, false);
		if (!created)
		{
			DontDestroyOnLoad(this.gameObject);
			created = true;
			CheckFirstLoad();
			LoadData();
			GameMode = 0;
		}
    	else
		{
			Destroy(this.gameObject);
		}
	}

	public void CheckFirstLoad()
	{
		if (!PlayerPrefs.HasKey("Progress"))
		{
			ResetGameData();
		}
	}

	public static void ResetGameData()
	{
		SaveData();
	}

	public static void LoadData()
	{
		progress = JsonUtility.FromJson<ProgressData>(PlayerPrefs.GetString("Progress"));
	}

	public static void SaveData()
	{
		PlayerPrefs.SetString("Progress", JsonUtility.ToJson(progress));
		PlayerPrefs.Save();
	}

	[Serializable]
	class ProgressData
	{
		public int LevelDiver;
		public int LevelSwarm;
		public int GameScore;
		public bool IsMute;

		public ProgressData(int _levelDiver, int _levelSwarm, int _gameScore, bool _isMute)
		{
			LevelDiver = _levelDiver;
			LevelSwarm = _levelSwarm;
			GameScore = _gameScore;
			IsMute = _isMute;
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.Audio;

public class ProgressScr : MonoBehaviour 
{
	private static ProgressData progress;
	private static AudioMixerGroup mixer;
	[SerializeField] private AudioMixerGroup masterVolume;

	public static int GameScore
	{
		get => progress.GameScore;
		set
		{
			progress.GameScore = value;
			SaveData();
		}
	}

	public static int Level
	{
		get => progress.Level;
		set
		{
			progress.Level = value;
			SaveData();
		}
	}

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
	
	public static bool IsMute
	{
		get => progress.IsMute;
		set
		{
			progress.IsMute = value;
			mixer.audioMixer.SetFloat("MasterVolume", IsMute ? -80 : 0);
			SaveData();
		}
	}

	public static int GameMode;//0 - null; 1 - diver; 2 - swarm;
	
	private static bool created = false;

	void Awake() 
	{
		if (!created)
		{
			created = true;
			progress = new ProgressData(0, 1, 1, 1, false);
			mixer = masterVolume;
			mixer.audioMixer.SetFloat("MasterVolume", IsMute ? -80 : 0);
			GameMode = 0;
			
			DontDestroyOnLoad(this.gameObject);
			CheckFirstLoad();
			LoadData();
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
		progress = new ProgressData(0, 1, 1, 1, false);
		SaveData();
		LoadData();
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
		public int GameScore;
		public int Level;
		public int LevelDiver;
		public int LevelSwarm;
		public bool IsMute;

		public ProgressData(int _gameScore, int _level, int _levelDiver, int _levelSwarm, bool _isMute)
		{
			GameScore = _gameScore;
			Level = _level;
			LevelDiver = _levelDiver;
			LevelSwarm = _levelSwarm;
			IsMute = _isMute;
		}
	}
}

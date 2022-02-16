using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
	[Header("Уровень")]
	public int gameMode;
	bool alredyWon;
	[HideInInspector] public bool isFail;

	[Header("Поле")]
	public GameObject gameMenu;
	public GameObject nextButton;
	public GameObject resumeButton;
	public GameObject restartButton;
	public Text scoreText;
	public Text XText;
	int timeX;
	public Text endText;
	public Text endScore;
	public Color buttonColor1;
	public Color buttonColor2;
	public BoxCollider2D top;
	public BoxCollider2D bottom;
	public BoxCollider2D right;
	public BoxCollider2D left;
	[HideInInspector] public Vector2 size;
	[HideInInspector] public int score;
	Vector2 stepPosition;
	Vector3 mousePosition;

	[Header("Игрок")]
	public GameObject step;
	[HideInInspector] public int stepCount;
	public int maxStepCount;
	public float playerTime;

	[Header("Рой")]
	public GameObject swarm;
	public GameObject hierarchySwarm;
	[HideInInspector] public SwarmBehaviour[,] swarms;
	public int[] friends;
	public int maxCountSwarms { get => DB.MaxCountSwarms; }
	public float flyTimeMin { get => DB.FlyTimeMin; }
	public float flyTimeMax { get => DB.FlyTimeMax; }
	float flyTime;
	public float catchTime { get => DB.CatchTime; }
	public int countToCatch { get => DB.CountToCatch; }
	[HideInInspector] public int countSwarms;
	public Color swarmColor1;
	public Color swarmColor2;
	public float swarmRadius;

	[Header("Дайвер")]
	public GameObject diver;
	public GameObject hierarchy;
	[HideInInspector] public DiverBehaviour[] divers;
	public int maxCountDivers { get => DB.MaxCountDivers; }
	public int countRespawn;
	[HideInInspector] public int countDivers;
	public float timeVisible { get => DB.TimeVisible; }
	public float timeRepeat { get => DB.TimeRepeat; }
	public float diverSpeed { get => DB.DiverSpeed; } //сделать зависимой от размера поля
	public Color diverColor;
	public float diverRadius;

	//[Header("Мина")]
	public int maxCountMines { get => DB.MaxCountMines; }
	public Color mineColor;

	public static GameManager GM { get; private set; }

	void Awake()
	{
		GM = this;
	}

	void Start ()
	{
		alredyWon = false;
		isFail = false;
		playerTime = 1f;
		LoadLevel();
		countRespawn = DB.CountRespawn;
		gameMenu.SetActive(false);
		XText.enabled = false;
		timeX = 0;
		score = 0;
		SetScore(0);
		stepCount = 0;
		//общие переменные
		size = 10 * (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1f));//пеесчитывать при изм экрана?
		divers = new DiverBehaviour[50];
		swarms = new SwarmBehaviour[20, 5];
		countDivers = 0;
		countSwarms = 0;

		//бордюры
		top.size = new Vector2(2 * size.x, 1);
		top.offset = new Vector2(0, size.y + 0.5f);
		bottom.size = new Vector2(2 * size.x, 1);
		bottom.offset = new Vector2(0, -size.y - 0.5f);
		right.size = new Vector2(1, 2 * size.y);
		right.offset = new Vector2(size.x + 0.5f, 0);
		left.size = new Vector2(1, 2 * size.y);
		left.offset = new Vector2(-size.x - 0.5f, 0);

		//спавн дайверов
		float x = 1;
		float y = 1;
		for (int i = 0; i < maxCountDivers + maxCountMines; i++)
		{
			Vector3 spawn = new Vector3(Random.Range(-x, x), Random.Range(-y, y), 0);
			SpawnDivers(i < maxCountDivers ? false : true, spawn);
		}

		//спавн роя
		x = size.x - swarmRadius * 1.5f;
		y = size.y - swarmRadius * 1.5f;
		for (; countSwarms < maxCountSwarms; countSwarms++)
		{
			flyTime = Random.Range(flyTimeMin, flyTimeMax);
			for (int j = 0; j < friends[countSwarms]; j++)
			{
				Vector3 spawn = new Vector3(Random.Range(-x, x), Random.Range(-y, y), 0);
				SpawnSwarms(j, spawn);
			}
			swarms[countSwarms, 0].isMain = true;
			swarms[countSwarms, 0].needPoint = true;
		}
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			MouseDown();
		}
		if (Input.GetButtonDown("Cancel"))
		{
			GamePause();//надо для сенсорных тоже
		}
		//надо проверку изменения экрана
	}

	public void MouseDown ()
	{
		//запрещает спамить
		if (stepCount >= maxStepCount)
			return;
		//размещение игрока на поле в точку клика
		mousePosition = Input.mousePosition;
		mousePosition.z = 1.0f;
		stepCount++;
		stepPosition = (Vector2)Camera.main.ScreenToWorldPoint(mousePosition) * 10;
		GameObject tryCatch = Instantiate(step, new Vector3(stepPosition.x, stepPosition.y, 0), new Quaternion(0, 0, 0, 0));
		Destroy(tryCatch, playerTime);

		//показать и скрыть объекты
		if (stepCount < 2)
		{
			for (int i = 0; i < countDivers; i++)
			{
				divers[i].isVisible = true;
			}
			Invoke("GoingDark", timeVisible);
		}
	}
		
	public void DeleteDiver (int who)
	{
		countDivers--;
		//DestroyObject(divers[who].gameObject);
		Object.Destroy(divers[who].gameObject);
		if (countDivers - maxCountMines == 0)
		{
			Win();
			return;
		}
		if (who != countDivers)
		{
			divers[who] = divers[countDivers];
			divers[countDivers].number = who;
		}
		if (countRespawn > 0)
		{
			SpawnDivers(false, new Vector3(0, 0, 0));
			countRespawn--;
		}
	}

	public void ChangeMine()
	{//проверять когда надо менять //увеличить countDivers и проверить изменения в гейм менеджере
		for (int i = 0; i < maxCountDivers + maxCountMines; i++)
		{
			if (divers[i].isMine)
			{
				divers[i].color = diverColor;
				divers[i].ChangeMine();
			}
		}
	}

	public void SwarmCatch (int group, int friend)
	{
		countSwarms--;
		SetScore(1);
		for (int i = 0; i < friend; i++)
		{
			//DestroyObject(swarms[group, i].gameObject);
			Object.Destroy(swarms[group, i].gameObject);
		}
		if (countSwarms == 0)
		{
			Win();
			return;
		}
		if (group != countDivers)
		{
			int temp = swarms[countSwarms, 0].friends;
			for (int i = 0; i < temp; i++)
			{
				swarms[group, i] = swarms[countSwarms, i];
				swarms[group, i].groupNum = group;
			}
		}
	}

	void MineDivide(int mineNumber)
	{ //нужно ли увеличить maxCountMines?? //появление в той же точке не вызовет проблем? может спава на окраине мины?
		Vector3 tempPlace = new Vector3(divers[mineNumber].rb2d.position.x, divers[mineNumber].rb2d.position.y, 0);
		SpawnDivers(true, tempPlace);
	}

	void SpawnDivers (bool isMine, Vector3 spawnPos)
	{
		GameObject dive = Instantiate(diver, spawnPos, new Quaternion(0, 0, 0, 0), hierarchy.transform);
		divers[countDivers] = dive.GetComponent<DiverBehaviour>();
		divers[countDivers].timeRepeat = timeRepeat;
		divers[countDivers].speed = diverSpeed;
		divers[countDivers].color = isMine ? mineColor : diverColor;
		divers[countDivers].radius = diverRadius;
		divers[countDivers].isMine = isMine;
		divers[countDivers].number = countDivers;
		countDivers++;
	}

	void SpawnSwarms (int number, Vector3 spawnPos)
	{
		GameObject swarmTemp = Instantiate(swarm, spawnPos, new Quaternion(0, 0, 0, 0), hierarchySwarm.transform);
		swarms[countSwarms, number] = swarmTemp.GetComponent<SwarmBehaviour>();
		swarms[countSwarms, number].groupNum = countSwarms;
		swarms[countSwarms, number].friends = friends[countSwarms];
		swarms[countSwarms, number].isMain = false;
		swarms[countSwarms, number].color1 = swarmColor1;
		swarms[countSwarms, number].color2 = swarmColor2;
		swarms[countSwarms, number].radius = swarmRadius;
		swarms[countSwarms, number].flyTime = flyTime;
		swarms[countSwarms, number].catchTime = catchTime;
	}

	//сделать невидимым
	void GoingDark ()
	{
		for (int i = 0; i < countDivers; i++)
		{
			divers[i].isVisible = false;
		}
	}

	public void Lose ()
	{
		GamePause();
		isFail = true;
		endText.text = "You lose.";
		resumeButton.SetActive(false);
	}

	public void SetScore (int addScore)
	{
		switch (gameMode)
		{
			case 1:
				score += addScore;
				scoreText.text = string.Concat(score.ToString());
				break;

			case 2:
				score += addScore;
				scoreText.text = string.Concat(score.ToString());
				break;
		}
	}

	public void Multycast(int cast)
	{
		XText.enabled = true;
		XText.text = "X" + cast;
		SetScore(cast);
		timeX++;
		Invoke("offX", 1.5f);
	}

	void offX()
	{ 
		if (timeX == 1)
			XText.enabled = false;
		timeX--;
	}

	void Win ()
	{
		GamePause();
		alredyWon = true;
		switch (gameMode)
		{
			case 1:
				ProgressScr.LevelDiver++;
				break;

			case 2:
				ProgressScr.LevelSwarm++;
				break;
		}
		ProgressScr.GameScore += score;//rework it when finish reload system
		ProgressScr.SaveData();
		endText.text = "You Win!!";
		resumeButton.SetActive(false);
		nextButton.SetActive(true);
		restartButton.SetActive(false);
	}

	public void GamePause ()
	{
		if (alredyWon)
			return;
		if (isFail)
			return;
		if (gameMenu.activeInHierarchy)
		{
			stepCount -= maxStepCount;
			gameMenu.SetActive(false);
			Time.timeScale = 1;
		}
		else
		{
			stepCount += maxStepCount;
			gameMenu.SetActive(true);
			endText.text = "Pause";
			Time.timeScale = 0;
		}
	}

	public void GoHome ()
	{
		StopGame();
		SceneManager.LoadScene("Start");
	}

	public void Restart ()
	{
		StopGame();
		SceneManager.LoadScene("MainGame");
	}

	public void NextLevel ()
	{
		StopGame();
		SceneManager.LoadScene("MainGame");
	}

	public void StopGame ()
	{
		Time.timeScale = 1;
	}

	void LoadLevel()
	{
		gameMode = ProgressScr.GameMode;
		switch (gameMode)
		{
			case 0:
				Debug.Log("Wrong gameMode");
				break;

			case 1:
				SetDivers();
				break;

			case 2:
				SetSwarms();
				break;
		}
	}

	void SetDivers ()
	{
		DB.SetDiversSettings();

		playerTime = timeVisible;
		maxStepCount = 1;
	}

	void SetSwarms ()
	{
		friends = new int[] {2, 2, 4, 3, 2, 3, 2, 4, 3, 2, 3};

		DB.SetSwarmsSettings();

		playerTime = 0.8f;
		maxStepCount = 2;
	}
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour 
{
	[SerializeField] private GameObject PauseMenu;
	[SerializeField] private GameObject NextButton;
	[SerializeField] private GameObject ResumeButton;
	[SerializeField] private GameObject RestartButton;
	[SerializeField] private Text ScoreText;
	[SerializeField] private Text MultycatchText;
	[SerializeField] private Text EndText;
	[SerializeField] private Text EndScore;
	[SerializeField] private BoxCollider2D TopBorder;
	[SerializeField] private BoxCollider2D BottomBorder;
	[SerializeField] private BoxCollider2D RightBorder;
	[SerializeField] private BoxCollider2D LeftBorder;

	private bool isEnded;
	private int multycatchInRow;
	private int levelScore;
	private StepController stepController;


	[Header("Игрок")]
	[SerializeField] private GameObject step;
	private List<StepBehaviourScr> steps; // !
	public int StepCount; // !

	[Header("Рой")]
	[SerializeField] private GameObject swarm;
	[SerializeField] private GameObject hierarchySwarms;
	[HideInInspector] public SwarmBehaviourScr[,] Swarms; // ?
	private int countSwarms;

	[Header("Дайвер")]
	[SerializeField] private GameObject diver;
	[SerializeField] private GameObject hierarchyDivers;
	[HideInInspector] public DiverBehaviourScr[] Divers; // ?
	private int countRespawn;
	private int countDivers;

	public static GameManager GM { get; private set; }
	public Vector2 Size { get; private set; }

	void Awake()
	{
		GM = this;
		Divers = new DiverBehaviourScr[50];
		Swarms = new SwarmBehaviourScr[20, 5];
		steps = new List<StepBehaviourScr>();
		countDivers = 0;
		countSwarms = 0;
		multycatchInRow = 0;
		levelScore = 0;
		StepCount = 0;
		isEnded = false;

		stepController = new StepController();
		stepController.Android.Tap.started += context => MakeStep();
	}

	void OnEnable()
	{
		stepController.Enable();
	}

	void OnDisable()
	{
		stepController.Disable();
	}

	void Start ()
	{
		Size = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1f));

		//бордюры
		TopBorder.size = new Vector2(2 * Size.x, 1);
		TopBorder.offset = new Vector2(0, Size.y + 0.5f);
		BottomBorder.size = new Vector2(2 * Size.x, 1);
		BottomBorder.offset = new Vector2(0, -Size.y - 0.5f);
		RightBorder.size = new Vector2(1, 2 * Size.y);
		RightBorder.offset = new Vector2(Size.x + 0.5f, 0);
		LeftBorder.size = new Vector2(1, 2 * Size.y);
		LeftBorder.offset = new Vector2(-Size.x - 0.5f, 0);

		SetScore(0);
		PauseMenu.SetActive(false);
		MultycatchText.enabled = false;

		LoadLevel();
	}

	void LoadLevel()
	{
		DB.SetZero();
		switch (ProgressScr.GameMode)
		{
			case 0:
				DB.SetLevelSettings();
				break;

			case 1:
				DB.SetDiversSettings();
				break;

			case 2:
				DB.SetSwarmsSettings();
				break;
		}
		countRespawn = DB.MaxCountRespawns;

		float x, y;

		//спавн дайверов
		if (DB.MaxCountDivers + DB.MaxCountMines > 0)
		{
			x = 1;
			y = 1;
			for (int i = 0; i < DB.MaxCountDivers + DB.MaxCountMines; i++)
			{
				Vector3 spawnPlace = new Vector3(Random.Range(-x, x), Random.Range(-y, y), 0);
				bool isMine = i < DB.MaxCountDivers ? false : true;
				SpawnDivers(isMine, spawnPlace);
			}
		}

		//спавн роя
		if (DB.MaxCountSwarms > 0)
		{
			x = Size.x - DB.SwarmRadius * 1.5f;
			y = Size.y - DB.SwarmRadius * 1.5f;
			for (int i = 0; i < DB.MaxCountSwarms; i++)
			{
				float flyTime = Random.Range(DB.FlyTimeMin, DB.FlyTimeMax);
				for (int j = 0; j < DB.GroupSize[i]; j++)
				{
					Vector3 spawn = new Vector3(Random.Range(-x, x), Random.Range(-y, y), 0);
					SpawnSwarms(j, spawn, flyTime);
				}
				Swarms[i, 0].SetMain();
				countSwarms++;
			}
		}

		ShowDivers();
	}

	void SpawnDivers(bool _isMine, Vector3 _spawnPos)
	{
		GameObject tempDiver = Instantiate(this.diver, _spawnPos, new Quaternion(0, 0, 0, 0), hierarchyDivers.transform);
		Divers[countDivers] = tempDiver.GetComponent<DiverBehaviourScr>();
		Divers[countDivers].Initiate(countDivers, DB.DiversSpeed, DB.DiverRadius, DB.TimeDirectionChange, _isMine);
		countDivers++;
	}

	void SpawnSwarms(int _number, Vector3 _spawnPos, float _flyTime)
	{
		GameObject tempSwarm = Instantiate(swarm, _spawnPos, new Quaternion(0, 0, 0, 0), hierarchySwarms.transform);
		Swarms[countSwarms, _number] = tempSwarm.GetComponent<SwarmBehaviourScr>();
		Swarms[countSwarms, _number].Initiate(countSwarms, DB.GroupSize[countSwarms], DB.SwarmRadius, _flyTime, DB.CatchTime, DB.SwarmColor1, DB.SwarmColor2);
	}

	/*void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			MakeStep();
		}
		if (Input.GetButtonDown("Cancel"))
		{
			GamePause();//надо для сенсорных тоже
		}
		//надо проверку изменения экрана
	}*/

	void FixedUpdate()
	{}

	public void MakeStep ()
	{
		//запрещает спамить
		if (StepCount >= DB.MaxStepCount || PauseMenu.activeInHierarchy)
		{
			return;
		}
		
		//размещение игрока на поле в точку клика
		StepCount++;
		//Vector3 tapPosition = Input.mousePosition; // !
		//Vector3 tapPosition = stepController.Android.TapPosition.ReadValue<Vector3>();
		Vector2 tapPosition = stepController.Android.TapPosition.ReadValue<Vector2>();
		//Vector3 tapPosition = Mouse.current.position;
		//Vector3 tapPosition = new Vector3();
		//tapPosition.z = 1.0f;
		//Vector2 stepPosition = (Vector2)Camera.main.ScreenToWorldPoint(tapPosition)/* * 10*/;
		Vector2 stepPosition = Camera.main.ScreenToWorldPoint(tapPosition)/* * 10*/;
		//Vector2 stepPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position) * 10;
		GameObject newStep = Instantiate(step, new Vector3(stepPosition.x, stepPosition.y, 0), new Quaternion(0, 0, 0, 0));
		steps.Add(newStep.GetComponent<StepBehaviourScr>());
		Destroy(newStep, DB.StepLifeTime);

		ShowDivers();
	}

	public void Multycatch(int _count)
	{
		SetScore(_count);
		MultycatchText.text = "X" + _count;
		MultycatchText.enabled = true;
		multycatchInRow++;
		Invoke("HideMultycatchMessage", DB.MultycatchTime); // !
	}

	void HideMultycatchMessage()
	{
		if (multycatchInRow == 1)
			MultycatchText.enabled = false;
		multycatchInRow--;
	}

	void ShowDivers() // TODO Добавить обработку мультикаста, чтобы не уходило в тень после первого каста и не было конфликта
	{
		for (int i = 0; i < countDivers; i++)
		{
			Divers[i].MakeVisible();
		}
		Invoke("GoingDark", DB.TimeVisible); // !
	}

	void GoingDark()
	{
		for (int i = 0; i < countDivers; i++)
		{
			Divers[i].MakeInvisible();
		}
	}
		
	public void DeleteDiver (int _deadDiver)
	{
		countDivers--;
		Object.Destroy(Divers[_deadDiver].gameObject);
		if (countDivers - DB.MaxCountMines == 0)
		{
			Win(); // !
			return; // !
		}
		if (_deadDiver != countDivers)
		{
			Divers[_deadDiver] = Divers[countDivers];
			Divers[countDivers].Number = _deadDiver;
		}
		if (countRespawn > 0)
		{
			SpawnDivers(false, new Vector3(0, 0, 0));
			countRespawn--;
		}
	}

	public void DeleteSwarm (int _groupNum, int _groupSize)
	{
		countSwarms--;
		SetScore(2);
		for (int i = 0; i < _groupSize; i++)
		{
			Object.Destroy(Swarms[_groupNum, i].gameObject);
		}
		if (countSwarms == 0)
		{
			Win(); // !
			return;
		}
		if (_groupNum != countDivers)
		{
			int temp = Swarms[countSwarms, 0].GroupSize;
			for (int i = 0; i < temp; i++)
			{
				Swarms[_groupNum, i] = Swarms[countSwarms, i];
				Swarms[_groupNum, i].GroupNum = _groupNum;
			}
		}
	}

	/*void MineDivide(int _mineNumber)
	{ //нужно ли увеличить maxCountMines?? //появление в той же точке не вызовет проблем? может спава на окраине мины?
		Vector3 tempPlace = new Vector3(Divers[_mineNumber].RB2D.position.x, Divers[_mineNumber].RB2D.position.y, 0); // While не выполнится условие, что в радиусе нет столкновений - продолжаем выбирать точку для спавна
		SpawnDivers(true, tempPlace);
	}

	public void ChangeMine()
	{//проверять когда надо менять //увеличить countDivers и проверить изменения в гейм менеджере
		for (int i = 0; i < DB.MaxCountDivers + DB.MaxCountMines; i++)
		{
			if (Divers[i].IsMine)
			{
				Divers[i].ChangeMine();
			}
		}
	}*/

	public void SetScore(int _addScore)
	{
		levelScore += _addScore;
		ScoreText.text = levelScore.ToString();
	}

	public void Lose ()
	{
		GamePause();
		isEnded = true;
		EndText.text = "You lose.";
		ResumeButton.SetActive(false);
	}

	void Win ()
	{
		GamePause();
		isEnded = true;
		switch (ProgressScr.GameMode)
		{
			case 0:
				ProgressScr.Level++;
				break;

			case 1:
				ProgressScr.LevelDiver++;
				break;

			case 2:
				ProgressScr.LevelSwarm++;
				break;
		}
		ProgressScr.GameScore += levelScore;

		EndText.text = "You Win!!";
		ResumeButton.SetActive(false);
		NextButton.SetActive(true);
		RestartButton.SetActive(false);
	}

	public void GamePause ()
	{
		if (isEnded)
			return;
		if (PauseMenu.activeInHierarchy)
		{
			PauseMenu.SetActive(false);
			Time.timeScale = 1;
		}
		else
		{
			PauseMenu.SetActive(true);
			EndText.text = "Pause";
			Time.timeScale = 0;
		}
	}

	public void ExitLevel ()
	{
		Time.timeScale = 1;
	}
}

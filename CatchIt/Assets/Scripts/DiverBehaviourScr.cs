using UnityEngine;

public class DiverBehaviourScr : MonoBehaviour 
{
	private GameManager gameManager;
	private Rigidbody2D RB2D;
	private SpriteRenderer sprite;
	private SpriteRenderer lilSprite;
	private Vector2 currentDirection;
	private Vector2 newDirection;

	public int Number;

	public bool isVisible { get; private set; } // !
	public bool IsMine { get; private set; } // TODO Вынести в отдельную логику

	private float speed;
	private float radius;
	private float timeDirectionChange;

	void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		RB2D = GetComponent<Rigidbody2D>();
	}

	void Start () 
	{
		newDirection = Vector2.zero;
		gameManager = GameManager.GM;

		InvokeRepeating("ChangeDirection", timeDirectionChange, 2*timeDirectionChange);
	}

	public void Initiate(int _number, float _speed, float _radius, float _timeRepeat, bool _isMine)
	{
		Number = _number;
		speed = _speed;
		radius = _radius;
		timeDirectionChange = _timeRepeat;
		IsMine = _isMine;
		RB2D.name = IsMine ? "Mine" : "Diver";
		sprite.color = IsMine ? DB.MineColor : DB.DiverColor;
	}

	void FixedUpdate () 
	{
		//выбор направления движения
		if (currentDirection.x == 0 && currentDirection.y == 0)
			currentDirection = (Vector2)Random.onUnitSphere;
		if (newDirection.x != 0 || newDirection.x != 0)
		{
			currentDirection = newDirection;
			newDirection = Vector2.zero;
		}
		RB2D.MovePosition(RB2D.position + currentDirection*Time.deltaTime*speed);
		if (Mathf.Abs(RB2D.position.x) > gameManager.Size.x || Mathf.Abs(RB2D.position.y) > gameManager.Size.y)
			RB2D.position = Vector2.zero;
	}

	void OnCollisionEnter2D (Collision2D _coll)
	{
		if (_coll.gameObject.name == "GameManager")
		{
			if (gameManager.Size.x - radius <= Mathf.Abs(RB2D.position.x))
				currentDirection.x = -currentDirection.x;
			if (gameManager.Size.y - radius <= Mathf.Abs(RB2D.position.y))
				currentDirection.y = -currentDirection.y;
		}
		if (_coll.gameObject.name == "Mine" || _coll.gameObject.name == "Diver")
		{
			newDirection = _coll.gameObject.GetComponent<DiverBehaviourScr>().currentDirection; // ?
		}
		if (_coll.gameObject.name == "Step")
		{
			if (name == "Diver")
			{
				_coll.gameObject.GetComponent<StepBehaviourScr>().Catch();
				gameManager.SetScore(1);
				gameManager.DeleteDiver(Number);
			}
			else
			{
				gameManager.Lose();
			}
		}
	}

	//смена направления движения
	void ChangeDirection ()
	{
		currentDirection.x = RandomSign() * currentDirection.x;
		currentDirection.y = RandomSign() * currentDirection.y;
	}

	/*public void ChangeMineToDiver()
	{
		IsMine = false;
		RB2D.name = "Diver";
		sprite.color = DB.DiverColor;
	}*/

	int RandomSign ()
	{
		return Random.value < .5? 1 : -1;
	}

	public void MakeInvisible()
	{
		sprite.enabled = false;
	}

	public void MakeVisible()
	{
		sprite.enabled = true;
	}
}

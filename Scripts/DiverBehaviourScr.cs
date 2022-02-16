using UnityEngine;

public class DiverBehaviour : MonoBehaviour 
{
	public Rigidbody2D rb2d;
	public SpriteRenderer sprite;
	public SpriteRenderer lil;
	[HideInInspector] public float speed;
	[HideInInspector] public Color color;
	[HideInInspector] public float radius;
	[HideInInspector] public bool isVisible;
	[HideInInspector] public bool isMine;
	[HideInInspector] public float timeRepeat;
	GameManager gameManager;
	[HideInInspector] public int number;
	Vector2 direction;
	Vector2 newDirection;

	void Start () 
	{
		newDirection = Vector2.zero;
		sprite.color = color;
		rb2d.name = isMine ? "Mine": "Diver";
		gameManager = GameManager.GM;

		InvokeRepeating("timeToChange", timeRepeat, timeRepeat);
	}

	void Update () 
	{
		//регулировка видимости
		sprite.enabled = isVisible;

		//выбор направления движения
		if (direction.x == 0 || direction.y == 0)
			direction = (Vector2)Random.onUnitSphere;
		if (newDirection.x != 0 && newDirection.x != 0)
		{
			direction = newDirection;
			newDirection = Vector2.zero;
		}
		rb2d.MovePosition(rb2d.position + direction*Time.deltaTime*speed);
		if (Mathf.Abs(rb2d.position.x) > gameManager.size.x || Mathf.Abs(rb2d.position.y) > gameManager.size.y)
			rb2d.position = Vector2.zero;
	}

	//столкновения
	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.name == "GameManager")
		{
			if (gameManager.size.x - radius <= Mathf.Abs(rb2d.position.x))
				direction.x = -direction.x;
			if (gameManager.size.y - radius <= Mathf.Abs(rb2d.position.y))
				direction.y = -direction.y;
		}
		if (coll.gameObject.name == "Mine" || coll.gameObject.name == "Diver")
		{
			newDirection = coll.gameObject.GetComponent<DiverBehaviour>().direction;//wrong
		}
		if (coll.gameObject.name == "Step")
		{
			if (name == "Diver")
			{
				coll.gameObject.GetComponent<PlayerBehaviour>().addCast();
				gameManager.SetScore(1);
				gameManager.DeleteDiver(number);
			}
			else
			{
				gameManager.Lose();
			}
		}
	}

	//смена направления движения
	void timeToChange ()
	{
		direction.x = RandomSign() * direction.x;
		direction.y = RandomSign() * direction.y;
	}

	public void ChangeMine()
	{
		isMine = false;
		rb2d.name = "Diver";
		sprite.color = color;
	}

	int RandomSign ()
	{
		return Random.value < .5? 1 : -1;
	}
}

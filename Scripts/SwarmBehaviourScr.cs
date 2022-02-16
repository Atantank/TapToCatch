using UnityEngine;

public class SwarmBehaviour : MonoBehaviour
{
	bool needDestroy;
	[HideInInspector] public int groupNum;
	[HideInInspector] public int friends;
	[HideInInspector] public bool isMain;
	[HideInInspector] public bool canCatch;
	[HideInInspector] public bool needPoint;
	public SpriteRenderer sprite;
	[HideInInspector] public Color color1;
	public Color color2;
	public Rigidbody2D rb2d;
	[HideInInspector] public float radius;
	GameManager gameManager;
	public Vector2 direction;
	[HideInInspector] public Vector2 point;
	public float endTime;
	public float currentTime;
	[HideInInspector] public float flyTime;
	[HideInInspector] public bool correcting;
	[HideInInspector] public float catchTime;

	void Awake()
	{
		needPoint = false;
		canCatch = false;
		correcting = false;
		needDestroy = false;
		direction = Vector2.zero;
	}

	void Start()
	{
		rb2d.name = "Swarm" + groupNum.ToString();
		currentTime = 0;
		endTime = flyTime;
		sprite.color = color1;

		gameManager = GameManager.GM;
	}

	void Update()
	{
		if (isMain && needPoint)
			setPoint();
		if (canCatch && sprite.color == color1)
			sprite.color = color2;
		if (!canCatch && sprite.color == color2)
			sprite.color = color1;
		currentTime += Time.deltaTime;
		rb2d.MovePosition(rb2d.position + direction * Time.deltaTime / endTime);
		if (Mathf.Abs(rb2d.position.x - point.x) + Mathf.Abs(rb2d.position.y - point.y) < 0.1f && canCatch)
		{
			direction = Vector2.zero;
			if (isMain)
				needPoint = true;
		}
	}

	void setPoint()
	{//need check distance btw old and new, set more distance
		float x = gameManager.size.x * 0.8f;
		float y = gameManager.size.y * 0.8f;
		point = new Vector2(Random.Range(-x, x), Random.Range(-y, y));
		needPoint = false;

		//передать другим из роя
		if (!gameManager)
			gameManager = GameManager.GM;
		for (int i = 1; i < friends; i++)
			gameManager.swarms[groupNum, i].point = point;
		SetDirectionMain();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.name == "Step" && canCatch)
			gameManager.swarms[groupNum, 0].SendDestroy();
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name == "GameManager")
			NewDirection();
	}

	public void SendDestroy()
	{
		if (!needDestroy)
		{
			needDestroy = true;
			gameManager.SwarmCatch(groupNum, friends);
		}
	}

	void NewDirection()
	{
		direction = point - rb2d.position;
		endTime = endTime - currentTime + 0.5f;
		correcting = true;
		int needCorrecting = 0;
		for (int i = 0; i < friends; i++)
			if (gameManager.swarms[groupNum, i].correcting)
				needCorrecting++;
		if (needCorrecting == friends)
			for (int i = 0; i < friends; i++)
				gameManager.swarms[groupNum, i].Again(endTime);
	}

	public void Again(float newTime)
	{
		correcting = false;
		direction = point - rb2d.position;
		endTime = newTime;
		Invoke("Catch", endTime - catchTime);
	}

	void Catch()
	{
		canCatch = true;
	}

	void SetDirectionMain()
	{
		int e = RandomSign() > 0 ? 1 : 0;
		int g = RandomSign();
		if (!gameManager)
			gameManager = GameManager.GM;
		switch (friends)
		{
			case 4:
				gameManager.swarms[groupNum, 3].SetDirection(e == 0 ? 1 : 0, g);
				goto case 3;
			case 3:
				gameManager.swarms[groupNum, 2].SetDirection(e, -g);
				goto case 2;
			case 2:
				gameManager.swarms[groupNum, 1].SetDirection(e == 0 ? 1 : 0, -g);
				goto case 1;
			case 1:
				SetDirection(e, g);
				break;
		}
	}

	public void SetDirection (int e, int g)
	{
		canCatch = false;
		//e = 0 или 1, g = -1 или 1
		//е - выбор вертикали или горизонтали, g - выбор стенки
		if (!gameManager)
			gameManager = GameManager.GM;
		float x = point.x * (1 - e) + e * g * gameManager.size.x;
		float y = point.y * e + (1 - e) * g * gameManager.size.y;
		Vector2 newPoint = new Vector2(x, y);
		Vector2 mirrow = (newPoint - point) * 2 + point;
		direction = mirrow - rb2d.position;
		endTime = flyTime;
		currentTime = 0;
	}

	int RandomSign ()
	{
		return Random.value < .5? 1 : -1;
	}
}

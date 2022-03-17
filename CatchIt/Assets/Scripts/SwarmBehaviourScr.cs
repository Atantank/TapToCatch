using UnityEngine;

public class SwarmBehaviourScr : MonoBehaviour
{
	public int GroupNum;
	public bool IsCorrecting { get; private set; }
	public int GroupSize { get; private set; }

	private GameManager gameManager;
	private SpriteRenderer sprite;
	private Rigidbody2D rb2d;
	private LineRenderer line;
	private Color color1;
	private Color color2;
	private Vector2 point;
	private Vector2 direction;
	private float radius; // ?
	private bool isNeedDestroy;
	private bool isMain;
	private bool canBeCatch;
	private float flyTime;
	private float currentTime;
	private float endTime;
	private float catchTime;

	void Awake()
	{
		sprite = GetComponent<SpriteRenderer>();
		rb2d = GetComponent<Rigidbody2D>();
		line = GetComponent<LineRenderer>();
		currentTime = 0;
		endTime = 1;
		canBeCatch = false;
		IsCorrecting = false;
		isNeedDestroy = false;
		direction = Vector2.zero;
		gameManager = GameManager.GM;
	}

	void Start()
	{
		rb2d.name = "Swarm" + GroupNum.ToString();
		line.positionCount = 3;
	}

	public void Initiate(int _groupNum, int _groupSize, float _radius, float _flyTime, float _catchTime, Color _color1, Color _color2)
	{
		GroupNum = _groupNum;
		GroupSize = _groupSize;
		radius = _radius;
		flyTime = _flyTime;
		catchTime = _catchTime;
		isMain = false;
		color1 = _color1;
		color2 = _color2;
		sprite.color = color1;
	}

	public void SetMain()
	{
		isMain = true;
		SetPoint();
	}

	void FixedUpdate()
	{
		//currentTime += Time.deltaTime;
		endTime -= Time.deltaTime;

		//print(endTime);
		if (isMain)
		{
			//if (Mathf.Abs(rb2d.position.x - point.x) + Mathf.Abs(rb2d.position.y - point.y) < 0.1f && canBeCatch)
			if (endTime < 0.001f)
			{
				SetPoint();
			}
		}
		if (canBeCatch && endTime < catchTime) // ?
			Catch();
		if (endTime > 0)
			//rb2d.MovePosition(rb2d.position + direction * Time.fixedDeltaTime / endTime);
			//rb2d.transform.Translate(direction);
			//rb2d.position = Vector3.MoveTowards(rb2d.position, point, direction.magnitude * Time.fixedDeltaTime / endTime);
			rb2d.position = Vector3.MoveTowards(rb2d.position, direction, (direction - rb2d.position).magnitude * Time.fixedDeltaTime / endTime);

	}

	void SetPoint()
	{
		float x = gameManager.Size.x * 0.8f;
		float y = gameManager.Size.y * 0.8f;
		point = new Vector2(Random.Range(-x, x), Random.Range(-y, y));

		//передать другим из роя
		for (int i = 1; i < GroupSize; i++)
			gameManager.Swarms[GroupNum, i].point = point;

		DistributeDifferentDirections();
	}

	void DistributeDifferentDirections()
	{
		int e = RandomSign();
		int g = RandomSign();
		switch (GroupSize)
		{
			case 5-10:
				goto case 4;
			case 4:
				gameManager.Swarms[GroupNum, 3].SetDirectionToMirrorPoint(e > 0 ? 1 : 0, g);
				goto case 3;
			case 3:
				gameManager.Swarms[GroupNum, 2].SetDirectionToMirrorPoint(e, -g);
				goto case 2;
			case 2:
				gameManager.Swarms[GroupNum, 1].SetDirectionToMirrorPoint(e > 0 ? 0 : 1, -g);
				goto case 1;
			case 1:
				SetDirectionToMirrorPoint(e, g);
				break;
		}
	}

	public void SetDirectionToMirrorPoint (int _e, int _g) // ? Point
	{
		int ee = RandomSign();
		int gg = RandomSign();
		canBeCatch = false;
		sprite.color = color1;
		//e = 0 или 1, g = -1 или 1
		//е - выбор вертикали или горизонтали, g - выбор стенки
		/*float x = point.x * (1 - e) + e * g * gameManager.size.x;
		float y = point.y * e + (1 - e) * g * gameManager.size.y;
		Vector2 newPoint = new Vector2(x, y);
		Vector2 mirrow = (newPoint - point) * 2 + point;*/
		Vector2 mirrowPoint = new Vector2(2* ee * (1 - _e) * (gameManager.Size.x - point.x) + point.x, 2 * gg * _e * (gameManager.Size.y - point.y) + point.y);
		direction = mirrowPoint - rb2d.position;
		direction = mirrowPoint;
		line.SetPosition(0, new Vector3(rb2d.position.x, rb2d.position.y, -1));
		line.SetPosition(1, new Vector3(mirrowPoint.x, mirrowPoint.y, -1));
		line.SetPosition(2, new Vector3(point.x, point.y, -1));
		endTime = flyTime;
		currentTime = 0;
		//Invoke("Catch", endTime - catchTime);
		/*print("__1__");
		print(point);
		print(rb2d.position);
		print(direction);
		print(endTime);*/
	}

	void SetDirectionToPoint()
	{
		/*print("__2__");
		print(point);
		print(rb2d.position);
		print(direction);
		print(endTime);*/
		direction = point - rb2d.position;
		direction = point;
		//endTime = endTime - currentTime/* + 0.5f*/; // !
		/*IsCorrecting = true;
		int needCorrecting = 0;
		for (int i = 0; i < GroupSize; i++)
			if (gameManager.Swarms[GroupNum, i].IsCorrecting)
				needCorrecting++;
		if (needCorrecting == GroupSize)
			for (int i = 0; i < GroupSize; i++)
				gameManager.Swarms[GroupNum, i].SetNewFlyTime(flyTime - currentTime);*/
		/*print("__3__");
		print(point);
		print(rb2d.position);
		print(direction);
		print(endTime);
		print(currentTime);*/
	}

	/*public void SetNewFlyTime(float _newTime)
	{
		IsCorrecting = false;
		direction = point - rb2d.position;
		endTime = _newTime;
		Invoke("Catch", endTime - catchTime);
		/*print("__4__");
		print(direction);
		print(endTime);*/
	//}

	void Catch() // !
	{
		canBeCatch = true;
		sprite.color = color2;
		//Time.timeScale = 0;
	}

	/*void OnTriggerStay2D(Collider2D _other)
	{
		if (_other.name == "Step" && canBeCatch)
			gameManager.Swarms[GroupNum, 0].SendDestroy();
	}*/

	void OnTriggerEnter2D(Collider2D _other)
	{
		if (_other.name == "Step" && canBeCatch)
			gameManager.Swarms[GroupNum, 0].SendDestroy();
		if (_other.gameObject.name == "GameManager")
			SetDirectionToPoint();
	}

	public void SendDestroy()
	{
		if (!isNeedDestroy)
		{
			isNeedDestroy = true;
			gameManager.DeleteSwarm(GroupNum, GroupSize);
		}
	}

	int RandomSign ()
	{
		return Random.value < .5? 1 : -1;
	}
}

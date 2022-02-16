using UnityEngine;

public class EnemyBehaviour : MonoBehaviour 
{	
	float speed;
    Rigidbody2D rb2d;
    Vector3 playerPosition;
    Vector2 direction;
	Vector2 stepAside;
	Vector2 size;
    bool isNearBorder;
	//float randomRange;
	float stepX;
	//int maxTime;
	int time;
	float enemySize;
	Vector2 lastPosition;
	float lastTime;
	
    void Start () 
	{
        rb2d = GetComponent<Rigidbody2D>();
		time = 0;
		size = 10*Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 1f));

        //определение параметров врага
        speed = 1f;
		//randomRange = 1f;
		//maxTime = 4;
		enemySize = 1f;
	}

	void Update () 
	{
		if (lastPosition.sqrMagnitude >= direction.sqrMagnitude)
		{
			lastPosition = Vector2.zero;
			playerPosition = Input.mousePosition;
			playerPosition.z = 1.0f;

			//проверка близости стенки
			if (Mathf.Abs(rb2d.position.x) > size.x - enemySize && Mathf.Abs(rb2d.position.y) > size.y - enemySize)
			{
				//float r = 0.25f * RandomSign();
				//direction = new Vector2((-0.25f + r) * rb2d.position.x + 0.1f, (-0.25f - r) * rb2d.position.y + 0.1f);
				//MoveEnemy(direction);
			}
			else //если не в углу
			{
				if (Mathf.Abs(rb2d.position.x) > size.x - enemySize || Mathf.Abs(rb2d.position.y) > size.y - enemySize)
				{
					//Vector(randomRange);
					//direction = 5 * stepAside * speed;
					//MoveEnemy(direction);
				}
				else //если не рядом со стенкой и не в углу
				{
					//направление убегания
					direction = (Vector2)rb2d.position - (Vector2)Camera.main.ScreenToWorldPoint(playerPosition) * 10;
					//отклонение в направлении убегания - рандомный шаг в сторону
					stepAside = Vector2.zero;
					//if (time == maxTime)
						//Vector(randomRange);
					time++;
					direction = stepAside + direction / (direction.magnitude);
					//float distance = direction.magnitude;
				}
			}
		}
		lastTime = Time.deltaTime;
		rb2d.MovePosition(rb2d.position + direction*lastTime*speed);
		lastPosition += direction*lastTime*speed;
        //rb2d.velocity = new Vector2(direction.x * speed / distance, direction.y * speed / distance);
		//rb2d.velocity = direction*speed*Time.deltaTime/distance;
	}

	//перпендикуляр к вектору рандомной длины на основе величины stepX
	void Vector (float range)
	{
		do
		{
			stepX = Random.Range(-range, range);
		}
		while (stepX == 0); 
		stepAside = new Vector2(stepX, -(direction.x*stepX)/direction.y);
		time = 0;
	}

	int RandomSign ()
	{
		return Random.value < .5? 1 : -1;
	}
}

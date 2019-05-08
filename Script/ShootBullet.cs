using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public class ShootBullet : MonoBehaviour {
	public bool canTouch;
	public bool touchStart;
	public Vector3 startPoint;
	public float speed = 5f;
	public int maxShootChance = 0;
    public ParticleSystem particle;
    public bool firstShoot = true;

	[SerializeField] private int shootChance;

    private bool isShootable = true;

    public int GetshootChance()
    {
        return shootChance;
    }
	public void AddShootChance(int pChance)
	{
		shootChance += pChance;
	}
	// Use this for initialization
	void Start ()
	{
		canTouch = true;
		touchStart = false;
		
		GetComponent<LineRenderer>().SetVertexCount(2);
		GetComponent<LineRenderer>().SetWidth(0.25f, 0f);
		GetComponent<LineRenderer>().SetColors(Color.red, Color.red);
		GetComponent<LineRenderer>().enabled = false;

        if (GetComponent<CloneControl>() == null)
        {
            shootChance = maxShootChance;
        } else
        {
            GameObject player = FindPlayer();
            maxShootChance = player.GetComponent<ShootBullet>().maxShootChance;
            shootChance = player.GetComponent<ShootBullet>().GetshootChance();
        }

    }
	
	// Update is called once per frame
	void Update ()
	{
        if (GameManger.Instance.IsPlayerDead)
            return;

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(t.fingerId) == true)
            {
                return;
            }
            Vector3 pos = Camera.main.ScreenToWorldPoint(t.position);
            ReadyShoot(pos, t.phase);
        }

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ReadyShoot(pos, TouchPhase.Began);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ReadyShoot(pos, TouchPhase.Moved);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ReadyShoot(pos, TouchPhase.Ended);
            }
        }
        if(shootChance == 0)
        {
            isShootable = false;
        } else
        {
            isShootable = true;
        }
    }

	void ReadyShoot(Vector3 pos, TouchPhase state)
	{
		if( state == TouchPhase.Began && !touchStart && shootChance > 0)
		{
			if ((shootChance != maxShootChance) || !firstShoot)
				Time.timeScale = 0f;
			--shootChance;

			startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			touchStart = true;
			GetComponent<LineRenderer>().enabled = true;
		}
		else if ( state == TouchPhase.Moved && touchStart)
		{
			Vector2 dir = (startPoint - pos).normalized;
			GetComponent<LineRenderer>().SetPosition(0, transform.position);
			GetComponent<LineRenderer>().SetPosition(1, transform.position + (Vector3)(dir * 0.5f));
		}
		else if ( state == TouchPhase.Ended && touchStart)
		{
            firstShoot = false;
			Vector2 dir = (startPoint - pos).normalized;
            if (dir == Vector2.zero)
            {
                float tmp = Random.Range(0, 360);
                tmp = tmp * Mathf.Deg2Rad;
                dir = new Vector2(Mathf.Cos(tmp), Mathf.Sin(tmp)).normalized;
            }
            GetComponent<Rigidbody2D>().velocity = dir * speed;
			touchStart = false;
			GetComponent<LineRenderer>().enabled = false;
			Time.timeScale = 1f;
            Instantiate(particle, transform.position, transform.rotation);
		}
	}

    private GameObject FindPlayer()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        for(int i = 0; i < targets.Length; i++)
        {
            if(targets[i].GetComponent<CloneControl>() == null)
            {
                maxShootChance = targets[i].GetComponent<ShootBullet>().maxShootChance;
                shootChance = targets[i].GetComponent<ShootBullet>().GetshootChance();
                return targets[i];
            }
        }
        return null;
    }

    public bool IsShootable {
        get { return isShootable; }
        set { isShootable = value; }
    }
}
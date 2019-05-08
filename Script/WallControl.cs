using UnityEngine;

public class WallControl : MonoBehaviour {

    [SerializeField] private Vector3[] initPosition;
    [SerializeField] private float speed;
    [SerializeField] private float pointStayTime = 0f;
    [SerializeField] private bool isPatrol = false;
    [SerializeField] private bool isBreak = false;
    [SerializeField] private int leftColideCount = 1;

    private bool isReturnPatrol = false;
    private bool isArrive = false;
    private int nextMovePointNumber = 1;
    private float tempPointStayTime = 0f;

    // Use this for initialization
    void Start () {
        if (initPosition.Length > 0)
        {
            transform.position = initPosition[0];
        }
        if (isBreak)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
            GetComponent<Renderer>().material = Resources.Load<Material>("Material/GreenBreakGlow");
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (initPosition.Length < 2)
        {
            return;
        }
        if (initPosition[nextMovePointNumber] == transform.position)
        {
            isArrive = true;
            if (!isPatrol)
                tempPointStayTime = 0;

            if (tempPointStayTime > 0 && (nextMovePointNumber == initPosition.Length - 1 || nextMovePointNumber == 0))
            {
                tempPointStayTime -= Time.deltaTime;
                //Debug.Log("tempPointStayTime : " + tempPointStayTime);
                return;
            }

            if (tempPointStayTime < 0)
            {
                tempPointStayTime = 0;
            }

            tempPointStayTime = pointStayTime;
            CalculateNextMovePointNumber();
            isArrive = false;
        }
    }

    private void FixedUpdate()
    {
        if (initPosition.Length < 2)
        {
            return;
        }
        if (!isArrive)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, initPosition[nextMovePointNumber], step);
        }
    }

    private void CalculateNextMovePointNumber()
    {
        if (isPatrol)
        {
            if (isReturnPatrol)
            {
                nextMovePointNumber -= 1;
                if (nextMovePointNumber == 0)
                {
                    isReturnPatrol = false;
                }
            }
            else
            {
                nextMovePointNumber += 1;
                if (initPosition.Length < nextMovePointNumber + 1)
                {
                    nextMovePointNumber -= 1;
                    isReturnPatrol = true;
                }
            }
        }
        else
        {
            nextMovePointNumber += 1;
            if (initPosition.Length < nextMovePointNumber + 1)
            {
                nextMovePointNumber = 0;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (initPosition.Length > 0)
        {
            for (int i = 0; i < initPosition.Length; i++)
            {
                Gizmos.DrawIcon(initPosition[i], "blueprint.png", false);
            }

            for (int i = 0; i < initPosition.Length - 1; i++)
            {
                Gizmos.DrawLine(initPosition[i], initPosition[i + 1]);
            }

            if (!isPatrol)
            {
                if (initPosition.Length > 1)
                {
                    Gizmos.DrawLine(initPosition[0], initPosition[initPosition.Length - 1]);
                }
            }
        }

        if (isBreak)
        {
            Gizmos.DrawIcon(transform.position, "break.png", false);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            //Debug.Log(coll.transform);
            if (isBreak)
            {
                
                leftColideCount -= 1;
                if (leftColideCount <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}

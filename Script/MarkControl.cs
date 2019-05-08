using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkControl : MonoBehaviour {

    [SerializeField] private Vector3[] initPosition;
    [SerializeField] private GameObject[] activationTargetObjects;

    [SerializeField] private float speed;
    [SerializeField] private float pointStayTime = 0f;
    [SerializeField] private bool isPatrol = false;
    [SerializeField] private int leftColideCount = 1;

    private bool isReturnPatrol = false;
    private bool isArrive = false;
    private int nextMovePointNumber = 1;
    private float tempPointStayTime = 0f;

    public ParticleSystem particle;

    // Use this for initialization
    void Start () {
        if (initPosition.Length > 0)
        {
            transform.position = initPosition[0];
        }
        HideActivationTargetObjects();
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
                Debug.Log("tempPointStayTime : " + tempPointStayTime);
                return;
            }

            if(tempPointStayTime < 0)
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
                if(nextMovePointNumber == 0)
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
        } else
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            leftColideCount -= 1;
            Instantiate(particle, transform.position, transform.rotation);
            if(leftColideCount <= 0)
            {
                Destroy(gameObject);

            }
        }
        
    }

    private void OnDestroy()
    {
            ShowActivationTargetObjects();
    }

    private void HideActivationTargetObjects()
    {
        if (activationTargetObjects.Length > 0)
        {
            for(int i = 0; i < activationTargetObjects.Length; i++)
            {
                activationTargetObjects[i].GetComponent<BoxCollider2D>().enabled = false;
                activationTargetObjects[i].transform.Find("mark").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.12f);
            }
        }
    }

    private void ShowActivationTargetObjects()
    {
        if (activationTargetObjects.Length > 0)
        {
            for (int i = 0; i < activationTargetObjects.Length; i++)
            {
                if(activationTargetObjects[i] != null)
                {
                    activationTargetObjects[i].GetComponent<BoxCollider2D>().enabled = true;
                    activationTargetObjects[i].transform.Find("mark").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
        }
    }
}

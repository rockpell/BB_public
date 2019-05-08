using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalControl : MonoBehaviour {

    [SerializeField] private Vector3[] initPosition;
    [SerializeField] private float speed;
    [SerializeField] private float pointStayTime = 0f;
    [SerializeField] private bool isPatrol = false;
    [SerializeField] private int portalNumber;
    [SerializeField] private int linkPortalNumber;

    private bool isReturnPatrol = false;
    private bool isArrive = false;
    private bool isTeleportable = true;
    private int nextMovePointNumber = 1;
    private float tempPointStayTime = 0f;

    private Transform spriteObject;

    // Use this for initialization
    void Start () {
        if (initPosition.Length > 0)
        {
            transform.position = initPosition[0];
        }

        GetComponent<LineRenderer>().SetVertexCount(2);
        GetComponent<LineRenderer>().SetWidth(0.02f, 0.02f);
        GetComponent<LineRenderer>().SetColors(Color.gray, Color.gray);
        GetComponent<LineRenderer>().enabled = true;
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, FindPortal(linkPortalNumber).transform.position);
        spriteObject = transform.Find("portal");
    }
	
	// Update is called once per frame
	void Update () {
        RotateSprite();
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

    private void RotateSprite()
    {
        spriteObject.Rotate(0, 0, Time.deltaTime * 15);
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
        if(initPosition.Length > 0)
        {
            for (int i = 0; i < initPosition.Length; i++)
            {
                Gizmos.DrawIcon(initPosition[i], "portal_gizmo.jpg", false);
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
        if (other.tag == "Player")
        {
            TeleportPlayer(other.gameObject);
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            SetIsTeleportable(true);
        }

    }

    void TeleportPlayer(GameObject obj)
    {
        GameObject _targetPortal = FindPortal(linkPortalNumber);
        if (_targetPortal && isTeleportable)
        {
            obj.transform.position = _targetPortal.transform.position;
            _targetPortal.GetComponent<PortalControl>().SetIsTeleportable(false);
        }
        
    }

    GameObject FindPortal(int portalNumber)
    {
        GameObject[] _portals;
        _portals = GameObject.FindGameObjectsWithTag("Portal");

        for(int i = 0; i < _portals.Length; i++)
        {
            if (_portals[i].GetComponent<PortalControl>().PortalNumber == portalNumber)
            {
                return _portals[i];
            }
        }

        return null;
    }

    public void SetIsTeleportable(bool value)
    {
        isTeleportable = value;
}

    public int PortalNumber {
        get { return portalNumber; }
    }
    public int LinkPortalNumber {
        get { return linkPortalNumber; }
    }
}

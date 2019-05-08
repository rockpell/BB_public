using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLineControl : MonoBehaviour {

    private GameObject linkPortal;
    public int linkPortalNumber;
    private GameObject myPortal;
    private Vector2 direction;
    public int myPortalNumber;

    void Start()
    {
        GameObject[] portals;
        portals = GameObject.FindGameObjectsWithTag("Portal");
        myPortalNumber = gameObject.transform.parent.GetComponent<PortalControl>().PortalNumber;
        linkPortalNumber = gameObject.transform.parent.GetComponent<PortalControl>().LinkPortalNumber;
        for (int i = 0; i < portals.Length; i++)
        {
            if (portals[i].GetComponent<PortalControl>().PortalNumber == linkPortalNumber)
                linkPortal = portals[i];
            if (portals[i].GetComponent<PortalControl>().PortalNumber == myPortalNumber)
                myPortal = portals[i];
        }
        GetComponent<ParticleSystem>().trigger.SetCollider(0, linkPortal.transform);
        direction = myPortal.transform.position - linkPortal.transform.position;
        direction.Normalize();
        float angle = Vector3.Angle(Vector3.up, direction);
        angle = angle + 180;
        Vector3 cross = Vector3.Cross(Vector3.up, direction);
        if (cross.z < 0)
        {
            angle = -angle;
        }
        this.transform.RotateAround(myPortal.transform.position, Vector3.forward, angle);
    }

    void UpdateAngle()
    {
        Vector2 nextDirection = myPortal.transform.position - linkPortal.transform.position;
        nextDirection.Normalize();
        float angle = Vector3.Angle(nextDirection, direction);
        Vector3 cross = Vector3.Cross(direction,nextDirection);
        if (cross.z < 0)
        {
            angle = -angle;
        }
        this.transform.RotateAround(myPortal.transform.position,Vector3.forward, angle);
        direction = nextDirection;
    }

	void Update ()
    {
        UpdateAngle();
	}
}

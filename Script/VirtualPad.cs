using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualPad : MonoBehaviour
{
	ShootBullet shootBullet;
	// Use this for initialization
	void Start ()
	{
		shootBullet = GameObject.Find("Player").GetComponent<ShootBullet>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        if (Input.touchCount > 0) //Mobile INPUT
        {
            Touch t = Input.GetTouch(0);
            if(EventSystem.current.IsPointerOverGameObject(t.fingerId) == true)
            {
                return;
            }
            Vector3 pos = Camera.main.ScreenToWorldPoint(t.position);
            SetVirtualPad(pos, t.phase);
        }

        if (EventSystem.current.IsPointerOverGameObject() == false)
        {
            //PC INPUT
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetVirtualPad(pos, TouchPhase.Began);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetVirtualPad(pos, TouchPhase.Moved);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetVirtualPad(pos, TouchPhase.Ended);
            }
        }
	}
	void SetVirtualPad(Vector2 pPos, TouchPhase pState)
	{
		if(pState == TouchPhase.Began && !(GameManger.Instance.IsGameOver || GameManger.Instance.IsGameClear))
		{
			int childCount = transform.GetChildCount();
			for (int i = 0; i < childCount; ++i)
			{
                if (shootBullet.IsShootable)
                {
                    Transform g = transform.GetChild(i);
                    g.gameObject.SetActive(true);
                    g.position = pPos;
                }
			}
		}
		else if(pState == TouchPhase.Moved)
		{
			if(Vector2.Distance(transform.GetChild(0).position, pPos) > 0.5f)
			{
				Vector2 dir = (pPos - (Vector2)transform.GetChild(0).position).normalized;
				transform.GetChild(1).position = (Vector2)transform.GetChild(0).position + (dir * 0.5f);
			}
			else transform.GetChild(1).position = pPos;
			
		}
		else if(pState == TouchPhase.Ended)
		{
			int childCount = transform.GetChildCount();
			for (int i = 0; i < childCount; ++i)
			{
				transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}

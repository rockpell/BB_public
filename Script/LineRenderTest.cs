using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineRenderTest : MonoBehaviour {

    public float _collisionLine;
    public float _seconLineLength;
    private LineRenderer _line;
    private Touch _tempTouchs;
    private Vector3 _touchedPos;
    private Vector3 _startPos;
    private bool _touchOn;
    private RaycastHit2D _hit;
    private GameObject _player;
    private bool _enable;

    private bool _clickOn;

    void Start ()
    {
        _enable = true;
        _touchOn = false;
        _player = GameObject.FindGameObjectWithTag("Player");
        _line = GetComponent<LineRenderer>();
        _line.startWidth = 0.05f;
        _line.endWidth = 0.05f;
        _line.startColor = Color.red;
        _line.endColor = Color.red;

        _clickOn = false;
	}

    void Update()
    {
        if (_enable)
        {
            if (Input.touchCount > 0)
            {
                
                _tempTouchs = Input.GetTouch(0);
                if (EventSystem.current.IsPointerOverGameObject(_tempTouchs.fingerId) == true)
                {
                    return;
                }
                if (_tempTouchs.phase == TouchPhase.Began && !_touchOn)
                {
                    _touchOn = true;
                    _line.enabled = true;
                    _startPos = Camera.main.ScreenToWorldPoint(_tempTouchs.position);
                }
                
            }

            if ((_tempTouchs.phase == TouchPhase.Moved || _tempTouchs.phase == TouchPhase.Stationary) && _touchOn)
            {
                _touchedPos = Camera.main.ScreenToWorldPoint(_tempTouchs.position);
                Vector2 direction = (_startPos - _touchedPos);
                Debug.DrawRay(_player.transform.position, direction, Color.magenta);
                direction.Normalize();
                _hit = Physics2D.Raycast(_player.transform.position, direction, _collisionLine, (1 << 9));
                if (_hit.collider != null)
                {
                    _line.SetPosition(0, _player.transform.position);
                    _line.SetPosition(1, _hit.point);
                    Vector3 indirection = Vector3.Reflect(direction, _hit.normal);
                    indirection.Normalize();
                    _line.SetPosition(2, _hit.point + new Vector2(indirection.x, indirection.y) * _seconLineLength);
                }
            }

            if (Input.GetMouseButtonUp(0) && _touchOn)
            {
                _touchOn = false;
                _line.enabled = false;
                _enable = false;
            }
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                //여기부터 마우스 처리 (나중에 삭제)
                if (Input.GetMouseButtonDown(0) && !_clickOn)
                {
                    _clickOn = true;
                    _line.enabled = true;
                    _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                if (Input.GetMouseButton(0) && _clickOn)
                {
                    _touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 direction = (_startPos - _touchedPos);
                    Debug.DrawRay(_player.transform.position, direction, Color.magenta);
                    direction.Normalize();
                    _hit = Physics2D.Raycast(_player.transform.position, direction, _collisionLine, (1 << 9));
                    if (_hit.collider != null)
                    {
                        _line.SetPosition(0, _player.transform.position);
                        _line.SetPosition(1, _hit.point);
                        //Debug.Log(_hit.point);
                        Vector3 indirection = Vector3.Reflect(direction, _hit.normal);
                        indirection.Normalize();
                        _line.SetPosition(2, _hit.point + new Vector2(indirection.x, indirection.y) * _seconLineLength);
                    }
                }
            }
            
            if (Input.GetMouseButtonUp(0) && _clickOn)
            {
                _clickOn = false;
                _line.enabled = false;
                _enable = false; 
            }
        }
    }
}

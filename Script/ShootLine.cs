using UnityEngine;
using UnityEngine.EventSystems;


public class ShootLine : MonoBehaviour {

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

    private float _radius;

    private bool _clickOn;
    
    void Start()
    {
        _enable = true;
        _touchOn = false;
        _player = GameObject.FindGameObjectWithTag("Player");
        _line = GetComponent<LineRenderer>();
        _line.startWidth = 0.05f;
        _line.endWidth = 0.05f;
        _line.startColor = Color.red;
        _line.endColor = Color.red;

        _radius = _player.transform.lossyScale.x / 2;

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
                direction.Normalize();

                _hit = Physics2D.CircleCast(_player.transform.position, _radius, direction,_collisionLine, (1 << 9));

                if (_hit.collider != null)
                {
                    _line.SetPosition(0, _player.transform.position);
                    _line.SetPosition(1, (Vector3)_hit.point);
                    Vector3 indirection = Vector3.Reflect(direction, _hit.normal);
                    indirection.Normalize();
                    _line.SetPosition(2, (Vector3)(_hit.point + new Vector2(indirection.x, indirection.y) * _seconLineLength));
                    _line.SetPosition(3, (Vector3)_hit.point);
                    _line.SetPosition(4, _player.transform.position);
                    _line.SetPosition(5, (Vector3)_hit.point);
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
                    direction.Normalize();
                    
                    _hit = Physics2D.CircleCast(_player.transform.position, _radius, direction, _collisionLine, (1 << 9));
                    if (_hit.collider != null)
                    {
                        _line.SetPosition(0, _player.transform.position);
                        _line.SetPosition(1, (Vector3)_hit.point);
                        
                        Vector3 indirection = Vector3.Reflect(direction, _hit.normal);
                        indirection.Normalize();
                        _line.SetPosition(2, (Vector3)(_hit.point + new Vector2(indirection.x, indirection.y) * _seconLineLength));
                        _line.SetPosition(3, (Vector3)_hit.point);
                        _line.SetPosition(4, _player.transform.position);
                        _line.SetPosition(5, (Vector3)_hit.point);
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
    
    void OnDrawGizmos()
    {
        if (Input.GetMouseButtonDown(0) && !_clickOn)
        {
            _startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(0) && _clickOn)
        {
            _touchedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (_startPos - _touchedPos);
            direction.Normalize();
            
            _hit = Physics2D.CircleCast(_player.transform.position, _radius, direction, _collisionLine, (1 << 9));
            if (_hit.collider != null)
            {
               
                Vector3 indirection = Vector3.Reflect(direction, _hit.normal);
                indirection.Normalize();
                
                bool isHit = Physics2D.CircleCast(_player.transform.position, _radius, direction, _collisionLine, (1 << 9));

                Gizmos.color = Color.red;
                if (isHit)
                {
                    Gizmos.DrawRay(_player.transform.position, transform.forward * _hit.distance);
                    Gizmos.DrawWireSphere(_hit.point, _player.transform.lossyScale.x / 2);
                }
                else
                {
                    Gizmos.DrawRay(_player.transform.position, transform.forward * _collisionLine);
                }
            }
        }
        
    }
}

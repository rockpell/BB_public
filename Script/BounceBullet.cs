using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    public Vector3 _startPos;
    public int _collisionMax;
    public ParticleSystem _particle;

    private Rigidbody2D _rigid;
    private int _collisionCounter;

    public int GetcollisionCounter()
    {
        return _collisionMax - _collisionCounter;
    }

	void Start ()
    {
        GameManger.Instance.IsPlayerDead = false;
		_collisionCounter = 0;
		if (_startPos == null)
			_startPos = transform.position;
	}
	
	void FixedUpdate ()
    {

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.tag == "Wall")
		{
            Instantiate(_particle, transform.position, transform.rotation);
			_collisionCounter++;
			if (_collisionCounter > _collisionMax)
			{
				GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				//transform.position = _startPos;
				//_collisionCounter = 0;
				gameObject.GetComponent<Renderer>().enabled = false;
                if(!GetComponent<CloneControl>())
                    GameManger.Instance.IsPlayerDead = true;
                Destroy(gameObject, 1f);
			}
		}
    }

    private void OnDestroy()
    {
        //GameManger.Instance.IsPlayerDead = true;
        Debug.Log("Player dead");
    }
}

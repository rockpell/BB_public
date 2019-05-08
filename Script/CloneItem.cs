using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneItem : MonoBehaviour {

    private GameObject player;
    public GameObject clone;
    public float angle;

	void Start ()
    {}
	
	void Update ()
    {}

    private Vector2 RotationVector2D(Vector2 dir, float angle)
    {
        float x = dir.x;
        float y = dir.y;
        angle = Mathf.Deg2Rad * angle;

        Vector2 newDir = new Vector2(x * Mathf.Cos(angle) - y * Mathf.Sin(angle), y * Mathf.Cos(angle) + x * Mathf.Sin(angle));

        Debug.DrawRay(player.transform.position, newDir, Color.red);
        return newDir;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        player = collider.gameObject;
        float speed = player.GetComponent<ShootBullet>().speed;
        Vector2 dir = player.GetComponent<Rigidbody2D>().velocity.normalized;

        Vector2 pDir = RotationVector2D(dir, angle);
        Vector2 cDir = RotationVector2D(dir, -angle);

        //player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        player.GetComponent<Rigidbody2D>().velocity = pDir * speed;
        Debug.Log("player: " + player.GetComponent<Rigidbody2D>().velocity);
        clone  = Instantiate(clone, player.transform.position, player.transform.rotation);
        clone.GetComponent<CloneControl>().SetVelocity(cDir * speed);
        clone.GetComponent<ShootBullet>().maxShootChance = player.GetComponent<ShootBullet>().maxShootChance;
        clone.GetComponent<ShootBullet>().AddShootChance(player.GetComponent<ShootBullet>().GetshootChance());
		clone.GetComponent<ShootBullet>().speed = speed;

        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneControl : MonoBehaviour {

    public void SetVelocity(Vector2 dir)
    {
        this.GetComponent<Rigidbody2D>().velocity = dir;
        Debug.Log("clone: " + this.GetComponent<Rigidbody2D>().velocity);
    }
}

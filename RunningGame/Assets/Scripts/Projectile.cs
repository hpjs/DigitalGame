﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    //Private timing variables
    public float damage;
    public bool destroyObjectOnCollision;
    public bool stopAttackOnCollision;

    public string damagesWhat;

    public Vector3 direction;
    public float speed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch(Vector3 aimLocation)
    {
        //direction = aimLocation;
        //direction = gameObject.transform.position - aimLocation;
        direction = aimLocation - gameObject.transform.position;
        direction.Normalize();
        gameObject.GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collision2D coll)
    {
        //Calls the 'take damage' function on the colliding object
        if (coll.gameObject.tag == damagesWhat)
        {
            coll.gameObject.SendMessage("takeDamage", damage);
        }

        if (destroyObjectOnCollision)
        {
            Destroy(gameObject);
        }
        else if (stopAttackOnCollision)
        {
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Hitbox must be enabled for this to happen
        //Calls the 'take damage' function on the colliding object
        if (coll.gameObject.tag == damagesWhat)
        {
            coll.gameObject.SendMessage("takeDamage", damage);
        }

        if (destroyObjectOnCollision)
        {
            Destroy(gameObject);
        }
        else if (stopAttackOnCollision)
        {
        }
    }
}

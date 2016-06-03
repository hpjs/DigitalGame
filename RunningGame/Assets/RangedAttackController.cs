﻿using UnityEngine;
using System.Collections;

public class RangedAttackController : MonoBehaviour
{
    //Private timing variables
    private bool attacking;
    private bool charging;
    private bool damaging;
    private bool onCooldown;

    private float timer = 0;

    //Attack attributes
    public float damage;
    public bool destroyObjectOnCollision;
    public bool stopAttackOnCollision;
    //Time that attack will last in seconds

    //Determines the sprites that the top level animator will use for animations
    public float chargeTime;
    public Sprite[] chargeSprites;
    public float damageTime;
    public Sprite[] damageSprites;
    public float cooldownTime;
    public Sprite[] cooldownSprites;

    private Collider2D hitbox;

    public GameObject projectile;

    public bool aimAtPlayer;
    public Vector3 aimPosition;


    // Use this for initialization
    void Start()
    {
        //Should select the first Collider2D in the instance
        hitbox = GetComponent<Collider2D>();
        if (damageTime != 0)
        {
            hitbox.enabled = false;
        }
        else
        {
            hitbox.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (charging)
            {
                startDamage();
            }
            else if (damaging)
            {
                startCooldown();
            }
            else if (onCooldown)
            {
                endAttack();
            }
        }
    }

    void startAttack()
    {
        //Doesn't start the attack if already attacking.
        if (!attacking)
        {
            attacking = true;
            charging = true;
            timer = chargeTime;
            //Tells upper objects not to attack because already attacking.
            SendMessageUpwards("isAttacking", true);
            SendMessageUpwards("startSpecialAnim", chargeSprites);
        }
    }

    void startDamage()
    {
        charging = false;
        damaging = true;
        hitbox.enabled = true;
        timer = damageTime;
        SendMessageUpwards("startSpecialAnim", damageSprites);

        //Creates projectile
        projectile.SetActive(true);

        projectile.GetComponent<Projectile>().Launch();

    }

    void startCooldown()
    {
        onCooldown = true;
        damaging = false;
        hitbox.enabled = false;
        timer = cooldownTime;
        if (cooldownSprites.Length != 0)
        {
            SendMessageUpwards("startSpecialAnim", cooldownSprites);
        }
        else
        {
            SendMessageUpwards("endSpecialAnim");
        }
    }

    void endAttack()
    {
        onCooldown = false;
        attacking = false;
        SendMessageUpwards("isAttacking", false);
        if (cooldownSprites.Length != 0)
        {
            SendMessageUpwards("endSpecialAnim");
        }
        //Not currently needed, may be needed later
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //Hitbox must be enabled for this to happen
        //Calls the 'take damage' function on the colliding object
        if (coll.gameObject.tag != "Environment")
        {
            coll.gameObject.SendMessage("takeDamage", damage);
        }

        if (destroyObjectOnCollision)
        {
            endAttack();
            Destroy(gameObject);
        }
        else if (stopAttackOnCollision)
        {
            endAttack();
        }
    }
}

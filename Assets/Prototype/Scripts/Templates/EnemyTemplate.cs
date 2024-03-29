﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTemplate : MonoBehaviour
{
    //health variables
    public float enemyHealth;
    public static float enemyMaxHealth;
    public static float enemyHealthAdjusted;

    //attack variables
    public bool enemyShotCooldown = true;
    float enemyCooldownSpeed = 2f;
    public float enemyCooldownTimer;

    //Gameplay Variables
    private float enemyDifficulty = 1;

    //Attached GameObject properties
    public Rigidbody2D rb;
    public GameObject enemyWeapon;
    public GameObject credit;

    // Start is called before the first frame update

    void Awake()
    {
        enemyMaxHealth = 30f;
        enemyHealthAdjusted = enemyMaxHealth *= .5f + ((WaveController.difficulty / 10f) * 5f);
        enemyHealth = enemyHealthAdjusted;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyShotCooldown)
        {//if on a shot cooldown subtract from the cooldown timer, 
            enemyCooldownTimer -= Time.deltaTime;
            //when the cooldown timer finishes, cooldown is off
            if (enemyCooldownTimer <= 0)
            {
                enemyShotCooldown = false;
            }
        }
        else
        //if not on shot cooldown, start a fire. Go back on cooldown and reset the cooldown timer.
        {
            enemyShotCooldown = true;
            enemyCooldownTimer = enemyCooldownSpeed + Random.Range(.2f, 2);
            Launch();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Collider2D w = other.collider.GetComponent<CompositeCollider2D>();

        if (other.gameObject.tag == "playerWeapon")
        {
            ChangeHealth();
            Destroy(other.gameObject);
        }
    }
    public void ChangeHealth()
    {
        //public method that player weapons can call to damage this.
        enemyHealth -= PlayerStats.playerDamage;
        if (enemyHealth <= 0)
        {
            //DropLoot();
            WaveController.DropLoot(enemyDifficulty, rb.position);
            WaveController.enemiesRemaining--;
            Destroy(gameObject);
        }
    }

    public void Launch()
    {
        StartCoroutine("Launch2");
    }
    IEnumerator Launch2()
    {

        //animator.SetTrigger("fire");
        yield return new WaitForSeconds(0.9f);
    }
}

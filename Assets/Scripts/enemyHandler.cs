using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyHandler : MonoBehaviour
{
    public enemyType currentEnemyType = enemyType.MELEE;
    public bool invulnerable = false;
    public int health = 50;
    public int currentHealth;
    public bool doesContactDamage = true;
    public int contactDamageToPlayer = 10;
    public int contactDamageFromPlayer = 10;
    public float attackCooldown = 0.5f;
    public int scoreWorth = 100;

    Transform player;
    NavMeshAgent nav;

    bool canAttack = true;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();

        currentHealth = health;

        DoSpawnSequence();
    }

    void LateUpdate()
    {
        // Change logic depending on current enemy type.
        // NOTE: this may not be performant - AI is expensive especially with multiple agents, so might be worth creating different scripts for each enemy type?
        switch(currentEnemyType){
            case enemyType.MELEE:
                DoMeleeLogic();
                break;
            case enemyType.RANGED:
                break;
            case enemyType.INERT:
                break;
            default:
                break;
        }

        // Check if enemy's health drops below 0.
        if(currentHealth <= 0){
            Die();
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        string otherTag = other.transform.tag;
        if(otherTag == "Player" && doesContactDamage){
            playerManager pm = other.GetComponent<playerManager>();
            if(canAttack){
                canAttack = false;
                DoDamage(pm, contactDamageToPlayer);
                TakeDamage(contactDamageFromPlayer);
                StartCoroutine(startCooldown(attackCooldown));
            }
            
        }
    }

    void DoDamage(playerManager pm, int dmg){
        pm.TakeDamage(dmg);
    }

    public void TakeDamage(int dmg){
        // If not invulnerable, take damage.
        if(!invulnerable){
            currentHealth -= dmg;
        }
        // Play any hurt vfx / sounds.
    }

    void Die(){
        Destroy(gameObject);
    }

    void DoSpawnSequence(){
        // Play spawn VFX.
    }

    void DoDeathSequence(){
        // Play death VFX.
    }

    void DoMeleeLogic(){
        float dist = Vector3.Distance(player.position, transform.position);

        if(dist > 2f && dist <20f){
            nav.SetDestination(player.position);

            // Construct new target position ignoring the player's Y position, to prevent weird rotation.
            Vector3 targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z);

            // Look at the player,
            transform.LookAt(targetPostition);

            // Rotate by 90 degrees because of modelling issue. TODO: reimport with correct rotation & remove this.
            transform.Rotate(0, 90, 0);
        }
    }
    public enum enemyType{
        MELEE,
        RANGED,
        INERT
    }

    IEnumerator startCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        canAttack = true;
    }
}

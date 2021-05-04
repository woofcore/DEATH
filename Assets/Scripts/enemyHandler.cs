using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyHandler : MonoBehaviour
{
    public enemyType currentEnemyType = enemyType.MELEE;
    public enemyState currentEnemyState = enemyState.WANDER;
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
    
    public float attackCircleRadius = 5.0f;
    public float playerNoticeRadius = 20.0f;
    public float wanderRadius = 10.0f;

    bool canAttack = true;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();

        currentHealth = health;

        // Change logic depending on current enemy type.
        switch(currentEnemyType){
            case enemyType.MELEE:
                InvokeRepeating("DoMeleeLogic", 1.0f, 1.0f);
                break;
            case enemyType.RANGED:
                break;
            case enemyType.INERT:
                break;
            default:
                break;
        }

        DoSpawnSequence();
    }

    void LateUpdate()
    {
        

        // Check if this enemy's health drops below 0.
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
        // TODO: Add attack sounds, VFX and animations.
        pm.TakeDamage(dmg);
    }

    public void TakeDamage(int dmg){
        // If not invulnerable, take damage.
        if(!invulnerable){
            currentHealth -= dmg;
        }
        // TODO: Add damage sounds, VFX and animations.
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

    void DoChase(){
        nav.SetDestination(player.position);

            // Construct new target position ignoring the player's Y position, to prevent weird rotation.
            Vector3 targetPostition = new Vector3(player.position.x, this.transform.position.y, player.position.z);

            // Look at the player,
            transform.LookAt(targetPostition);

            // Rotate by 90 degrees because of modelling issue. TODO: reimport with correct rotation & remove this.
            transform.Rotate(0, 90, 0);
    }

    void DoWander(){

    }

    void DoMeleeLogic(){
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        // Check for player in radius.
        
        Collider[] sphereCheckColliders = Physics.OverlapSphere(transform.position, playerNoticeRadius);
        foreach (var collider in sphereCheckColliders)
        {
            if(collider.CompareTag("Player")){
                // While player is within enemy's notice radius, chase.
                currentEnemyState = enemyState.CHASE;
                DoChase();
            } else {
                // While player isn't within the enemy's notice radius, wander randomly.
                currentEnemyState = enemyState.WANDER;
                DoWander();
            }
        }
    }
    
    public enum enemyType{
        MELEE,
        RANGED,
        INERT
    }
    public enum enemyState{
        WANDER,
        CHASE,
        ATTACK
    }

    IEnumerator startCooldown(float t)
    {
        yield return new WaitForSeconds(t);
        canAttack = true;
        
    }
    
}

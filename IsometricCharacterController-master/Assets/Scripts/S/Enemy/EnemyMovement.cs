using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    
    Transform player;
    EnemyStats enemy;
    Vector2 knockbackVelocity;
    float knockbackDuration;
    // Start is called before the first frame update
    void Start()
    {
        enemy=GetComponent<EnemyStats>();
        player=FindObjectOfType<IsometricCharacterController>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackDuration>0){
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }
        // Normalize the movement direction to maintain constant speed
        else
        {
        transform.position = Vector3.MoveTowards(transform.position, player.position, enemy.currentMoveSpeed * Time.deltaTime);
        }
        // Move the enemy towards the player's position in 3D
    }
    public void Knockback(Vector3 velocity, float duration)
    {
        if (knockbackDuration > 0) return;

        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurianBehaviour : MeleeWeaponBehaviour
{
    List<GameObject>markedEnemies;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }
    protected override void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")&&!markedEnemies.Contains(other.gameObject)){
            EnemyStats enemy= other.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(), transform.position);
            markedEnemies.Add(other.gameObject);
        }
        //  else if(other.CompareTag("Prop")){
        //     if(other.gameObject.TryGetComponent(out BreakableProps breakable)&&!markedEnemies.Contains(other.gameObject)){
        //         breakable.TakeDamage(GetCurrentDamage());
        //         markedEnemies.Add(other.gameObject);
        //     }
        // }
    }
    // Update is called once per frame

}

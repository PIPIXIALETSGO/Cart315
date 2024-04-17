using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public float destroyAfterSeconds;
    // Start is called before the first frame update
       protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
     protected int currentPierce;

     void Awake(){
        currentDamage=weaponData.Damage;
        currentSpeed=weaponData.Speed;
        currentCooldownDuration=weaponData.CooldownDuration;
        currentPierce=weaponData.Pierce;
}
    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().CurrentMight;
    }
    protected virtual void Start()
    {
        Destroy(gameObject,destroyAfterSeconds);
    }
 protected virtual void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            EnemyStats enemy=other.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(),transform.position);
        }
        else if(other.CompareTag("Prop")){
            if(other.gameObject.TryGetComponent(out BreakableProps breakable)){
                breakable.TakeDamage(GetCurrentDamage());
            }
    }

    // Update is called once per frame
   
}
}

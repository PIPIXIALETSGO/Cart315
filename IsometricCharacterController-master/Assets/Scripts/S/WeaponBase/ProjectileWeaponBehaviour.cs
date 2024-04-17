using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    // Start is called before the first frame update
    protected Vector3 direction;
    public float destroyAfterSeconds;

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

    // Update is called once per frame
    protected virtual void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy")){
            EnemyStats enemy=other.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage(), transform.position);
            ReducePierce();
        }
        // else if(other.CompareTag("Prop")){
        //     if(other.gameObject.TryGetComponent(out BreakableProps breakable)){
        //         breakable.TakeDamage(currentDamage);
        //         ReducePierce();
        //     }
        // }
    }
    void ReducePierce(){
        currentPierce--;
        if(currentPierce<=0){
            Destroy(gameObject);
        }
    }
}

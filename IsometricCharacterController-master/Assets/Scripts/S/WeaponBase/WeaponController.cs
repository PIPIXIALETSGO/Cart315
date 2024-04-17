using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponData;
    float currentCooldown;
    float currentSpeed;
    protected IsometricCharacterController pm;
    // Start is called before the first frame update
    void Awake()
    {
       // currentSpeed = weaponData.Speed;
    }
   protected virtual void Start()
    {
        pm=FindObjectOfType<IsometricCharacterController>();
        currentCooldown=weaponData.CooldownDuration;

    }

    // Update is called once per frame
   protected virtual void Update()
    {
        currentCooldown-=Time.deltaTime;
        if(currentCooldown<=0f){
            Attack();
            
        }
    }
    protected virtual void Attack(){
        currentCooldown=weaponData.CooldownDuration;
    }
}

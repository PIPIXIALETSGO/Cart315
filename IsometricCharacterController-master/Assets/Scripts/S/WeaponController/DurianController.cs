using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurianController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
    protected override void Attack(){
        base.Attack();
        GameObject spawnedDurian = Instantiate(weaponData.Prefab);
        spawnedDurian.transform.position=transform.position;
        spawnedDurian.transform.parent=transform;

    }
}

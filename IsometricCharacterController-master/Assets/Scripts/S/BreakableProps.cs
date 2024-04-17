using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float health;
    public void TakeDamage(float dmg){
        health-=dmg;
        if(health<=0){
            Kill();
        }
    }
    // Start is called before the first frame update
    public void Kill(){
        Destroy(gameObject);
    }
}

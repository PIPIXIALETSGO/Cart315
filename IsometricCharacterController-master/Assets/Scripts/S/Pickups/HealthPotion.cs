using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Pickup
{
    // Start is called before the first frame update
    public int healthToRestore;
  public override void Collect()
    {
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
    PlayerStats player=FindObjectOfType<PlayerStats>();
    player.Resotorehealth(healthToRestore);
   
  }
}

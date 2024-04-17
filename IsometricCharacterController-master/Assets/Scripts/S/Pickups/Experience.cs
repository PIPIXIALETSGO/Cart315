using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : Pickup
{
    // Start is called before the first frame update
    public int experienceGranted;
    public override void Collect(){
        if (hasBeenCollected)
        {
            return;
        }
        else
        {
            base.Collect();
        }
        PlayerStats player=FindObjectOfType<PlayerStats>();
        player.IncreaseExperience(experienceGranted);
    }
   
   
}

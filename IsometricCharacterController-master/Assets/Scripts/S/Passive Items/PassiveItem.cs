using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    protected PlayerStats player;
    public PassiveItemscriptableObject passiveItemData;
    protected virtual void ApllyModifier()
    {

    }
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        ApllyModifier();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

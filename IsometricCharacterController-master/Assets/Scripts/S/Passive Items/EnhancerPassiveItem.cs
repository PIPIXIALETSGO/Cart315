using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnhancerPassiveItem : PassiveItem
{
   protected override void ApllyModifier()
    {
        player.CurrentMight*=1+passiveItemData.Multipler/100f;
    }
}

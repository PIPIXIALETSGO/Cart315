using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    PlayerStats player;
    CapsuleCollider playerCollector;
    public float pullSpeed;
    void Start(){
        player=FindObjectOfType<PlayerStats>();
        playerCollector=GetComponent<CapsuleCollider>();
    }
    void Update(){
        playerCollector.radius=player.CurrentMagnet;
    }
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ICollectible collectible))
        {
            Rigidbody rb=other.gameObject.GetComponent<Rigidbody>();
            Vector3 forceDirection=(transform.position-other.transform.position).normalized;
            rb.AddForce(forceDirection*pullSpeed);
            collectible.Collect();
        }
    }
}

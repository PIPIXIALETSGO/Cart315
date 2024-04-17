using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperController : WeaponController
{
   // Start is called before the first frame update
    public GameObject sniperPrefab;
    public float rotationSpeed = 5f;
    private GameObject sniperInstance;
    protected override void Start()
    {
        base.Start();
        if (sniperPrefab != null)
        {
            sniperInstance = Instantiate(sniperPrefab, transform);
        }
    }
    protected override void Update()
    {
        base.Update();

        // Call RotateTowardsEnemy in Update for continuous rotation
        RotateTowardsEnemy();
    }
    protected override void Attack(){
        base.Attack();

        // Determine the spawn position of the bullet
        Vector3 bulletSpawnPosition = transform.position;

        // Instantiate the banana prefab at the calculated spawn position
        GameObject spawnedBanana = Instantiate(weaponData.Prefab, bulletSpawnPosition, Quaternion.identity);

        // Get the Rigidbody component of the spawned banana
        Rigidbody bananaRigidbody = spawnedBanana.GetComponent<Rigidbody>();

        // Ensure the banana has a Rigidbody component
        if (bananaRigidbody != null)
        {
            // Apply force to the banana in the direction the player is facing
            bananaRigidbody.AddForce(transform.forward * weaponData.Speed, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("The spawned banana does not have a Rigidbody component.");
        }

    }
    private void RotateTowardsEnemy()
    {
        // Find the closest enemy within a certain range
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
            }
        }

        // Rotate towards the closest enemy
        if (closestEnemy != null)
        {
            Vector3 direction = closestEnemy.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Rotate the shotgun prefab to match the controller's rotation
            if (sniperInstance != null)
            {
                sniperInstance.transform.rotation = transform.rotation;
            }
        }
    }

}

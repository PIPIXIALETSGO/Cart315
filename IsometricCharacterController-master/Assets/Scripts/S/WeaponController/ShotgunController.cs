using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunController : WeaponController
{
    public GameObject shotgunPrefab;
    public float rotationSpeed = 5f;
    private GameObject shotgunInstance;
    protected override void Start()
    {
        base.Start();
        if (shotgunPrefab != null)
        {
            shotgunInstance = Instantiate(shotgunPrefab, transform);
        }
    }

    protected override void Update()
    {
        base.Update();

        // Call RotateTowardsEnemy in Update for continuous rotation
        RotateTowardsEnemy();
    }
    protected override void Attack()
    {
        base.Attack();
        // Determine the spawn position of the bullet

        int numBullets = weaponData.NumberOfBullet; // Number of bullets to spawn
        float spreadAngle = weaponData.SpreadAngle; ; // Angle of spread in degrees

        // Calculate the fixed Z direction
        Vector3 fixedDirection = transform.forward;

        // Get the position of the bullet spawn point
        Vector3 bulletSpawnPosition = transform.position;

        // Instantiate multiple bullet prefabs with randomized directions
        for (int i = 0; i < numBullets; i++)
        {
            // Calculate a random direction within the spread angle
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(-spreadAngle, spreadAngle), 0f);
            Vector3 bulletDirection = randomRotation * transform.forward;

            // Instantiate the bullet prefab at the calculated spawn position
            GameObject spawnedBullet = Instantiate(weaponData.Prefab, bulletSpawnPosition, Quaternion.identity);

            // Get the Rigidbody component of the spawned bullet
            Rigidbody bulletRigidbody = spawnedBullet.GetComponent<Rigidbody>();

            // Ensure the bullet has a Rigidbody component
            if (bulletRigidbody != null)
            {
                // Apply force to the bullet in the randomized direction
                bulletRigidbody.AddForce(bulletDirection * weaponData.Speed, ForceMode.Impulse);
            }
            else
            {
                Debug.LogError("The spawned bullet does not have a Rigidbody component.");
            }
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
            if (shotgunInstance != null)
            {
                shotgunInstance.transform.rotation = transform.rotation;
            }
        }
    }
}

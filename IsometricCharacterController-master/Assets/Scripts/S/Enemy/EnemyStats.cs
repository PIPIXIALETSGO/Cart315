using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    public float despawnDistance=20f;
    Transform player;
    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1);
    public float damageFlashDuration = 0.2f;
    public float deathFadetime = 0.6f;
    Color originalColor;
    Renderer meshRenderer;
    EnemyMovement movement;
    // Start is called before the first frame update
    void Awake()
    {
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }
    void Start(){
        player=FindObjectOfType<PlayerStats>().transform;
        meshRenderer = GetComponent<Renderer>(); 
        originalColor = meshRenderer.material.color; 
        movement = GetComponent<EnemyMovement>();
    }
    void Update(){
       
    }
    public void TakeDamage(float dmg ,Vector3 sourcePosition, float knockbackForce = 5f,float knockbackDuration=0.2f)
    {
        currentHealth -= dmg;
        StartCoroutine(DamageFlash());
        if (dmg > 0)
        {
            //GameManager.GenerateFloatingText(Mathf.FloorToInt(dmg).ToString(), transform);
        }
        if (knockbackForce > 0)
        {
            Vector3 dir = transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackForce, knockbackDuration);
        }
        if (currentHealth <= 0)
        {
            Kill();
        }
    }
    IEnumerator DamageFlash()
    {
        meshRenderer.material.color = damageColor; // Change material.color instead of SpriteRenderer.color
        yield return new WaitForSeconds(damageFlashDuration);
        meshRenderer.material.color = originalColor; // Revert to original material color
    }
    public void Kill()
    {
        StartCoroutine(KillFade());
    }
    IEnumerator KillFade()
    {
        float t = 0;
        Color startColor = meshRenderer.material.color; // Store the start color
        while (t < deathFadetime)
        {
            t += Time.deltaTime;
            meshRenderer.material.color = Color.Lerp(startColor, Color.clear, t / deathFadetime); // Fade out to transparent
            yield return null;
        }
        Destroy(gameObject);
    }
     private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")){
            PlayerStats player=other.gameObject.GetComponent<PlayerStats>();
            player.TakeDamage(currentDamage);
            
        }
    }
    private void OnDestroy() {
        //EnemySpawner es=FindObjectOfType<EnemySpawner>();
        //es.onEnemyKilled();
    }
   
}

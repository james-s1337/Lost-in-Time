using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private string targetTag = "Player";
    private Core core;
    private GameObject target;

    private float movementSpeed;
    private float enemyBoundaryForce = 3f;
    private int currentHealth;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();

        movementSpeed = enemyData.speed;
        currentHealth = enemyData.health;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
    }

    private void FixedUpdate()
    {
        Chase();
        core.LogicUpdate();
    }

    void Chase()
    {
        if (core && target)
        {
            // Move towards player using movement core
            Vector2 angle = (target.transform.position - transform.position);
            int direction = 1;
            if (angle.x < 0)
            {
                direction = -1;
            }

            core.Movement.SetVelocity(movementSpeed, angle);
            core.Movement.CheckIfShouldFlip(direction);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }  
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}

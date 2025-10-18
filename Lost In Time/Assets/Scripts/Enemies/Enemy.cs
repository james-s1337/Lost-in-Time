using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private string targetTag = "Player";

    public Core core { get; private set; }
    private BoxCollider2D boxCollider;

    private GameObject target;
    private Player player;

    private float movementSpeed;
    private int currentHealth;

    private void Awake()
    {
        core = GetComponentInChildren<Core>();
        boxCollider = GetComponent<BoxCollider2D>();

        movementSpeed = enemyData.speed;
        currentHealth = enemyData.health;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
        if (target)
        {
            player = target.GetComponent<Player>();
        }
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            return;
        }

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
            core.Movement.SetVelocityZero();
            StartCoroutine(Die());
        }  
    }

    private IEnumerator Die()
    {
        // Give player drops
        if (player)
        {

        }

        // Die effects
        boxCollider.enabled = false;

        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            player.core.Movement.AddForce(-(transform.position - player.gameObject.transform.position), enemyData.knockback);
            // DO Damage to player
            IDamageable playerDamageable = player.GetComponent<IDamageable>();
            if (playerDamageable == null)
            {
                return;
            }

            playerDamageable.TakeDamage(enemyData.damage);
        }
    }
}

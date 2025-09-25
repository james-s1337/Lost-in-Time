using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float travelSpeed;
    [SerializeField] protected float travelTime;
    protected int damage = 1;
    // Include audio for hit sound
    // Include audio for shoot sound
    private void Start()
    {
        // Play audio
    }

    private void OnEnable()
    {
        Invoke(nameof(DisableBullet), travelTime);
    }

    private void DisableBullet()
    {
        transform.rotation = Quaternion.identity;
        gameObject.SetActive(false);
    }
    protected virtual void Update()
    {
        transform.Translate(new Vector2(travelSpeed * Time.deltaTime, 0f));
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            return;
        }
    }
}

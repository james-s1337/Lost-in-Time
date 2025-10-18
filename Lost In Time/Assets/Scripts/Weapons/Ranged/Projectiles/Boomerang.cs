using UnityEngine;

public class Boomerang : Projectile
{
    public bool isReturning { get; private set; }
    public AnimationCurve easeOutCurve;
    public AnimationCurve easeInCurve;
    private int maxTargets = 1; // how many enemies can be hit through one run at start
    private float travelDistance = 8f;
    private int targetsHit;
    // travel time is the lerp time
    private Vector2 startPos;
    private Vector2 targetPos;
    private Player player;
    private float lerpTime;
    protected override void OnEnable()
    {
        isReturning = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lerpTime = 0;
        targetsHit = 0;
        maxTargets = piercing;
        travelTime = travelDistance / travelSpeed;

        startPos = transform.position;
        targetPos = startPos + (Vector2)(travelDirection.normalized * travelDistance);
    }

    protected override void Update()
    {
        if (!isReturning)
        {
            lerpTime += Time.deltaTime / travelTime;
            float t = Mathf.Clamp01(lerpTime);

            // apply easing
            float easedT = easeOutCurve.Evaluate(t);
            transform.position = Vector2.Lerp(startPos, targetPos, easedT);

            if (t >= 1f) // finished outward path
            {
                ResetLerpForReturn();
            }
        }
        else // returning
        {
            lerpTime += Time.deltaTime / travelTime;
            float t = Mathf.Clamp01(lerpTime);

            float easedT = easeInCurve.Evaluate(t);
            transform.position = Vector2.Lerp(startPos, player.transform.position, easedT);

            Vector2 toPlayer = (Vector2)player.transform.position - (Vector2)transform.position;
            float angleRad = Mathf.Atan2(toPlayer.y, toPlayer.x);
            float angleDeg = angleRad * Mathf.Rad2Deg;
            SetFacingAngle(angleDeg);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (isReturning)
        {
            Rang rang = collision.GetComponent<Rang>();

            if (rang)
            {
                rang.SetCanFireTrue();
                DisableBullet();
                return;
            }
        }
        
        if (collision.tag == "Enemy")
        {
            targetsHit += 1;

            ApplyKnockback(collision.GetComponent<Enemy>());
            DamageEnemy(collision.GetComponent<IDamageable>());

            if (targetsHit >= maxTargets && !isReturning)
            {
                ResetLerpForReturn();
            }   
        }
    }

    protected override void DamageEnemy(IDamageable enemyDamageable)
    {
        if (enemyDamageable == null)
        {
            return;
        }

        enemyDamageable.TakeDamage(damage);
    }

    private void ResetLerpForReturn()
    {
        isReturning = true;
        lerpTime = 0; // reset for return trip
        startPos = transform.position; // reset starting pos for return
    }

    public void SetTravelDistance(float travelDistance)
    {
        this.travelDistance = travelDistance;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isReturning)
        {
            Rang rang = collision.GetComponent<Rang>();

            if (rang)
            {
                rang.SetCanFireTrue();
                DisableBullet();
                return;
            }
        }
    }
}

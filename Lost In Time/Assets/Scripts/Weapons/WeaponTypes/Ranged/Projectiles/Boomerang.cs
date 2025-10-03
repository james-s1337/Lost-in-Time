using UnityEngine;

public class Boomerang : Projectile
{
    public bool isReturning { get; private set; }
    [SerializeField] float travelDistance;
    public AnimationCurve easeOutCurve;
    public AnimationCurve easeInCurve;
    public int maxTargets = 1; // how many enemies can be hit through one run at start
    private int targetsHit;
    // travel time is the lerp time
    private Vector2 travelPosition;
    private Vector2 startPos;
    private Player player;
    private float lerpTime;
    protected override void OnEnable()
    {
        isReturning = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lerpTime = 0;
        targetsHit = 0;
        travelPosition = new Vector2(transform.position.x + (travelDistance * player.core.Movement.facingDir), transform.position.y);
        startPos = transform.position;
    }

    protected override void Update()
    {
        if (!isReturning)
        {
            lerpTime += Time.deltaTime / travelTime;
            float t = Mathf.Clamp01(lerpTime);

            // apply easing
            float easedT = easeOutCurve.Evaluate(t) * travelSpeed;
            transform.position = Vector2.Lerp(startPos, travelPosition, easedT);

            if (t >= travelTime) // finished outward path
            {
                isReturning = true;
                lerpTime = 0; // reset for return trip
                startPos = transform.position; // reset starting pos for return
            }
        }
        else // returning
        {
            lerpTime += Time.deltaTime / travelTime;
            float t = Mathf.Clamp01(lerpTime);

            float easedT = easeInCurve.Evaluate(t) * travelSpeed;
            transform.position = Vector2.Lerp(startPos, player.transform.position, easedT);

        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
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
        
        if (collision.tag == "Enemy")
        {
            targetsHit += 1;
            if (targetsHit >= maxTargets)
            {
                isReturning = true;
            }   
        }
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

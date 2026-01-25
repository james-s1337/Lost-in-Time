using UnityEngine;

// Put these scripts in empty gameobjects under the weapon objects
public class WeaponSkill : MonoBehaviour
{
    public float duration; // Time it will take before the skill ends and normal player function is restored
    public float cooldownTime;

    protected float timeSinceLastUse;
    protected bool isActive;

    protected WeaponType weapon;

    public virtual void Start()
    {
        timeSinceLastUse = 0f;
        weapon = GetComponentInParent<WeaponType>();
    }

    public virtual void Activate()
    {
        if (timeSinceLastUse != 0f && Time.time >= timeSinceLastUse + cooldownTime)
        {
            return;
        }

        timeSinceLastUse = Time.time;
        isActive = true;
        // Update UI
        // Do the skill
    }

    public virtual void Update()
    {
        if (!isActive)
        {
            return;
        }

        if (Time.time >= timeSinceLastUse + duration)
        {
            isActive = false;
        }
    }
}

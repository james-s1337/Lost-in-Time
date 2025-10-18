using UnityEngine;

public interface IOnHitEffect
{
    void ApplyEffects(GameObject hit, Vector2 startPos, Vector2 endPos, int facingDirTotal, WeaponStats stats);
}

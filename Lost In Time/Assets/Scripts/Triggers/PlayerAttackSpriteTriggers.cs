using UnityEngine;

public class PlayerAttackSpriteTriggers : MonoBehaviour
{
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    public void AnimationFinishTrigger()
    {
        player.AnimationFinishTrigger();
    }
}

using UnityEngine;

public class PowerCell : Interactable
{
    Player player;
    protected override void Activate()
    {
        base.Activate();

        if (!player)
        {
            return;
        }

        player.ChangeToRandomWeapon();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player = collision.GetComponent<Player>();
            Activate();
        }
    }
}

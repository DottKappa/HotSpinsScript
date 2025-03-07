using UnityEngine;

public class DespawnTrigger : MonoBehaviour
{
    public RespawnTrigger respawnTrigger;

    void OnTriggerEnter2D(Collider2D slotCell)
    {
        float xPosition = slotCell.transform.position.x;
        Destroy(slotCell.gameObject);
        if (respawnTrigger != null && slotCell.CompareTag("SlotCell")) {
            respawnTrigger.RespawnAtX(xPosition);
        }
    }
}

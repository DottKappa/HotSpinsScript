using UnityEngine;

public class DespawnTrigger : MonoBehaviour
{
    public RespawnTrigger respawnTrigger;

    void OnTriggerEnter2D(Collider2D slotCell)
    {
        float xPosition = slotCell.transform.position.x;
        if (respawnTrigger != null && slotCell.tag.Contains("_SlotCell")) {
            Destroy(slotCell.gameObject);
            respawnTrigger.RespawnAtX(xPosition);
        }
    }
}

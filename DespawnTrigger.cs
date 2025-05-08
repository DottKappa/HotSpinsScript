using UnityEngine;
using System.Collections;

public class DespawnTrigger : MonoBehaviour
{
    public RespawnTrigger respawnTrigger;
    private SceneManager sceneManager;

    void Start()
    {
        sceneManager = Object.FindFirstObjectByType<SceneManager>();
    }

    void OnTriggerEnter2D(Collider2D slotCell)
    {
        float xPosition = slotCell.transform.position.x;
        if (respawnTrigger != null && slotCell.tag.Contains("_SlotCell")) {
            var controller = slotCell.GetComponent<SlotController>();
            if (!controller.GetMoving()) return;

            StartCoroutine(DelayedRespawn(slotCell.gameObject, xPosition));
        }
    }

    private IEnumerator DelayedRespawn(GameObject slotCell, float xPosition)
    {
        sceneManager.SetBusy(true);
        Destroy(slotCell);
        yield return null; // aspetta il prossimo frame
        respawnTrigger.RespawnAtX(xPosition);
        sceneManager.SetBusy(false);
    }
}

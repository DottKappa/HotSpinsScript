using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject prefabBuff;
    public GameObject prefabDebuff;
    public int numberOfSparks = 0;
    private CanvasController canvasController;

    private void Start()
    {
        canvasController = FindFirstObjectByType<CanvasController>();
    }

    public void addSpark(int numberOfSparksInSlot)
    {
        Debug.Log("Spark picked up! " + numberOfSparksInSlot);
        numberOfSparks += numberOfSparksInSlot;
        
        canvasController.ToggleCanvasElements(false);
        spawnBuff();
        spawnDebuff();
    }

    public void spawnBuff()
    {
        GameObject buff = Instantiate(prefabBuff);
        buff.transform.position = buff.transform.position;
        buff.GetComponent<PowerUp>().SetKeyToPickUp("q");
    }

    public void spawnDebuff()
    {
        GameObject debuff = Instantiate(prefabDebuff);
        debuff.transform.position = debuff.transform.position;
        debuff.GetComponent<PowerUp>().SetKeyToPickUp("e");
    }
}

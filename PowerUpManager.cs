using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public GameObject prefabBuff;
    public GameObject prefabDebuff;
    public int numberOfSparks = 0;
    private bool buffOnOdd = true;
    private CanvasController canvasController;
    private SceneManager sceneManager;
    private BuffDebuffManager buffDebuffManager;
    private PointSystemController pointSystemController;
    private RespawnTrigger respawnTrigger;

    private void Start()
    {
        canvasController = FindFirstObjectByType<CanvasController>();
        sceneManager = FindFirstObjectByType<SceneManager>();
        buffDebuffManager = FindFirstObjectByType<BuffDebuffManager>();
        pointSystemController = FindFirstObjectByType<PointSystemController>();
        respawnTrigger = FindFirstObjectByType<RespawnTrigger>();
    }

    public void addSpark(int numberOfSparksInSlot)
    {
        Debug.Log("Spark picked up! " + numberOfSparksInSlot);
        numberOfSparks += numberOfSparksInSlot;
        
        canvasController.ToggleCanvasElements(false);

        if (sceneManager.GetNumberOfSpins() % 2 == 0 || buffOnOdd) {
            spawnBuff("q");
            spawnBuff("e", false);
            buffOnOdd = false;
        } else {
            spawnDebuff("q");
            spawnDebuff("e", false);
            buffOnOdd = true;
        }        
    }

    public void spawnBuff(string keyToPickUp, bool isFirstCard = true)
    {
        string[] powerUp = buffDebuffManager.GetRandomPowerUp(true);

        GameObject buff = Instantiate(prefabBuff);
        buff.GetComponent<PowerUp>().isFirstCard = isFirstCard;
        buff.GetComponent<PowerUp>().SetKeyToPickUp(keyToPickUp);
        buff.GetComponent<PowerUp>().SetText(powerUp[0], powerUp[1]);
        buff.GetComponent<PowerUp>().SetWhoAmI(powerUp[0]);
    }

    public void spawnDebuff(string keyToPickUp, bool isFirstCard = true)
    {
        string[] powerUp = buffDebuffManager.GetRandomPowerUp(false);

        GameObject debuff = Instantiate(prefabDebuff);
        debuff.GetComponent<PowerUp>().isFirstCard = isFirstCard;
        debuff.GetComponent<PowerUp>().SetKeyToPickUp(keyToPickUp);
        debuff.GetComponent<PowerUp>().SetText(powerUp[0], powerUp[1]);
        debuff.GetComponent<PowerUp>().SetWhoAmI(powerUp[0]);
    }

    public void ManagePowerUp(string enumPowerUp)
    {
        if (System.Enum.TryParse(enumPowerUp, out BuffType powerUpBuff)) {
            Debug.Log("Parsed BuffType: " + powerUpBuff);
            switch (powerUpBuff) {
                case BuffType.SlowDown:
                    respawnTrigger.ManipulateSpeed(13.0f, 3);
                    break;
                case BuffType.DoubleScore:
                    pointSystemController.MultipliePoints(2);
                    break;
                case BuffType.Next3TripleScore:
                    sceneManager.ManipulateMultiplierBySpins(3f, 3);
                    break;
                case BuffType.Every5DoubleScore:
                    // Aggiungi la logica per Every3DoubleScore
                    break;
                case BuffType.AddMoreSparks:
                    // Aggiungi la logica per AddMoreSparks
                    break;
                case BuffType.AddMoreBasicCells:
                    // Aggiungi la logica per AddMoreBasicCells
                    break;
                case BuffType.AddMoreMultiplierCells:
                    // Aggiungi la logica per AddMoreMultiplierCells
                    break;
                case BuffType.ResetBuffSpawn:
                    // Aggiungi la logica per ResetBuffSpawn
                    break;
                case BuffType.Nothing:
                    // Aggiungi la logica per Nothing
                    break;
                default:
                    Debug.LogWarning("Unhandled power-up buff type: " + powerUpBuff);
                    break;
            }
        } else if (System.Enum.TryParse(enumPowerUp, out DebuffType powerUpDebuff)) {
            Debug.Log("Parsed DebuffType: " + powerUpDebuff);
            switch (powerUpDebuff) {
                case DebuffType.SpeedUp:
                    respawnTrigger.ManipulateSpeed(33.0f, 3);
                    break;
                case DebuffType.HalfScore:
                    pointSystemController.DividePoints(2);
                    break;
                case DebuffType.Next5HalfScore:
                    sceneManager.ManipulateMultiplierBySpins(0.5f, 5);
                    break;
                case DebuffType.Every10HalfScore:
                    // Aggiungi la logica per Every10HalfScore
                    break;
                case DebuffType.RemoveSparks:
                    // Aggiungi la logica per RemoveSparks
                    break;
                case DebuffType.RemoveMultiplierCells:
                    // Aggiungi la logica per RemoveMultiplierCells
                    break;
                case DebuffType.ResetDebuffSpawn:
                    // Aggiungi la logica per ResetDebuffSpawn
                    break;
                case DebuffType.Nothing:
                    // Aggiungi la logica per Nothing
                    break;
                default:
                    Debug.LogWarning("Unhandled power-up debuff type: " + powerUpDebuff);
                    break;
            }
        }
    }
}

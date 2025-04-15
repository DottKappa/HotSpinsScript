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
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.DoubleScore:
                    pointSystemController.MultipliePoints(2);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.Next3TripleScore:
                    sceneManager.ManipulateMultiplierBySpins(3f, 3);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.Every5DoubleScore:
                    pointSystemController.SetNumberOfSpinToBuff(5);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.AddMoreSparks:
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("powerup"), 1f);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.AddMoreBasicCells:
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("cherry"), 10f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("melon"), 10f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("strawberry"), 10f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("banana"), 10f);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.AddMoreSpecialCells:
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("sun"), 10f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("moon"), 10f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("seven"), 10f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("special"), 10f);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.ResetBuffSpawn:
                    buffDebuffManager.ResetBuffDictionary();
                    buffDebuffManager.SetPowerUpUsed(BuffType.Every5DoubleScore.ToString(), true);
                    break;
                case BuffType.Nothing:
                    buffDebuffManager.SetPowerUpUsed(BuffType.Nothing.ToString(), true);
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
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                case DebuffType.HalfScore:
                    pointSystemController.DividePoints(2);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                case DebuffType.Next5HalfScore:
                    sceneManager.ManipulateMultiplierBySpins(0.5f, 5);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                case DebuffType.Every11HalfScore:
                    pointSystemController.SetNumberOfSpinToDebuff(11);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                case DebuffType.RemoveSparks:
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("powerup"), 10f, false);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                case DebuffType.RemoveSpecialCells:
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("sun"), 10f, false);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("moon"), 10f, false);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("seven"), 10f, false);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("special"), 10f, false);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                case DebuffType.ResetDebuffSpawn:
                    buffDebuffManager.ResetDebuffDictionary();
                    buffDebuffManager.SetPowerUpUsed(DebuffType.Every11HalfScore.ToString(), false);
                    break;
                case DebuffType.Nothing:
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                default:
                    Debug.LogWarning("Unhandled power-up debuff type: " + powerUpDebuff);
                    break;
            }
        }
    }
}

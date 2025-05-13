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
    [Header("SpecialArrow")]
    public GameObject horizontalUp;
    public GameObject horizontalDown;
    public GameObject verticalLeft;
    public GameObject verticalRight;


    private void Awake()
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
                    respawnTrigger.ManipulateSpeed(15.0f, 3);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.DoubleScore:
                    int actualPoints = pointSystemController.GetPoints();
                    float multiplier = CalculateMultiplier(actualPoints, true);
                    pointSystemController.MultipliePoints(multiplier);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.Every5DoubleScore:
                    pointSystemController.SetNumberOfSpinToBuff(5);
                    canvasController.SetHasMultiplierBuff(true);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.AddMoreSparks:
                    respawnTrigger.ResetWeights();
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("powerup"), 2f);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    buffDebuffManager.SetPowerUpUnused(BuffType.AddMoreBasicCells.ToString(), true);
                    buffDebuffManager.SetPowerUpUnused(BuffType.AddMoreSpecialCells.ToString(), true);
                    break;
                case BuffType.AddMoreBasicCells:
                    respawnTrigger.ResetWeights();
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("cherry"), 20f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("melon"), 20f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("strawberry"), 20f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("banana"), 20f);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);                    
                    buffDebuffManager.SetPowerUpUnused(BuffType.AddMoreSparks.ToString(), true);
                    buffDebuffManager.SetPowerUpUnused(BuffType.AddMoreSpecialCells.ToString(), true);
                    break;
                case BuffType.AddMoreSpecialCells:
                    respawnTrigger.ResetWeights();
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("sun"), 10f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("moon"), 10f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("seven"), 10f);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("special"), 10f);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);                    
                    buffDebuffManager.SetPowerUpUnused(BuffType.AddMoreSparks.ToString(), true);
                    buffDebuffManager.SetPowerUpUnused(BuffType.AddMoreBasicCells.ToString(), true);
                    break;
                case BuffType.Nothing:
                    buffDebuffManager.SetPowerUpUsed(BuffType.Nothing.ToString(), true);
                    break;
                case BuffType.CreateHorizontalUp:
                    pointSystemController.SetHorizontalUp(true);
                    horizontalUp.SetActive(true);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.CreateHorizontalDown:
                    pointSystemController.SetHorizontalDown(true);
                    horizontalDown.SetActive(true);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.CreateVerticalLeft:
                    pointSystemController.SetVerticalLeft(true); //
                    verticalLeft.SetActive(true);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                case BuffType.CreateVerticalRight:
                    pointSystemController.SetVerticalRight(true); //
                    verticalRight.SetActive(true);
                    buffDebuffManager.SetPowerUpUsed(powerUpBuff.ToString(), true);
                    break;
                default:
                    Debug.LogWarning("Unhandled power-up buff type: " + powerUpBuff);
                    break;
            }
        } else if (System.Enum.TryParse(enumPowerUp, out DebuffType powerUpDebuff)) {
            Debug.Log("Parsed DebuffType: " + powerUpDebuff);
            switch (powerUpDebuff) {
                case DebuffType.SpeedUp:
                    respawnTrigger.ManipulateSpeed(31.0f, 3);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                case DebuffType.HalfScore:
                    int actualPoints = pointSystemController.GetPoints();
                    float multiplier = CalculateMultiplier(actualPoints, false);
                    pointSystemController.DividePoints(multiplier);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                case DebuffType.Every11HalfScore:
                    pointSystemController.SetNumberOfSpinToDebuff(11);
                    canvasController.SetHasDivideDebuff(true);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    break;
                case DebuffType.RemoveSparks:
                    respawnTrigger.ResetWeights();
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("powerup"), 0.3f, false);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    buffDebuffManager.SetPowerUpUnused(DebuffType.RemoveSpecialCells.ToString(), false);
                    break;
                case DebuffType.RemoveSpecialCells:
                    respawnTrigger.ResetWeights();
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("sun"), 6f, false);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("moon"), 6f, false);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("seven"), 3.8f, false);
                    respawnTrigger.ManipulateWeights(respawnTrigger.GetIndexOfWeightsByTag("special"), 3.7f, false);
                    buffDebuffManager.SetPowerUpUsed(powerUpDebuff.ToString(), false);
                    buffDebuffManager.SetPowerUpUnused(DebuffType.RemoveSparks.ToString(), false);
                    break;
                case DebuffType.ResetDebuffSpawn:
                    buffDebuffManager.ResetDebuffDictionary();
                    buffDebuffManager.SetPowerUpUsed(DebuffType.Every11HalfScore.ToString(), false);
                    break;
                default:
                    Debug.LogWarning("Unhandled power-up debuff type: " + powerUpDebuff);
                    break;
            }
        }
    }

    private float CalculateMultiplier(int punti, bool invertito = false)
    {
        if (punti < 10000)
            return invertito ? 2.0f : 1.1f;

        int step = (punti - 10000) / 10000;
        float moltiplicatore;

        if (invertito) {
            moltiplicatore = 2.0f - (step * 0.1f);
            moltiplicatore = Mathf.Max(moltiplicatore, 1.1f);
        } else {
            moltiplicatore = 1.1f + (step * 0.1f);
            moltiplicatore = Mathf.Min(moltiplicatore, 2.0f);
        }

        return moltiplicatore;
    }
}

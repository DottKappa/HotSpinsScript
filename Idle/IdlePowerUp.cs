using UnityEngine;

public class IdlePowerUp : MonoBehaviour
{
    private IdleFileManager idleFileManager;
    private PointSystemIdleController pointSystemIdleController;
    private HoverTooltipSmartPositioning hoverTooltipSmartPositioning;

    void Start()
    {
        idleFileManager = FindFirstObjectByType<IdleFileManager>();
        pointSystemIdleController = FindFirstObjectByType<PointSystemIdleController>();
        hoverTooltipSmartPositioning = GetComponent<HoverTooltipSmartPositioning>();
    }

    public void IdlePowerUpButton()
    {
        int availability = hoverTooltipSmartPositioning.GetAvailability();
        if (availability == 1) {
            Destroy(gameObject);
        } else {
            hoverTooltipSmartPositioning.UpdateAvailability(-1);
        }

        idleFileManager.UpdateOrCreatePowerUp(gameObject.name, availability-1);
        UsePowerUp(gameObject.name);
        idleFileManager.SaveIdleFile();
    }

    private void UsePowerUp(string powerUpName)
    {
        switch(IdleStatic.GetRarityByPowerUpName(powerUpName)) {
            case "common": {UseCommon(powerUpName); break;}
            case "rare": {UseRare(powerUpName); break;}
            case "mythic": {UseMythic(powerUpName); break;}
        }
    }

    private void UseCommon(string powerUpName)
    {
        switch(powerUpName) {
            case "3x_on_horizontal": {
                pointSystemIdleController.SetNumberOfHorizontal(3);
                pointSystemIdleController.SetHorizontalMultiplier(3);
                break;
            }
        }
    }

    private void UseRare(string powerUpName)
    {
        switch(powerUpName) {
            case "5x_on_horizontal": {
                pointSystemIdleController.SetNumberOfHorizontal(2);
                pointSystemIdleController.SetHorizontalMultiplier(5);
                break;
            }
        }
    }

    private void UseMythic(string powerUpName)
    {
        switch(powerUpName) {
            case "10x_on_horizontal": {
                pointSystemIdleController.SetNumberOfHorizontal(1);
                pointSystemIdleController.SetHorizontalMultiplier(10);
                break;
            }
        }
    }
}

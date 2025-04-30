using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public GameObject thunderPrefab;
    private Vector3 spawnPosition = new Vector3(3f, 2f, 0f);
    private Quaternion spawnRotation = Quaternion.Euler(0f, 0f, -90f);

    public void PlayThunder(string rotation = "H")
    {
        GameObject thunderInstance = Instantiate(thunderPrefab, spawnPosition, spawnRotation);

        if (rotation == "H") {
            SetHorizontal(thunderInstance);
        } else if (rotation == "HU") {
            SetHorizontalUp(thunderInstance);
        } else if (rotation == "HD") {
            SetHorizontalDown(thunderInstance);
        } else if (rotation == "U") {
            SetUpDown(thunderInstance);
        } else if (rotation == "D") {
            SetDownUp(thunderInstance);
        }

        Destroy(thunderInstance, 1f); // Distrugge il prefab dopo 1 secondo
    }

    private void SetHorizontal(GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    private void SetHorizontalUp(GameObject gameObject)
    {
        SetHorizontal(gameObject);
        gameObject.transform.position += new Vector3(0, 1.5f, 0);
    }
    
    private void SetHorizontalDown(GameObject gameObject)
    {
        SetHorizontal(gameObject);
        gameObject.transform.position += new Vector3(0, -1.65f, 0);
    }

    private void SetUpDown(GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 65);
    }

    private void SetDownUp(GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 115);
    }
}
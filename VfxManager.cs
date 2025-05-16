using UnityEngine;

public class VfxManager : MonoBehaviour
{
    public GameObject[] vfxPrefab;
    public GameObject coinEmitter;
    private Vector3 spawnPosition = new Vector3(3f, 2f, 0f);
    private Quaternion spawnRotation = Quaternion.Euler(0f, 0f, -90f);
    private CameraSlot cameraSlot;

    void Start()
    {
        cameraSlot = FindFirstObjectByType<CameraSlot>();
    }

    public void PlayVfx(string rotation = "H")
    {
        GameObject selectedPrefab = vfxPrefab[Random.Range(0, vfxPrefab.Length)];
        GameObject vfxInstance = Instantiate(selectedPrefab);

        if (rotation == "H")
        {
            SetHorizontal(vfxInstance);
        }
        else if (rotation == "HU")
        {
            SetHorizontalUp(vfxInstance);
        }
        else if (rotation == "HD")
        {
            SetHorizontalDown(vfxInstance);
        }
        else if (rotation == "U")
        {
            SetUpDown(vfxInstance);
        }
        else if (rotation == "D")
        {
            SetDownUp(vfxInstance);
        }
        else if (rotation == "VL")
        {
            SetVerticalLeft(vfxInstance);
        }
        else if (rotation == "VR")
        {
            SetVerticalRight(vfxInstance);
        }

        PlayAudioByVfxName(vfxInstance.name);
        Destroy(vfxInstance, 1f); // Distrugge il prefab dopo 1 secondo
    }

    public void PlayCoinEmitter(int power = 1)
    {
        GameObject coinInstance = Instantiate(coinEmitter);
        CoinRainController coinScript = coinInstance.GetComponent<CoinRainController>();
        coinScript.PlayCoinRain(power);
        PlayAudioByVfxName(coinInstance.name);
    }

    private void SetHorizontal(GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        if (gameObject.name.Contains("LaserBeam"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            gameObject.transform.localScale = new Vector3(0.5f, 0.45f, 0.45f);
        }
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

        if (gameObject.name.Contains("Thunder"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 65);
        }
        else if (gameObject.name.Contains("Explosion"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -115);
        }
        else if (gameObject.name.Contains("LaserBeam"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -26);
        }
    }

    private void SetDownUp(GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 115);

        if (gameObject.name.Contains("Thunder"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 115);
        }
        else if (gameObject.name.Contains("Explosion"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, -60);
        }
        else if (gameObject.name.Contains("LaserBeam"))
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 26);
        }
    }

    private void SetVerticalLeft(GameObject gameObject)
    {
        if (gameObject.name.Contains("LaserBeam"))
        {
            gameObject.transform.position = new Vector3(0f, 1.9f, 0f);
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            gameObject.transform.localScale = new Vector3(0.3f, 0.36f, 0.733f);
        }
        else if (gameObject.name.Contains("Explosion"))
        {
            gameObject.transform.position = new Vector3(0f, 1.5f, 0f);
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (gameObject.name.Contains("Thunder"))
        {
            gameObject.transform.position = new Vector3(0f, 1.5f, 0f);
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            gameObject.transform.localScale = new Vector3(0.22f, 0.36f, 0.733f);
        }
    }

    private void SetVerticalRight(GameObject gameObject)
    {
        if (gameObject.name.Contains("LaserBeam"))
        {
            gameObject.transform.position = new Vector3(5.8f, 1.9f, 0f);
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            gameObject.transform.localScale = new Vector3(0.3f, 0.36f, 0.733f);
        }
        else if (gameObject.name.Contains("Explosion"))
        {
            gameObject.transform.position = new Vector3(6f, 1.5f, 0f);
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (gameObject.name.Contains("Thunder"))
        {
            gameObject.transform.position = new Vector3(6f, 1.5f, 0f);
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            gameObject.transform.localScale = new Vector3(0.22f, 0.36f, 0.733f);
        }
    }

    private void PlayAudioByVfxName(string name)
    {
        if (name.Contains("Explosion"))
        {
            cameraSlot.StartVfxExplosion();
        }
        else if (name.Contains("LaserBeam"))
        {
            cameraSlot.StartVfxLaserBeam();
        }
        else if (name.Contains("Thunder"))
        {
            cameraSlot.StartVfxThunder();
        }
        else if (name.Contains("CoinRain"))
        {
            cameraSlot.StartVfxCoin();
        }
    }
}
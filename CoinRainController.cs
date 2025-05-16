using UnityEngine;

public class CoinRainController : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void PlayCoinRain(int power = 1)
    {
        power = Mathf.Clamp(power, 1, 3);
        
        int maxParticles = 1000 * power;

        if (ps != null) {
            var main = ps.main;
            main.maxParticles = maxParticles;

            ps.Play();
        }
    }
}

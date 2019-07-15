using Kino;
using UnityEngine;

public class GlitchController : MonoBehaviour {
    [SerializeField] AnalogGlitch analog;
    [SerializeField] DigitalGlitch digital;

    [Header("Jitter")]
    [SerializeField] float JitterSpeed = 1;
    [SerializeField] float JitterIntensity = 1;
    [SerializeField] float JitterOffset = 0;

    [Header("Color Drift")]
    [SerializeField] float ColorSpeed = 1;
    [SerializeField] float ColorIntensity = 1;
    [SerializeField] float ColorOffset = 0;

    [Header("Horiz Shake")]
    [SerializeField] float HShakeSpeed = 1;
    [SerializeField] float HShakeIntensity = 1;
    [SerializeField] float HShakeOffset = 0;

    [Header("Vertical Jump")]
    [SerializeField] float VJumpSpeed = 1;
    [SerializeField] float VJumpIntensity = 1;
    [SerializeField] float VJumpOffset = 0;

    [Header("Digital")]
    [SerializeField] float DigitalSpeed = 1;
    [SerializeField] float DigitalIntensity = 1;
    [SerializeField] float DigitalOffset = 0;

    void Start() {
//        DontDestroyOnLoad(this);
    }

    void Update() {
        analog.scanLineJitter = Mathf.Clamp01(JitterOffset + Mathf.PerlinNoise(0, Time.time * JitterSpeed) * JitterIntensity);
        analog.colorDrift = Mathf.Clamp01(ColorOffset + Mathf.PerlinNoise(0, Time.time * ColorSpeed) * ColorIntensity);
        analog.horizontalShake = Mathf.Clamp01(HShakeOffset + Mathf.PerlinNoise(0, Time.time * HShakeSpeed) * HShakeIntensity);
        analog.verticalJump = Mathf.Clamp01(VJumpOffset + Mathf.PerlinNoise(0, Time.time * VJumpSpeed) * VJumpIntensity);

        digital.intensity = Mathf.Clamp01(DigitalOffset + Mathf.PerlinNoise(0, Time.time * DigitalSpeed) * DigitalIntensity);
    }
}

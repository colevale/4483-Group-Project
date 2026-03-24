using UnityEngine;

public class CrystalActivation : MonoBehaviour
{
    public Animator anim;

    bool inAWave = false;
    public Light crystalLight;
    public AudioSource audio;
    public float audioVolumeMax = 0.5f;

    int nearCrystal;

    float volumeTimer = 0;
    public float volumeTransitionSpeed = 0.2f;


    void OnTriggerEnter(Collider other)
    {
        nearCrystal = 1;
        if (inAWave)
            return;

        anim.SetBool("OnOff", true);
        PlayerController.playcon.SetNearCrystal(true);

        
    }

    private void OnTriggerExit(Collider other)
    {
        nearCrystal = -1;
        anim.SetBool("OnOff", false);
        PlayerController.playcon.SetNearCrystal(false);
    }

    private void Update()
    {
        volumeTimer += Time.deltaTime * nearCrystal * volumeTransitionSpeed;

        if (volumeTimer > audioVolumeMax)
            volumeTimer = audioVolumeMax;
        if (volumeTimer < 0)
            volumeTimer = 0;


        audio.volume = volumeTimer/audioVolumeMax;
    }


    public void WaveStart()
    {
        anim.SetBool("OnOff", false);
        PlayerController.playcon.SetNearCrystal(false);
        crystalLight.intensity = 500;
        inAWave = true;
    }
}

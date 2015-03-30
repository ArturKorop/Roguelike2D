using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioSource ExfSource;
    public AudioSource MusicSource;
    public static SoundManager Instance = null;

    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        this.ExfSource.clip = clip;
        ExfSource.Play();
    }

    public void RandomixzeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        var randomPitch = Random.Range(this.LowPitchRange, this.HighPitchRange);

        this.ExfSource.pitch = randomPitch;
        this.ExfSource.clip = clips[randomIndex];
        this.ExfSource.Play();
    }
}

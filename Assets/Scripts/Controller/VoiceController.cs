using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource hitSource;

    [Range(0f, 1f)] public float hitvolume = 0.6f;
    [Range(0f, 1f)] public float audiovolume = 0.6f;

    private static VoiceController instance;
    public static VoiceController Instance
    {
        get
        {
            if (instance == null)
            {
                instance=FindObjectOfType<VoiceController>();
                if (instance == null)
                {
                    Debug.Log("No VoiceController in the scene.");
                }
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = PlayerSet.Instance.volumn*PlayerSet.Instance.totalvolumn/10000;
        hitSource.volume = PlayerSet.Instance.voiceeffect* PlayerSet.Instance.totalvolumn/10000;
    }

    public void PlayHit()
    {
        hitSource.Stop();
        hitSource.Play();
    }

    public void SetAudioVolume(float v)
    {
        audiovolume = Mathf.Clamp01(v);
        audioSource.volume = audiovolume;
    }

    public void SetHitVolume(float v)
    {
        hitvolume = Mathf.Clamp01(v);
        hitSource.volume = hitvolume;
    }

    public void PauseAudioMusic() => audioSource.Pause();
    public void ResumeAudioMusic() => audioSource.UnPause();
    public void StopAudioMusic() => audioSource.Stop();
}

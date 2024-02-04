using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private float fadeTime;
    private float targetVolume;

    private void Start()
    {
        targetVolume = 0.0f;
        audioSource.volume = 0.0f;
    }

    private void Update()
    {
        // transition over time to the target volume
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, (1.0f / fadeTime) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
            targetVolume = 1.0f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
            targetVolume = 0.0f;
    }
}

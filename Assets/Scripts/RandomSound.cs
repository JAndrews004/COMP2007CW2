using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSound : MonoBehaviour
{
    public AudioClip soundClip;
    public float minDelay = 2f;
    public float maxDelay = 5f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        StartCoroutine(PlaySoundAtRandomIntervals());
    }

    System.Collections.IEnumerator PlaySoundAtRandomIntervals()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(soundClip);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip soundClip;
}
public class SoundController : MonoBehaviour
{
    public List<Sound> sounds = new();

    public void Playback(string soundName, bool loop = false)
    {
        Sound foundSound = sounds.Find(sound => sound.name == soundName);

        if (foundSound != null)
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();

            audioSource.loop = loop;

            if (loop)
            {
                audioSource.clip = foundSound.soundClip;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(foundSound.soundClip);
            }
        }
        else
        {
            Debug.LogWarning("Ses bulunamadý: " + soundName);
        }
    }
    public void StopPlayback()
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }


}

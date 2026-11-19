using UnityEngine;

public static class Sfx
{
    // Один виклик = один звук. Грає біля камери, щоб гучність не залежала від відстані.
    public static void Play(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null || Camera.main == null)
        {
            return;
        }
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }
}

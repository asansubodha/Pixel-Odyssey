using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource AttackAudioSource;
    public AudioSource CoinAudioSource;
    public AudioSource EnemyDyingAudioSource;

    public void PlayAudio()
    {
        AttackAudioSource.Play();
    }
    public void PlayCoinAudio()
    {
        CoinAudioSource.Play();
    }
    public void PlayEnemyDyingAudio()
    {
        EnemyDyingAudioSource.Play();
    }
}

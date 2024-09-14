using DG.Tweening;
using UnityEngine;

namespace Services.Audio
{
    public class AudioService
    {
        public static AudioService Instance { get; private set; }
        
        private AudioSource _audioSource;
        
        public AudioService()
        {
            GameObject audioSourceGameObject = new GameObject("audio_source");
            
            _audioSource = audioSourceGameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            _audioSource.spatialBlend = 0.0f;
            
            Object.DontDestroyOnLoad(audioSourceGameObject);

            Instance = this;
        }

        public static void Mute(bool mute)
        {
            Instance._audioSource.mute = mute;
        }

        public void PlayOneShot(string audioClipName)
        {
            AudioClip audioClip = Resources.Load<AudioClip>(audioClipName);
            //_audioSource.pitch = Random.Range(0.95f, 1.05f);
            _audioSource.PlayOneShot(audioClip);

            DOVirtual.DelayedCall(audioClip.length, () => { Resources.UnloadAsset(audioClip); }, false)
                .SetLink(_audioSource.gameObject);
        }
        
    }
}
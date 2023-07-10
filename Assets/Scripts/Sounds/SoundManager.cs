using UnityEngine;

namespace Assets.Scripts.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        //Drag a reference to the audio source which will play the sound effects.
        public AudioSource efxSource;
        //Drag a reference to the audio source which will play the music.
        public AudioSource musicSource;
        //Allows other scripts to call functions from the SoundManager
        public static SoundManager instance = null;
        //The lowest a sound effect will be randomly pitched.
        public float lowPitchRange = .95f;
        //The highest a sound effect will be randomly pitched.
        public float highPitchRange = 1.05f;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        public void PlaySingle(AudioClip clip)
        {
            efxSource.clip = clip;
            efxSource.Play();
        }

        public void RandomizeSfx(params AudioClip[] clips)
        {
            int randomIndex = Random.Range(0, clips.Length);
            float randomPitch = Random.Range(lowPitchRange, highPitchRange);
            efxSource.pitch = randomPitch;
            efxSource.clip = clips[randomIndex];

            efxSource.Play();
        }
    }
}
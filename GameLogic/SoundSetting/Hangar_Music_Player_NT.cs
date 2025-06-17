using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace nirtothunder
{
    public class Hangar_Music_Player_NT : MonoBehaviour
    {
        public List<AudioClip> Audio_Clips;
        public Vector2 Delay_Range = new Vector2(1.0f, 5.0f);

        private AudioSource audio_Source;
        private Coroutine play_Routine;

        void Start()
        {
            audio_Source = GetComponent<AudioSource>();
            if (audio_Source == null)
            {
                audio_Source = gameObject.AddComponent<AudioSource>();
            }

            Start_Audio_Playback();
        }

        void Start_Audio_Playback()
        {
            if (play_Routine != null)
            {
                StopCoroutine(play_Routine);
            }
            play_Routine = StartCoroutine(Play_Random_Audio_With_Delay());
        }

        IEnumerator Play_Random_Audio_With_Delay()
        {
            while (true)
            {
                if (Audio_Clips.Count > 0)
                {
                    AudioClip random_Clip = Audio_Clips[Random.Range(0, Audio_Clips.Count)];
                    
                    audio_Source.clip = random_Clip;
                    audio_Source.Play();

                    yield return new WaitForSeconds(random_Clip.length);

                    float random_Delay = Random.Range(Delay_Range.x, Delay_Range.y);
                    yield return new WaitForSeconds(random_Delay);
                }
                else
                {
                    Debug.LogWarning("No audio clips assigned in the list!");
                    yield return null;
                }
            }
        }

        public void Update_Audio_Clips(List<AudioClip> new_Clips)
        {
            Audio_Clips = new_Clips;
            Start_Audio_Playback();
        }
    }
}

using UnityEngine;
using System.Collections;

namespace nirtothunder
{
    public class Hangar_Radio_Manager_NT : MonoBehaviour
    {
        public Hangar_Music_Player_NT Hangar_Music_Player;
        public AudioSource Radio_Music_Source;
        public float Fade_Duration = 1.0f;
        public float Hangar_Music_Delay = 20.0f; // Задержка перед включением музыки ангара

        private bool is_Radio_Active = false;
        private Coroutine currentFadeCoroutine;
        private Coroutine delayedMusicCoroutine;

        void OnMouseDown()
        {
            ToggleRadio();
        }

        public void ToggleRadio()
        {
            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
            }

            if (delayedMusicCoroutine != null)
            {
                StopCoroutine(delayedMusicCoroutine);
                delayedMusicCoroutine = null;
            }

            is_Radio_Active = !is_Radio_Active;

            if (is_Radio_Active)
            {
                currentFadeCoroutine = StartCoroutine(SwitchToRadioMusic());
            }
            else
            {
                currentFadeCoroutine = StartCoroutine(StopRadioMusic());
                delayedMusicCoroutine = StartCoroutine(DelayedHangarMusicStart());
            }
        }

        IEnumerator SwitchToRadioMusic()
        {
            float start_Volume = Hangar_Music_Player.audio_Source.volume;
            float timer = 0f;

            while (timer < Fade_Duration)
            {
                timer += Time.deltaTime;
                Hangar_Music_Player.audio_Source.volume = Mathf.Lerp(start_Volume, 0f, timer / Fade_Duration);
                yield return null;
            }

            Hangar_Music_Player.Stop_Audio_Playback();
            
            Radio_Music_Source.volume = 0f;
            Radio_Music_Source.Play();
            
            timer = 0f;
            while (timer < Fade_Duration)
            {
                timer += Time.deltaTime;
                Radio_Music_Source.volume = Mathf.Lerp(0f, 1f, timer / Fade_Duration);
                yield return null;
            }
        }

        IEnumerator StopRadioMusic()
        {
            float start_Volume = Radio_Music_Source.volume;
            float timer = 0f;

            while (timer < Fade_Duration)
            {
                timer += Time.deltaTime;
                Radio_Music_Source.volume = Mathf.Lerp(start_Volume, 0f, timer / Fade_Duration);
                yield return null;
            }

            Radio_Music_Source.Stop();
        }

        IEnumerator DelayedHangarMusicStart()
        {
            yield return new WaitForSeconds(Hangar_Music_Delay);
            
            Hangar_Music_Player.audio_Source.volume = 0f;
            Hangar_Music_Player.Start_Audio_Playback();
            
            float timer = 0f;
            while (timer < Fade_Duration)
            {
                timer += Time.deltaTime;
                Hangar_Music_Player.audio_Source.volume = Mathf.Lerp(0f, 1f, timer / Fade_Duration);
                yield return null;
            }
        }
    }
}

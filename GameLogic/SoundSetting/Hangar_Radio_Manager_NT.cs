using UnityEngine;
using System.Collections;

namespace nirtothunder
{
    public class Hangar_Radio_Manager_NT : MonoBehaviour
    {
        public Hangar_Music_Player_NT Hangar_Music_Player;
        public AudioSource Radio_Music_Source;
        public float Fade_Duration = 1.0f;
        public KeyCode Activation_Key = KeyCode.R;

        private bool is_Radio_Active = false;

        void Update()
        {
            if (Input.GetKeyDown(Activation_Key))
            {
                ToggleRadio();
            }
        }

        public void ToggleRadio()
        {
            is_Radio_Active = !is_Radio_Active;

            if (is_Radio_Active)
            {
                StartCoroutine(SwitchToRadioMusic());
            }
            else
            {
                StartCoroutine(SwitchToHangarMusic());
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

        IEnumerator SwitchToHangarMusic()
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
            
            Hangar_Music_Player.audio_Source.volume = 0f;
            Hangar_Music_Player.Start_Audio_Playback();
            
            timer = 0f;
            while (timer < Fade_Duration)
            {
                timer += Time.deltaTime;
                Hangar_Music_Player.audio_Source.volume = Mathf.Lerp(0f, 1f, timer / Fade_Duration);
                yield return null;
            }
        }
    }
}

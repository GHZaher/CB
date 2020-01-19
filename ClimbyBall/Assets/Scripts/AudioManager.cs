using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource audioSourceUI;
    [SerializeField]
    private AudioSource audioSourceMusic;

    [Header("Sounds Info")]
    [SerializeField] private SoundInfo soundInfo;

    private static bool isSoundsMuted = false;
    public bool IsSoundsMuted { get { return isSoundsMuted; } }
    private static bool isMusicMuted = false;
    public bool IsMusicsMuted { get { return isMusicMuted; } }

    private static bool instance = false;

    [System.Serializable]
    public class SoundInfo
    {
        [Header("UI Audio Clips")]
        public AudioClip pressBtnClip;
        public float pressBtnVolume;
        [Header("Game Play Clips")]
        public AudioClip wallHitClip;
        public float wallHitVolume;
        public AudioClip ballClickClip;
        public float ballClickVolume;
        public AudioClip loseClip;
        public float loseVolume;

        [Header("Music Audio Clips")]
        public AudioClip mainMusicClip;
        public float mainMusicVolume;
    }

    void Awake()
    {
        if (!instance && SceneManager.GetActiveScene().name == "StartScene")
        {
            instance = true;

            if (SaveFiles.GetStringPlayerPref("IsMusicMuted") != null)//if there are music state before nad this is not the first time you play the game
            {
                if (SaveFiles.GetStringPlayerPref("IsMusicMuted") == "True")//check the old music state
                {
                    isMusicMuted = true;
                }
                else
                {
                    isMusicMuted = false;
                    UnMuteMainMusic();//play main music
                }
            }
            else//if this the first time you play the game
            {
                isMusicMuted = false;
                UnMuteMainMusic();
                SaveFiles.SetStringPlayerPref("IsMusicMuted", "False");
            }
            if (SaveFiles.GetStringPlayerPref("IsSoundsMuted") != null)//if there are sounds state before nad this is not the first time you play the game
            {
                if (SaveFiles.GetStringPlayerPref("IsSoundsMuted") == "True")//check the old sounds state
                {
                    isSoundsMuted = true;
                }
                else
                {
                    isSoundsMuted = false;
                }
            }
            else//if this the first time you play the game
            {
                isSoundsMuted = false;
                SaveFiles.SetStringPlayerPref("IsSoundsMuted", "False");
            }
            DontDestroyOnLoad(gameObject);
        }
        else if (SceneManager.GetActiveScene().name == "StartScene")
        {
            Destroy(gameObject);
        }
    }

    //private void OnApplicationQuit()
    //{
    //    SaveFiles.SetStringPlayerPref("IsMusicMuted", isMusicMuted.ToString());//save the current music state
    //    SaveFiles.SetStringPlayerPref("IsSoundsMuted", isSoundsMuted.ToString());//save the current sounds state
    //}

   
    public void UnMuteMainMusic()
    {
        if (!audioSourceMusic.isPlaying)
        {
            isMusicMuted = false;
            audioSourceMusic.clip = soundInfo.mainMusicClip;
            audioSourceMusic.loop = true;
            audioSourceMusic.Play();
            SaveFiles.SetStringPlayerPref("IsMusicMuted", isMusicMuted.ToString());//save the current music state
        }
    }

    public void MuteMainMusic()
    {
        if (audioSourceMusic.isPlaying)
        {
            isMusicMuted = true;
            audioSourceMusic.Pause();
            SaveFiles.SetStringPlayerPref("IsMusicMuted", isMusicMuted.ToString());//save the current music state
        }
    }

    public void MuteSounds()
    {
        isSoundsMuted = true;
        audioSourceUI.Stop();
        SaveFiles.SetStringPlayerPref("IsSoundsMuted", isSoundsMuted.ToString());//save the current sounds state
    }

    public void UnMuteSounds()
    {
        isSoundsMuted = false;
        SaveFiles.SetStringPlayerPref("IsSoundsMuted", isSoundsMuted.ToString());//save the current sounds state
    }

    public void PlayPressedBtnAudio()
    {
        if (!isSoundsMuted)
        {
            audioSourceUI.PlayOneShot(soundInfo.pressBtnClip, soundInfo.pressBtnVolume);
        }
    }

    public void PlayWallHitAudio()
    {
        if (!isSoundsMuted)
        {
            audioSourceUI.PlayOneShot(soundInfo.wallHitClip, soundInfo.wallHitVolume);
        }
    }

    public void PlayBallClickAudio()
    {
        if (!isSoundsMuted)
        {
            audioSourceUI.PlayOneShot(soundInfo.ballClickClip, soundInfo.ballClickVolume);
        }
    }

    public void PlayLoseAudio()
    {
        if (!isSoundsMuted)
        {
            audioSourceUI.PlayOneShot(soundInfo.loseClip, soundInfo.loseVolume);
        }
    }
}
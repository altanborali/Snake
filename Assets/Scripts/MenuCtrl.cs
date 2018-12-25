using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace SA
{

    public class MenuCtrl : MonoBehaviour
    {

        // Singleton instance.
        public static MenuCtrl instance = null;


        //public Button soundButton;
        //public Sprite soundOnSprite;
        //public Sprite soundOffSprite;

        public Button easyBtn;
        public Button normalBtn;
        public Button hardBtn;

        public float moveRate = 0.12f;

        //private Color selectedColor = new Color(1, 0.521f, 0, 1);




        public void SelectDifficulty(int n)   // GameManager public void GameOverdan da değiştir ki veriyi tutsun
        {
            if (n == 1) 
            {
                moveRate = 0.20f;
                SoundManager.Instance.Play("Button");

            }

            else if (n == 2)
            {
                moveRate = 0.12f;
                SoundManager.Instance.Play("Button");
            }

            else if (n == 3)
            {
                moveRate = 0.08f;
                SoundManager.Instance.Play("Button");
            }

        }


        public void loadScene(string sceneName)
        {
            SoundManager.Instance.Play("Button");
            if (sceneName.Equals("GameScene"))
            {
                //SoundManager.Instance.MusicSource.Stop();
                SceneManager.LoadScene(sceneName);
            }

        }


        /*public void muteSound()
        {
            if (!SoundManager.Instance.isMuted)
            {
                SoundManager.Instance.isMuted = !SoundManager.Instance.isMuted;
                soundButton.GetComponent<Image>().sprite = soundOffSprite;
                //SoundManager.instance.EffectsSource.mute = true;
                SoundManager.Instance.MusicSource.mute = true;
            }
            else
            {
                //soundManager.SetActive(true);
                //SoundManager.instance.EffectsSource.mute = false;
                //SoundManager.Instance.MusicSource.mute = false;
                //SoundManager.Instance.isMuted = !SoundManager.Instance.isMuted;
                //soundButton.GetComponent<Image>().sprite = soundOnSprite;
                //SoundManager.instance.PlayMusic("GameSound");
            }
        }*/


        void Awake()
        {
            // If there is not already an instance of MenuCtrl, set it to this.
            if (instance == null)
            {
                instance = this;
            }
            //If an instance already exists, destroy whatever this object is to enforce the singleton.
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            //Set MenuCtrl to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            //DontDestroyOnLoad(gameObject);
        }

        /*void turnToWhite(Button one, Button two)
        {
            ColorBlock colors = one.colors;
            colors.normalColor = Color.white;
            one.colors = colors;

            ColorBlock colors2 = two.colors;
            colors.normalColor = Color.white;
            two.colors = colors;
        }*/


        void Start()
        {

            normalBtn.Select();
            //ColorBlock colors = normalBtn.colors;
            //colors.normalColor = selectedColor;
            //normalBtn.colors = colors;
            //moveRate = FindObjectOfType<GameManager>();

            /*if (!SoundManager.Instance.MusicSource.isPlaying)
            {
                SoundManager.Instance.PlayMusic("GameSound");
            }
            if (SoundManager.Instance.MusicSource.mute == true)
            {
                soundButton.GetComponent<Image>().sprite = soundOffSprite;
            }*/

        }

    }
}
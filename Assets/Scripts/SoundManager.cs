using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SoundManager : MonoBehaviour
{

    public AudioClip ButtonSound, GameOverSound, PointSound; //GameSound, StartSound, ExplosionSound, EnemySound, GrapeFruitJokerSound, RadishJokerSound, BroccoliJokerSound, BeerJokerSound, ResetSound, 
         //CoinSound, AlertSound;
    public bool isMuted = false;


    // Audio players components.
    public AudioSource EffectsSource;
    public AudioSource MusicSource;


    // Singleton instance.
    public static SoundManager Instance = null;

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    // Play a single clip through the sound effects source.
    public void Play(string clip)
    {

        switch (clip)
        {
            case "Button":
                EffectsSource.clip = ButtonSound;
                EffectsSource.Play();
                break;

                case "GameOver": //explosion'un bir kere oynaması için.
                    EffectsSource.clip = GameOverSound;
                    EffectsSource.Play();
                    break;

                case "Point":
                    EffectsSource.clip = PointSound;
                    EffectsSource.Play();
                    break;

                /*case "GrapeFruitJoker":
                    EffectsSource.clip = GrapeFruitJokerSound;
                    EffectsSource.Play();
                    break;

                case "BeerJoker":
                    EffectsSource.clip = BeerJokerSound;
                    EffectsSource.Play();
                    break;

                case "BroccoliJoker":
                    EffectsSource.clip = BroccoliJokerSound;
                    EffectsSource.Play();
                    break;

                case "Start":
                    EffectsSource.clip = StartSound;
                    EffectsSource.Play();
                    break;

                case "Reset": 
                    EffectsSource.clip = ResetSound;
                    EffectsSource.Play();
                    break;



                case "Alert":
                    EffectsSource.clip = AlertSound;
                    EffectsSource.Play();
                    break;*/
        }
    }

    // Play a single clip through the music source.
    public void PlayMusic(string clip)
    {

        /*switch (clip)
        {

            case "GameSound":
                MusicSource.clip = GameSound;
                MusicSource.Play();
                break;

            case "RadishJoker": //Shield joker belirli bir süre oynayacağı için PlayOneShot olmaz.
                MusicSource.clip = RadishJokerSound;
                MusicSource.Play();
                break;

            case "CoinSound":
                MusicSource.clip = CoinSound;
                MusicSource.Play();
                break;
        }*/
    }
}







/*public class SoundManagerScript : MonoBehaviour
{

    public AudioClip GameSound, StartSound, ExplosionSound, EnemySound, RabbitJokerSound, ShieldJokerSound, HalfSizeJokerSound, BeerJokerSound;
    public AudioSource audioSrc;
    public bool isMuted = false;


    private void Awake() // Audio dosyasında olan isimler turuncu olanlar. onlara da kodda isim veriyoruz. Taglerdeki, sound dosyasındaki isimler değişecek !!!
    {
        GameSound = Resources.Load<AudioClip>("GameSound");
        StartSound = Resources.Load<AudioClip>("Start");
        ExplosionSound = Resources.Load<AudioClip>("Explosion");
        EnemySound = Resources.Load<AudioClip>("Enemy");
        RabbitJokerSound = Resources.Load<AudioClip>("RabbitJoker");
        ShieldJokerSound = Resources.Load<AudioClip>("ShieldJoker");
        HalfSizeJokerSound = Resources.Load<AudioClip>("HalfSizeJoker");
        BeerJokerSound = Resources.Load<AudioClip>("BeerJoker");


    }

    void Start()
    {


    }


    public void PlaySound(string clip) //Seslerin geldiği yerde JokerControl scriptinde void OnTriggerEnter2D(Collider2D col) altında hangi seslerin çıkacağı var. Scriptleri de birbrine yine JokerControl de private SoundManagerScript soundManager şeklinde bağladık.
    {
        switch (clip)
        {

            case "GameSound":
                audioSrc = gameObject.GetComponent<AudioSource>();
                audioSrc.clip = GameSound;
                audioSrc.Play();
                break;

            case "Explosion": //explosion'un bir kere oynaması için.
                audioSrc = gameObject.GetComponent<AudioSource>();
                audioSrc.PlayOneShot(ExplosionSound); //yukarda solda tanımlı olan isim buraya yazılır.
                break;

            case "Enemy":
                audioSrc = gameObject.GetComponent<AudioSource>();
                audioSrc.PlayOneShot(EnemySound);
                break;

            case "RabbitJoker":
                audioSrc = gameObject.GetComponent<AudioSource>();
                audioSrc.PlayOneShot(RabbitJokerSound);
                break;

            case "BeerJoker":
                audioSrc = gameObject.GetComponent<AudioSource>();
                audioSrc.PlayOneShot(BeerJokerSound);
                break;

            case "HalfSizeJoker":
                audioSrc = gameObject.GetComponent<AudioSource>();
                audioSrc.PlayOneShot(HalfSizeJokerSound);
                break;

            case "ShieldJoker": //Shield joker belirli bir süre oynayacağı için PlayOneShot olmaz.
                audioSrc = gameObject.GetComponent<AudioSource>();
                audioSrc.clip = ShieldJokerSound;
                audioSrc.Play();
                break;

            case "Start":
                audioSrc = gameObject.GetComponent<AudioSource>();
                audioSrc.PlayOneShot(StartSound);
                break;
        }

    }
}
*/

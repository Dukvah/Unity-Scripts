using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;
using TMPro;

public class MenuManager : MonoBehaviour
{
    #region VAR
    [SerializeField] private Button _mainMusicButton;
    [SerializeField] private Button _secondMusicButton;
    [SerializeField] private Button _mainVoiceButton;
    [SerializeField] private Button _secondVoiceButton;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _inGameMenu;
    [SerializeField] private GameObject _victoryMenu;
    [SerializeField] private GameObject _loseMenu;

    [SerializeField] private GameObject _levelInfo;
    [SerializeField] private GameObject _gameplayTime;

    [SerializeField] private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;

    [SerializeField] private GameObject _inGame3StarPop;
    [SerializeField] private GameObject _inGame2StarPop;
    [SerializeField] private GameObject _inGame1StarPop;

    [SerializeField] private float maxLimit;
    [SerializeField] private float minLimit;
    [SerializeField] private GameObject _maxLimitUI;
    [SerializeField] private GameObject _minLimitUI;



    private float gameplayTimer;
    private int minutes;
    private int seconds;
    private string currentTime;

    private bool _musicStatus;
    private bool _voiceStatus;
    private GameObject _music;
    private GameObject[] _voice;

    //URL adresleri
    [SerializeField] private string _webLink;
    [SerializeField] private string _playStoreLink;
    [SerializeField] private string _appStoreLink;

    #endregion

    #region FUNCTIONS

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        RequestInterstitial();
        //UI level bilgisi güncelleme
        _levelInfo.GetComponent<TextMeshProUGUI>().text = "Level" + "\n" + PlayerPrefs.GetInt("CurrentLevel") + "/20";
        //InGameUI Yýldýz kazanma limitleri güncelleme
        CalculateStarLimits();
        //TimerResetleme
        gameplayTimer = 0f;
        //Müzik ayarlamalarý
        _music = GameObject.FindGameObjectWithTag("Music");
        _voice = GameObject.FindGameObjectsWithTag("Voice");
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            _musicStatus = true;
            _mainMusicButton.transform.GetChild(0).gameObject.SetActive(true);
            _mainMusicButton.transform.GetChild(1).gameObject.SetActive(false);

            _secondMusicButton.transform.GetChild(0).gameObject.SetActive(true);
            _secondMusicButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            _musicStatus = false;
            _mainMusicButton.transform.GetChild(0).gameObject.SetActive(false);
            _mainMusicButton.transform.GetChild(1).gameObject.SetActive(true);

            _secondMusicButton.transform.GetChild(0).gameObject.SetActive(false);
            _secondMusicButton.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (PlayerPrefs.GetInt("Voice") == 1)
        {
            _voiceStatus = true;
            _mainVoiceButton.transform.GetChild(0).gameObject.SetActive(true);
            _mainVoiceButton.transform.GetChild(1).gameObject.SetActive(false);

            _secondVoiceButton.transform.GetChild(0).gameObject.SetActive(true);
            _secondVoiceButton.transform.GetChild(1).gameObject.SetActive(false);
            PlayerPrefs.SetInt("Voice", 1);

            foreach (var Voice in _voice)
            {
                Voice.GetComponent<AudioSource>().volume = 1;
            }
        }
        if (PlayerPrefs.GetInt("Voice") == 0)
        {
            _voiceStatus = false;
            _mainVoiceButton.transform.GetChild(0).gameObject.SetActive(false);
            _mainVoiceButton.transform.GetChild(1).gameObject.SetActive(true);

            _secondVoiceButton.transform.GetChild(0).gameObject.SetActive(false);
            _secondVoiceButton.transform.GetChild(1).gameObject.SetActive(true);
            PlayerPrefs.SetInt("Voice", 0);

            foreach (var Voice in _voice)
            {
                Voice.GetComponent<AudioSource>().volume = 0;
            }
        }
    }

    private void Update()
    {
        //Timer
        if (Player.isPlaying == true)
        {
            gameplayTimer += Time.deltaTime;
            minutes = Mathf.FloorToInt(gameplayTimer / 60F);
            seconds = Mathf.FloorToInt(gameplayTimer - minutes * 60);
            currentTime = string.Format("{0:0}:{1:00}", minutes, seconds);

            _gameplayTime.GetComponent<TextMeshProUGUI>().text = currentTime;

            if (gameplayTimer > minLimit && gameplayTimer < maxLimit) //Ingame menu yýldýz limitleri güncelleme
            {
                _inGame3StarPop.SetActive(false);
                _inGame2StarPop.SetActive(true);
            }
            else if (gameplayTimer > maxLimit)
            {
                _inGame2StarPop.SetActive(false);
                _inGame1StarPop.SetActive(true);
            }
        }
    }
    private void CalculateStarLimits()
    {
        minutes = Mathf.FloorToInt(minLimit / 60F);
        seconds = Mathf.FloorToInt(minLimit - minutes * 60);
        currentTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        _minLimitUI.GetComponent<TextMeshProUGUI>().text = currentTime;

        minutes = Mathf.FloorToInt(maxLimit / 60F);
        seconds = Mathf.FloorToInt(maxLimit - minutes * 60);
        currentTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        _maxLimitUI.GetComponent<TextMeshProUGUI>().text = currentTime;
    }
    public void CalculateStars()
    {
        PlayerPrefs.SetInt("Star Level " + (SceneManager.GetActiveScene().buildIndex + 1).ToString(), 0);
        if (gameplayTimer <= minLimit) //3 YILDIZ KAZANMAK
        {
            if (PlayerPrefs.GetInt("Star Level " + SceneManager.GetActiveScene().buildIndex.ToString()) < 3 || !PlayerPrefs.HasKey("Star Level " + SceneManager.GetActiveScene().buildIndex.ToString()))
            {
                PlayerPrefs.SetInt("Star Level " + SceneManager.GetActiveScene().buildIndex.ToString(), 3);
            }
            _star1.SetActive(true);
            _star2.SetActive(true);
            _star3.SetActive(true);
        }
        else if (gameplayTimer > minLimit && gameplayTimer < maxLimit) //2 YILDIZ KAZANMAK
        {
            if (PlayerPrefs.GetInt("Star Level " + SceneManager.GetActiveScene().buildIndex.ToString()) < 2 || !PlayerPrefs.HasKey("Star Level " + SceneManager.GetActiveScene().buildIndex.ToString()))
            {
                PlayerPrefs.SetInt("Star Level " + SceneManager.GetActiveScene().buildIndex.ToString(), 2);
            }
            _star1.SetActive(true);
            _star2.SetActive(true);
            _star3.SetActive(false);
        }
        else if (gameplayTimer >= maxLimit)   //1 YILDIZ KAZANMAK
        {
            if (!PlayerPrefs.HasKey("Star Level " + SceneManager.GetActiveScene().buildIndex.ToString()))
            {
                PlayerPrefs.SetInt("Star Level " + SceneManager.GetActiveScene().buildIndex.ToString(), 1);
            }
            _star1.SetActive(true);
            _star2.SetActive(false);
            _star3.SetActive(false);
        }
    }
    public void GoNextLevel()
    {   
        if (this.interstitial.IsLoaded())
        {
            PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex + 1);
            this.interstitial.Show();
        }
        else
        {
            Time.timeScale = 1;
            PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        }
    }
    public void TapToPlay()
    {
        if (Player.isPlaying == false)
        {
            _gameMenu.SetActive(false);
            _inGameMenu.SetActive(true);
            Player.isPlaying = true;
            Time.timeScale = 1;
        }
    }

    public void ResumeGame()
    {
        if (Player.isPlaying == false)
        {
            _pauseMenu.SetActive(false);
            _inGameMenu.SetActive(true);
            Player.isPlaying = true;
        }
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
        else
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
            Time.timeScale = 1;
            Player.isPlaying = false;
        }
    }

    public void PauseGame()
    {
        if (Player.isPlaying == true)
        {
            _inGameMenu.SetActive(false);
            _pauseMenu.SetActive(true);
            Player.isPlaying = false;
            Time.timeScale = 0;
        }
    }

    public void Music() 
    {
        if (_musicStatus == true)
        {
            _musicStatus = false;

            _mainMusicButton.transform.GetChild(0).gameObject.SetActive(false);
            _mainMusicButton.transform.GetChild(1).gameObject.SetActive(true);

            _secondMusicButton.transform.GetChild(0).gameObject.SetActive(false);
            _secondMusicButton.transform.GetChild(1).gameObject.SetActive(true);

            PlayerPrefs.SetInt("Music", 0);
            _music.GetComponent<AudioSource>().Stop();
            //MÜZÝÐÝ KAPAT

        }
        else if (_musicStatus == false)
        {
            _musicStatus = true;

            _mainMusicButton.transform.GetChild(0).gameObject.SetActive(true);
            _mainMusicButton.transform.GetChild(1).gameObject.SetActive(false);

            _secondMusicButton.transform.GetChild(0).gameObject.SetActive(true);
            _secondMusicButton.transform.GetChild(1).gameObject.SetActive(false);

            PlayerPrefs.SetInt("Music", 1);
            _music.GetComponent<AudioSource>().Play();
            //MÜZÝÐÝ AÇ
        }
    }

    public void Voice()
    {
        if (_voiceStatus == true)
        {
            _voiceStatus = false;

            _mainVoiceButton.transform.GetChild(0).gameObject.SetActive(false);
            _mainVoiceButton.transform.GetChild(1).gameObject.SetActive(true);

            _secondVoiceButton.transform.GetChild(0).gameObject.SetActive(false);
            _secondVoiceButton.transform.GetChild(1).gameObject.SetActive(true);

            PlayerPrefs.SetInt("Voice", 0);

            foreach (var Voice in _voice)
            {
                Voice.GetComponent<AudioSource>().volume = 0;
            }
            //SESÝ KAPAT

        }
        else if (_voiceStatus == false)
        {
            _voiceStatus = true;

            _mainVoiceButton.transform.GetChild(0).gameObject.SetActive(true);
            _mainVoiceButton.transform.GetChild(1).gameObject.SetActive(false);

            _secondVoiceButton.transform.GetChild(0).gameObject.SetActive(true);
            _secondVoiceButton.transform.GetChild(1).gameObject.SetActive(false);

            PlayerPrefs.SetInt("Voice", 1);

            foreach (var Voice in _voice)
            {
                Voice.GetComponent<AudioSource>().volume = 1;
            }
            //SESÝ AÇ
        }
    }
    public void GoLevelMenu()
    {
        SceneManager.LoadScene((SceneManager.sceneCountInBuildSettings - 1));
    }

    public void Info()
    {
        Application.OpenURL(_webLink);
    }

    //public void GotoStore()
    //{
    //    if (Application.platform == RuntimePlatform.Android)
    //        Application.OpenURL(_playStoreLink);
    //    else if (Application.platform == RuntimePlatform.IPhonePlayer)
    //        Application.OpenURL(_appStoreLink);
    //}
    #endregion

    #region GETTER & SETTERS
    public float Get_GameplayTimer()
    {
        return gameplayTimer;
    }
    public float Get_MaxLimit()
    {
        return maxLimit;
    }
    public float Get_MinLimit()
    {
        return minLimit;
    }
    public GameObject Get_InGameMenu()
    {
        return _inGameMenu;
    }
    public GameObject Get_VictoryMenu()
    {
        return _victoryMenu;
    }
    public GameObject Get_DefeatMenu()
    {
        return _loseMenu;
    }
    public GameObject Get_GameMenu()
    {
        return _gameMenu;
    }
    public bool Get_MusicStatus()
    {
        return _musicStatus;
    }
    public bool Get_VoiceStatus()
    {
        return _voiceStatus;
    }
    #endregion

    #region ADS
    private InterstitialAd interstitial;

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpening;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }

    public void HandleOnAdOpening(object sender, EventArgs args)
    {

    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        interstitial.Destroy();
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
    #endregion
}

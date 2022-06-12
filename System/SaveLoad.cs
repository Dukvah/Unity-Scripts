
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoad : MonoBehaviour
{
    [SerializeField] private GameObject _music;
    [SerializeField] private GameObject noIntUI;

    private void Awake()
    {
        //for (int i = 1; i < 21; i++) {PlayerPrefs.SetInt("Star Level " + i.ToString(),0);} //Yýldýzlarý sýfýrlamak için
        
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            noIntUI.SetActive(true);//int yok arayuz
        }
        else
        {
            if (!PlayerPrefs.HasKey("CurrentLevel"))
            {
                PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex + 1);
            }
            //PlayerPrefs.SetInt("CurrentLevel",1); //Level-1 den baþlamak için..
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));

            if (!PlayerPrefs.HasKey("Music"))
            {
                PlayerPrefs.SetInt("Music", 1);
            }
            if (!PlayerPrefs.HasKey("Voice"))
            {
                PlayerPrefs.SetInt("Voice", 1);
            }

            if (PlayerPrefs.GetInt("Music") == 1)
            {
                _music.GetComponent<AudioSource>().Play(0);
            }
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                _music.GetComponent<AudioSource>().Stop();
            }
            DontDestroyOnLoad(_music);
        }
        

    }
    public void Exit()
    {
        Application.Quit(0);
    }


}

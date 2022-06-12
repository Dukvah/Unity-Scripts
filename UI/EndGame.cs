using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject _totalStar;
    private int currentStar;
    private int totalStar;
    private void Awake()
    {
        for (int i = 1; i < 21; i++)
        {
            currentStar = PlayerPrefs.GetInt("Star Level " + i.ToString());
            totalStar = totalStar + currentStar;
        }
        _totalStar.GetComponent<TextMeshProUGUI>().text = totalStar.ToString() + "/60";
    }
    public void GoFirstLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel",1);
        SceneManager.LoadScene(1);
    }
}

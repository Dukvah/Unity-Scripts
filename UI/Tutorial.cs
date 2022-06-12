using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject _step1;
    [SerializeField] private GameObject _step2;
    [SerializeField] private GameObject _step3;
    [SerializeField] private GameObject _step4;
    [SerializeField] private GameObject _step5;
    [SerializeField] private GameObject _step6;
    [SerializeField] private GameObject _step7;

    [SerializeField] private GameObject _allyCity1;
    [SerializeField] private GameObject _allyCity2;
    [SerializeField] private GameObject _neutralCity;
    [SerializeField] private GameObject _enemyCity;

    private CapsuleCollider AllyCity1Col;
    private CapsuleCollider AllyCity2Col;
    private CapsuleCollider NeutralCityCol;
    private CapsuleCollider EnemyCityCol;

    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _inGameMenu;

    private void Start()
    {
        AllyCity1Col = _allyCity1.GetComponent<CapsuleCollider>();
        AllyCity2Col = _allyCity2.GetComponent<CapsuleCollider>();
        EnemyCityCol = _enemyCity.GetComponent<CapsuleCollider>();
        NeutralCityCol = _neutralCity.GetComponent<CapsuleCollider>();
    }

    public void TapToPlay()
    {
        if (Player.isPlaying == false)
        {
            _gameMenu.SetActive(false);
            _step1.SetActive(true);
        }
    }

    public void Step1()
    {
        _step1.SetActive(false);
        _step2.SetActive(true);

    }
    public void Step2()
    {
        _step2.SetActive(false);
        _step3.SetActive(true);
    }
    public void Step3()
    {
        _step3.SetActive(false);
        _step4.SetActive(true);
        Player.isPlaying = true;
        Time.timeScale = 1;

        AllyCity1Col.enabled = true;
        EnemyCityCol.enabled = true;
    }
    public void Step4()
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(.1f);
            if (_allyCity1.GetComponent<AllyCity>().Get_isSelected() == true)
            {
                _step4.SetActive(false);
                _step5.SetActive(true);
            }
        }
    }
    public void Step5()
    {
        _step5.SetActive(false);
        _step6.SetActive(true);

        AllyCity2Col.enabled = true;
        NeutralCityCol.enabled = true;
    }
    public void Step6()
    {
        StartCoroutine(Delay2());
        IEnumerator Delay2()
        {
            yield return new WaitForSeconds(.1f);
            if (_allyCity2.GetComponent<AllyCity>().Get_isSelected() == true)
            {
                _step6.SetActive(false);
                _step7.SetActive(true);
            }
        }  
    }
    public void Step7()
    {
        _step7.SetActive(false);
        _inGameMenu.SetActive(true);
    }
}

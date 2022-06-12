using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LevelMenu : MonoBehaviour
{
    public List<GameObject> _LevelCards = new List<GameObject>();

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private void Awake()
    {
        
        for (int i = _LevelCards.Count-1; i > -1 ; i--)
        {
            _LevelCards[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = (i+1).ToString(); //Kart numaralarýnýn yazýlmasý
            
            

            if (!PlayerPrefs.HasKey("Star Level " + (i+1).ToString()))   //Henüz oynanmayan bölümlerin kartlarýnýn kapatýlmasý.
            {
                _LevelCards[i].SetActive(false); 
            }
            else if (PlayerPrefs.GetInt("Star Level " + (i + 1).ToString()) == 1) // 1 yýldýz kazanýlan kart
            {
                _LevelCards[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                _LevelCards[i].transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(false);
                _LevelCards[i].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("Star Level " + (i + 1).ToString()) == 2) //2 yýldýz kazanýlan kart
            {
                _LevelCards[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                _LevelCards[i].transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(true);
                _LevelCards[i].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("Star Level " + (i + 1).ToString()) == 3) //3 yýldýz kazanýlan kart
            {
                _LevelCards[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                _LevelCards[i].transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.SetActive(true);
                _LevelCards[i].transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }

        }

        if (!PlayerPrefs.HasKey("Star Level " + 1))
        {
            _LevelCards[0].SetActive(true);
        }

    }

    void Start()
    {
        m_Raycaster = GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("LevelCards"))
                {
                    PlayerPrefs.SetInt("CurrentLevel", Int32.Parse(result.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text));
                    Time.timeScale = 1;
                    SceneManager.LoadScene(Int32.Parse(result.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text));
                }
            }
        }
    }
}

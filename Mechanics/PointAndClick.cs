using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClick : MonoBehaviour
{
    RaycastHit hitDown;
    RaycastHit hit;

    [SerializeField] private GameObject _allySoldier;

    [SerializeField] private GameObject _citySelectEffect;

    [SerializeField] private GameObject _cityAllySelectSounds;
    [SerializeField] private GameObject _cityEnemySelectSounds;
    [SerializeField] private GameObject _cityUnselectSounds;

    private GameObject hitDownObj;
    private GameObject hitUpObj;

    public GameObject ps;

    Vector2 touchDest;
    Vector2 firstPoint;
    Vector2 secPoint;

    private void Update()
    {
        Click();
    }

    private void Click()
    {
        if (Input.GetMouseButtonDown(0) && Player.isPlaying == true)
        {
            if (Input.touchCount > 0)
            {
                firstPoint = Input.GetTouch(0).position;
            }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitDown))
            {
                hitDownObj = hitDown.transform.gameObject;
            }
            else
            {
                hitDownObj = null;
            }
            
           
        }
        GameObject currentClickedCity = Player.currentClickedCity;

        if (Input.GetMouseButtonUp(0) && Player.isPlaying == true)
        {
            if (Input.touchCount > 0)
            {
                secPoint = Input.GetTouch(0).position;
            }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                hitUpObj = hit.transform.gameObject;
                if (hit.transform.gameObject.GetComponent<City>() && hitUpObj == hitDownObj)
                {
                    if (currentClickedCity == null && !hit.transform.gameObject.GetComponent<EnemyCity>() && !hit.transform.gameObject.GetComponent<NeutralCity>())
                    {
                        Player.currentClickedCity = hit.transform.gameObject;
                        ps = Instantiate(_citySelectEffect, hit.transform.GetChild(0).gameObject.transform.position + new Vector3 (0f, 0.15f, 0f), 
                            _citySelectEffect.transform.rotation, hit.transform.gameObject.transform);

                        if (PlayerPrefs.GetInt("Voice") == 1)
                        {
                            _cityAllySelectSounds.GetComponent<AudioSource>().Play(0);
                        }
                        hit.transform.gameObject.GetComponent<AllyCity>().Set_isSelected(true);
                        //Dost þehir seçildi
                    }
                    else if (currentClickedCity  != null && hit.transform != Player.currentClickedCity.transform)
                    {
                        Player.aldreadyClickedCity = Player.currentClickedCity;
                        Player.currentClickedCity = hit.transform.gameObject;
                        ps.transform.parent.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                        Destroy(ps);

                        if (PlayerPrefs.GetInt("Voice") == 1)
                        {
                            _cityEnemySelectSounds.GetComponent<AudioSource>().Play(0);
                        }
                        //Askerler Gönderildi
                    }
                    else if (currentClickedCity != null && hit.transform == Player.currentClickedCity.transform)
                    {
                        ps.transform.parent.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                        Destroy(ps);

                        Player.aldreadyClickedCity = null;
                        Player.currentClickedCity = null;

                        if (PlayerPrefs.GetInt("Voice") == 1)
                        {
                            _cityUnselectSounds.GetComponent<AudioSource>().Play(0);
                        }

                        hit.transform.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                        //Dost þehir iptal edildi (kendi üzerine týklanýp)
                    }
                }
                else if (currentClickedCity != null && hitDownObj == hitUpObj)
                {
                    ps.transform.parent.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                    Destroy(ps);
                    Player.aldreadyClickedCity = null;
                    Player.currentClickedCity = null;

                    if (PlayerPrefs.GetInt("Voice") == 1)
                    {
                        _cityUnselectSounds.GetComponent<AudioSource>().Play(0);
                    }
                    //Dost þehir iptal edildi (þehir hariç bir yere týklanýp) (colliderý olan)
                }
                
            }
            else if (currentClickedCity != null)
            {
                hitDownObj = null;
                if (hitDownObj != null && hitUpObj != null)
                {
                    ps.transform.parent.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                    Destroy(ps);
                    Player.aldreadyClickedCity = null;
                    Player.currentClickedCity = null;
                    if (PlayerPrefs.GetInt("Voice") == 1)
                    {
                        _cityUnselectSounds.GetComponent<AudioSource>().Play(0);
                    }
                }
                else if (Vector2.Distance(firstPoint, secPoint) < 10f)
                {
                    ps.transform.parent.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                    Destroy(ps);
                    Player.aldreadyClickedCity = null;
                    Player.currentClickedCity = null;
                    if (PlayerPrefs.GetInt("Voice") == 1)
                    {
                        _cityUnselectSounds.GetComponent<AudioSource>().Play(0);
                    }
                }
                //Dost þehir iptal edildi (þehir hariç bir yere týklanýp) (colliderý olmayan)
            }
            

        }
    }
}
/*
private void Click()
{
    GameObject currentClickedCity = Player.currentClickedCity;

    if (Input.GetMouseButtonDown(0) && Player.isPlaying == true)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.transform.gameObject.GetComponent<City>())
            {
                if (currentClickedCity == null && !hit.transform.gameObject.GetComponent<EnemyCity>() && !hit.transform.gameObject.GetComponent<NeutralCity>())
                {
                    Player.currentClickedCity = hit.transform.gameObject;
                    ps = Instantiate(_citySelectEffect, hit.transform.GetChild(0).gameObject.transform.position + new Vector3(0f, 0.15f, 0f),
                        _citySelectEffect.transform.rotation, hit.transform.gameObject.transform);

                    if (PlayerPrefs.GetInt("Voice") == 1)
                    {
                        _cityAllySelectSounds.GetComponent<AudioSource>().Play(0);
                    }
                    hit.transform.gameObject.GetComponent<AllyCity>().Set_isSelected(true);
                    //Dost þehir seçildi
                }
                else if (currentClickedCity != null && hit.transform != Player.currentClickedCity.transform)
                {
                    Player.aldreadyClickedCity = Player.currentClickedCity;
                    Player.currentClickedCity = hit.transform.gameObject;
                    ps.transform.parent.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                    Destroy(ps);

                    if (PlayerPrefs.GetInt("Voice") == 1)
                    {
                        _cityEnemySelectSounds.GetComponent<AudioSource>().Play(0);
                    }
                    //Askerler Gönderildi
                }
                else if (currentClickedCity != null && hit.transform == Player.currentClickedCity.transform)
                {
                    ps.transform.parent.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                    Destroy(ps);

                    Player.aldreadyClickedCity = null;
                    Player.currentClickedCity = null;

                    if (PlayerPrefs.GetInt("Voice") == 1)
                    {
                        _cityUnselectSounds.GetComponent<AudioSource>().Play(0);
                    }

                    hit.transform.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                    //Dost þehir iptal edildi (kendi üzerine týklanýp)
                }
            }
            else if (currentClickedCity != null)
            {
                ps.transform.parent.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
                Destroy(ps);
                Player.aldreadyClickedCity = null;
                Player.currentClickedCity = null;

                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    _cityUnselectSounds.GetComponent<AudioSource>().Play(0);
                }
                //Dost þehir iptal edildi (þehir hariç bir yere týklanýp) (colliderý olan)
            }

        }
        else if (currentClickedCity != null)
        {
            ps.transform.parent.gameObject.GetComponent<AllyCity>().Set_isSelected(false);
            Destroy(ps);
            Player.aldreadyClickedCity = null;
            Player.currentClickedCity = null;

            if (PlayerPrefs.GetInt("Voice") == 1)
            {
                _cityUnselectSounds.GetComponent<AudioSource>().Play(0);
            }
            //Dost þehir iptal edildi (þehir hariç bir yere týklanýp) (colliderý olmayan)
        }

    }
}*/

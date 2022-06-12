using System;
using System.Collections;
using UnityEngine;
public class AllyCity : City
{
    [SerializeField] private GameObject _selectEffect;

    [SerializeField] private GameObject _soldierHitVoice;

    [SerializeField] private bool _isSelected;
    private readonly float _selectEffectTime = 1f;

    private void Awake()
    {
        foreach (Transform i in gameObject.transform)
        {
            if (i.CompareTag("SelectedRing"))
            {
                Destroy(i.gameObject);
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Soldier>() && collision.gameObject.GetComponent<Soldier>().Get_StarterCity() != this.gameObject)
        {
            if (collision.gameObject.GetComponent<AllySoldier>())
            {
                UpdateSoldierAmount(Get_IncreaseAmount());

                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    var sh = Instantiate(_soldierHitVoice);
                    Destroy(sh, 0.3f);
                }
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.GetComponent<EnemySoldier>())
            {
                UpdateSoldierAmount(-collision.gameObject.GetComponent<EnemySoldier>().Get_SoldierPower());
                Set_LastSoldier(collision.gameObject.GetComponent<EnemySoldier>());

                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    var sh = Instantiate(_soldierHitVoice);
                    Destroy(sh, 0.3f);
                }
                Destroy(collision.gameObject);
            }
            DeathEffect();
        }
    }
    private void SelectEffect()
    {
        GameObject ps = Instantiate(_selectEffect, new Vector3(Player.currentClickedCity.transform.position.x , Player.currentClickedCity.transform.position.y + 0.5f
            ,Player.currentClickedCity.transform.position.z), Player.currentClickedCity.transform.rotation);
        Destroy(ps, _selectEffectTime);
    }
    private void DeathEffect()
    {
        GameObject ps = Instantiate(Get_soldierDeathEffect(), transform.position + new Vector3(0f, 0.25f, 0f), transform.rotation);
        Destroy(ps, Get_EffectTime());
    }
    private void Update()
    {
        if (Player.aldreadyClickedCity == this.gameObject && Player.currentClickedCity != null)
        {
            SelectEffect();
            StartCoroutine(StartSpawn(Get_SoldierAmount(), Player.currentClickedCity.transform.position));
            Player.aldreadyClickedCity = null;
            Player.currentClickedCity = null;
        }
    }
    IEnumerator StartSpawn(int amount, Vector3 destination)
    {
        yield return new WaitForSeconds(Get_SoldierSpawnTime());
        if (amount > 1 && Get_SoldierAmount() > 0)
        {
            GameObject temp = Instantiate(Get_Soldier(), transform.GetChild(2).position, Get_Soldier().transform.rotation);
            temp.GetComponent<Soldier>().Set_SoldierDestination(destination);
            temp.GetComponent<Soldier>().Set_StarterCity(this.gameObject);
            UpdateSoldierAmount(-1);
            StartCoroutine(StartSpawn(amount - 1,destination));
        }
    }

    public void Set_isSelected(bool enabled)
    {
       _isSelected = enabled;
    }
    public bool Get_isSelected()
    {
        return _isSelected;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCity : City
{
    public List<GameObject> _cities = new List<GameObject>();
    public List<GameObject> _citiesAlternative = new List<GameObject>();
    public GameObject tempCity;
    public float attackTime;
    public float citySearchTime;

    private Vector3 _tempTargetCity;
    private Vector3 _alternativeTargetCity;
    private int _totalArmy;

    [SerializeField] private GameObject _soldierHitVoice;

    private void Start()
    {
        StartCoroutine(CitySearcher());
        StartCoroutine(CreateNewSoldier(Get_SoldierCreateTime()));
        StartCoroutine(AttackTimer());
    }
    public IEnumerator CitySearcher()
    {
        if (Player.isPlaying == true)
        {
            _cities.Clear();
            _citiesAlternative.Clear();
            foreach (var i in GameObject.FindGameObjectsWithTag("City"))
            {
                if (i.GetComponent<City>().Get_SoldierAmount() + 5 < Get_SoldierAmount() && i.layer != gameObject.layer)
                {
                    _cities.Add(i);
                }
            }
            for (int i = 0; i < _cities.Count; i++)
            {
                for (int k = i + 1; k < _cities.Count; k++)
                {
                    if (Vector3.Distance(_cities[i].transform.position, gameObject.transform.position) > Vector3.Distance(_cities[k].transform.position, gameObject.transform.position))
                    {
                        tempCity = _cities[i];
                        _cities[i] = _cities[k];
                        _cities[k] = tempCity;
                    }
                }
            }
            if (_cities.Count != 0)
            {
                _tempTargetCity = _cities[0].transform.position;
            }
            else if (_cities.Count == 0)
            {
                _totalArmy = Get_SoldierAmount();
                foreach (var j in GameObject.FindGameObjectsWithTag("City"))
                {
                    if (j.layer == gameObject.layer)
                    {
                        _totalArmy += j.GetComponent<City>().Get_SoldierAmount();
                    }
                }
                foreach (var r in GameObject.FindGameObjectsWithTag("City"))
                {
                    if (r.layer != gameObject.layer)
                    {
                        if (r.GetComponent<City>().Get_SoldierAmount() < _totalArmy)
                        {
                            _citiesAlternative.Add(r);
                        }
                    }
                }
                for (int i = 0; i < _citiesAlternative.Count; i++)
                {
                    for (int k = i + 1; k < _citiesAlternative.Count; k++)
                    {
                        if (Vector3.Distance(_citiesAlternative[i].transform.position, gameObject.transform.position) > Vector3.Distance(_citiesAlternative[k].transform.position, gameObject.transform.position))
                        {
                            tempCity = _citiesAlternative[i];
                            _citiesAlternative[i] = _citiesAlternative[k];
                            _citiesAlternative[k] = tempCity;
                        }
                    }
                }
                if (_citiesAlternative.Count != 0)
                {
                    _alternativeTargetCity = _citiesAlternative[0].transform.position;
                }
            }
        }
        yield return new WaitForSeconds(citySearchTime);
        StartCoroutine(CitySearcher());
    }

    public IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(1f);
        if (Player.isPlaying == true)
        {
            yield return new WaitForSeconds(attackTime);
            if (_cities.Count != 0)
            {
                StartCoroutine(Attack(Get_SoldierAmount(), _tempTargetCity));
            }
            else if (_cities.Count == 0 && _citiesAlternative.Count != 0)
            {
                StartCoroutine(Attack(Get_SoldierAmount(), _alternativeTargetCity));
            }
        }
        StartCoroutine(AttackTimer());
    }
    public IEnumerator Attack(int amount, Vector3 destination)
    {
        yield return new WaitForSeconds(Get_SoldierSpawnTime());
        if (amount > 1 && Get_SoldierAmount() > 0 && _tempTargetCity != null)
        {
            GameObject temp = Instantiate(Get_Soldier(), transform.GetChild(2).position, Get_Soldier().transform.rotation);
            temp.GetComponent<Soldier>().Set_SoldierDestination(destination);
            temp.GetComponent<Soldier>().Set_StarterCity(this.gameObject);
            UpdateSoldierAmount(-1);
            if (Get_SoldierAmount() > 0)
            {
                StartCoroutine(Attack(amount - 1, destination));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Soldier>())
        {
            if (collision.gameObject.GetComponent<AllySoldier>())
            {
                if (Get_SoldierAmount() > 0)
                {
                    DecreaseSoldierAmount(collision.gameObject.GetComponent<AllySoldier>().Get_SoldierPower());
                    Set_LastSoldier(collision.gameObject.GetComponent<AllySoldier>());

                    if (PlayerPrefs.GetInt("Voice") == 1)
                    {
                        var sh = Instantiate(_soldierHitVoice);
                        Destroy(sh, 0.3f);
                    }
                    Destroy(collision.gameObject);
                }
            }
            if (collision.gameObject.GetComponent<EnemySoldier>())
            {
                UpdateSoldierAmount(Get_IncreaseAmount());

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
    private void DeathEffect()
    {
        GameObject ps = Instantiate(Get_soldierDeathEffect(), transform.position + new Vector3(0f, 0.25f, 0f), transform.rotation);
        Destroy(ps, Get_EffectTime());
    }
}

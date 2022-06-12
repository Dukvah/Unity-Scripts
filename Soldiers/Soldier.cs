using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Soldier : MonoBehaviour
{
    #region VAR
    [SerializeField] private float _soldierSpeed;
    [SerializeField] private Vector3 _soldierDestination;
    [SerializeField] private int _soldierPower;
    [SerializeField] private GameObject _starterCity;
    
    //Effects
    [SerializeField] private GameObject _deathEffect;
    [SerializeField] private GameObject _cityDestroyEffect;
    [SerializeField] private GameObject _stepDustEffect;

    //Sounds
    [SerializeField] private GameObject _soldierHit;
    [SerializeField] private GameObject _cityConq;
    [SerializeField] private GameObject _cityLose;


    private readonly float _stepDustEffectTime = 0.5f;
    private readonly float _soldierDeathEffectTime = 2f;
    private readonly float _cityDestroyEffectTime = 3f;


    private NavMeshAgent _navMeshAgent;
    private GameObject _UIMenu;

    private List<GameObject> _cityList = new List<GameObject>();
    private List<GameObject> _allyCityList = new List<GameObject>();
    #endregion

    #region Unity Methods
    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.destination = _soldierDestination;
        _navMeshAgent.speed = _soldierSpeed;
        StartCoroutine(StepEffect());
        _UIMenu = GameObject.Find("UI Menus");

        this.gameObject.GetComponent<NavMeshAgent>().updateRotation = false;
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(this.gameObject.GetComponent<NavMeshAgent>().velocity.normalized);
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<City>() && (collision.gameObject != _starterCity) && (collision.gameObject.GetComponent<City>().Get_SoldierAmount() == 0 || collision.gameObject.GetComponent<City>().Get_SoldierAmount() < 0))
        {
            GameObject temp = Instantiate(Get_StarterCity(),collision.gameObject.transform.position, collision.gameObject.transform.rotation);
            temp.GetComponent<City>().Set_SoldierAmount(1);
            temp.transform.GetChild(0).gameObject.SetActive(false);

            if (collision.gameObject.GetComponent<AllyCity>())
            {
                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    var sh = Instantiate(_cityLose);
                    Destroy(sh, 0.5f);
                } 
            }
            if (!collision.gameObject.GetComponent<AllyCity>())
            {
                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    var sh = Instantiate(_cityConq);
                    Destroy(sh, 0.5f);
                }
            }

            CityDestroyEffect();
            Destroy(collision.gameObject);

            _allyCityList.Clear();
            _cityList.Clear();
            foreach (var obj in GameObject.FindGameObjectsWithTag("City"))
            { 
               if (obj != collision.gameObject)
               {
                    _cityList.Add(obj);
               }
            }
            foreach (var j in _cityList)
            {
                if (j.GetComponent<AllyCity>())
                {
                    _allyCityList.Add(j);
                }
            }

            if (_allyCityList.Count == _cityList.Count)  //Level Win
            {
                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    _UIMenu.GetComponent<MenuManager>().Get_VictoryMenu().GetComponent<AudioSource>().volume = 1;
                }
                if (PlayerPrefs.GetInt("Voice") == 0)
                {
                    _UIMenu.GetComponent<MenuManager>().Get_VictoryMenu().GetComponent<AudioSource>().volume = 0;
                }
                _UIMenu.GetComponent<MenuManager>().CalculateStars();
                _UIMenu.GetComponent<MenuManager>().Get_InGameMenu().SetActive(false);
                _UIMenu.GetComponent<MenuManager>().Get_VictoryMenu().SetActive(true);
                Player.isPlaying = false;
            }
            else if (_allyCityList.Count == 0)     // Level Lose
            {
                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    _UIMenu.GetComponent<MenuManager>().Get_DefeatMenu().GetComponent<AudioSource>().volume = 1;
                }
                if (PlayerPrefs.GetInt("Voice") == 0)
                {
                    _UIMenu.GetComponent<MenuManager>().Get_DefeatMenu().GetComponent<AudioSource>().volume = 0;
                }

                _UIMenu.GetComponent<MenuManager>().Get_InGameMenu().SetActive(false);
                _UIMenu.GetComponent<MenuManager>().Get_DefeatMenu().SetActive(true);
                
                Player.isPlaying = false;
            }
            else   // A City Conquered
            {
                Debug.Log("DENGELER DEÐÝÞTÝ OYUN DEVAM EDÝYOR");
            }

        }
        else if (collision.gameObject.GetComponent<Soldier>() && collision.gameObject.layer != this.gameObject.layer) //Farklý renkteki askerler birbirlerine çarptýðýnda yok olur
        {
            GameObject ps = Instantiate(_deathEffect, collision.GetContact(0).point, transform.GetChild(0).transform.rotation);
            Destroy(ps, _soldierDeathEffectTime);
            if (!GetComponent<EnemySoldier>())
            {
                if (PlayerPrefs.GetInt("Voice") == 1)
                {
                    var sh = Instantiate(_soldierHit);
                    Destroy(sh, 0.3f);
                }              
            }
            Destroy(collision.gameObject);
            Destroy(this.gameObject);  
        }
    }


    #endregion

    #region Effects
    private void CityDestroyEffect()
    {
        GameObject ps = Instantiate(_cityDestroyEffect, transform.position + new Vector3(0f, 0.75f, 0f), transform.rotation);
        ps.transform.position = this.gameObject.transform.position;
        Destroy(ps, _cityDestroyEffectTime);
    }
    private IEnumerator StepEffect()
    {
        GameObject ps = Instantiate(_stepDustEffect, transform.GetChild(0).transform.position, transform.GetChild(0).transform.rotation);
        Destroy(ps, _stepDustEffectTime);

        yield return new WaitForSeconds(0.4f);
        StartCoroutine(StepEffect());  
    }
    #endregion

    #region Getter & Setter
    public float Get_SoldierSpeed()
    {
        return _soldierSpeed;
    }
    public Vector3 Get_SoldierDestination()
    {
        return _soldierDestination;
    }
    public int Get_SoldierPower()
    {
        return _soldierPower;
    }
    public GameObject Get_StarterCity()
    {
        return _starterCity;
    }
    public void Set_StarterCity(GameObject _starterCity)
    {
        this._starterCity = _starterCity;
    }

    public void Set_SoldierDestination(Vector3 _soldierDestination)
    {
        this._soldierDestination = _soldierDestination;
    }
    public void Set_SoldierSpeed(float _soldierSpeed)
    {
        this._soldierSpeed = _soldierSpeed;
    }

    #endregion
}

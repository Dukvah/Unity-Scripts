using System.Collections;
using UnityEngine;

public class City : MonoBehaviour
{
    #region Var
    [SerializeField] private int _soldierAmount;
    [SerializeField] private int _increaseAmount;
    [SerializeField] private float _soldierCreateTime;
    [SerializeField] private float _soldierSpawnTime;
    [SerializeField] private int _maxSoldierAmount;
    [SerializeField] private GameObject _soldier;
    [SerializeField] private Soldier _lastSoldier;
    [SerializeField] private GameObject _soldierDeathEffect;
    private readonly float _effectTime = 0.6f;
    #endregion

    #region Getters
    public int Get_SoldierAmount()
    {
        return _soldierAmount;
    }
    public int Get_MaxSoldierAmount()
    {
        return _maxSoldierAmount;
    }
    public int Get_IncreaseAmount()
    {
        return _increaseAmount;
    }
    public float Get_SoldierCreateTime()
    {
        return _soldierCreateTime;
    }
    public float Get_SoldierSpawnTime()
    {
        return _soldierSpawnTime;
    }
    public float Get_EffectTime()
    {
        return _effectTime;
    }

    public GameObject Get_Soldier()
    {
        return _soldier;
    }
    public GameObject Get_soldierDeathEffect()
    {
        return _soldierDeathEffect;
    }
    public Soldier Get_LastSoldier()
    {
        return _lastSoldier;
    }
    #endregion

    #region Setters
    public void Set_SoldierAmount(int _soldierAmount)
    {
        this._soldierAmount = _soldierAmount;
    }
    private void Set_MaxSoldierAmount(int _maxSoldierAmount)
    {
        this._maxSoldierAmount = _maxSoldierAmount;
    }
    public void Set_LastSoldier(Soldier _lastSoldier)
    {
        this._lastSoldier = _lastSoldier;
    }
    #endregion

    #region Unity Metods
    private void Start()
    {
        StartCoroutine(CreateNewSoldier(_soldierCreateTime));
    }
    #endregion

    #region My Update Metods
    public void UpdateSoldierAmount(int increaseAmount)
    {
        if (Get_MaxSoldierAmount() >= Get_SoldierAmount() + increaseAmount)
        {
            Set_SoldierAmount(Get_SoldierAmount() + increaseAmount);
        }
    }
    public void DecreaseSoldierAmount(int decreaseAmount)
    {
        if (Get_SoldierAmount() >= 0)
        {
            Set_SoldierAmount(Get_SoldierAmount() - decreaseAmount);
        }
    }
    public IEnumerator CreateNewSoldier(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (Player.isPlaying == true)
        {
            UpdateSoldierAmount(_increaseAmount);
        }
        StartCoroutine(CreateNewSoldier(waitTime));
    }
    #endregion
}

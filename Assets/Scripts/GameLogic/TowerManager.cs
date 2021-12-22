﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 塔的逻辑管理器
/// </summary>
public class TowerManager : MonoBehaviour
{
    //塔个体对象prefab
    public List<GameObject> _towerPrefabs;

    public Dictionary<GameObject, GameObject> base2tower = new Dictionary<GameObject, GameObject>();
    //游戏场景里的塔基
    private GameObject[] _towerBases;

    //塔的数据库
    private TowerDataBase _towerDataBase;

    //金钱
    private MoneyManager _moneyManager;

    //消息系统
    private MessageSystem messageSystem;

    //塔基对应是否有塔
    private Dictionary<GameObject, bool> _towerBaseHasTower = new Dictionary<GameObject, bool>();

    [HideInInspector]
    private GameObject targetTowerBase = null;

    public GameObject TargetTowerBase { get => targetTowerBase; set => targetTowerBase = value; }

    private void Awake()
    {
        _towerBases = GameObject.FindGameObjectsWithTag("TowerBase");

        _moneyManager = GetComponent<MoneyManager>();

        //初始化默认都没有塔
        for (int i = 0; i < _towerBases.Length; ++i)
        {
            _towerBaseHasTower.Add(_towerBases[i], false);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        messageSystem = GetComponent<MessageSystem>();   
        messageSystem.registerDieEvent(RecoverTowerBase);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //检测是否可以造塔
    public bool IfCanBuildTower()
    {
        if (_towerBaseHasTower.ContainsKey(TargetTowerBase))
        {
            return !_towerBaseHasTower[TargetTowerBase];
        }
        Logger.Log("TowerBase Doesn't exist!", LogType.Tower);
        return false;
    }

    //检测是否可以造某种塔(取决塔的造价)
    public bool IfCanBuildTower(int towerIndex)
    {
        if (!IfCanBuildTower())
        {
            return false;
        }

        //目前而言都是50元 造塔
        //TODO:检测塔 钱
        if(_moneyManager.Cash < 50)
        {
            return false;
        }

        switch (towerIndex)
        {
            case 0:
                if(_moneyManager.Cash<50)
                    return false;
                break;
            case 1:
                if (_moneyManager.Cash < 100)
                    return false;
                break;
            case 2:
                if (_moneyManager.Cash < 50)
                    return false;
                break;
        }

        return true;
    }

    //建造塔
    public void BuildTower(int towerIndex)
    {
        if (!IfCanBuildTower(towerIndex))
        {
            Logger.Log("Can't Build Tower!", LogType.Tower);
            return;
        }

        //TODO：建造一个塔，耗费50元
        switch (towerIndex)
        {
            case 0:
                _moneyManager.ReduceCash(50);
                break;
            case 1:
                _moneyManager.ReduceCash(100);
                break;
            case 2:
                _moneyManager.ReduceCash(50);
                break;
        }

        _towerBaseHasTower[TargetTowerBase] = true;
        var tower = Instantiate(_towerPrefabs[towerIndex], TargetTowerBase.transform);
        base2tower.Add(tower, TargetTowerBase);
        Logger.Log("Succeessfully Build A Tower.", LogType.Tower);
    }

    public void DestroyTower(GameObject t)
    {

        _towerBaseHasTower[t] = false;
        Logger.Log("Succeessfully recover A TowerBase.", LogType.Tower);
    }
    public void RecoverTowerBase(int individualID)
    {
        //只有人类势力个体恢复塔基
        if (Factory.GetIndividual(individualID).power != Individual.Power.Human) return;

        //恢复塔基
        DestroyTower(base2tower[Factory.GetIndividual(individualID).gameObject]);
    }


}

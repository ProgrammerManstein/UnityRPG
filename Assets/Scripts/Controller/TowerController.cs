using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
public class TowerController : BaseIndividualController
{
    private Individual selfIndividual;
    private new Rigidbody rigidbody;
    private CapsuleCollider capsuleCollider;
    private BehaviorTree behaviorTree;
    void Awake()
    {
        selfIndividual = GetComponent<Individual>();
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }
    private void Start()
    {
        InitRegister();
    }

    public override void Walk(Vector3 velocity)
    {
        //塔不可行走
        throw new System.NotImplementedException();
    }

    public override void GetDamaged(int sourceID, float damage)
    {
        selfIndividual.HealthChange(-damage);
        //生命值少于0，调用死亡行为
        if (selfIndividual.health < 0)
        {
            Die();
        }
    }

    public override void Attack(Individual ind)
    {
        //塔不可攻击
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        gameObject.SetActive(false);
        //关闭脚本
        selfIndividual.enabled = false;
        messageSystem.enabled = false;
        this.enabled = false;
        //发出死亡消息
        messageSystem.SendMessage(3, 0, 0);
    }
}


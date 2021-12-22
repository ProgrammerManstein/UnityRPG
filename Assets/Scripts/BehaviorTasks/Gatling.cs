﻿using System.Collections;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("直射子弹行为")]
    [TaskCategory("Straight Gatling")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}gatlingIcon.png")]
    public class Gatling : Action
    {
        public SharedTransform target;
        public GameObject bullet;
        public GameObject bulletPoint;
        public float bulletSpeed;
        public float attackRate;

        private Individual master;          //寄主
        private bool attacking = false;     //攻击正在进行中

        IEnumerator Attack()
        {
            attacking = true;//开始攻击

            GameObject bulletObj = GameObject.Instantiate(bullet,
                bulletPoint.transform.position, Quaternion.identity);

            //给子弹对象脚本赋值
            bulletObj.GetComponent<BulletTriggerEvent>().tower = master;

            Vector3 fireDirection = target.Value.position - bulletPoint.transform.position;
            bulletObj.GetComponent<Rigidbody>().velocity = transform.TransformDirection
                (fireDirection * bulletSpeed);

            yield return new WaitForSeconds(attackRate);

            attacking = false;//可以准备下一次攻击了
        }

        public override void OnStart()
        {
            master = gameObject.GetComponent<Individual>();
            //初始化寄主的攻击速度 攻击间隔 = 1s / 攻击速度
            attackRate = 1.0f / master.GetComponent<Individual>().attackSpeed;
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null)
            {
                return TaskStatus.Failure;
            }

            //无需检测目标对象是否正确，CanSeeMonster已经过滤掉非Monster对象
            //若没在进行攻击，则可以进行一次攻击行为
            if (!attacking)
            {
                StartCoroutine(Attack());
            }
            return TaskStatus.Success;
        }
    }
}


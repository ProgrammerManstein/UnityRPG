using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{    
    //Ӣ�ۼ��ܶ����б�
    List<ISkill> heroSkills = new List<ISkill>();
    public List<ISkill> HeroSkills { get => heroSkills; set => heroSkills = value; }

    private SkillEffectManager skillEffectManager;
    
    ////װ�����ܶ����б�
    //List<ISkill> EquipmentSkill = new List<ISkill>();

    private Individual selfIndividual;
    private MessageSystem messageSystem;

    private void Awake()
    {
        selfIndividual = GetComponent<Individual>();
        messageSystem = GetComponent<MessageSystem>();
        //TODO
        //ĿǰӲ�������Ҹ���2������
        HeroSkills.Add(new BuffSkill(6, true, true, 5.0f));   //�������ܣ�����Buff
        HeroSkills.Add(new BuffSkill(14, true, false));       //�������ܣ����ٽ�ָbuff

        skillEffectManager = GameObject.FindGameObjectWithTag("Effects").GetComponent<SkillEffectManager>();
    }


    void Start()
    {
        foreach (ISkill skill in HeroSkills)
        {
            skill.InitSkill(selfIndividual);
        }

        //������Ϣ
        messageSystem.registerDieEvent((int sender) => { if (sender == selfIndividual.ID) { this.enabled = false; } });
    }

    // Update is called once per frame
    void Update()
    {
        foreach(ISkill skill in HeroSkills)
        {
            skill.UpdateSkill(selfIndividual);
        }
    } 

    /// <summary>
    /// �ͷż���
    /// </summary>
    /// <param name="index"></param>
    public void ReleaseSkill(int index)
    {
        Logger.Log("Release Skill " + index , LogType.Skill);

        if (!selfIndividual.enabled)
            return;

        if (index >= HeroSkills.Count){ return; }

        if (HeroSkills[index].IsColdTimeEnd())
        {
            HeroSkills[index].ReleaseSkill(selfIndividual);
            skillEffectManager.PlayEffect(transform, index);
        }
    }

    public void ReceiveMessage(Individual attacker,float damage)
    {
        //Ϊ������ԣ���ʱʲôҲ����
        //foreach (var skill in HeroSkill)
        //{
        //    skill.DealAttackMessage(attacker, damage);
        //}
        //foreach (var skill in EquipmentSkill)
        //{
        //    skill.DealAttackMessage(attacker, damage);
        //}
    }
}

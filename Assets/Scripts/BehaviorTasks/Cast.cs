using System.Collections;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("�����ӵ���Ϊ")]
    [TaskCategory("Cast")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}gatlingIcon.png")]
    public class Cast : Action
    {
        public SharedTransform target;
        public GameObject bullet;
        public GameObject bulletPoint;
        public float bulletSpeed;
        public float attackRate;

        private Individual master;          //����
        private bool attacking = false;     //�������ڽ�����
        private float gravity = 9.8f;       //����

        IEnumerator Attack()
        {
            attacking = true;//��ʼ����

            GameObject bulletObj = GameObject.Instantiate(bullet,
                bulletPoint.transform.position, Quaternion.identity);

            //���ӵ�����ű���ֵ
            bulletObj.GetComponent<BulletTriggerEvent>().tower = master;

            Vector3 dt = target.Value.position - bulletPoint.transform.position;

            float distance = Mathf.Sqrt(dt.x * dt.x + dt.z * dt.z);
            float time = distance / bulletSpeed;

            float dh = Mathf.Abs(dt.y);

            float vh = 2.0f * dh / time +  gravity * time / 4.0f;

            Vector2 vv = new Vector2(dt.x, dt.z);
            vv.Normalize();
            vv *= bulletSpeed;

            Vector3 fireVelocity = new Vector3(vv.x,vh,vv.y);

            bulletObj.GetComponent<Rigidbody>().velocity = transform.TransformDirection(fireVelocity);

            yield return new WaitForSeconds(attackRate);

            attacking = false;//����׼����һ�ι�����
        }

        public override void OnStart()
        {
            master = gameObject.GetComponent<Individual>();
            //��ʼ�������Ĺ����ٶ� ������� = 1s / �����ٶ�
            attackRate = 1.0f / master.GetComponent<Individual>().attackSpeed;
        }

        public override TaskStatus OnUpdate()
        {
            if (target.Value == null)
            {
                return TaskStatus.Failure;
            }

            //������Ŀ������Ƿ���ȷ��CanSeeMonster�Ѿ����˵���Monster����
            //��û�ڽ��й���������Խ���һ�ι�����Ϊ
            if (!attacking)
            {
                StartCoroutine(Attack());
            }
            return TaskStatus.Success;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicCube : MonoBehaviour
{
    // Start is called before the first frame update

        //自身属性
    public int hp;
    public int totalhp;
    public float cd;//技能冷却时间
    private double cdTimer;//cd计时器
    //public double resistance;//伤害抗性
    public float speed;
    //public bool activated;

    //武器属性
    public int damage;//平均伤害
    public int damageChange;//伤害变化范围
    public double timer;//射击时间间隔
    private double countTimer;
    public float  AccurancyRange;//射击范围
    //private RaycastHit e_hit;

    //射击部分
    private GameObject Controller;//获取主角对象
    public int shootRange;//射程
    private RaycastHit beHit;//判断射击目标
   //public float shootTimer;//射击瞄准间隔

    //技能部分
    private float cubeRadius;//方块直径，起名的时候本来想用半径来着，后来发现直径更简单，没法改了
    private RaycastHit jumpTo;//跳跃判定射线击中目标

    //AI部分
    private double beShotTimes;//被射击次数
    private RaycastHit[] moveToTry;//以射线判定的方式决定移动方向
    private RaycastHit moveTo;
    public float moveTimer;//移动改方向平均间隔时间
    public float moveTimerChange;//改方向变化
    private float countMoveTimer;//计时器
    //private Vector3 moveToPoint;//移动目标点
    //private float viewAngle;//判断方块相对人物位置
    private Vector3 positionDir;//方块相对人物方向向量
    private bool shooting;//是否射击模式
    private bool moving;
    private bool backShooting;//是否偷袭模式
    void Start()
    {
        cubeRadius = gameObject.GetComponent<MeshFilter>().mesh.bounds.size.x;//获得水平方向直径（左端点到右端点）
        //activated = false;
        cdTimer = cd;
        beShotTimes = 0;//被攻击一次加一
        Controller = GameObject.FindWithTag("controller");
        positionDir = this.transform.position - Controller.transform.position;
        countTimer = 0;
        countMoveTimer = 0;
        shooting = false;
        moving = false;
        backShooting = false;
    }
    // Update is called once per frame
    void Update()
    {
        cdTimer -= Time.deltaTime;//cd时间衰减
        //主角屁股对我？射他！
        if (GetAngle() > 150)
        {
            shooting = true;
            backShooting = true;
        }
        //cd快到了，主角半个屁股对我，这还不射？
        else if (GetAngle() > 110 && cdTimer < 0.75)
        {
            shooting = true;
        }
        //cd又短了一点，主角看不到我，射他！
        else if (GetAngle() > 45 && cdTimer < 0.5)
        {
            shooting = true;
        }
        //这一点时间你能做个锤子？射他！
        else if (cdTimer < 0.25)
        {
            shooting = true;
        }
        //解释一下，之前是直接放的shoot函数在上面的分支里，但是有了移动之后就可能发生由于移动速度较快，在临界值0.25s内转换分支导致射击停止。
        //而实际上一旦触发三个else中任意一个都应该保持射击直到发动技能，所以引入shooting并采用下面判断持续射击
        if(shooting)
        {
            Shoot();
        }
        if(GetAngle ()<150&&backShooting )
        {
            backShooting = false;
            shooting = false;
            countTimer = 0;//这是唯一一个射到一半可能停下的分支，为了避免停下对射击间隔计时和被发现判定的影响，引入判定
        }
        //三十六计，走为上计，hit&run，我跑！
        if(cdTimer <=0)
        {
            JumpHide();
            beShotTimes = 0;//刷新被击次数
            cdTimer = cd;//刷新cd
        }
    }
    private void FixedUpdate()
    {//妈耶，被发现了，那个人射我，快跑！
        if (beShotTimes > 5)
        {
            Shoot();//边跑边射
            MoveMeantime();//跑
        }
        else if(shooting&&beShotTimes >=1)
        {
            //shooting==1意味着已经update射击了，再加一个shoot没有必要
            MoveMeantime();//跑
        }
    }

        //使用技能
    void JumpHide()
    {//这个技能使用不考虑重力，上天入地皆可贴
        gameObject.GetComponent<Rigidbody>().useGravity = false;//去掉重力
        
        if (Physics.Raycast(this.transform.position, RandVector3(), out jumpTo,shootRange,1))
        {
            //射到找到目的地才行，这个不能有半点损失，否则小命不保
            while (!(jumpTo.transform.CompareTag("box")))
            {
                Physics.Raycast(this.transform.position, RandVector3(), out jumpTo, shootRange,1);
            }
            //找到目的地瞬移，按照目的地点法线方向移动到目的地中心偏移一个直径
            this.transform.position = jumpTo.transform.position + (cubeRadius * jumpTo.normal);
        }
        //刷新各个统计值
        shooting = false;
        moving = false;
        countMoveTimer = 0;
        countTimer = 0;
    }

    //射击
    void Shoot()
    {
        shooting = true;
        //判断射击间隔
        if (countTimer  <= 0)
        {
            //射击方向判定
            Vector3 shootDir;
            //尝试射击以保证可以射中（见函数）
            shootDir = TryShoot();
            if (shootDir != Vector3.zero)
            {
                //目标方向加一个偏移量
                shootDir = shootDir + new Vector3(Random.Range(-AccurancyRange, AccurancyRange), Random.Range(-AccurancyRange, AccurancyRange), 0);
                if (Physics.Raycast(transform.position, shootDir, out beHit, shootRange))
                {
                    //如果射中
                    if (beHit.transform.CompareTag("controller"))
                    {
                        //伤害波动
                        Controller.GetComponent<Controller>().hp -= damage + Random.Range(-damageChange, damageChange);
                    }
                }
                //加载射击间隔
                countTimer = timer;
            }
            else
            {
                countTimer = 0;//如果试射失败，不耗费射击次数
            }
        }
        else
        {
            countTimer -= Time.deltaTime;
        }
    }

    //移动代码
    void MoveMeantime()
    {
        moving = true;
        //移动时加上重力，实现“击落”效果，且抛物线加大射击难度
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        //移动方向
        Vector3 moveDir;
        //判定是否重新规划方向
        if(countMoveTimer <=0)
        {
            //5条射线寻求较好移动方向
            for (int i = 0; i < 5; i++)
            {
                //随机方向
                moveDir = RandVector3();
                //这里，由于重力影响，竖直方向最多就是跳跃效果。当然跳跃效果可以有效起到增强灵活性的功能，但是跳跃高度太低一段时间一跳就跟个傻子一样……
                //那样只能说明程序写的有问题，不是特性。
                if(moveDir .y<50)
                {
                    moveDir.y = 0;
                }
                else
                {
                    moveDir.y = 90;
                }
               /* while(!(Vector3 .Angle ( moveDir,positionDir )>45&& Vector3.Angle(moveDir, positionDir) <150))
                {
                    moveDir = RandVector3();
                }*/
                Physics.Raycast(this.transform.position, moveDir, out moveToTry[i]);
            }
            moveTo = moveToTry[0];
            for (int i=1;i<5;i++)
            {
                if(moveToTry [i].distance >moveTo .distance )
                {
                    moveTo = moveToTry[i];
                }
            }
            gameObject.GetComponent<Rigidbody>().velocity = speed * (moveTo.point - this.transform.position);
            countMoveTimer = moveTimer + Random.Range(-moveTimerChange, moveTimerChange);
        }
        else
        {
            countMoveTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(moving )
        {
            countMoveTimer = 0;
        }
    }

    Vector3 RandVector3()
    {
        return new Vector3(Random.value, Random.value, Random.value);
    }

    float GetAngle()
    {
        return Mathf.Abs(Vector3.Angle(-Controller.transform.forward, positionDir));
    }

    private Vector3 TryShoot()
    {
        float randx, randy;
        Vector3 shootDir;
        if(Physics .Raycast (this.transform .position,positionDir ,out beHit ,shootRange))
        {
            if(beHit .collider .tag=="Controller")
            {
                return positionDir;
            }
        }
        else
        {
            for(int i=0;i<5;i++)
            {
                randx = 0.5f * Random.value * Controller.GetComponent<MeshFilter>().mesh.bounds.size.x;
                randy = 0.5f * Random.value * Controller.GetComponent<MeshFilter>().mesh.bounds.size.y;
                shootDir = positionDir +new Vector3(randx, 0, 0) +new Vector3(0, randy, 0);
                if (Physics.Raycast(this.transform.position, shootDir , out beHit, shootRange))
                {
                    if (beHit.collider.tag == "Controller")
                    {
                        return shootDir;
                    }
                }
            }
        }
        return new Vector3(0, 0, 0);
    }
}

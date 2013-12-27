using UnityEngine;
using System.Collections;

public class ActorAttack
{
    public ActorAttackType actorAttackType;                             //攻击类型
    public int affectedDistance;                                                     //攻击范围
    public GameObject affectedObjects;                                     //受影响物体
    public int harmPerAttack;                                                        //每次攻击的伤害
    public int stunProbability;                                                         //眩晕几率
    public int stunDuration;                                                            //眩晕持续时长
    public int increaseFriendlyForcesArmor;                              //增加友军护甲
    public int increaseFriendlyForcesArmorDistance;               //增加友军护甲的影响范围
    public bool resurrection;                                                          //使目标单位复活
}


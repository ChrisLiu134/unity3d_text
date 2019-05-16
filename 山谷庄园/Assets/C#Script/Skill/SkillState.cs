using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能状态
/// </summary>
public class SkillState {

    /// <summary>
    /// 状态名称
    /// </summary>
    public string name;

    /// <summary>
    /// 状态简介
    /// </summary>
    public string info;

    /// <summary>
    /// 状态贴图
    /// </summary>
    public Sprite stateSpr;

    /// <summary>
    /// 状态包含的技能
    /// </summary>
    public Skill buffskill;

    /// <summary>
    /// 持续时间
    /// </summary>
    public int time;

    /// <summary>
    /// 计算cd时间
    /// </summary>
    public float cd_time;

    /// <summary>
    /// 是不是有状态
    /// </summary>
    public bool haveState;

}

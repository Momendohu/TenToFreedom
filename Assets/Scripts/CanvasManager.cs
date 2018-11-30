using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
    //=============================================================
    private Player player;
    private Image jiyuuSkillGauge;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        player = GameObject.Find("Player").GetComponent<Player>();
        jiyuuSkillGauge = transform.Find("Jiyuu/SkillGauge").GetComponent<Image>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        //アクション待機時間を適用
        jiyuuSkillGauge.fillAmount = Mathf.Clamp01(player.ActionWaitTime);
    }
}
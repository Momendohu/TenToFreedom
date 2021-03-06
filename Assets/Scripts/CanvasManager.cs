﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
    //=============================================================
    private Player player;
    private GameObject jiyuu;
    private Image jiyuuSkillGauge;
    private Text yuPointText;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        player = GameObject.Find("Player").GetComponent<Player>();
        jiyuu = transform.Find("Jiyuu").gameObject;
        jiyuuSkillGauge = transform.Find("Jiyuu/SkillGauge").GetComponent<Image>();
        yuPointText = transform.Find("Jiyuu/Image/Text").GetComponent<Text>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    //=============================================================
    /// <summary>
    /// 「ゆ」ポイントを表示に適用する
    /// </summary>
    public void ApplyYuPointText (int num) {
        yuPointText.text = ""+num;
    }

    //=============================================================
    /// <summary>
    /// アクション待機時間を適用
    /// </summary>
    /// <param name="time">パラメータ</param>
    public void ApplySkillGauge (float time) {
        jiyuuSkillGauge.fillAmount = Mathf.Clamp01(time);
    }

    //=============================================================
    /// <summary>
    /// uiのアニメーション
    /// </summary>
    /// <param name="timeLength">発動時間</param>
    /// <param name="size">大きさ</param>
    /// <returns></returns>
    public IEnumerator JiyuuUIAnim1 (float timeLength,float size) {
        float time = 0;
        while(time < 1) {
            time += Time.fixedDeltaTime / timeLength;

            jiyuu.transform.localScale = Vector3.one * (1 + Mathf.Sin(Mathf.Deg2Rad * time * 180) * size);
            yield return null;
        }
    }
}
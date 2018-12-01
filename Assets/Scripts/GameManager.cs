using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    /// <summary>
    /// パラメーター(大域的な)
    /// </summary>
    public struct Parameter {
        public int YuPoint;
    }

    public Parameter parameter = new Parameter {
        YuPoint = 10,
    };

    //=============================================================
    private CanvasManager canvasManager;
    private SoundManager soundManager;

    private void Awake () {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Start () {
        soundManager.TriggerBGM("BGM001",true);
    }

    private void Update () {
        canvasManager.ApplyYuPointText(parameter.YuPoint);
    }

    //=============================================================
    /// <summary>
    /// 「ゆ」ポイントに数値を適用する
    /// </summary>
    /// <param name="num"></param>
    public void ApplyYuPoint (int num) {
        parameter.YuPoint += num;
    }
}
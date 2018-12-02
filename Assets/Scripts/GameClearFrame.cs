using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClearFrame : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private GameObject above;
    private GameObject under;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        above = transform.Find("FrameAbove").gameObject;
        under = transform.Find("FrameUnder").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        above.SetActive(false);
        under.SetActive(false);
    }

    private void Update () {
        if(gameManager.GameClearFlag) {
            above.SetActive(true);
            under.SetActive(true);

            under.transform.Find("Text").GetComponent<Text>().text = "ゆ ポイント " + gameManager.parameter.YuPoint;
        }
    }
}
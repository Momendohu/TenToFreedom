using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    /// <summary>
    /// パラメーター(大域的な)
    /// </summary>
    public struct Parameter {
        public int YuPoint;
    }

    public Parameter parameter = new Parameter {
        YuPoint = 1,
    };

    //ゲームスタート演習終了フラグ
    private bool gameStartEndedFlag;
    public bool GameStartEndedFlag {
        get { return gameStartEndedFlag; }
        set { gameStartEndedFlag = value; }

    }

    //ゲームスタートフラグ
    private bool gameStartFlag;
    public bool GameStartFlag {
        get { return gameStartFlag; }
        set { gameStartFlag = value; }
    }

    //ゲームクリアフラグ
    private bool gameClearFlag;
    public bool GameClearFlag {
        get { return gameClearFlag; }
        set { gameClearFlag = value; }
    }

    //ボス出現フラグ
    private bool bossAppearFlag;
    public bool BossAppearFlag {
        get { return bossAppearFlag; }
        set { bossAppearFlag = value; }
    }

    //ボス撃破フラグ
    private bool bossDefeatFlag;
    public bool BossDefeatFlag {
        get { return bossDefeatFlag; }
        set { bossDefeatFlag = value; }
    }

    private bool gameStartOnce;
    private bool gameCrearOnce;
    private bool bossAppearOnce;
    private bool bossDefeatOnce;

    //=============================================================
    private CanvasManager canvasManager;
    private SoundManager soundManager;
    private GameObject superOri1; //ボス用オリの管理のため
    private GameObject superOri2; //ボス用オリの管理のため

    private void Awake () {
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        superOri1 = GameObject.Find("Field/Object/SuperOris/SuperOri1");
        superOri2 = GameObject.Find("Field/Object/SuperOris/SuperOri2");
    }

    private void Start () {
        superOri1.SetActive(false);
    }

    private void Update () {
        if(gameStartFlag) {
            if(!gameStartOnce) {
                soundManager.TriggerBGM("BGM001",true);
                gameStartOnce = true;
            }

            canvasManager.ApplyYuPointText(parameter.YuPoint);

            //ボスフラグ発生時一回だけ処理
            if(bossAppearFlag) {
                if(!bossAppearOnce) {
                    StartCoroutine(BossAppearPerform());
                    bossAppearOnce = true;
                }
            }

            //ボスフラグ発生時一回だけ処理
            if(bossDefeatFlag) {
                if(!bossDefeatOnce) {
                    StartCoroutine(BossDefeatPerform());
                    superOri1.SetActive(false);
                    superOri2.SetActive(false);
                    bossDefeatOnce = true;
                }
            }
        } else {
            if(InputController.IsPushButtonDown(KeyCode.Space)) {
                soundManager.TriggerSE("SE013");
                gameStartFlag = true;
            }
        }

        if(gameClearFlag) {
            //ゲームクリア時一回だけ処理
            if(!gameCrearOnce) {
                //スコアのランキングへの適用、ランキング表示
                naichilab.RankingLoader.Instance.SendScoreAndShowRanking(parameter.YuPoint);

                gameCrearOnce = true;
            }

            if(InputController.IsPushButtonDown(KeyCode.Space)) {
                soundManager.StopBGM("BGM001");
                SceneManager.LoadScene("Title");
            }
        }
    }

    //=============================================================
    private IEnumerator BossAppearPerform () {
        CreateBoss(new Vector3(531.4f,9,0));
        superOri1.SetActive(true);
        soundManager.TriggerSE("SE012");

        yield return null;
    }

    //=============================================================
    private IEnumerator BossDefeatPerform () {
        soundManager.TriggerSE("SE012");

        yield return null;
    }

    //=============================================================
    /// <summary>
    /// ボスの作成
    /// </summary>
    private void CreateBoss (Vector3 pos) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Enemy"),pos,Quaternion.identity) as GameObject;
        obj.GetComponent<Enemy>().Id = 5;
    }

    //=============================================================
    /// <summary>
    /// 「ゆ」ポイントに数値を適用する
    /// </summary>
    /// <param name="num"></param>
    public void ApplyYuPoint (int num) {
        parameter.YuPoint += num;
        if(parameter.YuPoint < 0) {
            parameter.YuPoint = 0;
        }
    }
}
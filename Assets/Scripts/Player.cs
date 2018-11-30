using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Player : MonoBehaviour {
    //=============================================================
    private TimeSlowModule tsm;
    private CanvasManager canvasManager;

    private Rigidbody2D _rigidbody2D;
    private GameObject eye;
    private SoundManager soundManager;

    private float gravityScale = 2; //重力

    private Vector3 speed = Vector3.zero; //移動スピード(ジャンプ含む)

    private float maxSpeedX = 0.3f; //最大横速度
    private Vector3 acc = new Vector3(0.2f,0,0); //加速度
    private float maxJumpPower = 1; //最大ジャンプ速度
    private Vector3 jumpPower = new Vector3(0,10,0); //ジャンプ力

    private float attackSpeedRate = 3; //横攻撃時の速度補正
    private float tackleWaitTime = 0.3f; //タックルの待機時間

    private Vector3 eyePos = new Vector3(0.384f,0.29f,-5); //目の位置

    private Vector3 blowUpPower = Vector3.up / 3; //吹っ飛ばし力

    private float actionWaitTimeLength = 2; //アクション発動後の待機時間
    private float actionWaitTime = 1; //アクション発動後の待機時間

    //アクションタイプ
    private enum ActionType {
        Normal = 0,
        Wait = 1,
        TackleR = 2,
        TackleL = 3,
    }
    private ActionType actionType = ActionType.Normal;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        tsm = GameObject.Find("TimeSlowModule").GetComponent<TimeSlowModule>();
        canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();

        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        eye = transform.Find("Eye").gameObject;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        canvasManager.ApplySkillGauge(actionWaitTime);

        EyeMove();

        switch(actionType) {
            case ActionType.Normal:
            _rigidbody2D.gravityScale = 2;

            Tackle(tackleWaitTime);
            Jump();
            Move();
            break;

            case ActionType.Wait:
            _rigidbody2D.gravityScale = 2;

            Jump();
            Move();
            break;

            case ActionType.TackleR:
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = Vector3.zero;
            speed.x = maxSpeedX * attackSpeedRate;
            speed.y = 0;
            break;

            case ActionType.TackleL:
            _rigidbody2D.gravityScale = 0;
            _rigidbody2D.velocity = Vector3.zero;
            speed.x = -maxSpeedX * attackSpeedRate;
            speed.y = 0;
            break;

            default:
            break;
        }


        //Debug.Log(speed);
        transform.position += speed * tsm.GetTimeScale();
    }

    //=============================================================
    /// <summary>
    /// 目の動き
    /// </summary>
    private void EyeMove () {
        if(speed.x > 0) {
            eye.transform.localPosition = eyePos;
        }

        if(speed.x < 0) {
            eye.transform.localPosition = new Vector3(-eyePos.x,eyePos.y,eyePos.z);
        }
    }

    //=============================================================
    /// <summary>
    /// アクション発動時の待機処理
    /// </summary>
    /// <param name="time"></param>
    /// <param name="effectObj"></param>
    /// <returns></returns>
    private IEnumerator ActionWait (float time,GameObject effectObj) {
        StartCoroutine(canvasManager.JiyuuUIAnim1(0.3f,0.5f));

        float _time = 0;
        while(_time < 1) {
            _time += Time.fixedDeltaTime * tsm.GetTimeScale() / time;

            yield return null;
        }

        Destroy(effectObj);

        StartCoroutine(ActionWaitBefore(actionWaitTimeLength));
        actionType = ActionType.Wait;
    }

    //=============================================================
    /// <summary>
    /// アクション発動後の待機処理
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator ActionWaitBefore (float time) {
        actionWaitTime = 0;
        while(actionWaitTime < 1) {
            actionWaitTime += Time.fixedDeltaTime * tsm.GetTimeScale() / time;

            yield return null;
        }

        actionWaitTime = 1;
        actionType = ActionType.Normal;
    }

    //=============================================================
    /// <summary>
    /// 横攻撃
    /// </summary>
    private void Tackle (float waitTime) {
        if(InputController.IsPushButtonDown(KeyCode.Space)) {
            if(speed.x > 0) {
                StartCoroutine(ActionWait(waitTime,CreateTakcleEffect()));
                soundManager.TriggerSE("SE001");
                actionType = ActionType.TackleR;
            } else if(speed.x < 0) {
                StartCoroutine(ActionWait(waitTime,CreateTakcleEffect()));
                soundManager.TriggerSE("SE001");
                actionType = ActionType.TackleL;
            }
        }
    }

    //=============================================================
    /// <summary>
    /// ジャンプ
    /// </summary>
    private void Jump () {
        if(InputController.IsPushButtonDown(KeyCode.UpArrow) || InputController.IsPushButtonDown(KeyCode.W)) {
            soundManager.TriggerSE("SE002");
            _rigidbody2D.velocity = Vector3.zero;
            DriveUp(maxJumpPower,3);
        } else {
            Drivedown(0,0.2f);
        }
    }

    //=============================================================
    /// <summary>
    /// 左右移動
    /// </summary>
    private void Move () {
        bool r = InputController.IsPushButton(KeyCode.RightArrow) || InputController.IsPushButton(KeyCode.D);
        bool l = InputController.IsPushButton(KeyCode.LeftArrow) || InputController.IsPushButton(KeyCode.A);

        //両方押し
        if(r && l) {
            //減速
            if(speed.x > 0) {
                DriveLeft(0,18);
            }

            //減速
            if(speed.x < 0) {
                DriveRight(0,18);
            }
            return;
        }

        //右
        if(r) {
            DriveRight(maxSpeedX,6);
        } else {
            //減速
            if(speed.x > 0) {
                DriveLeft(0,18);
            }
        }

        //左
        if(l) {
            DriveLeft(-maxSpeedX,6);
        } else {
            //減速
            if(speed.x < 0) {
                DriveRight(0,18);
            }
        }
    }

    //=============================================================
    /// <summary>
    /// 上に加速
    /// </summary>
    /// <param name="limit">速度制限</param>
    /// <param name="power">効果の強さ</param>
    private void DriveUp (float limit,float power) {
        speed += jumpPower * Time.fixedDeltaTime * power * tsm.GetTimeScale();
        if(speed.y >= limit) {
            speed.y = limit;
        }
    }

    //=============================================================
    /// <summary>
    /// 下に加速
    /// </summary>
    /// <param name="limit">速度制限</param>
    /// <param name="power">効果の強さ</param>
    private void Drivedown (float limit,float power) {
        speed -= jumpPower * Time.fixedDeltaTime * power * tsm.GetTimeScale();
        if(speed.y <= limit) {
            speed.y = limit;
        }
    }

    //=============================================================
    /// <summary>
    /// 右に加速
    /// </summary>
    /// <param name="limit">速度制限</param>
    /// <param name="power">効果の強さ</param>
    private void DriveRight (float limit,float power) {
        speed += acc * Time.fixedDeltaTime * power * tsm.GetTimeScale();
        if(speed.x >= limit) {
            speed.x = limit;
        }
    }

    //=============================================================
    /// <summary>
    /// 左に加速
    /// </summary>
    /// <param name="limit">速度制限</param>
    /// <param name="power">効果の強さ</param>
    private void DriveLeft (float limit,float power) {
        speed -= acc * Time.fixedDeltaTime * power * tsm.GetTimeScale();
        if(speed.x <= limit) {
            speed.x = limit;
        }
    }

    //=============================================================
    /// <summary>
    /// 衝突判定(トリガー)
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D (Collider2D collision) {
        if(collision.gameObject.tag == "Enemy") {
            //通常アクションタイプでないなら
            if(!(actionType == ActionType.Normal || actionType == ActionType.Wait)) {
                collision.gameObject.GetComponent<Enemy>().Collide(speed + blowUpPower,16,10);
            } else {
                collision.gameObject.GetComponent<Enemy>().Collide(speed + blowUpPower,8,0);
            }
        }

        if(collision.gameObject.tag == "Yu") {
            soundManager.TriggerSE("SE004");
            Destroy(collision.gameObject);
        }   
    }

    //=============================================================
    /// <summary>
    /// 衝突判定
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D (Collision2D collision) {
        if(collision.gameObject.tag == "ReflectionWall") {
            transform.position += speed * (-1);
        }
    }

    //=============================================================
    /// <summary>
    /// エフェクトの生成
    /// </summary>
    /// <returns></returns>
    private GameObject CreateTakcleEffect () {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Effect/Tackle")) as GameObject;
        obj.transform.position = transform.position + new Vector3(0,0,-50);
        obj.transform.SetParent(transform);

        return obj;
    }
}
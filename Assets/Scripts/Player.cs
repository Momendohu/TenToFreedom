﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Player : MonoBehaviour {
    //=============================================================
    private Rigidbody2D _rigidbody2D;
    private GameObject eye;

    private float gravityScale = 2; //重力

    private Vector3 speed = Vector3.zero; //移動スピード(ジャンプ含む)
    private float maxSpeedX = 0.2f; //最大横速度
    private Vector3 acc = new Vector3(1,0,0); //加速度
    private float maxJumpPower = 1; //最大ジャンプ速度
    private Vector3 jumpPower = new Vector3(0,10,0); //ジャンプ力

    private float attackSpeedRate = 5; //横攻撃時の速度補正
    private float tackleWaitTime = 0.2f; //タックルの待機時間
    private Vector3 eyePos = new Vector3(0.384f,0.29f,-5); //目の位置


    //アクションタイプ
    private enum ActionType {
        Normal = 0,
        TackleR = 1,
        TackleL = 2,
    }
    private ActionType actionType = ActionType.Normal;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        eye = transform.Find("Eye").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        EyeMove();

        switch(actionType) {
            case ActionType.Normal:
            _rigidbody2D.gravityScale = 2;

            Tackle(tackleWaitTime);
            Jump();
            Move();
            break;

            case ActionType.TackleR:
            _rigidbody2D.gravityScale = 0;
            speed.x = maxSpeedX * attackSpeedRate;
            speed.y = 0;
            break;

            case ActionType.TackleL:
            _rigidbody2D.gravityScale = 0;
            speed.x = -maxSpeedX * attackSpeedRate;
            speed.y = 0;
            break;

            default:
            break;
        }


        //Debug.Log(speed);
        transform.position += speed;
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
    /// <returns></returns>
    private IEnumerator ActionWait (float time) {
        yield return new WaitForSecondsRealtime(time);

        actionType = ActionType.Normal;
    }

    //=============================================================
    /// <summary>
    /// 横攻撃
    /// </summary>
    private void Tackle (float waitTime) {
        if(InputController.IsPushButtonDown(KeyCode.Space)) {
            if(speed.x >= 0) {
                actionType = ActionType.TackleR;
            } else {
                actionType = ActionType.TackleL;
            }

            StartCoroutine(ActionWait(waitTime));
        }
    }

    //=============================================================
    /// <summary>
    /// ジャンプ
    /// </summary>
    private void Jump () {
        if(InputController.IsPushButtonDown(KeyCode.UpArrow) || InputController.IsPushButtonDown(KeyCode.W)) {
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
            return;
        }

        //右
        if(r) {
            DriveRight(maxSpeedX,1);
            return;
        }

        //左
        if(l) {
            DriveLeft(-maxSpeedX,1);
            return;
        }

        //減速
        if(speed.x > 0) {
            DriveLeft(0,3);
        }

        //減速
        if(speed.x < 0) {
            DriveRight(0,3);
        }
    }

    //=============================================================
    /// <summary>
    /// 上に加速
    /// </summary>
    /// <param name="limit">速度制限</param>
    /// <param name="power">効果の強さ</param>
    private void DriveUp (float limit,float power) {
        speed += jumpPower * Time.fixedDeltaTime * power;
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
        speed -= jumpPower * Time.fixedDeltaTime * power;
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
        speed += acc * Time.fixedDeltaTime * power;
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
        speed -= acc * Time.fixedDeltaTime * power;
        if(speed.x <= limit) {
            speed.x = limit;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Player : MonoBehaviour {
    //=============================================================
    private Vector3 speed = Vector3.zero; //移動スピード(ジャンプ含む)
    private float maxSpeedX = 0.5f; //最大横速度
    private Vector3 acc = new Vector3(1,0,0); //加速度
    private float maxJumpPower = 2; //最大ジャンプ速度
    private Vector3 jumpPower = new Vector3(0,10,0); //ジャンプ力

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {

    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        Jump();
        Move();

        //Debug.Log(speed);
        transform.position += speed;
    }

    //=============================================================
    /// <summary>
    /// ジャンプ
    /// </summary>
    private void Jump () {
        if(InputController.IsPushButtonDown(KeyCode.Space)) {
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
        bool r = InputController.IsPushButton(KeyCode.RightArrow);
        bool l = InputController.IsPushButton(KeyCode.LeftArrow);

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
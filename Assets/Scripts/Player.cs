using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Player : MonoBehaviour {
    //=============================================================
    private Vector3 speed = Vector3.zero;
    private float maxSpeedX = 5;
    private Vector3 acc = new Vector3(1,0,0);

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
        Move();
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
            transform.position += speed;
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
    /// 右に加速
    /// </summary>
    /// <param name="limit">速度制限</param>
    private void DriveRight (float limit,float power) {
        speed += acc * Time.fixedDeltaTime*power;
        if(speed.x >= limit) {
            speed.x = limit;
        }

        transform.position += speed;
    }

    //=============================================================
    /// <summary>
    /// 左に加速
    /// </summary>
    /// <param name="limit">速度制限</param>
    private void DriveLeft (float limit,float power) {
        speed -= acc * Time.fixedDeltaTime*power;
        if(speed.x <= limit) {
            speed.x = limit;
        }

        transform.position += speed;
    }
}
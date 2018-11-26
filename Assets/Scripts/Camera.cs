using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Camera : MonoBehaviour {
    //=============================================================
    private float easingSpeed = 0.2f;
    private Vector3 marginZ = Vector3.back * 10;

    private GameObject player;
    private Camera cam;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        player = GameObject.Find("Player");
        cam = this.GetComponent<Camera>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Update () {
        //プレイヤーに追従
        this.transform.position = Vector3.Lerp(this.transform.position,player.transform.position,easingSpeed) + marginZ;
    }
}
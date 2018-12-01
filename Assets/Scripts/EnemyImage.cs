using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class EnemyImage : MonoBehaviour {
    private Enemy enemy;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        enemy = transform.parent.GetComponent<Enemy>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void OnWillRenderObject () {
        enemy.MoveFlag = true;
    }
}
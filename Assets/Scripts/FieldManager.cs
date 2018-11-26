using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class FieldManager : MonoBehaviour {
    //=============================================================
    private Vector3 fieldBasePosition = new Vector3(-9,-4,0); //基準位置
    private float blockSize = 1.024f; //ブロックのサイズ

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

    private void Start () {
        CreateField();
    }

    //=============================================================
    /// <summary>
    /// フィールドの作成
    /// </summary>
    private void CreateField () {
        for(int i = 0;i < 100;i++) {
            CreateBlock(new Vector3(i*blockSize,0,0) + fieldBasePosition);
        }
    }

    //=============================================================
    /// <summary>
    /// ブロックの作成
    /// </summary>
    private void CreateBlock (Vector3 pos) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Block"),pos,Quaternion.identity) as GameObject;
    }
}
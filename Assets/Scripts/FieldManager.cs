using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class FieldManager : MonoBehaviour {
    //=============================================================
    private Vector3 fieldBasePosition = new Vector3(-9,-4,0); //基準位置
    private float blockSize = 5.12f * 3; //ブロックのサイズ

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
        for(int i = 0;i < 4;i++) {
            CreateBlock(new Vector3(blockSize * (-1),blockSize * (i - 1),0) + fieldBasePosition,"ReflectionWall");
            //CreateBlock(new Vector3(blockSize * (38 - 1),blockSize * (i - 1),0) + fieldBasePosition,"ReflectionWall");
        }

        for(int i = 0;i < 15;i++) {
            CreateBlock(new Vector3(i * blockSize,blockSize * 2,0) + fieldBasePosition,"ReflectionWall");
            CreateBlock(new Vector3(i * blockSize,blockSize * 1,0) + fieldBasePosition,"ReflectionWall");
            CreateBlock(new Vector3(i * blockSize,blockSize * 0.5f,0) + fieldBasePosition,"ReflectionWall");
            CreateBlock(new Vector3(i * blockSize,-blockSize,0) + fieldBasePosition);
        }

        for(int i = 15;i < 45;i++) {
            if(i != 18) {
                CreateBlock(new Vector3(i * blockSize,blockSize * 2,0) + fieldBasePosition,"ReflectionWall");
            }

            CreateBlock(new Vector3(i * blockSize,-blockSize,0) + fieldBasePosition);
        }

        for(int i = 45;i < 300;i++) {
            CreateBlock(new Vector3(i * blockSize,-blockSize,0) + fieldBasePosition);
        }

        /*
        for(int i = 0;i < 100;i++) {
            CreateEnemy(new Vector3(0,0,0) + fieldBasePosition);
        }*/
    }

    //=============================================================
    /// <summary>
    /// 敵の作成
    /// </summary>
    private void CreateEnemy (Vector3 pos) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Enemy"),pos,Quaternion.identity) as GameObject;
    }

    //=============================================================
    /// <summary>
    /// ブロックの作成
    /// </summary>
    private void CreateBlock (Vector3 pos) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Block"),pos,Quaternion.identity) as GameObject;
    }

    //=============================================================
    /// <summary>
    /// ブロックの作成(タグ設定付き)
    /// </summary>
    private void CreateBlock (Vector3 pos,string tag) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Block"),pos,Quaternion.identity) as GameObject;
        obj.tag = tag;
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    //=============================================================
    private BoxCollider2D col;
    private BoxCollider2D colTrigger;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private Slider hpGauge;

    private Vector3 damageTextForwardPos = Vector3.forward * 5; //ダメージ表示を手前に描画するための数値

    /// <summary>
    /// ステータス
    /// </summary>
    private struct State {
        public float MaxHp;
        public float Hp;
    }

    private State state = new State {
        Hp = 10,
        MaxHp = 10,
    };

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        col = transform.Find("ColliderBody").GetComponent<BoxCollider2D>();
        colTrigger = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.Find("Image").GetComponent<SpriteRenderer>();
        hpGauge = transform.Find("Canvas/HpGauge").GetComponent<Slider>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {
        hpGauge.value = state.Hp / state.MaxHp;
    }

    //=============================================================
    /// <summary>
    /// 破壊時アニメーション処理
    /// </summary>
    /// <param name="size">大きさ</param>
    /// <param name="speed">アニメーション速度</param>
    /// <returns></returns>
    public IEnumerator DestroyAnim (float size,float speed) {
        //コライダーを切る
        col.enabled = false;
        colTrigger.enabled = false;

        //速度、重力を切る
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.gravityScale = 0;


        float time = 0;
        while(time < 1) {
            time += Time.fixedDeltaTime * speed;
            spriteRenderer.color = new Color(1,1,1,1 - time);
            transform.localScale += Vector3.one * size;

            yield return null;
        }

        //Destroy(this.gameObject);
        yield return null;
    }

    //=============================================================
    /// <summary>
    /// 衝突
    /// </summary>
    /// <param name="vec">吹っ飛ぶ方向</param>
    /// <param name="power">吹っ飛ぶ力</param>
    /// <param name="damage">与えるダメージ</param>
    public void Collide (Vector2 vec,float power,float damage) {
        _rigidbody2D.velocity += vec * power; //速度加算
        state.Hp -= damage; //ダメージを与える
        CreateDamageText(transform.position + damageTextForwardPos); //ダメージ表示

        //Debug.Log(state.Hp);
        //hpが0になったら
        if(state.Hp <= 0) {
            StartCoroutine(DestroyAnim(1,5));
        }

        //Debug.Log(_rigidbody2D.velocity);
    }

    //=============================================================
    private void CreateDamageText (Vector3 pos) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/DamageText")) as GameObject;
        obj.transform.position = pos;
    }
}
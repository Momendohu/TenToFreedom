using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    //=============================================================
    private BoxCollider2D col;
    private BoxCollider2D colTrigger;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer spriteRenderer;

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
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {

    }

    private void Update () {

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
    /// <param name="vec"></param>
    /// <param name="power"></param>
    public void Collide (Vector2 vec,float power) {
        _rigidbody2D.velocity += vec * power;

        //Debug.Log(_rigidbody2D.velocity);
    }
}
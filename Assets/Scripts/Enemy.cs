using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    //=============================================================
    private TimeSlowModule tsm;

    private BoxCollider2D col;
    private BoxCollider2D colTrigger;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private Slider hpGauge;

    private Vector3 damageTextForwardPos = Vector3.forward * 5; //ダメージ表示を手前に描画するための数値

    private bool damageWaitFlag; //ダメージ時待機フラグ
    private Coroutine damageWaitCoroutine; //ダメージ待機コルーチン
    private float damageWaitTime = 0.2f; //ダメージ待機時間

    /// <summary>
    /// ステータス
    /// </summary>
    private struct State {
        public float MaxHp;
        public float Hp;
    }

    private State state = new State {
        Hp = 5,
        MaxHp = 5,
    };

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        tsm = GameObject.Find("TimeSlowModule").GetComponent<TimeSlowModule>();

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

    private void Update () {
        hpGauge.value = state.Hp / state.MaxHp;

        if(damageWaitFlag && damageWaitCoroutine == null) {
            damageWaitCoroutine = StartCoroutine(DamageWait(damageWaitTime));
        }
    }

    //=============================================================
    /// <summary>
    /// ダメージ時の待機処理
    /// </summary>
    /// <param name="time">待機時間</param>
    /// <returns></returns>
    private IEnumerator DamageWait (float time) {
        float _time = 0;
        while(_time < time) {
            _time += Time.fixedDeltaTime * tsm.GetTimeScale();

            yield return null;
        }

        damageWaitFlag = false;
        damageWaitCoroutine = null;
    }

    //=============================================================
    /// <summary>
    /// 破壊時アニメーション処理
    /// </summary>
    /// <param name="size">大きさ</param>
    /// <param name="speed">アニメーション速度</param>
    /// <returns></returns>
    public IEnumerator DestroyAnim (float size,float speed) {
        //ゲージ表示を切る
        hpGauge.gameObject.SetActive(false);

        //コライダーを切る
        col.enabled = false;
        colTrigger.enabled = false;

        //速度、重力を切る
        _rigidbody2D.velocity = Vector3.zero;
        _rigidbody2D.gravityScale = 0;


        float time = 0;
        while(time < 1) {
            time += Time.fixedDeltaTime * speed * tsm.GetTimeScale();
            spriteRenderer.color = new Color(1,1,1,1 - time);
            transform.localScale += Time.fixedDeltaTime * Vector3.one * size * tsm.GetTimeScale();

            yield return null;
        }

        Destroy(this.gameObject);
    }

    //=============================================================
    /// <summary>
    /// 衝突
    /// </summary>
    /// <param name="vec">吹っ飛ぶ方向</param>
    /// <param name="power">吹っ飛ぶ力</param>
    /// <param name="damage">与えるダメージ</param>
    public void Collide (Vector2 vec,float power,float damage) {
        if(!damageWaitFlag) {
            damageWaitFlag = true;
            _rigidbody2D.velocity += vec.normalized * power; //速度加算
            state.Hp -= damage; //ダメージを与える
            CreateDamageText(transform.position + damageTextForwardPos); //ダメージ表示

            //hpが0になったら
            if(state.Hp <= 0) {
                tsm.SlowFlag = true; //スロー処理発動
                StartCoroutine(DestroyAnim(10,5));
            }
        }
    }

    //=============================================================
    private void CreateDamageText (Vector3 pos) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/DamageText")) as GameObject;
        obj.transform.position = pos;
    }
}
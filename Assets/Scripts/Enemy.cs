﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private GameObject player;

    private BoxCollider2D col;
    private BoxCollider2D colTrigger;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer spriteRenderer;
    private Slider hpGauge;

    private float colliderTriggerSizeRate = 1.2f; //トリガー用当たり判定の大きさ倍率

    private Vector3 damageTextForwardPos = Vector3.forward * 5; //ダメージ表示を手前に描画するための数値
    private Vector2 yuInitSpeed = new Vector2(0,10); //「ゆ」生成時の最初のスピード
    private float yuInitSpeedRandomRangeY = 5;

    private bool damageWaitFlag; //ダメージ時待機フラグ
    private Coroutine damageWaitCoroutine; //ダメージ待機コルーチン
    private float damageWaitTime = 0.3f; //ダメージ待機時間

    private bool moveFlag = false; //動作フラグ
    public bool MoveFlag {
        get { return moveFlag; }
        set { moveFlag = value; }
    }

    private bool moveOnce = false; //初期動作用フラグ

    //=============================================================
    public Sprite[] EnemyImages; //見た目
    public int Id; //ID

    /// <summary>
    /// 見てる方向
    /// </summary>
    public enum LookDirection {
        Left = 0,
        Right = 1,
    }

    [System.NonSerialized]
    public LookDirection lookDirection = LookDirection.Left;

    /// <summary>
    /// ステータス
    /// </summary>
    private struct State {
        public int Id;
        public string Name;
        public float MaxHp;
        public float Hp;
        public int HoldYu;
        public int ReflectDamage;

        public float Size;
        public Vector2 ColliderSize;

        public float MoveSpeed;
    }

    private State[] state = {
        new State {Id=0,Name="ジュース",Hp = 3,MaxHp = 3,HoldYu = 2,ReflectDamage=0,Size=0.5f,ColliderSize=new Vector2(1,1.4f),MoveSpeed=0},
        new State {Id=1,Name="ハサミ",Hp =1,MaxHp = 1,HoldYu = 0,ReflectDamage=3,Size=0.5f,ColliderSize=new Vector2(1,1.4f),MoveSpeed=0},
        new State {Id=2,Name="コーヒー",Hp =3,MaxHp = 3,HoldYu = 0,ReflectDamage=0,Size=0.5f,ColliderSize=new Vector2(1,1.4f),MoveSpeed=0},
        new State {Id=3,Name="クルマ",Hp =3,MaxHp = 3,HoldYu = 0,ReflectDamage=3,Size=1.5f,ColliderSize=new Vector2(4f,1.9f),MoveSpeed=10f},

        new State {Id=4,Name="ギュウニュウ",Hp =1,MaxHp = 1,HoldYu = 5,ReflectDamage=0,Size=0.5f,ColliderSize=new Vector2(1,1.4f),MoveSpeed=0},
        new State {Id=5,Name="ニュウギュウ",Hp =100,MaxHp = 100,HoldYu =100,ReflectDamage=8,Size=3,ColliderSize=new Vector2(8,5.4f),MoveSpeed=8f},

        new State{ Id=6,Name="シアワセノネコ",Hp =9999,MaxHp = 9999,HoldYu = 0,ReflectDamage=0,Size=1,ColliderSize=new Vector2(2.3f,2.6f),MoveSpeed=0},
        new State{ Id=7,Name="キュウシュウ",Hp =100,MaxHp = 100,HoldYu = 100,ReflectDamage=8,Size=2,ColliderSize=new Vector2(9,11.6f),MoveSpeed=0},
        new State{ Id=8,Name="オリ",Hp =1,MaxHp = 1,HoldYu = 0,ReflectDamage=0,Size=4,ColliderSize=new Vector2(10,10),MoveSpeed=0}
    };

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        col = transform.Find("ColliderBody").GetComponent<BoxCollider2D>();
        colTrigger = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.Find("Image").GetComponent<SpriteRenderer>();
        hpGauge = transform.Find("c/HpGauge").GetComponent<Slider>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        //画像をidによって変える
        spriteRenderer.sprite = EnemyImages[Id];

        //大きさをidによって変える
        spriteRenderer.transform.localScale = Vector3.one * state[Id].Size;

        //当たり判定の大きさをidによって変える
        col.size = state[Id].ColliderSize;
        colTrigger.size = state[Id].ColliderSize * colliderTriggerSizeRate;
    }

    private void Update () {
        if(moveFlag) {
            if(!moveOnce) {
                //特殊行動
                StartCoroutine(UniqueAct());
                moveOnce = true;
            }

            //速度が0じゃないなら
            if(state[Id].MoveSpeed != 0) {
                //ダメージ待機じゃないなら
                if(!damageWaitFlag) {
                    //プレイヤーの位置で方向転換
                    if(transform.position.x <= player.transform.position.x) {
                        lookDirection = LookDirection.Right;
                    } else {
                        lookDirection = LookDirection.Left;
                    }

                    //方向によって移動
                    switch(lookDirection) {
                        case LookDirection.Left:
                        transform.eulerAngles = Vector3.zero;
                        transform.position += Vector3.left * state[Id].MoveSpeed * Time.fixedDeltaTime * TimeSlowModule.Instance.GetTimeScale();
                        break;

                        case LookDirection.Right:
                        transform.eulerAngles = new Vector3(0,180,0);
                        transform.position += Vector3.right * state[Id].MoveSpeed * Time.fixedDeltaTime * TimeSlowModule.Instance.GetTimeScale();
                        break;
                    }
                }
            }

            if(damageWaitFlag && damageWaitCoroutine == null) {
                damageWaitCoroutine = StartCoroutine(DamageWait(damageWaitTime));
            }
        }

        //hpゲージにhpを適用
        hpGauge.value = state[Id].Hp / state[Id].MaxHp;
    }

    //=============================================================
    /// <summary>
    /// ダメージ時の待機処理
    /// </summary>
    /// <param name="time">待機時間</param>
    /// <returns></returns>
    private IEnumerator DamageWait (float time) {
        if(state[Id].Name == "クルマ") {
            AudioManager.Instance.PlaySE("SE009");
        }

        if(state[Id].Name == "ニュウギュウ") {
            AudioManager.Instance.PlaySE("SE008");
        }

        float _time = 0;
        while(_time < time) {
            _time += Time.fixedDeltaTime * TimeSlowModule.Instance.GetTimeScale();

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

        //消える演出
        float time = 0;
        while(time < 1) {
            time += Time.fixedDeltaTime * speed * TimeSlowModule.Instance.GetTimeScale();
            spriteRenderer.color = new Color(1,1,1,1 - time);
            transform.localScale += Time.fixedDeltaTime * Vector3.one * size * TimeSlowModule.Instance.GetTimeScale();

            yield return null;
        }

        //「ゆ」を生成
        for(int i = 0;i < state[Id].HoldYu;i++) {
            CreateYu(transform.position,yuInitSpeed + new Vector2(Random.Range(-yuInitSpeedRandomRangeY,yuInitSpeedRandomRangeY),0));
        }

        //ギュウニュウならボスフラグを立てる
        if(state[Id].Name == "ギュウニュウ") {
            AudioManager.Instance.PlaySE("SE008");
            gameManager.BossAppearFlag = true;
        }

        //ニュウギュウならボス撃破を立てる
        if(state[Id].Name == "ニュウギュウ") {
            gameManager.BossDefeatFlag = true;
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

            //ダメージ量が0でないなら
            if(damage != 0) {
                _rigidbody2D.velocity += vec.normalized * power; //速度加算

                state[Id].Hp -= damage; //ダメージを与える
                CreateDamageText(transform.position + damageTextForwardPos,(int)damage); //ダメージ表示
            } else {
                //反射ダメージが0じゃないなら
                if(state[Id].ReflectDamage == 0 && state[Id].Name != "オリ") {
                    _rigidbody2D.velocity += vec.normalized * power; //速度加算
                } else {
                    //プレイヤー反射
                    StartCoroutine(player.GetComponent<Player>().DamageWait(1));
                    Vector3 reflectVec = (player.transform.position - transform.position + Vector3.up * 0.5f).normalized / 2f; //少し上に補正
                    reflectVec.y *= 0.5f; //y軸方向の速度を1/2に
                    reflectVec.z = 0;
                    player.GetComponent<Player>().Speed += reflectVec;

                    //ダメージを受ける + 「ゆ」を落とす(ダメージの1/2)
                    int _damage = 0;
                    if(gameManager.parameter.YuPoint < state[Id].ReflectDamage) {
                        _damage = gameManager.parameter.YuPoint;
                    } else {
                        _damage = state[Id].ReflectDamage;
                    }

                    gameManager.ApplyYuPoint(-_damage);
                    for(int i = 0;i < _damage / 2;i++) {
                        CreateYu(player.transform.position,yuInitSpeed + new Vector2(Random.Range(-yuInitSpeedRandomRangeY,yuInitSpeedRandomRangeY),0));
                    }
                }
            }

            //hpが0になったら
            if(state[Id].Hp <= 0) {
                TimeSlowModule.Instance.SlowFlag = true; //スロー処理
                AudioManager.Instance.PlaySE("SE003");
                StartCoroutine(DestroyAnim(10,5));
            } else {
                if(damage == 0) {
                    if(state[Id].ReflectDamage == 0 && state[Id].Name != "オリ") {
                        AudioManager.Instance.PlaySE("SE006");
                    } else {
                        AudioManager.Instance.PlaySE("SE007");
                    }
                } else {
                    AudioManager.Instance.PlaySE("SE005");
                }
            }
        }
    }

    //=============================================================
    private void CreateDamageText (Vector3 pos,int damage) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/DamageText")) as GameObject;
        obj.transform.position = pos;
        obj.GetComponent<TextMesh>().text = "" + damage;
    }

    //=============================================================
    private void CreateYu (Vector3 pos,Vector2 speed) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Yu")) as GameObject;
        obj.transform.position = pos;
        obj.GetComponent<Rigidbody2D>().velocity += speed;
    }

    //=============================================================
    private void CreateEnemy (Vector3 pos,Vector2 speed,int id) {
        GameObject obj = Instantiate(Resources.Load("Prefabs/Enemy")) as GameObject;
        obj.transform.position = pos;
        obj.GetComponent<Rigidbody2D>().velocity += speed;
        obj.GetComponent<Enemy>().Id = id;
    }

    //=============================================================
    private void GenerateObj () {
        switch(state[Id].Name) {
            case "シアワセノネコ":
            //ジュースを30こ生成
            for(int i = 0;i < 30;i++) {
                CreateEnemy(transform.position,yuInitSpeed + new Vector2(Random.Range(-yuInitSpeedRandomRangeY,yuInitSpeedRandomRangeY),0),0);
            }
            break;
        }
    }

    //=============================================================
    private IEnumerator UniqueAct () {
        switch(state[Id].Name) {
            case "シアワセノネコ":
            float time = 0;
            _rigidbody2D.gravityScale = 0;

            AudioManager.Instance.PlaySE("SE010");
            while(time < 2) {
                Vector3 goal = transform.position + Vector3.up * 0.5f;
                time += Time.fixedDeltaTime * TimeSlowModule.Instance.GetTimeScale();
                transform.position = Vector3.Lerp(transform.position,goal,0.1f);
                yield return null;
            }

            AudioManager.Instance.PlaySE("SE011");

            GenerateObj();
            StartCoroutine(DestroyAnim(10,5));

            break;
        }
    }
}
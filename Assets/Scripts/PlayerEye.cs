using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class PlayerEye : MonoBehaviour {
    //=============================================================
    private TimeSlowModule tsm;

    private MeshRenderer meshRenderer;

    private float blinkProb = 2; //瞬きする確率
    private float blinkSpeed = 5; //瞬きのスピード

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        tsm = GameObject.Find("TimeSlowModule").GetComponent<TimeSlowModule>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    //=============================================================
    private void Awake () {
        Init();

        meshRenderer.sortingLayerName = "Player";
        meshRenderer.sortingOrder = 100;
    }

    private void Update () {
        //瞬き判定
        if(Random.Range(0,100) >= (100 - blinkProb)) {
            StartCoroutine(Blink(blinkSpeed));
        }
    }

    //=============================================================
    /// <summary>
    /// 瞬き
    /// </summary>
    /// <returns></returns>
    private IEnumerator Blink (float speed) {
        float time = 0;
        while(time < 1) {
            time += Time.fixedDeltaTime * speed * tsm.GetTimeScale();
            transform.localScale = new Vector3(1,1 - Mathf.Sin(Mathf.Deg2Rad * 180 * time),1);

            yield return null;
        }

        yield break;
    }
}
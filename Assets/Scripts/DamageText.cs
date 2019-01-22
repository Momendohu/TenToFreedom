using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class DamageText : MonoBehaviour {
    //=============================================================
    private TextMesh textMesh;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        textMesh = GetComponent<TextMesh>();
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        StartCoroutine(ToDestroy(2,2));
        StartCoroutine(MoveToAbove(0.1f,(transform.position + Vector3.up)));
    }

    //=============================================================
    /// <summary>
    /// 時間差で消滅する
    /// </summary>
    /// <param name="speed">透明になるスピード</param>
    /// <param name="displayTime">表示時間</param>
    /// <returns></returns>
    private IEnumerator ToDestroy (float speed,float displayTime) {
        float time = 0;
        while((displayTime - time) > 0) {
            time += Time.fixedDeltaTime * speed * TimeSlowModule.Instance.GetTimeScale();

            textMesh.color = new Color(0,0,0,displayTime - time);

            yield return null;
        }

        Destroy(this.gameObject);
    }

    //=============================================================
    /// <summary>
    /// 上部に移動する
    /// </summary>
    /// <param name="speed">移動スピード</param>
    /// <param name="goal">目的位置</param>
    /// <returns></returns>
    private IEnumerator MoveToAbove (float speed,Vector3 goal) {
        while(true) {
            transform.position = Vector3.Lerp(transform.position,goal,Mathf.Clamp01(speed * TimeSlowModule.Instance.GetTimeScale()));

            yield return null;
        }
    }
}
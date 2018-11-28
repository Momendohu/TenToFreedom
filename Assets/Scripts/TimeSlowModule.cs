using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class TimeSlowModule : MonoBehaviour {
    //=============================================================
    private Coroutine coroutine;

    private bool slowFlag; //スロー処理フラグ
    public bool SlowFlag {
        get { return slowFlag; }
        set { slowFlag = value; }
    }

    //=============================================================
    private void Update () {
        Time.timeScale = GetTimeScale();

        //スロー処理
        if(slowFlag && coroutine == null) {
            coroutine = StartCoroutine(SlowWait());
        }
    }

    //=============================================================
    public float GetTimeScale () {
        if(slowFlag) {
            return Constant.SLOW_TIME_SCALE;
        } else {
            return 1;
        }
    }

    //=============================================================
    private IEnumerator SlowWait () {
        yield return new WaitForSecondsRealtime(Constant.SLOW_TIME_LENGTH);
        slowFlag = false;
    }
}
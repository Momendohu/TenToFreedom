using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public static class ErrorManager {
    public enum ERROR_CODE {
        NONE = 0,
        SYSTEM_SINGLETON_OBJECT_NONE = 1,
        AUDIO_SE_NONE = 101,
        AUDIO_BGM_NONE = 201,
    }

    /// <summary>
    /// エラーログを表示する
    /// </summary>
    public static void ErrorLog (ERROR_CODE code) {
        string str = "no message";
        switch(code) {
            case ERROR_CODE.NONE:
            str = "no message";
            break;

            case ERROR_CODE.SYSTEM_SINGLETON_OBJECT_NONE:
            str = "シングルトンオブジェクト未検地";
            break;

            case ERROR_CODE.AUDIO_SE_NONE:
            str = "存在しないSEです";
            break;

            case ERROR_CODE.AUDIO_BGM_NONE:
            str = "存在しないBGMです";
            break;

            default:
            break;
        }

        Debug.LogError(str);
    }
}
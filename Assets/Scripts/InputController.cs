using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 入力コントローラー
/// </summary>
public static class InputController {
    /// <summary>
    /// ボタンを押してる状態なら
    /// </summary>
    /// <param name="code">キーコード</param>
    /// <returns>真偽値</returns>
    public static bool IsPushButton (KeyCode code) {
        return Input.GetKey(code);
    }

    /// <summary>
    /// ボタンを押したら
    /// </summary>
    /// <param name="code">キーコード</param>
    /// <returns>真偽値</returns>
    public static bool IsPushButtonDown (KeyCode code) {
        return Input.GetKey(code);
    }

    /// <summary>
    /// ボタンが離されたら
    /// </summary>
    /// <param name="code">キーコード</param>
    /// <returns>真偽値</returns>
    public static bool IsPushButtonUp (KeyCode code) {
        return Input.GetKey(code);
    }
}
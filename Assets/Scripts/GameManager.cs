using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    /// <summary>
    /// パラメーター(大域的な)
    /// </summary>
    public struct Parameter {
        public float YuPoint;
    }

    public Parameter paramater = new Parameter {
        YuPoint = 0,
    };
}
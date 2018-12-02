using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour {
    //=============================================================
    private GameManager gameManager;
    private SoundManager soundManager;
    private Text title;
    private Text titleShadow;
    private Image titleImage;
    private Image titleImageShadow;

    private Text mainText;
    private Text shadowText;

    private GameObject vail;
    private GameObject spotFrame;

    //=============================================================
    private void Init () {
        CRef();
    }

    //=============================================================
    private void CRef () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        title = transform.Find("Title/Text").GetComponent<Text>();
        titleShadow = transform.Find("Title/Shadow").GetComponent<Text>();
        titleImage = transform.Find("Title/Image").GetComponent<Image>();
        titleImageShadow = transform.Find("Title/ShadowImage").GetComponent<Image>();
        mainText = transform.Find("PushSpace/Main").GetComponent<Text>();
        shadowText = transform.Find("PushSpace/Shadow").GetComponent<Text>();
        vail = transform.Find("Vail").gameObject;
        spotFrame = transform.Find("SpotFrame").gameObject;
    }

    //=============================================================
    private void Awake () {
        Init();
    }

    private void Start () {
        StartCoroutine(BlinkText(1));
    }

    //=============================================================
    private IEnumerator BlinkText (float speed) {
        float time = 0;
        float time2 = 0;

        Color prevColorMain = mainText.color;
        Color prevColorShadow = shadowText.color;

        Color prevColorTitle = title.color;
        Color prevColorTitleShadow = titleShadow.color;
        Color prevColorTitleImage = titleImage.color;
        Color prevColorTitleImageShadow = titleImageShadow.color;

        while(true) {
            if(gameManager.GameStartFlag) {
                time += Time.fixedDeltaTime * speed * 20;
                time2 += Time.fixedDeltaTime * 2;
                if(time2 >= 1) {
                    break;
                }
            } else {
                time += Time.fixedDeltaTime * speed;
            }

            float alpha = Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * time * 360)) + 0.3f;
            mainText.color = new Color(prevColorMain.r,prevColorMain.g,prevColorMain.b,alpha * (1 - time2));
            shadowText.color = new Color(prevColorShadow.r,prevColorShadow.g,prevColorShadow.b,alpha * (1 - time2));

            title.color = new Color(prevColorTitle.r,prevColorTitle.g,prevColorTitle.b,(1 - time2));
            titleShadow.color = new Color(prevColorTitleShadow.r,prevColorTitleShadow.g,prevColorTitleShadow.b,(1 - time2));
            titleImage.color = new Color(prevColorTitleImage.r,prevColorTitleImage.g,prevColorTitleImage.b,(1 - time2));
            titleImageShadow.color = new Color(prevColorTitleImageShadow.r,prevColorTitleImageShadow.g,prevColorTitleImageShadow.b,(1 - time2));

            yield return null;
        }

        Destroy(vail);
        soundManager.TriggerSE("SE014");

        float time3 = -1;
        while(time3 < 0.2f) {
            time3 += Time.fixedDeltaTime;
            if(time3 >= 0) {
                spotFrame.transform.localScale += Vector3.one;
            }

            yield return null;
        }

        Destroy(this.gameObject);
        gameManager.GameStartEndedFlag = true;
    }
}
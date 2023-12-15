using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FadeController : MonoBehaviour {

    private const string COROUTINE_NAME = "AlphaTransition";

    private Canvas m_canvas;
    [SerializeField]
    private Color m_color = Color.black;
    private Action m_callBack;
    private static FadeController m_instance;
    private static Image m_fadePanel;
    private static Canvas m_canvasComp;

    private FadeController() { }

    private void Start() {
        IsFinish = false;
    }

    public void FadeIn(float _fadeTime) {
        SetColor(m_color);
        StartCoroutine(COROUTINE_NAME, -(_fadeTime));
    }

    public void FadeOut(float _fadeTime) {
        SetColor(m_color);
        StartCoroutine(COROUTINE_NAME, _fadeTime);
    }

    public void FadeIn(float _fadeTime, Color _color) {
        SetColor(_color);
        FadeIn(_fadeTime);
    }

    public void FadeOut(float _fadeTime, Color _color) {
        SetColor(_color);
        FadeOut(_fadeTime);
    }

    public void FadeIn(float _fadeTime, Action _finishedCallBack) {
        m_callBack = _finishedCallBack;
        FadeIn(_fadeTime);
    }

    public void FadeOut(float _fadeTime, Action _finishedCallBack) {
        m_callBack = _finishedCallBack;
        FadeOut(_fadeTime);
    }

    public void FadeIn(float _fadeTime, Color _color, Action _finishedCallBack) {
        SetColor(_color);
        FadeIn(_fadeTime, _finishedCallBack);
    }

    public void FadeOut(float _fadeTime, Color _color, Action _finishedCallBack) {
        SetColor(_color);
        FadeOut(_fadeTime, _finishedCallBack);
    }

    public void SetColor(Color _color) {
        m_color = _color;
    }

    public void SetSortingOrder(int _sortingOrder) {
        m_canvasComp.sortingOrder = _sortingOrder;
    }

    private IEnumerator AlphaTransition(float _fadeTime) {
        float alfa = _fadeTime < 0 ? 1.0f : 0.0f;
        IsFinish = false;

        while (0.0f <= alfa && alfa <= 1.0f) {
            float delta = Time.deltaTime;
            alfa += delta / _fadeTime;
            m_fadePanel.color = new Color(m_color.r, m_color.g, m_color.b, alfa);
            yield return new WaitForSeconds(delta);
        }

        IsFinish = true;
        
        if (m_callBack != null) {
            m_callBack.Invoke();
            m_callBack = null;
        }
    }

    public bool IsFinish { private set; get; }

    public static FadeController Instance {
        get {
            if (m_instance == null) {

                GameObject controllerObj = new GameObject("FadeControllerObject");
                m_instance = controllerObj.AddComponent<FadeController>();
                DontDestroyOnLoad(controllerObj);

                GameObject canvasObj = new GameObject("FadeCnavas");
                m_canvasComp = canvasObj.AddComponent<Canvas>();
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                canvasObj.transform.SetParent(controllerObj.transform);

                GameObject fadePanelObj = new GameObject("FadePanel");
                fadePanelObj.AddComponent<CanvasRenderer>();
                fadePanelObj.transform.SetParent(canvasObj.transform);
                m_fadePanel = fadePanelObj.AddComponent<Image>();

                m_canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;
                m_fadePanel.rectTransform.anchorMin = Vector2.zero;
                m_fadePanel.rectTransform.anchorMax = Vector2.one;
                m_fadePanel.rectTransform.sizeDelta = Vector2.zero;
            }
            return m_instance;
        }
    }
}
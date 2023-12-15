using UnityEngine;

public class FadeControllerDEMO1 : MonoBehaviour {
    [SerializeField]
    private float m_fadeInTime = 1.0f;
    [SerializeField]
    private float m_fadeOutTime = 1.0f;
    [SerializeField]
    private Color m_fadeInColor = Color.black;
    [SerializeField]
    private Color m_fadeOutColor = Color.black;
    private bool m_isIn = true;
    private FadeController m_fadeController;

    private void Start() {
        m_fadeController = FadeController.Instance;
        m_fadeController.SetSortingOrder(1);
        m_fadeController.FadeIn(m_fadeInTime, m_fadeInColor);
    }

    private void Update () {
        if (Input.anyKeyDown && m_fadeController.IsFinish) {
            m_isIn = !m_isIn;
            if (m_isIn) {
                m_fadeController.FadeIn(m_fadeInTime, m_fadeInColor);
            } else {
                m_fadeController.FadeOut(m_fadeOutTime, m_fadeOutColor);
            }
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeControllerDEMO3 : MonoBehaviour {
    [SerializeField]
    private GameObject TextObj;
    [SerializeField]
    private string m_scene;
    private FadeController m_fadeController;
    private bool m_isFade = false;

    private void Start() {

        m_fadeController = FadeController.Instance;
        m_fadeController.SetSortingOrder(0);
        m_fadeController.FadeIn(1.0f, Color.black);

        if(TextObj != null) {
            TextObj.SetActive(false);
        }
    }

    private void Update() {
        if (Input.anyKeyDown && m_fadeController.IsFinish && !m_isFade) {
            m_isFade = true;
            m_fadeController.FadeOut(1.0f, Color.black, ShowText);
        }
    }

    private void ShowText() {
        if (TextObj != null) {
            TextObj.SetActive(true);
        }else {
            GoToScene();
        }
    }

    public void GoToScene() {
        SceneManager.LoadScene(m_scene);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeControllerDEMO2 : MonoBehaviour {
    [SerializeField]
    private string m_scene;
    private FadeController m_fadeController;

    private void Start() {

        m_fadeController = FadeController.Instance;
        m_fadeController.SetSortingOrder(1);
        m_fadeController.FadeIn(1.0f, Color.black);

        FadeController.Instance.FadeIn(1.0f);

    }

    private void Update() {
        if (Input.anyKeyDown && m_fadeController.IsFinish) {
            m_fadeController.FadeOut(1.0f, Color.black, GotoScene);
        }
    }

    private void GotoScene() {
        SceneManager.LoadScene(m_scene);
    }
}

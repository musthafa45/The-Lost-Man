using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitText : MonoBehaviour {

    private string m_allText;
    private Text m_textComp;
    private int m_textLength;
    private string m_text;


    void Start () {
        m_textComp = this.GetComponent<Text>();
        m_text = m_textComp.text;
        m_textLength = m_text.Length;
        StartCoroutine("TransitionText");
    }
	
	void Update () {
		
	}

    private IEnumerator TransitionText() {
        int count = 0;
        while (count < m_textLength - 1) {
            m_allText += m_text[count];
            m_textComp.text = m_allText;
            count++;
            yield return new WaitForSeconds(Time.deltaTime * 0.01f);
        }
    }
}

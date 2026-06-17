using TMPro;
using UnityEngine;

public class TextPopUp : MonoBehaviour
{
    private Animator m_animator         = null;
    private TextMeshProUGUI m_textMesh  = null;

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_textMesh = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public void DisplayMessage(string _message)
    {
        m_textMesh.text = _message;
        m_animator.Play("Display", -1);
    }

    public void SetColor(Color _color)
    {
        m_textMesh.color = _color;
    }
}

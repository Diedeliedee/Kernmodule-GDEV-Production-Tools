using TMPro;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private AnimationClip[] m_clips;

    private TextMeshProUGUI m_textMesh = null;

    private int m_currentIndex = 0;

    public void Setup()
    {
        m_textMesh      = GetComponentInChildren<TextMeshProUGUI>();
        m_textMesh.text = m_clips[0].name;
    }

    public void Begin()
    {
        m_animator.Play(m_clips[0].name);
    }

    public void NextScene()
    {
        m_currentIndex++;
        if (m_currentIndex >= m_clips.Length) m_currentIndex = 0;

        var clipName = m_clips[m_currentIndex].name;

        m_animator.Play(clipName);
        m_textMesh.text = clipName;
    }

    public void PreviousScene()
    {
        m_currentIndex--;
        if (m_currentIndex < 0) m_currentIndex = m_clips.Length - 1;

        var clipName = m_clips[m_currentIndex].name;

        m_animator.Play(clipName);
        m_textMesh.text = clipName;
    }

    public void ResetScene()
    {
        m_currentIndex  = 0;
        m_textMesh.text = m_clips[0].name;

        m_animator.Play("Empty");
        m_animator.Update(0f);
        m_animator.Update(0f);
    }
}
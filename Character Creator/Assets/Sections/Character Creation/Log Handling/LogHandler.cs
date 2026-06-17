using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogHandler : MonoBehaviour
{
    [Header("Properties:")]
    [SerializeField] private float m_logKillTime = 5f;

    [Header("Reference:")]
    [SerializeField] private GameObject m_logObject;

    public void OnLogCallReceived(string _log, Color _color)
    {
        var spawnedObject   = Instantiate(m_logObject, transform);
        var textObject      = spawnedObject.GetComponent<TextPopUp>();

        textObject.DisplayMessage(_log);
        textObject.SetColor(_color);

        Destroy(spawnedObject, m_logKillTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class PilotScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pilotText;
    [SerializeField] private float _speed;
    [SerializeField] private float _posYBegin, _posYEnd;
    [SerializeField] private Button _btnClose, _btnShow;

    private RectTransform m_pilotTextRectTrans;
    private bool m_isShow;
    void Start()
    {
        _btnClose.onClick.AddListener(HandleClose);
        _btnShow.onClick.AddListener(HandleShow);
        if (!Data.isPlayedPilot)
        {
            Data.isPlayedPilot = true;
            m_isShow = true;
            ShowPilot();
        }
    }

    private void HandleClose()
    {
        m_isShow = false;
        gameObject.SetActive(false);
    }

    private void HandleShow()
    {
        m_isShow = true;
        gameObject.SetActive(true);
        ShowPilot();
    }

    private IEnumerator AutoScrollPilot()
    {
        while (m_pilotTextRectTrans.localPosition.y <= _posYEnd)
        {
            if (!m_isShow) break;
            m_pilotTextRectTrans.Translate(Vector3.up * Time.deltaTime * _speed);
            yield return null;
        }

        m_isShow = false;
        this.gameObject.SetActive(false);
    }

    private void ShowPilot()
    {
        m_pilotTextRectTrans = _pilotText.GetComponent<RectTransform>();
        var localPosition = m_pilotTextRectTrans.localPosition;
        localPosition = new Vector3(localPosition.x, _posYBegin, localPosition.z);
        m_pilotTextRectTrans.localPosition = localPosition;
        StartCoroutine(AutoScrollPilot());
    }
}

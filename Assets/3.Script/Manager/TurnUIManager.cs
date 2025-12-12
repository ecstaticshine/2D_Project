using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup phaseCanvas;
    [SerializeField] private Image phaseColorImage;
    [SerializeField] private TMP_Text TurnMessage;

    public IEnumerator ShowTurnText(string turnText)
    {
        phaseCanvas.blocksRaycasts = true;

        TurnMessage.text = turnText;

        if (turnText.Equals("Player Phase"))
        {
            phaseColorImage.color = Color.softBlue;
        }
        else if(turnText.Equals("Enemy Phase"))
        {
            phaseColorImage.color = Color.softRed;
        }

        // 페이드 인
        for (float t = 0; t < 1; t += Time.deltaTime * 2f)
        {
            phaseCanvas.alpha = t;
            yield return null;
        }
        phaseCanvas.alpha = 1f;

        // 잠깐 유지
        yield return new WaitForSeconds(0.7f);

        // 페이드 아웃
        for (float t = 1; t > 0; t -= Time.deltaTime * 2f)
        {
            phaseCanvas.alpha = t;
            yield return null;
        }
        phaseCanvas.alpha = 0f;

        phaseCanvas.blocksRaycasts = false;
    }
}

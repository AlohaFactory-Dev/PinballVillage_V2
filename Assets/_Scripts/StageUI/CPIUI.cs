using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class CPIUI : MonoBehaviour
{
    [SerializeField] private GoldText goldText;
    private CanvasGroup _canvasGroup;

    public void Init()
    {
        goldText.Init();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void On()
    {
        _canvasGroup.alpha = 1;
    }

    public void Off()
    {
        _canvasGroup.alpha = 0;
    }
}
#endif
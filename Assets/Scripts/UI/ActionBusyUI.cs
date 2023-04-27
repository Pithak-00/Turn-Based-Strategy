using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ActionBusyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

        Hide();
    }

    private void Show()
    {
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        textMeshPro.text = selectedAction.GetActionName();

        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}

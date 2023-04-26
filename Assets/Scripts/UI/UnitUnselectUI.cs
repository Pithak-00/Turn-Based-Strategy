using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUnselectUI : MonoBehaviour
{
    [SerializeField] private Button unitUnselectBtn;

    private void Start()
    {
        unitUnselectBtn.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedUnit(null);
        });
    }
}

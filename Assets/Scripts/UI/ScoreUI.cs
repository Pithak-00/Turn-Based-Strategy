using TMPro;
using UniRx;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        ScoreManager.Instance
            .Score
            .Subscribe(x => textMeshPro.text = $"Score: {x}")
            .AddTo(this);
    }
}

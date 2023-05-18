using UniRx;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private readonly ReactiveProperty<int> score = new ReactiveProperty<int>(0);
    public IReadOnlyReactiveProperty<int> Score => score;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ScoreManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        this.score.AddTo(this);
    }

    public void AddScore(int score)
    {
        this.score.Value += score;
    }

    public void ResetScore()
    {
        this.score.Value = 0;
    }
}

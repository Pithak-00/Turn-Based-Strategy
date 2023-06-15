using UnityEngine;
using Stage;

namespace Path
{
    public class PathfindingUpdater : MonoBehaviour
    {
        private void Start()
        {
            DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
        }

        private void DestructibleCrate_OnAnyDestroyed(object sender, System.EventArgs e)
        {
            DestructibleCrate destructibleCrate = sender as DestructibleCrate;
            Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
            ScoreManager.Instance.AddScore(50);
        }
    }
}
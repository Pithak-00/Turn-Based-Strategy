using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AddressablesLoader : MonoBehaviour
{
    public static AddressablesLoader Instance { get; private set; }

    public static event EventHandler OnAnyAssetBundleLoaded;

    [SerializeField] private string assetBundleUrl;

    private AssetBundle assetBundle;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one AddressablesLoader! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadBundle(assetBundleUrl));
    }

    IEnumerator LoadBundle(string assetBundleUrl)
    {
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl))
        {
            yield return www.SendWebRequest();

            assetBundle = DownloadHandlerAssetBundle.GetContent(www);

            OnAnyAssetBundleLoaded?.Invoke(this, EventArgs.Empty);
        }
    }

    public AssetBundle GetAssetBundle()
    {
        return assetBundle;
    }

    private void OnDestroy()
    {
        assetBundle.UnloadAsync(false);
    }
}

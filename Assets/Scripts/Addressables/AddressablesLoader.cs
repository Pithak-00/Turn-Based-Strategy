using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class AddressablesLoader : MonoBehaviour
{
    public static AddressablesLoader Instance { get; private set; }

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

        DontDestroyOnLoad(gameObject);
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
        }

        SceneManager.LoadScene("TestScene");
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

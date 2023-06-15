using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;

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

    private async UniTaskVoid Start()
    {
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleUrl);
        await request.SendWebRequest().ToUniTask();

        if (request.result == UnityWebRequest.Result.Success)
        {
            assetBundle = DownloadHandlerAssetBundle.GetContent(request);
            SceneManager.LoadScene("TestScene");
        }
        else
        {
            Debug.LogError("アセットロード失敗: " + request.error);
        }

    }

    public AssetBundle GetAssetBundle()
    {
        return assetBundle;
    }

    private void OnDestroy()
    {
        assetBundle.Unload(false);
    }
}
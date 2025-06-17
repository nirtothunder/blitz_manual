using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class CacheSettings
{
    public bool useCache = true;
    public float cacheDuration = 86400f;
}

public class GitHub_Data_Loader_NT : MonoBehaviour
{
    [Header("GitHub Settings")]
    public string githubRawUrl = "https://raw.githubusercontent.com/username/repo/branch/";
    public string imagePath = "path/to/image.png";
    public string dataPath = "path/to/data.json";

    [Header("UI References")]
    public RawImage targetImage;
    public Text titleText;
    public Text descriptionText;
    public Text statusText;

    [Header("Settings")]
    public CacheSettings cacheSettings;
    public float refreshInterval = 60f;
    public float timeoutDuration = 10f;

    private string cacheDirectory;
    private Coroutine refreshCoroutine;

    private void Awake()
    {
        cacheDirectory = Application.persistentDataPath + "/GitHubCache/";
        if (!Directory.Exists(cacheDirectory))
        {
            Directory.CreateDirectory(cacheDirectory);
        }
    }

    private void Start()
    {
        StartLoading();
    }

    public void StartLoading()
    {
        if (refreshCoroutine != null)
        {
            StopCoroutine(refreshCoroutine);
        }
        refreshCoroutine = StartCoroutine(RefreshRoutine());
    }

    private IEnumerator RefreshRoutine()
    {
        while (true)
        {
            yield return LoadDataFromGitHub();
            yield return new WaitForSeconds(refreshInterval);
        }
    }

    private IEnumerator LoadDataFromGitHub()
    {
        statusText.text = "Loading...";

        if (cacheSettings.useCache && TryLoadFromCache())
        {
            statusText.text = "Using cached data";
        }

        yield return LoadImage();
        yield return LoadJsonData();

        statusText.text = "Last update: " + System.DateTime.Now.ToString("HH:mm:ss");
    }

    private IEnumerator LoadImage()
    {
        string imageUrl = githubRawUrl + imagePath;
        string cachePath = cacheDirectory + Path.GetFileName(imagePath);

        if (cacheSettings.useCache && File.Exists(cachePath) && 
            (System.DateTime.Now - File.GetLastWriteTime(cachePath)).TotalSeconds < cacheSettings.cacheDuration)
        {
            byte[] imageData = File.ReadAllBytes(cachePath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageData))
            {
                targetImage.texture = texture;
                yield break;
            }
        }

        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl);
        imageRequest.timeout = (int)timeoutDuration;
        yield return imageRequest.SendWebRequest();

        if (imageRequest.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(imageRequest);
            targetImage.texture = texture;

            if (cacheSettings.useCache)
            {
                File.WriteAllBytes(cachePath, texture.EncodeToPNG());
            }
        }
        else
        {
            Debug.LogError("Image load failed: " + imageRequest.error);
            statusText.text = "Image load failed";
        }
    }

    private IEnumerator LoadJsonData()
    {
        string jsonUrl = githubRawUrl + dataPath;
        string cachePath = cacheDirectory + Path.GetFileName(dataPath);

        if (cacheSettings.useCache && File.Exists(cachePath) && 
            (System.DateTime.Now - File.GetLastWriteTime(cachePath)).TotalSeconds < cacheSettings.cacheDuration)
        {
            string cachedJson = File.ReadAllText(cachePath);
            ProcessJsonData(cachedJson);
            yield break;
        }

        UnityWebRequest jsonRequest = UnityWebRequest.Get(jsonUrl);
        jsonRequest.timeout = (int)timeoutDuration;
        yield return jsonRequest.SendWebRequest();

        if (jsonRequest.result == UnityWebRequest.Result.Success)
        {
            string jsonData = jsonRequest.downloadHandler.text;
            ProcessJsonData(jsonData);

            if (cacheSettings.useCache)
            {
                File.WriteAllText(cachePath, jsonData);
            }
        }
        else
        {
            Debug.LogError("JSON load failed: " + jsonRequest.error);
            statusText.text = "Data load failed";
        }
    }

    private bool TryLoadFromCache()
    {
        bool loadedFromCache = false;
        string imageCachePath = cacheDirectory + Path.GetFileName(imagePath);
        string jsonCachePath = cacheDirectory + Path.GetFileName(dataPath);

        if (File.Exists(imageCachePath))
        {
            byte[] imageData = File.ReadAllBytes(imageCachePath);
            Texture2D texture = new Texture2D(2, 2);
            if (texture.LoadImage(imageData))
            {
                targetImage.texture = texture;
                loadedFromCache = true;
            }
        }

        if (File.Exists(jsonCachePath))
        {
            string jsonData = File.ReadAllText(jsonCachePath);
            ProcessJsonData(jsonData);
            loadedFromCache = true;
        }

        return loadedFromCache;
    }

    private void ProcessJsonData(string jsonData)
    {
        try
        {
            JObject jsonObject = JObject.Parse(jsonData);
            titleText.text = jsonObject["title"]?.ToString() ?? "No Title";
            descriptionText.text = jsonObject["description"]?.ToString() ?? "No Description";
        }
        catch (System.Exception e)
        {
            Debug.LogError("JSON parse error: " + e.Message);
            titleText.text = "Data Error";
            descriptionText.text = "Failed to parse content";
        }
    }

    public void ClearCache()
    {
        if (Directory.Exists(cacheDirectory))
        {
            Directory.Delete(cacheDirectory, true);
            Directory.CreateDirectory(cacheDirectory);
            statusText.text = "Cache cleared";
        }
    }

    private void OnDestroy()
    {
        if (refreshCoroutine != null)
        {
            StopCoroutine(refreshCoroutine);
        }
    }
}

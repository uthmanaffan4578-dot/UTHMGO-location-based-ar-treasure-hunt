using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LocationBased : MonoBehaviour
{
    [Header("Chest Prefabs (1 per location)")]
    public GameObject[] chestPrefabs;

    [Header("UI References")]
    public TMP_Text debugTxt;
    public Button interactButton;
    public Button takeButton;

    public TMP_Text riddleText;

    public TMP_Text chestCountText;

    private const float proximityThreshold = 0.01f;
    private char unit = 'K';
    private bool gps_ok = false;
    private GameObject currentChest = null;
    private int currentTreasureIndex = -1;
    private int treasuresCollected = 0;

    private string[] riddles = new string[]
{
    "Where hearts find peace and spirits rise, Echoes of prayer reach the skies. A place of stillness, pure and deep, Find the chest where faith does sleep.",
    "Where minds are coded and logic takes flight, Machines and humans work day and night. From binary dreams to tech so supreme, Find the chest where the programmers scheme.",
    "Where thunderous cheers break the air, And athletes show strength beyond compare. Run, leap, sweat — no time to rest, Search where athletes give their best.",
    "Robes in black and scrolls in hand, Proud faces gather as families stand. Cheers and tears fill the air so high, Find the chest where dreams touch the sky.",
    "Silent whispers between the shelves, Guarding knowledge like secret elves. Pages turn and minds ignite, Find your prize near wisdom’s light."
};

    private GPSLoc currLoc = new GPSLoc();
    private List<GPSLoc> treasureLocations = new List<GPSLoc>
    {
        new GPSLoc(103.081412f, 1.855177f), //  1.855177, 103.081412    MASJID    
        new GPSLoc(103.084421f, 1.860211f), //  1.860211, 103.084421    FSKTM   
        new GPSLoc(103.086006f, 1.853502f), //  1.853502, 103.086006    lokasi Stadium  
        new GPSLoc(103.080764f, 1.857747f), //  1.857747, 103.080764    DSI
        new GPSLoc(103.082112f, 1.857112f), //  1.857112, 103.082112    lokasi library
    };

    private List<bool> foundTreasure;

    IEnumerator Start()
    {
#if UNITY_ANDROID
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.Camera))
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.Camera);
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
#endif

        foundTreasure = new List<bool>(new bool[treasureLocations.Count]);

        if (!Input.location.isEnabledByUser)
        {
            debugTxt.text = "❌ Location not enabled.";
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            debugTxt.text = "❌ GPS Timeout or Failed.";
            yield break;
        }

        gps_ok = true;
        debugTxt.text = "✅ GPS Ready";
    }

    void Update()
    {
        if (!gps_ok) return;

        currLoc.lat = Input.location.lastData.latitude;
currLoc.lon = Input.location.lastData.longitude;

debugTxt.text = $"📍 Lat: {currLoc.lat:F6}\nLon: {currLoc.lon:F6}";

for (int i = 0; i < treasureLocations.Count; i++)
{
    double dist = CalculateDistance(
        currLoc.lat, currLoc.lon,
        treasureLocations[i].lat, treasureLocations[i].lon,
        unit);

    debugTxt.text += $"\n🎯 Treasure {i + 1}: {dist:F3} km";

    if (dist < proximityThreshold && !foundTreasure[i])
    {
        SpawnChest(i);
    }
    else if (dist >= proximityThreshold && currentTreasureIndex == i)
    {
        RemoveCurrentChest(); // ✅ hanya remove, tak reset flag
    }
}
    }

    void SpawnChest(int index)
{
    if (currentChest != null) return;

    currentTreasureIndex = index;

    currentChest = Instantiate(chestPrefabs[index],
        Camera.main.transform.position + Camera.main.transform.forward * 2f,
        Quaternion.identity);

    INTERACT_CHEST chestScript = currentChest.GetComponent<INTERACT_CHEST>();
    if (chestScript != null)
    {
        chestScript.Setup(interactButton, takeButton, () => OnTreasureTaken(index), riddleText, index);
    }

    interactButton.gameObject.SetActive(true);
    takeButton.gameObject.SetActive(false);

    debugTxt.text += $"\n🎉 Treasure {index + 1} spawned!";
}

    void RemoveCurrentChest()
    {
        if (currentChest != null)
        {
            Destroy(currentChest);
            currentChest = null;
            currentTreasureIndex = -1;
        }

        interactButton.onClick.RemoveAllListeners();
        takeButton.onClick.RemoveAllListeners();

        interactButton.gameObject.SetActive(false);
        takeButton.gameObject.SetActive(false);
    }

    void OnTreasureTaken(int index)
{
    foundTreasure[index] = true;
    treasuresCollected++;
    chestCountText.text = $": {treasuresCollected}";
    RemoveCurrentChest();

    // ✅ Update riddle ke next (jika masih ada)
    int nextIndex = index + 1;
    if (nextIndex < riddles.Length && riddleText != null)
    {
        riddleText.text = riddles[nextIndex];
    }

    if (treasuresCollected >= treasureLocations.Count)
    {
        SceneManager.LoadScene(6);
    }
}

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2, char unit)
{
    double R = 6371.0; // Radius of Earth in KM

    double dLat = Deg2Rad(lat2 - lat1);
    double dLon = Deg2Rad(lon2 - lon1);

    double a = Mathf.Sin((float)dLat / 2) * Mathf.Sin((float)dLat / 2) +
               Mathf.Cos((float)Deg2Rad(lat1)) * Mathf.Cos((float)Deg2Rad(lat2)) *
               Mathf.Sin((float)dLon / 2) * Mathf.Sin((float)dLon / 2);

    double c = 2 * Mathf.Atan2(Mathf.Sqrt((float)a), Mathf.Sqrt((float)(1 - a)));

    double distance = R * c;

    if (unit == 'N') distance *= 0.539957; // Nautical Miles
    return distance;
}

    private double Deg2Rad(double deg) => deg * Mathf.PI / 180.0;
    private double Rad2Deg(double rad) => rad * 180.0 / Mathf.PI;
}

[System.Serializable]
public class GPSLoc
{
    public float lon;
    public float lat;

    public GPSLoc() { lon = 0; lat = 0; }

    public GPSLoc(float lon, float lat)
    {
        this.lon = lon;
        this.lat = lat;
    }
} 
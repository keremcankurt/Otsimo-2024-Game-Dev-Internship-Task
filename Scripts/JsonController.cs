using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

[System.Serializable]
class Data
{
    public List<LineRendererStats> lines;
    public List<PaintballStats> paintballs;
    public Color bg = Color.white;
}
public class JsonController : MonoBehaviour
{
    public GameObject pen;
    GameManager gameManager;

    [SerializeField] Data data = new();


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
        GetPermission();
        
        LoadJson();
        string savePath = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    void GetPermission()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageRead);

            }
        }
    }
    public void AddLineRenderer(LineRenderer line)
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < line.positionCount; i++)
        {
            positions.Add(line.GetPosition(i));

        }

        LineRendererStats stats = new(line.startColor, positions, line.startWidth, line.sortingOrder);
        data.lines.Add(stats);


    }
    public void AddPaintball(GameObject paintball)
    {
        Vector3 position = paintball.transform.position;

        Renderer renderer = paintball.GetComponent<Renderer>();
        Color color = renderer.material.color;
        int sortingOrder = renderer.sortingOrder;
        PaintballStats stats = new(position, color, sortingOrder);
        data.paintballs.Add(stats);
    }
    public void SaveData()
    {
        gameManager.soundController.Playback("touch");
        try
        {
            GetPermission();
            data.lines.Clear();
            data.paintballs.Clear();
            for (int i = 0; i < pen.transform.childCount; i++)
            {
                if (pen.transform.GetChild(i).name == "LineRenderer")
                {
                    AddLineRenderer(pen.transform.GetChild(i).GetComponent<LineRenderer>());
                }
                else if (pen.transform.GetChild(i).name == "Paintball")
                {
                    AddPaintball(pen.transform.GetChild(i).gameObject);
                }
            }
            data.bg = gameManager.canvas.GetComponent<Renderer>().material.color;
            string json = JsonUtility.ToJson(data);
            string path;
#if UNITY_ANDROID
            path = Application.persistentDataPath + "/Saves/Data.json";
#endif
#if UNITY_EDITOR
            path = Application.dataPath + "/Saves/Data.json";
#endif
            File.WriteAllText(path, json);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }

    }

    public void LoadJson()
    {
        try
        {
            string path;
#if UNITY_ANDROID
            path = Application.persistentDataPath + "/Saves/Data.json";
#endif
#if UNITY_EDITOR
            path = Application.dataPath + "/Saves/Data.json";
#endif
            if (File.Exists(path))
            {

                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<Data>(json);
                if (data.bg != null)
                {
                    gameManager.canvas.GetComponent<Renderer>().material.color = data.bg;
                }
                for (int i = 0; i < data.lines.Count; i++)
                {
                    AddLineRenderer(data.lines[i]);
                }
                for (int i = 0; i < data.paintballs.Count; i++)
                {
                    AddPaintball(data.paintballs[i]);
                }
            }
            
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            throw;
        }

    }
    void AddPaintball(PaintballStats stats)
    {
        GameObject paintball = Instantiate(gameManager.paintball, pen.transform);
        paintball.transform.position = stats.position;
        Renderer renderer = paintball.GetComponent<Renderer>();
        renderer.material.color = stats.bg;
        renderer.sortingOrder = stats.sortingOrder;
        paintball.name = "Paintball";
    }
    void AddLineRenderer(LineRendererStats line)
    {
        LineRenderer _line = Instantiate(gameManager.trailPrefab, pen.transform);
        _line.startColor = line.color;
        _line.endColor = line.color;
        _line.sortingOrder = line.sortingOrder;
        _line.startWidth = line.startWidth;
        Vector3[] positionsArray = line.points.ToArray();
        _line.positionCount = positionsArray.Length;
        _line.SetPositions(positionsArray);
        _line.name = "LineRenderer";
    }
}

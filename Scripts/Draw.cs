using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class Draw : MonoBehaviour
{
    [SerializeField] Camera Cam;

    private LineRenderer currentTrail;

    private List<Vector3> points = new();

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!Cam)
        {
            Cam = Camera.main;
        }
    }

    
    void Update()
    {
        if (gameManager.selected == "Pen" || gameManager.selected == "Rubber")
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                

                if (touch.phase == TouchPhase.Began)
                {
                    
                    if (!IsTouchOverUI(touch.position))
                    {
                        gameManager.soundController.Playback("spray", true);
                        CreateNewLine();
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    
                    if (!IsTouchOverUI(touch.position))
                    {
                        AddPoint();
                    }
                }
                else if(touch.phase == TouchPhase.Ended)
                {
                    gameManager.soundController.StopPlayback();
                }
            }
        }
        else if (gameManager.selected == "Fill")
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Stationary)
                {
                    if (!IsTouchOverUI(touch.position))
                    {

                        gameManager.canvas.GetComponent<Renderer>().material.color = gameManager.color;


                        for (int i = transform.childCount - 1; i >= 0; i--)
                        {
                            Destroy(transform.GetChild(i).gameObject);
                        }
                    }
                }
            }
        }
        else if (gameManager.selected == "Paintball")
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (!IsTouchOverUI(touch.position))
                {
                    if (touch.phase == TouchPhase.Ended)
                    {
                        Paintball();
                    }
                }
            }
        }
    }

    bool IsTouchOverUI(Vector2 touchPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = touchPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0; 
    }


    private void CreateNewLine()
    {
        currentTrail = Instantiate(gameManager.trailPrefab);
        currentTrail.name = "LineRenderer";
        currentTrail.transform.SetParent(transform, true);
        currentTrail.sortingOrder = transform.childCount;
        if (gameManager.selected == "Rubber")
        {
            currentTrail.startColor = Color.white;
            currentTrail.endColor = Color.white;
            currentTrail.startWidth = 0.5f;
        }
        else
        {
            currentTrail.startColor = gameManager.color;
            currentTrail.endColor = gameManager.color;
        }



        points.Clear();
    }

    private void UpdateLinePoints()
    {
        if (currentTrail != null && points.Count > 1)
        {
            currentTrail.positionCount = points.Count;
            currentTrail.SetPositions(points.ToArray());

        }
    }

    private void AddPoint()
    {
        var Ray = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(Ray, out hit))
        {
            if (hit.collider.CompareTag("Writeable"))
            {
                points.Add(hit.point);

                UpdateLinePoints();
            }
        }
    }

    void Paintball()
    {
        gameManager.soundController.Playback("paintball");
        var Ray = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(Ray, out hit))
        {
            if (hit.collider.CompareTag("Writeable"))
            {
                GameObject _paintball = Instantiate(gameManager.paintball, hit.point, gameManager.paintball.transform.rotation, transform);
                _paintball.name = "Paintball";
                Renderer renderer = _paintball.GetComponent<Renderer>();
                renderer.material.color = gameManager.color;
                renderer.sortingOrder = transform.childCount;
            }
        }
    }

}
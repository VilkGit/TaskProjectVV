using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawPanel : MonoBehaviour
{
    public delegate void StoppedDraw(List<Vector3> pointDrawLeg);
    public StoppedDraw stoppedDraw;
        
    [Header("Settings draw panel")]
    [SerializeField] float _distanсeBetweenPointsToSave = 1;
    [Range(0, 1)]
    [SerializeField] float _slowTime = 1;
    [Header("Set relation")]
    [SerializeField] Camera _cameraDraw;
    [SerializeField] GameObject _panelDraw;

    private LineRenderer _lineDraw;

    // Start is called before the first frame update
    void Start()
    {
        _lineDraw = GetComponent<LineRenderer>();
        _lineDraw.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            RaycastHit hit = DropRay(_cameraDraw, Input.touches[0].position);
            if (!CheckColliderRayWithPanel(hit)) return;

            Vector3 positionLocalSpace = transform.TransformDirection(hit.point);

            if (Input.touches[0].phase == TouchPhase.Began)
            {
                StartDraw(positionLocalSpace);
            }
            else if(Input.touches[0].phase == TouchPhase.Moved)
            {
                Drawing(positionLocalSpace);
            }
            else if(Input.touches[0].phase == TouchPhase.Ended)
            {
                StopDraw();
            }
        }
        else if(Input.GetMouseButton(0))
        {
            RaycastHit hit = DropRay(_cameraDraw, Input.mousePosition);
            if (!CheckColliderRayWithPanel(hit)) return;

            Vector3 positionLocalSpace = hit.point - transform.position;

            if (Input.GetMouseButtonDown(0))
            {
                StartDraw(positionLocalSpace);
            }
            else if (Input.GetMouseButton(0))
            {
                Drawing(positionLocalSpace);
            }    
        }

        else if (Input.GetMouseButtonUp(0))
        {
            StopDraw();
        }
    }

    bool CheckColliderRayWithPanel(RaycastHit hit)
    {
        if (hit.collider != null && hit.collider.gameObject == _panelDraw.gameObject)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    RaycastHit DropRay(Camera camera, Vector2 positionScreen)
    {
        Ray ray = camera.ScreenPointToRay(positionScreen);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        return hit;
    }

    void StartDraw(Vector3 pointInLocalSpase)
    {
        Time.timeScale = _slowTime;
        _lineDraw.positionCount = 2;
        _lineDraw.SetPosition(0, pointInLocalSpase);
        _lineDraw.SetPosition(1, pointInLocalSpase);
    }
    
    void Drawing(Vector3 pointInLocalSpase)
    {
        _lineDraw.SetPosition(_lineDraw.positionCount - 1, pointInLocalSpase);

        Vector3 previousPositionPointLine = _lineDraw.GetPosition(_lineDraw.positionCount - 2);

        if ((pointInLocalSpase - previousPositionPointLine)
            .magnitude > _distanсeBetweenPointsToSave)
        {
            _lineDraw.positionCount++;
            _lineDraw.SetPosition(_lineDraw.positionCount - 1, pointInLocalSpase);
        }
    }

    void StopDraw()
    {
        Time.timeScale = 1;
        Vector3[] points = new Vector3[_lineDraw.positionCount];
        _lineDraw.GetPositions(points);
        List<Vector3> lerpPoint = points.ToList<Vector3>();

        for(int i = 1; i < lerpPoint.Count; i++)
        {
            lerpPoint[i] = Vector3.Lerp(lerpPoint[i-1], lerpPoint[i], 0.6f);
        }

        if(stoppedDraw != null)
        {
            stoppedDraw(lerpPoint);
        }
        
        _lineDraw.positionCount = 0;
    }

    
}

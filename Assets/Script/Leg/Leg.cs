using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg : MonoBehaviour
{
    private AdminLegs adminLegs = null;
    private List<Vector3> _controlPointDrawLeg = new List<Vector3>();
    private List<GameObject> _partsLeg = new List<GameObject>();
    private GameObject _point1 = null;
    private GameObject _point2 = null;
    private Coroutine _showPartLeg = null;

    private void Awake()
    {
        adminLegs = Resources.Load<AdminLegs>("AdminLegs");
        _point1 = new GameObject();
        _point1.transform.SetParent(transform);
        _point2 = new GameObject();
        _point2.transform.SetParent(transform);
    }

    public void DrawNewLeg(List<Vector3> pointsDrawLeg)
    {
        DeleteLeg();

        _controlPointDrawLeg = ChengePositionRelativeOrigin(pointsDrawLeg);

        for(int i = 1; i < _controlPointDrawLeg.Count; i++)
        {
            _point1.transform.localPosition = _controlPointDrawLeg[i - 1];
            _point2.transform.localPosition = _controlPointDrawLeg[i];
            DrawPartLegForPoint(_point1, _point2, adminLegs.sizePartLeg);
        }

        _showPartLeg = StartCoroutine(ShowPartLeg());
    }    

    public void DeleteLeg()
    {
        if(_showPartLeg != null) StopCoroutine(_showPartLeg);

        if (_partsLeg.Count != 0)
        {
            for(int i = _partsLeg.Count-1; i >= 0; i--)
            {
               Destroy(_partsLeg[i]);
            }
            _partsLeg.Clear();
        }
        
    }

    public void DestroyLastPartLeg()
    {
        if(_partsLeg.Count != 0)
        {
            GameObject partLeg = _partsLeg[_partsLeg.Count - 1];
            _partsLeg.Remove(partLeg);
            Destroy(partLeg);
        }
    }

    IEnumerator ShowPartLeg()
    {
        for (int i = 0; i < _partsLeg.Count; i++)
        {
            _partsLeg[i].SetActive(true);
            yield return new WaitForSeconds(adminLegs.timeWaitShowPartLeg);
        }
    }

    private void DrawPartLegForPoint(GameObject point1, GameObject point2, float sizePartLeg)
    {
        Vector3 p1Position = point1.transform.localPosition;
        Vector3 p2Position = point2.transform.localPosition;
        Transform lookAtTransformPoint = point2.transform;
        Vector3 deltaPosition = p2Position - p1Position;

        for(int i = 0; i < deltaPosition.magnitude/sizePartLeg; i++)
        {
            GameObject partLeg = CreatePartLeg(adminLegs.prefPartLeg, p1Position, lookAtTransformPoint, sizePartLeg);
            partLeg.SetActive(false);
            _partsLeg.Add(partLeg);
            p1Position = СalculateNextPointPositionPartLeg(p1Position, deltaPosition.normalized, sizePartLeg);
        }
    }

    // offsets the leg draw coordinate to the beginning of the local leg position
    private List<Vector3> ChengePositionRelativeOrigin(List<Vector3> listPosition)
    {
        List<Vector3> newList = new List<Vector3>();
        for (int i = 0; i < listPosition.Count; i++)
        {
            newList.Add(listPosition[i] - listPosition[0]);
        }

        return newList;
    }

    private Vector3 СalculateNextPointPositionPartLeg(Vector3 actualPoint, Vector3 normalDirection, float sizeLeg)
    {
        return actualPoint + normalDirection * sizeLeg;
    }

    private GameObject CreatePartLeg(GameObject partLeg, Vector3 localPosition, Transform lookAtLocalPoint, float sizePart)
    {
        GameObject part = Instantiate<GameObject>(partLeg);
        part.transform.SetParent(transform);
        part.transform.localScale = Vector3.one * sizePart;
        part.transform.localPosition = localPosition;
        part.transform.LookAt(lookAtLocalPoint);

        return part;
    }

}

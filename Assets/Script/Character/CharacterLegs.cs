using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLegs : MonoBehaviour
{
    [SerializeField] DrawPanel _drawPanel = null;
    private List<Leg> _legs = new List<Leg>();
    private Rigidbody _legsBody = null;

    public List<Leg> legs
    {
        get
        {
            return _legs;
        }
    }

    private void Awake()
    {
        _legsBody = GetComponent<Rigidbody>();
        transform.GetComponentsInChildren<Leg>(_legs);

        foreach (Leg l in _legs)
        {
            _drawPanel.stoppedDraw += l.DrawNewLeg;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 velocityBody = _legsBody.velocity;
        velocityBody.y = 0;
        _legsBody.velocity = velocityBody;
    }
    
    private void OnDestroy()
    {
        foreach (Leg l in legs)
        {
            _drawPanel.stoppedDraw -= l.DrawNewLeg;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    
    private CharacterLegs _characterLegs = null;
    private CharacterBody _characterBody = null;

    public CharacterLegs characterLegs
    {
        get
        {
            return _characterLegs;
        }
    }
    public CharacterBody characterBody
    {
        get
        {
            return _characterBody;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponentInChildren<CharacterLegs>(
            _characterLegs);
        transform.GetComponentInChildren<CharacterBody>(
            _characterBody);
    }


}

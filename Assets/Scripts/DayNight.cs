using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
    [SerializeField] private float _velocidad = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_velocidad * Time.deltaTime, 0, 0);
    }
}

using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Polyline))]
public class PolylineArmTest : MonoBehaviour
{
    private Vector2 mousePos;
    //[SerializeField] float spawnDistance = .5f;
    private Polyline pl;

    private void Start()
    {
        pl = GetComponent<Polyline>();
    }
    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pl.SetPointPosition(pl.points.Count-1, mousePos);
    }
}

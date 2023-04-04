using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField]
    private Camera cam;

    private Vector3 dragOrigin;

    [SerializeField]
    private SpriteRenderer mapRenderer;

    [SerializeField]
    private float maxZoom = 4.12f;

    [SerializeField]
    private float minZoom = 1.0f;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;


    private void Awake()
    {
        

        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y /2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y /2f;

    }
    void Start()
    {
        
    }

    void Update()
    {
        PanCamera();
        ZoomCamera();
    }

    void PanCamera()
    {
        //save position of the mouse in world space when drag starts (first time clicked)
        if(Input.GetMouseButtonDown(1))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        //calculate distance between drag origin and new position if the mouse is still held down

        if(Input.GetMouseButton(1))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            //move the camera by that distance
            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }

        
    }


    void ZoomCamera()
    {
        float zoom = -Input.GetAxis("Mouse ScrollWheel"); // invert the direction of zoom
        cam.orthographicSize += zoom;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

        cam.transform.position = ClampCamera(cam.transform.position);
    }


    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3 (newX, newY, targetPosition.z);
    }
}

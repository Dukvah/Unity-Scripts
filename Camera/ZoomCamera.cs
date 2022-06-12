using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    Vector3 touchStart;

    public Transform rotater;

    public float xMax;
    public float xMin;
    public float yMax;
    public float yMin;
        
    public float zoomOutMin;
    public float zoomOutMax;


    void Update()
    {
        if (Player.isPlaying == true && Time.timeScale == 1)
        {
            Camera.main.transform.position =
                    new Vector3(Mathf.Clamp(Camera.main.transform.position.x, xMin, xMax), 
                    Mathf.Clamp(Camera.main.transform.position.y, yMin, yMax), Camera.main.transform.position.z);

            if (Input.GetMouseButtonDown(0))
            {
                touchStart = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                Zoom(difference * 0.01f);
            }
            else if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - Camera.main.ScreenToViewportPoint(Input.mousePosition);

                Camera.main.transform.position =
                    new Vector3(Mathf.Clamp(Camera.main.transform.position.x + direction.x * 6, xMin, xMax), 
                    Mathf.Clamp(Camera.main.transform.position.y + direction.y * 6, yMin, yMax), Camera.main.transform.position.z);
                Vector3 rotaterPos = new Vector3(this.transform.position.x, rotater.position.y, rotater.position.z);
                Camera.main.transform.LookAt(rotaterPos);
                touchStart = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                direction.x = 0.0f;
                direction.y = 0.0f;
            }
            Zoom(Input.GetAxis("Mouse ScrollWheel") * 3);
        }
    }

    void Zoom(float increment)
    {
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - increment * 7, zoomOutMin, zoomOutMax);
    }
}

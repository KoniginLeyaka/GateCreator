using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EditSystem : MonoBehaviour
{
    private Camera camera;
    bool isSelected = false;
    GameObject selectedObject = null;

    private void Awake()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null)
                {
                    int layer = hit.collider.gameObject.layer;
                    if (layer == 6 && isSelected == false)
                    {
                        isSelected = true;
                        selectedObject = hit.collider.gameObject;
                    }
                    else if (layer == 6 && isSelected == true)
                    {
                        isSelected = false;
                        selectedObject = null;
                    }
                }
            }
        }
        if (selectedObject != null)
        {
            ObjectMove(ref selectedObject);
        }
    }
    private void ObjectMove(ref GameObject editObject)
    {
        Vector3 editObjectPos = editObject.transform.position;
        if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3 mouseStartPos = Input.mousePosition; // тут доделай блять
            Vector3 mouseCurrentPos = Input.mousePosition; // и тут нахуй
            //editObjectPos = editObjectPos + new Vector3(mousePos.x,mousePos.y,mousePos.z);
            //editObject.transform.position = editObjectPos;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EditSystem : MonoBehaviour
{
    private Camera camera;
    bool isSelected = false,isOperation = false;
    private float moveY,moveX;
    private float storage;
    GameObject selectedObject = null;

    private void Awake()
    {
        camera = Camera.main;
    }

    void Update()
    {
        moveY = Input.GetAxis("Mouse Y") * (Time.deltaTime * 150);
        moveX = Input.GetAxis("Mouse X") * (Time.deltaTime * 150);
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null)
                {
                    int layer = hit.collider.gameObject.layer;
                    if (layer == 6 && !isSelected && !isOperation)
                    {
                        isSelected = true;
                        Debug.Log("Объект выбран: " + hit.collider.gameObject.name);
                        selectedObject = hit.collider.gameObject;
                    }
                    else if (layer == 6 && isSelected && !isOperation)
                    {
                        isSelected = false;
                        Debug.Log("Выделение объекта убрано: " + hit.collider.gameObject.name);
                        selectedObject = null;
                    }
                }
            }
            if (Input.GetMouseButtonDown(0) && isOperation)
            {
                isOperation = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3 mouseStartPos = Input.mousePosition;
            if (selectedObject != null) ObjectMove(ref selectedObject, mouseStartPos);
        }
        if (Input.GetKeyDown(KeyCode.S) && isSelected && !isOperation)
        {
            Debug.Log("c");
            isOperation = true;
            StartCoroutine(resize());
            if (Input.GetKeyDown(KeyCode.Escape)) isOperation = false;
        }
    }
    private IEnumerator resize()
    {
        while(isOperation)
        {
            storage += moveY + moveX;
            selectedObject.transform.localScale = new Vector3(storage, storage, storage);
            yield return new WaitForSeconds(0.02f);
        }
    }
    private void ObjectMove(ref GameObject editObject, Vector3 mouseStartPos)
    {
        Vector3 editObjectPos = editObject.transform.position;
        Vector3 mouseCurrentPos = Input.mousePosition;
        editObjectPos = camera.ScreenToWorldPoint(new Vector3(mouseCurrentPos.x, mouseCurrentPos.y, 10f));
        editObject.transform.position = editObjectPos;
    }
}
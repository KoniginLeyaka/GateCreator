using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class EditSystem : MonoBehaviour
{
    private Camera camera;
    bool isSelected = false,isOperation = false,isActiveRmbMenu = false;
    private float moveY,moveX;
    private float storageX,storageY,storageZ;
    GameObject selectedObject = null;
    [SerializeField] GameObject rmbMenu;
    [SerializeField] GameObject lmbMenu;
    [SerializeField] Material shaderObject;
    [SerializeField] Material defaultMaterial;

    private void Awake()
    {
        camera = Camera.main;
        rmbMenu.SetActive(false);
        lmbMenu.SetActive(false);
    }

    private void SelectObject(RaycastHit hit, Vector3 mousePos, Vector3 vector)
    {
        isSelected = true;
        Debug.Log("Объект выбран: " + hit.collider.gameObject.name);
        selectedObject = hit.collider.gameObject;
        lmbMenu.transform.position = mousePos + vector;
        lmbMenu.SetActive(true);
        selectedObject.GetComponent<MeshRenderer>().material = shaderObject;
    }
    private void UnselectObject(RaycastHit hit)
    {
        isSelected = false;
        lmbMenu.SetActive(false);
        Debug.Log("Выделение объекта убрано: " + hit.collider.gameObject.name);
        selectedObject.GetComponent<MeshRenderer>().material = defaultMaterial;
        selectedObject = null;
    }

    void Update()
    {
        moveY = Input.GetAxis("Mouse Y") * (Time.deltaTime * 150);
        moveX = Input.GetAxis("Mouse X") * (Time.deltaTime * 150);
        Vector3 mousePos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0)) // Выделение объекта ЛКМ
        {
            Vector3 vector = new Vector3(30, 0);
            Ray ray = camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null)
                {
                    int layer = hit.collider.gameObject.layer;
                    if (layer == 6 && !isSelected && !isOperation)
                    {
                        SelectObject(hit, mousePos, vector);
                    }
                    else if (layer == 6 && isSelected && !isOperation)
                    {
                        if (hit.collider.gameObject == selectedObject) UnselectObject(hit);
                        else
                        {
                            UnselectObject(hit);
                            SelectObject(hit, mousePos, vector);
                        }
                    }
                    if (layer != 6 && isSelected && !isOperation) UnselectObject(hit);
                }
            }
            if (Input.GetMouseButtonDown(0) && isOperation)
            {
                isOperation = false;
            }
        }

        if (Input.GetMouseButtonDown(1) && isSelected)
        {
            Ray ray = camera.ScreenPointToRay(mousePos);
            Vector3 vector = new Vector3(0, -150);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && hit.collider.gameObject == selectedObject && !isActiveRmbMenu)
                {
                    rmbMenu.transform.position = Input.mousePosition + vector;
                    rmbMenu.SetActive(true);
                    isActiveRmbMenu = true;
                } else if ((hit.collider == null || hit.collider.gameObject != selectedObject || hit.collider.gameObject == selectedObject) && isActiveRmbMenu)
                {
                    rmbMenu.SetActive(false);
                    isActiveRmbMenu = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Vector3 mouseStartPos = Input.mousePosition;
            if (selectedObject != null) ObjectMove(ref selectedObject, mouseStartPos);
        }
        if (Input.GetKeyDown(KeyCode.S) && isSelected && !isOperation)
        {
            StartCoroutine(resize());
            if (Input.GetKeyDown(KeyCode.Escape)) isOperation = false;
        }
    }
    private IEnumerator resize()
    {
        Debug.Log("resize");
        isOperation = true;
        Vector3 localScale = selectedObject.transform.localScale;
        storageX = localScale.x;
        storageY = localScale.y;
        storageZ = localScale.z;
        while (isOperation)
        {
            storageX += moveY + moveX;
            storageY += moveY + moveX;
            storageZ += moveY + moveX;
            selectedObject.transform.localScale = new Vector3(storageX, storageY, storageZ);
            yield return new WaitForSeconds(0.02f);
        }
    }
    public void helpResizeFunction()
    {
        StartCoroutine(resize()); 
    }
    private void ObjectMove(ref GameObject editObject, Vector3 mouseStartPos)
    {
        Vector3 editObjectPos = editObject.transform.position;
        Vector3 mouseCurrentPos = Input.mousePosition;
        editObjectPos = camera.ScreenToWorldPoint(new Vector3(mouseCurrentPos.x, mouseCurrentPos.y, 10f));
        editObject.transform.position = editObjectPos;
    }
}
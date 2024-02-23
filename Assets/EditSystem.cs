using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EditSystem : MonoBehaviour
{
    private Camera camera;
    bool isSelected = false,isOperation = false,isActiveRmbMenu = false;
    private float moveY,moveX;
    private float storage;
    GameObject selectedObject = null;
    [SerializeField] GameObject rmbMenu;
    [SerializeField] GameObject lmbMenu;
    [SerializeField] Button resizeButton;

    private void Awake()
    {
        camera = Camera.main;
        rmbMenu.SetActive(false);
        lmbMenu.SetActive(false);
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
                        isSelected = true;
                        Debug.Log("Объект выбран: " + hit.collider.gameObject.name);
                        selectedObject = hit.collider.gameObject;
                        lmbMenu.transform.position = mousePos + vector;
                        lmbMenu.SetActive(true);
                    }
                    else if (layer == 6 && isSelected && !isOperation)
                    {
                        isSelected = false;
                        lmbMenu.SetActive(false);
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
        while (isOperation)
        {
            storage += moveY + moveX;
            selectedObject.transform.localScale = new Vector3(storage, storage, storage);
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
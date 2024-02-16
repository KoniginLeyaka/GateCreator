using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] List<GameObject> prefabs = new List<GameObject>();

    public void BtnCubeCreate()
    {
        GameObject cube = Instantiate(prefabs[0],transform.position,transform.rotation) as GameObject;
    }
}

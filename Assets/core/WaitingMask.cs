using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WaitingMask : MonoBehaviour
{
    [SerializeField]
    public RawImage imageWaitting;


    private void Update()
    {
        this.imageWaitting.transform.Rotate(0.0f, 0.0f, Time.deltaTime * 200);
    }

    public void show() {
        this.imageWaitting.transform.rotation = Quaternion.Euler(0, 0, 0);
        this.gameObject.SetActive(true);
    }

    public void hide() {
        this.gameObject.SetActive(false);
    }
}

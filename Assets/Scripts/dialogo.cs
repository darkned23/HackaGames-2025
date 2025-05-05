using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dialogo : MonoBehaviour
{
    public TextMeshProUGUI Texto;
    public string[] Lineas;
    public float speed = 0.1f;
    int index;
    void Start()
    {
        Texto.text = string.Empty;
        Dialogo();
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Texto.text == Lineas[index])
            {
                siguiente();
            }
            else
            {
                StopAllCoroutines();
                Texto.text = Lineas[index];
            }
        }
    }

    public void Dialogo()
    {
        index = 0;
        StartCoroutine(Escrito());
    }

    IEnumerator Escrito()
    {
        foreach (char letter in Lineas[index].ToCharArray())
        {
            Texto.text += letter;
            yield return new WaitForSeconds(speed);
        }
    }

    public void siguiente()
    {
        if (index < Lineas.Length - 1)
        {
            index++;
            Texto.text = string.Empty;
            StartCoroutine(Escrito());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

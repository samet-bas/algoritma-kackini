using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class addFunction : MonoBehaviour
{
    public Transform parentContent;
    public Transform panelContent;
    public TMP_InputField nameInput;
    public GameObject functionPrefab;
    
    
    public void saveFunction()
    {
        // Yeni fonksiyon prefabını oluştur
        GameObject newFunction = Instantiate(functionPrefab, parentContent);
    
        // İçeriğini bul
        Transform functionContent = newFunction.transform.Find("Scroll View/Viewport/Content");

        // Paneldeki kod bloklarını kopyala
        foreach (CodeBlocks block in panelContent.GetComponentsInChildren<CodeBlocks>())
        {
            Instantiate(block.gameObject, functionContent);
        }

        // Fonksiyon ismini ayarla
        newFunction.GetComponentInChildren<TextMeshProUGUI>().text = nameInput.text;

        // Paneli temizle
        nameInput.text = "";

        foreach (CodeBlocks child in panelContent.GetComponentsInChildren<CodeBlocks>())
        {
            Destroy(child.gameObject);
        }
    }

}

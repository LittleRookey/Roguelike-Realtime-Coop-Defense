using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;

[CreateAssetMenu(menuName ="Litkey/Chant")]
public class Chant : ScriptableObject
{
    [SerializeField] private GameObject chantTextObject;
    [SerializeField] private Vector2 chantOffset = new Vector2(-0.01f, 4.5f);
    [TextArea]
    [SerializeField] private string chantSentence;
    [SerializeField] private Color textColor;
    [SerializeField] private float readSpeed = 0.05f;

    public string ChantSentence => chantSentence;
    public float ReadSpeed => readSpeed;

    private GameObject chantTextObjectCopy;
    private TextMeshProUGUI chantText;

    string emptyText;

    public void CreateChant(GameObject player)
    {
        if (chantText == null)
        {
            chantTextObjectCopy = Instantiate(chantTextObject, player.transform.position + (Vector3)chantOffset, Quaternion.identity, player.transform);
            chantText = chantTextObjectCopy.GetComponentInChildren<TextMeshProUGUI>();
            chantText.color = textColor;
        }

        chantText.gameObject.SetActive(true);
    }


    public async Task ReadChant()
    {
        chantText.SetText("");
        emptyText = "";
        for (int i = 0; i < chantSentence.Length; i++)
        {
            emptyText += chantSentence[i];
            chantText.SetText(emptyText);
            //yield return readSec;
            Debug.Log(emptyText);
            await Task.Delay((int)(readSpeed * 1000));
        }

        chantText.gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public enum ReadMode
{
    LineByLine,
    Linear
}
[CreateAssetMenu(menuName ="Litkey/Chant")]
public class Chant : ScriptableObject
{
    [SerializeField] private GameObject chantTextObject;
    [SerializeField] private Vector2 chantOffset = new Vector2(-0.01f, 4.5f);

    [TextArea]
    [SerializeField] private string chantSentence;
    [SerializeField] private ReadMode readMode;
    [SerializeField] private Color textColor;
    [SerializeField] private float readSpeed = 0.05f;
    [SerializeField] private float commaWaitTime; // how long to wait when there is comma
    [SerializeField] private float newLineWaitTime; // how long to wait when there is new line
    [field: SerializeField] public float delayEndChantTime { get; private set; } // how long to wait when Chant is done
    [Header("Light Settings")]
    
    [SerializeField] private Light2D lightVFX;
    [SerializeField] private Vector3 lightOffset;

    private Light2D lightCopy;

    public string ChantSentence => chantSentence;
    public float ReadSpeed => readSpeed;

    private GameObject chantTextObjectCopy;
    private TextMeshProUGUI chantText;

    string emptyText;

    string[] newLineSeparated;
    string[] commaSeparated;

    public UnityAction OnChantEnd;

    private void OnEnable()
    {
        newLineSeparated = chantSentence.Split('\n');
        commaSeparated = null;
        //if (chantText == null && chantTextObject != null)
        //{
        //    chantTextObjectCopy = Instantiate(chantTextObject);
        //    chantText = chantTextObjectCopy.GetComponentInChildren<TextMeshProUGUI>();
        //    chantText.color = textColor;
        //    chantTextObjectCopy.gameObject.SetActive(false);
        //}
        //if (lightCopy == null && lightVFX != null)
        //{
        //    lightCopy = Instantiate(lightVFX);
        //    lightCopy.intensity = 0f;
        //    lightCopy.gameObject.SetActive(false);
        //}
    }

    private void OnDisable()
    {
        commaSeparated = null;
        //if (chantTextObjectCopy)
        //    DestroyImmediate(chantTextObjectCopy.gameObject);
        //if (lightCopy)
        //    DestroyImmediate(lightCopy.gameObject);
    }

    public float CalculateChantTimeInSec()
    {
        float sum = 0f;
        switch (readMode)
        {
            case ReadMode.Linear:
                for (int i = 0; i < chantSentence.Length; i++)
                {
                    sum += readSpeed;
                }
                break;
            // 나의 힘, 나의 별 
            // 나의 이름 아래
            case ReadMode.LineByLine:
                //string[] lines = chantSentence.Split('\n'); // splits by new line
                for (int i = 0; i < newLineSeparated.Length; i++)
                {
                    commaSeparated = newLineSeparated[i].Split(',');
                    // 나의 힘 / 나의 별
                    for (int k = 0; k < commaSeparated.Length; k++)
                    {
                        // 나의 별
                        sum += GetLineTime(commaSeparated[k]);
                        if (k + 1 < commaSeparated.Length)
                            sum += commaWaitTime;
                    }
                    if (i + 1 < newLineSeparated.Length)
                        sum += newLineWaitTime;
                    else
                        sum += delayEndChantTime;
                }
                break;
        }
        return sum;
    }
    public void CreateChant(GameObject player)
    {
        if (chantText == null)
        {
            chantTextObjectCopy = Instantiate(chantTextObject, player.transform.position + (Vector3)chantOffset, Quaternion.identity, player.transform);
            chantText = chantTextObjectCopy.GetComponentInChildren<TextMeshProUGUI>();
            chantText.color = textColor;
        }
        if (lightCopy == null)
        {
            lightCopy = Instantiate(lightVFX, player.transform.position + lightOffset, Quaternion.identity, player.transform);
            lightCopy.intensity = 0f;
        }
        
        //chantTextObjectCopy.transform.SetParent(player.transform);
        //chantTextObjectCopy.transform.position = player.transform.position + (Vector3)chantOffset;
        chantTextObjectCopy.gameObject.SetActive(true);
        chantText.transform.gameObject.SetActive(true);

        //lightCopy.transform.SetParent(player.transform);
        //lightCopy.transform.position = player.transform.position + lightOffset;
        lightCopy.gameObject.SetActive(true);
        DOTween.To(() => lightCopy.intensity, x => lightCopy.intensity = x, 0.4f, 2f);
        //DotMoveFloat(ref lightCopy.intensity, 0.4f, 2f, () => Debug.Log(lightCopy.intensity), () => Debug.Log(lightCopy.intensity));
    }


    public async Task ReadChant()
    {
        //Debug.Log(chantSentence);
        //Debug.Log(chantSentence.Replace('\n', 'Q'));
        chantText.SetText("");
        emptyText = "";
        switch(readMode)
        {
            case ReadMode.Linear:
                for (int i = 0; i < chantSentence.Length; i++)
                {
                    emptyText += chantSentence[i];
                    chantText.SetText(emptyText);
                    Debug.Log(emptyText);
                    await Task.Delay((int)(readSpeed * 1000));
                }
                break;
                // 나의 힘, 나의 별 
                // 나의 이름 아래
            case ReadMode.LineByLine:
                //string[] lines = chantSentence.Split('\n'); // splits by new line
                for (int i = 0; i < newLineSeparated.Length; i++)
                {
                    commaSeparated = newLineSeparated[i].Split(',');
                    // 나의 힘 / 나의 별
                    for (int k = 0; k < commaSeparated.Length; k++)
                    {
                        //Debug.Log(commaSeparated[k]);
                        // 나의 별
                        await ReadThrough(commaSeparated[k]);
                        if (k+1 < commaSeparated.Length)
                            await Task.Delay((int)(commaWaitTime * 1000));
                    }
                    emptyText += '\n';
                    chantText.SetText(emptyText);
                    if (i+1 < newLineSeparated.Length)
                        await Task.Delay((int)(newLineWaitTime * 1000));
                    else
                        await Task.Delay((int)(delayEndChantTime * 1000));
                }
                break;
        }

        // On Chant End
        Debug.Log("LIGHTS OFFFFFFFFFFFFFFFFF");
        DOTween.To(() => lightCopy.intensity, x => lightCopy.intensity = x, 0f, .5f)
            .OnComplete(() => {
                lightCopy.gameObject.SetActive(false);
                chantText.gameObject.SetActive(false);
            });

        


    }

    private float GetLineTime(string context)
    {
        float sum = 0f;
        for (int i = 0; i < context.Length; i++)
        {
            sum += readSpeed;
        }
        return sum;
    }
    private async Task ReadThrough(string context)
    {
        for (int i = 0; i < context.Length; i++)
        {
            emptyText += context[i];
            chantText.SetText(emptyText);
            await Task.Delay((int)(readSpeed * 1000));
        }
    }

}

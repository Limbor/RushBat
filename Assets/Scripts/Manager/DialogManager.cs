using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{

    [Header("UI Component")]
    public Text text;
    public Image dialogImage;

    [Header("Text File")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;

    [Header("Photo")]
    public Sprite face1;
    public Sprite face2;

    bool textFinished;
    bool cancelTyping;

    List<string> textList = new List<string>();
 
    void Awake()
    {
        GetText(textFile);
    }

    private void OnEnable() {
    
        textFinished=true;
        StartCoroutine(SetTextUI());
    }

    void Update()
    {
        if(InputManager.GetButtonDown("Interact")&&(index==textList.Count)){
            gameObject.SetActive(false);
            index=0;
            InputManager.ExitTalkingState();
            return;
        }
        if(InputManager.GetButtonDown("Interact")){
           if(textFinished && !cancelTyping)
           {
               StartCoroutine(SetTextUI());
           }
           else if(!textFinished){
               cancelTyping=!cancelTyping;
           }
        }
    }

    void GetText(TextAsset file){
        textList.Clear();
        index=0;
        var lineData = file.text.Split('\n');

        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI(){
        textFinished=false;
        text.text="";
        switch (textList[index])
        {
            case"A":
              dialogImage.sprite=face1;
              index++;
              break;
            case"B":
              dialogImage.sprite=face2;
              index++;
              break;
        }
        int letterPos=0;
        while(!cancelTyping && letterPos<textList[index].Length-1){
            text.text+=textList[index][letterPos];
            letterPos++;
             yield return new WaitForSeconds(textSpeed);
        }
        text.text=textList[index];
        cancelTyping=false;
        textFinished=true;
        index++;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Textbox : MonoBehaviour
{
    public static Textbox Instance;

    [Header("Text box")]
    public float defaultTextSpeed = 0.05f;

    [SerializeField] private GameObject textBox;
    [SerializeField] private Text mainText;

    private void Awake()
    {
        Instance = this;
        
        HideBox();
    }

    public void ShowBox()
    {
        textBox.SetActive(true);
    }

    public void HideBox()
    {
        textBox.SetActive(false);
    }


    public Coroutine SetText(string text)
    {
        return SetText(text, true, defaultTextSpeed);
    }
    public Coroutine SetText(string text, float textSpeed)
    {
        return SetText(text, true, textSpeed);
    }
    public Coroutine SetText(string text, bool promptToClose, float textSpeed = -1)
    {
        if (textSpeed == -1)
            textSpeed = defaultTextSpeed;

        return StartCoroutine(AddText(name, text, promptToClose, textSpeed));
    }

    private IEnumerator AddText(string name, string text, bool promptToClose, float textSpeed)
    {
        ShowBox();

        string currText = "";
        float nextCharTime = 0;
        //skipText.text = "Skip";

        while (currText != text)
        {
            if (Time.unscaledTime >= nextCharTime)
            {
                nextCharTime = Time.unscaledTime + textSpeed;
                currText += text.Substring(currText.Length, 1);
                mainText.text = currText;
            }

            //if (skipButton.Pressed())
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            {
                promptToClose = true;
                currText = text;
                mainText.text = currText;
                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }

        //skipText.text = "Next";

        if (promptToClose)
        {
            //yield return new WaitUntil(() => skipButton.Pressed());
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));// Input.anyKeyDown);
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return new WaitForSeconds(0.75f);
        }
    }
}

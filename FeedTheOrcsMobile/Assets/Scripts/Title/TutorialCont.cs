using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCont : MonoBehaviour
{
    public Image tutorialImage;
    public Sprite[] tutorialSprites;
    public Image[] dotImages;
    public Sprite[] dots;

    private int currentSprite; // 0 - Image 1, 1- Image 2, .. 3 - Image 4

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickRight()
    {
        if (currentSprite < tutorialSprites.Length)
        {
            currentSprite += 1;
            ClearDots();
            tutorialImage.sprite = tutorialSprites[currentSprite];
            dotImages[currentSprite].sprite = dots[1];
        }
    }

    public void ClickLeft()
    {
        if (currentSprite > 0)
        {
            currentSprite -= 1;
            ClearDots();
            tutorialImage.sprite = tutorialSprites[currentSprite];
            dotImages[currentSprite].sprite = dots[1];
        }
    }

    public void ClearDots()
    {
        for (int i = 0; i < dotImages.Length; i++)
        {
            dotImages[i].sprite = dots[0];
        }
    }
}

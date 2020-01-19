using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AT = Enums.BallsName.BallNamesEnum;

public class Ball : MonoBehaviour
{
    [SerializeField] private AT ballName;
    [SerializeField] private SpriteRenderer ballSprite;
    [SerializeField] private GameObject usedBallPanel;
    [SerializeField] private Material ballMaterial;
    [SerializeField] private float ballScoreOpen;
    [SerializeField] private bool isAdNeeded;

    public AT BallName
    {
        get
        {
            return ballName;
        }
    }

    public Sprite BallSprite
    {
        get
        {
            return ballSprite.sprite;
        }
    }

    public Material BallMaterial
    {
        get
        {
            return ballMaterial;
        }
    }

    public GameObject UsedBallPanel
    {
        get
        {
            return usedBallPanel;
        }
    }

    public float BallScoreOpen
    {
        get
        {
            return ballScoreOpen;
        }
    }

    public bool IsAdNeeded
    {
        get
        {
            return isAdNeeded;
        }
        set
        {
            isAdNeeded = value;
        }
    }
}

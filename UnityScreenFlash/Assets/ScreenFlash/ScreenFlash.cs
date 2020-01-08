using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class ScreenFlash : MonoBehaviour
{
    //TODO ease 함수를 적용할 수 있도록 하는것도 좋을듯..

    //
    // Public methods
    //

    public static void Flash(Color color, float duration)
    {
        instance.AddItem(Time.time, color, duration * 0.5f, 0f, duration * 0.5f);
    }

    public static void Flash(Color color, float inDuration, float outDuration)
    {
        instance.AddItem(Time.time, color, inDuration, 0f, outDuration);
    }

    public static void Flash(Color color, float inDuration, float holdDuration, float outDuration)
    {
        instance.AddItem(Time.time, color, inDuration, holdDuration, outDuration);
    }

    public static void Cancel()
    {
        instance.items.Clear();
        instance.gameObject.SetActive(false);
    }



    //
    // Implementation
    //

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }


    struct Item
    {
        public Color color;
        public float inDuration;
        public float holdDuration;
        public float outDuration;
        public float addedTime;
    }

    void AddItem(float timePoint, Color color, float inDuration, float holdDuration, float outDuration)
    {
        Assert.IsTrue(inDuration >= 0f);
        Assert.IsTrue(holdDuration >= 0f);
        Assert.IsTrue(outDuration >= 0f);
        Assert.IsTrue(inDuration > 0f || holdDuration > 0f || outDuration > 0f);

        // Master alpha scaling.
        color.a *= masterAlphaScale;

        Item item = new Item {
            color = color,
            inDuration = inDuration,
            holdDuration = holdDuration,
            outDuration = outDuration,
            addedTime = timePoint
        };

        items.Add(item);

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);

            StartCoroutine(CoFlashing());
        }
    }

    bool BlendItemColors(float timePoint, out Color result)
    {
        int blendedCount = 0;

        // Warning: initial color's alpha element must be 0.
        result = new Color(0, 0, 0, 0);

        for (int i = 0; i < items.Count; ++i)
        {
            if (GetItemColorAtTime(items[i], timePoint, out Color itemColor))
            {
                result = ColorBlender.Blend(itemColor, result);
                blendedCount++;
            }
            else
            {
                // Expired
                items.RemoveAt(i--);
            }
        }

        return blendedCount == 0; // Returns whether done or not.
    }

    //bool IsExpiredItem(Item item, float timePoint)
    //{
    //    float duration = item.inDuration + item.holdDuration + item.outDuration;
    //    float elapsedTime = timePoint - item.addedTime;
    //    return elapsedTime >= duration;
    //}

    bool GetItemColorAtTime(Item item, float timePoint, out Color result)
    {
        float duration = item.inDuration + item.holdDuration + item.outDuration;
        float elapsedTime = timePoint - item.addedTime;

        if (elapsedTime >= duration)
        {
            // Expired!
            result = Color.black;
            return false;
        }


        float alphaScale;

        if (item.inDuration > 0f && elapsedTime < item.inDuration)
        {
            alphaScale = elapsedTime / item.inDuration;
        }
        else if (item.holdDuration > 0f && elapsedTime < (item.inDuration + item.holdDuration))
        {
            alphaScale = 1f;
        }
        else if (item.outDuration > 0f)
        {
            alphaScale = 1f - ((elapsedTime - item.inDuration - item.holdDuration) / item.outDuration);
        }
        else
        {
            // Some duration paramters are wrong.
            result = Color.black;
            return false;
        }

        //alphaScale = EasingFunction.EaseInBounce(0f, 1f, alphaScale);

        result = new Color(item.color.a, item.color.g, item.color.b, item.color.a * alphaScale);

        return true;
    }

    IEnumerator CoFlashing()
    {
        while (true)
        {
            yield return null;

            bool done = BlendItemColors(Time.time, out Color color);
            if (done)
            {
                break;
            }

            flashCover.color = color;
        }

        gameObject.SetActive(false);
    }


    //
    // Member variables
    //

    static ScreenFlash instance;
    readonly List<Item> items = new List<Item>();

    public float masterAlphaScale = 1f;
    [SerializeField] Image flashCover;
}

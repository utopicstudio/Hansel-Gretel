using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRectController : MonoBehaviour
{
    public UnityEngine.UI.ScrollRect scrollRect;
    [Range(0f,1f)]
    public float scrollDelta = 0.1f;
    public float scrollCooldown = 0.1f;

    protected bool _movingUp = false;
    protected bool _movingDown = false;
    protected float _cooldown = 0f;

    private void Start()
    {
        _ResetCooldown();
    }


    private void Update()
    {
        if(_cooldown <= 0f)
        {
            if (_movingUp)
            {
                Up();
            }
            if (_movingDown)
            {
                Down();
            }
            _ResetCooldown();
        }
        _cooldown -= Time.deltaTime;
    }
    protected void _ResetCooldown()
    {
        _cooldown = scrollCooldown;
    }

    public void EnableUp()
    {
        _movingUp = true;
    }
    public void DisableUp()
    {
        _movingUp = false;
    }
    public void EnableDown()
    {
        _movingDown = true;
    }
    public void DisableDown()
    {
        _movingDown = false;
    }



    public void Up()
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition + scrollDelta);
        print("up: " + scrollRect.verticalNormalizedPosition);
    }

    public void Down()
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(scrollRect.verticalNormalizedPosition - scrollDelta);
        print("down: " + scrollRect.verticalNormalizedPosition);
    }
}

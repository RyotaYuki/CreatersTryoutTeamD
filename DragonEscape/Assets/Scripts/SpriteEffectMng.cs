using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteEffectMng : MonoBehaviour
{
    private GameObject _sprite;
    private float _maxScale;
    private float _minScale;
    private float _flamTime;

    void Start()
    {
        _sprite = null;
        _maxScale = 1;
        _minScale = 0;
        _flamTime = 0;
    }

    public void Expansion(GameObject obj, float maxScale, float flamTime)
    {
        _sprite = obj;
        _maxScale = maxScale;
        _flamTime = flamTime;
        StartCoroutine("ExpansionMove");

    }
    IEnumerator ExpansionMove()
    {
        for(int j = 0; j < _flamTime; j++)
        {
            var scale = _sprite.transform.localScale;
            if(_flamTime > 0)
            {
                scale.x += ((_maxScale - scale.x) / _flamTime);
                scale.y += ((_maxScale - scale.y) / _flamTime);
            }
            else
            {
                scale.x += (_maxScale - scale.x);
                scale.y += (_maxScale - scale.y);
            }
            _sprite.transform.localScale = scale;
        }
        yield return null;
    }

    public void Shrink(GameObject obj, float minScale, float flamTime)
    {
        _sprite = obj;
        _minScale = minScale;
        _flamTime = flamTime;
        StartCoroutine("ShrinkMove");

    }
    IEnumerator ShrinkMove()
    {
        for (int j = 0; j < _flamTime; j++)
        {
            var scale = _sprite.transform.localScale;
            if (_flamTime > 0)
            {
                scale.x -= ((scale.x - _minScale) / _flamTime);
                scale.y -= ((scale.y - _minScale) / _flamTime);
            }
            else
            {
                scale.x -= (scale.x - _minScale);
                scale.y -= (scale.x - _minScale);
            }
            if(scale.x >= 0 || scale.y >= 0)
            {
                _sprite.transform.localScale = scale;
            }
        }
        yield return null;
    }
}

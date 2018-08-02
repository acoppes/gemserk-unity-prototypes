using UnityEngine;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField]
    protected int _averageFrameCount;
    
    public int FPS { get; private set; }

    private float _averageAccumulatedTime;
    private int _frames;

    private void Update () {
        if (_averageFrameCount > 0)
        {
            _averageAccumulatedTime += Time.unscaledDeltaTime;

            _frames++;

            if (_frames > _averageFrameCount)
            {
                var averageTime = _averageAccumulatedTime / _frames;
                FPS = (int) (1f / averageTime);
                _averageAccumulatedTime = 0.0f;
                _frames = 0;
            }
        }            
        else
        {
            FPS = (int)(1f / Time.unscaledDeltaTime);        
        }
    }
}
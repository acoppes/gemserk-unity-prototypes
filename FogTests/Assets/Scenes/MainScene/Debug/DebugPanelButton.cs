using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelButton : MonoBehaviour
{
    [SerializeField]
    protected Button _button;

    [SerializeField]
    protected Text _text;

    private Action<DebugPanelButton> _callbackAction;

    private Action<DebugPanelButton> _updateAction;

    private void Start()
    {
        _button.onClick.AddListener(delegate
        {
            _callbackAction?.Invoke(this);
        });
    }
    
    public void UpdateText(string text)
    {
        _text.text = text;
    }
    
    public void SetCallbackAction(Action<DebugPanelButton> callbackAction)
    {
        _callbackAction = callbackAction;
    }

    public void SetUpdateAction(Action<DebugPanelButton> updateAction)
    {
        _updateAction = updateAction;
    }

    private void Update()
    {
        _updateAction?.Invoke(this);
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanelButton : MonoBehaviour
{
    public struct Actions
    {
        public Action<DebugPanelButton> callbackAction;
        public Action<DebugPanelButton> updateAction;
        public Action<DebugPanelButton> refreshAction;
    }
    
    [SerializeField]
    protected Button _button;

    [SerializeField]
    protected Text _text;

    private Action<DebugPanelButton> _callbackAction;

    private Action<DebugPanelButton> _updateAction;
    
    private Action<DebugPanelButton> _refreshAction;

    private void Start()
    {
        _button.onClick.AddListener(delegate
        {
            _callbackAction?.Invoke(this);
            _refreshAction?.Invoke(this);
        });
        
        _refreshAction?.Invoke(this);
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
    
    public void SetRefreshAction(Action<DebugPanelButton> refreshAction)
    {
        _refreshAction = refreshAction;
    }

    private void Update()
    {
        _updateAction?.Invoke(this);
    }
}
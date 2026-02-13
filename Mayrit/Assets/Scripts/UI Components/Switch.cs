using System;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class Switch : VisualElement
{
    public event Action<bool> Toggled;

    [UxmlAttribute]
    public string Text
    {
        get => _label.text;
        set => _label.text = value;
    }
    [UxmlAttribute]
    public bool Value
    {
        get => _value;
        set => Set(value);
    }

    bool _value;
    readonly Label _label;
    readonly VisualElement _border;
    readonly VisualElement _knob;

    public Switch()
    {
        _label = new("Label")
        {
            name = "Label"
        };

        _border = new()
        {
            name = "Border"
        };
        _border.AddToClassList("switch-border");

        _knob = new()
        {
            name = "Knob"
        };
        _knob.AddToClassList("switch-knob");

        Add(_border);
        _border.Add(_knob);
        Add(_label);

        _border.RegisterCallback<ClickEvent>(_ => Value = !Value);
    }

    void Set(bool value)
    {
        _value = value;
        Toggled?.Invoke(_value);
        SetState(value);
    }

    void SetState(bool value)
    {
        _border.EnableInClassList("switch-border_on", value);
        _knob.EnableInClassList("switch-knob_on", value);
    }
}

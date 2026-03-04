using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ContextualPanelUI : AUIState
{
    #region PROPERTIES
    public event Action PlayTourClickedEvent;
    public event Action ResetTourClickedEvent;
    public event Action ShownEvent;
    public event Action ClosedEvent;

    readonly VisualElement _root;

    Label _header,
        _subHeader,
        _description,
        _imageCaption;

    Button _closeButton,
        _startTourButton,
        _resetTourButton;

    VisualElement _image;
    //_icon;

    Tour CurrentTour => ServiceLocator.Instance.Get<Tour>();

    // Tracking flags
    bool _hadIcon;
    bool _hadImage;
    bool _hadPlayButton;
    #endregion

    #region CONSTRUCTOR
    public ContextualPanelUI(UIDocument uIDocument, VisualElement root) : base("ContextualPanel", uIDocument)
    {
        if (root == null)
        {
            Debug.LogError("ContextualPanelUI: Root VisualElement is null");
            return;
        }

        _root = root;
    }
    #endregion

    #region INHERITED METHODS
    protected override void ConfigureUIElementsOnAwake()
    {
        _header = GetByName<Label>("Header", _root);
        _subHeader = GetByName<Label>("SubHeader", _root);
        _description = GetByName<Label>("Description", _root);
        _imageCaption = GetByName<Label>("Caption", _root);
        _closeButton = GetButtonAndRegisterCallback("CloseContextualPanelButton", OnCloseButton, _root);
        _image = GetByName<VisualElement>("Image", _root);
        _startTourButton = GetButtonAndRegisterCallback("StartTourButton", OnStartTour, _root);
        _resetTourButton = GetButtonAndRegisterCallback("ResetTourButton", OnResetTour, _root);
    }
    #endregion

    #region PUBLIC METHODS
    public void ShowInfo(DataSO data)
    {
        // Clear previous content
        _header.text = data.Header;
        _subHeader.text = data.SubHeader;
        _description.text = data.Description;

        // TODO remove later
        // // Data has icon
        // if (data.Icon != null)
        // {
        //     // Show it
        //     _icon.style.backgroundImage = new StyleBackground(data.Icon.texture);
        //     if (!_hadIcon)
        //     {
        //         _icon.style.display = DisplayStyle.Flex;
        //         _hadIcon = true;
        //     }
        // }
        // else
        // {
        //     _icon.style.backgroundImage = new StyleBackground();
        //     _icon.style.display = DisplayStyle.None;
        //     _hadIcon = false;
        // }

        // Handle image
        if (data.Image != null)
        {
            _image.style.backgroundImage = new StyleBackground(data.Image.texture);
            _imageCaption.text = data.ImageCaption;
            if (!_hadImage)
            {
                _image.style.display = DisplayStyle.Flex;
                _imageCaption.style.display = DisplayStyle.Flex;
                _hadImage = true;
            }
        }
        else
        {
            _image.style.backgroundImage = new StyleBackground();
            _image.style.display = DisplayStyle.None;
            _imageCaption.style.display = DisplayStyle.None;
            _imageCaption.text = string.Empty;
            _hadImage = false;
        }

        // Handle tour butons
        if (data.IsPlayer)
        {
            if (!_hadPlayButton)
            {
                _startTourButton.style.display = DisplayStyle.Flex;
                _hadPlayButton = true;

                // Only show reset button if tour is already completed
                if (CurrentTour.IsCompleted)
                    _resetTourButton.style.display = DisplayStyle.Flex;
                else
                    _resetTourButton.style.display = DisplayStyle.None;
            }
        }
        else
        {
            _startTourButton.style.display = DisplayStyle.None;
            _resetTourButton.style.display = DisplayStyle.None;
            _hadPlayButton = false;
        }

        _root.style.display = DisplayStyle.Flex; // Show

        ShownEvent?.Invoke();
    }

    public void Hide()
    {
        _root.style.display = DisplayStyle.None; // Hide

        // TODO remove later
        // // Only clear if something was shown
        // if (_hadIcon)
        // {
        //     _icon.style.backgroundImage = new StyleBackground();
        //     _icon.style.display = DisplayStyle.None;
        //     _hadIcon = false;
        // }

        if (_hadImage)
        {
            _image.style.backgroundImage = new StyleBackground();
            _image.style.display = DisplayStyle.None;
            _imageCaption.style.display = DisplayStyle.None;
            _hadImage = false;
        }

        if (_hadPlayButton)
        {
            _startTourButton.style.display = DisplayStyle.None;
            _hadPlayButton = false;
        }

        _header.text = string.Empty;
        _subHeader.text = string.Empty;
        _description.text = string.Empty;
    }
    #endregion

    #region CALLBACK METHODS
    void OnCloseButton(ClickEvent evt)
    {
        Hide();
        ClosedEvent?.Invoke();

        if (_soundManager == null)
            _soundManager = ServiceLocator.Instance.Get<SoundManager>();

        _soundManager.PlayButtonClickSFX();
    }

    void OnStartTour(ClickEvent evt)
    {
        Hide();
        PlayTourClickedEvent?.Invoke();
    }

    void OnResetTour(ClickEvent evt)
    {
        Hide();
        ResetTourClickedEvent?.Invoke();
    }
    #endregion
}

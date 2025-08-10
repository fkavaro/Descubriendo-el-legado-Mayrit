using UnityEngine;
using UnityEngine.UI;

public class PlayerButton : MonoBehaviour
{

    public RectTransform _playerButton;

    [Header("Offset (pixels)")]
    public Vector2 _screenOffset = new(0, 30);

    PlayableCharacter _playableCharacter;

    void LateUpdate()
    {
        if (_playerButton == null)
            return;

        // Hide button if game pause or orbital camera state
        if (GameManager.Instance._fsm.IsCurrentState(GameManager.Instance._pauseState) ||
            CameraManager.Instance._fsm.IsCurrentState(CameraManager.Instance._orbitalState))
        {
            if (_playerButton.gameObject.activeSelf)
                _playerButton.gameObject.SetActive(false);
            return;
        }

        // Current playable character has changed
        if (_playableCharacter != GameManager.Instance._currentPlayableCharacter)
            _playableCharacter = GameManager.Instance._currentPlayableCharacter;

        // Get player position in world space and convert to screen space
        Vector3 worldPos = _playableCharacter.transform.position + Vector3.up;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // Check if player is in-screen
        bool playerInScreen = screenPos.z > 0 &&
                    screenPos.x >= 0 && screenPos.x <= Screen.width &&
                    screenPos.y >= 0 && screenPos.y <= Screen.height;

        // Show button if is in-screen and not already active
        if (playerInScreen && !_playerButton.gameObject.activeSelf)
            _playerButton.gameObject.SetActive(true);

        // And move button
        if (playerInScreen)
            _playerButton.position = screenPos + (Vector3)_screenOffset;
    }

    public void OnPlayerButtonClick()
    {
        if (_playerButton == null) return;

        // Spectator camera
        if (CameraManager.Instance._spectatorState.IsCurrentState())
        {
            CameraManager.Instance.SwitchToOrbitalCamera(_playableCharacter.transform, _playableCharacter._information);
        }
        // Orbital camera
        else if (CameraManager.Instance._orbitalState.IsCurrentState())
        {
            // Show the player information in contextual panel
            //UIManager.Instance._spectatorHUDState.ShowContextualPanel(_playableCharacter._characterInformation);
            // Button in contextual panel will change to 3rd person camera
        }
        // Third person camera
        else if (CameraManager.Instance._thirdPersonState.IsCurrentState())
        {
            CameraManager.Instance.SwitchToSpectatorCamera();
        }
    }
}
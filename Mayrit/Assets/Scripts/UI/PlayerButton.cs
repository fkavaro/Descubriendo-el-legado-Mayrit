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

        // Hide if not in spectator HUD state or if orbital camera is active
        if (!UIManager.Instance._spectatorHUDState.IsCurrentState() ||
            CameraManager.Instance._orbitalState.IsCurrentState())
        {
            _playerButton.gameObject.SetActive(false);
            return;
        }

        // Update current playable character if changed
        if (_playableCharacter != GameManager.Instance._currentPlayableCharacter)
            _playableCharacter = GameManager.Instance._currentPlayableCharacter;

        // Get player position in world space and convert to screen space
        Vector3 worldPos = _playableCharacter.transform.position + Vector3.up;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // Check if player is in-screen
        bool playerInScreen = screenPos.z > 0 &&
                    screenPos.x >= 0 && screenPos.x <= Screen.width &&
                    screenPos.y >= 0 && screenPos.y <= Screen.height;

        // Show button and update position if player is in-screen
        if (playerInScreen)
        {
            if (!_playerButton.gameObject.activeSelf)
                _playerButton.gameObject.SetActive(true);

            // Update button position
            _playerButton.position = screenPos + (Vector3)_screenOffset;
        }
        // Hide button if not
        else
        {
            if (_playerButton.gameObject.activeSelf)
                _playerButton.gameObject.SetActive(false);
        }
    }

    public void OnPlayerButtonClick()
    {
        if (_playerButton == null) return;

        // Spectator camera
        if (CameraManager.Instance._spectatorState.IsCurrentState())
            CameraManager.Instance.SwitchToOrbitalCamera(_playableCharacter.transform, _playableCharacter._information);
        // Third person camera
        else if (CameraManager.Instance._thirdPersonState.IsCurrentState())
            CameraManager.Instance.SwitchToSpectatorCamera();
    }
}
using UnityEngine;
using UnityEngine.UI;

public class PlayerFollower : MonoBehaviour
{
    public RectTransform _playerButton;

    [Header("Offset (pixels)")]
    public Vector2 screenOffset = new(0, 30);

    void LateUpdate()
    {
        if (_playerButton == null)
            return;

        Vector3 worldPos = FindFirstObjectByType<PlayerManager>().transform.position + Vector3.up;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // Check if player is in-screen
        bool playerInScreen = screenPos.z > 0 &&
                    screenPos.x >= 0 && screenPos.x <= Screen.width &&
                    screenPos.y >= 0 && screenPos.y <= Screen.height;

        // Set button active if is in-screen
        _playerButton.gameObject.SetActive(playerInScreen);

        // And move button
        if (playerInScreen)
            _playerButton.position = screenPos + (Vector3)screenOffset;
    }

    public void ChangeCamera()
    {
        if (_playerButton == null) return;

        //_image.enabled = false; // Hide button
        CameraManager.Instance.ToggleCameraState();
    }
}
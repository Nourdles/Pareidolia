using UnityEngine;

public class TodoListNotiManager : MonoBehaviour
{
    [SerializeField] private GameObject notification;

    private void DisableNotification()
    {
        notification.SetActive(false);
        Destroy(this);
    }
    
    private void EnableNotification()
    {
        notification.SetActive(true);
    }

    void OnEnable()
    {
        UpdateUI.TasksUpdatedEvent += EnableNotification;
        OpenCloseNote.NotepadFirstCheckEvent += DisableNotification;
    }

    void OnDisable()
    {
        UpdateUI.TasksUpdatedEvent -= EnableNotification;
        OpenCloseNote.NotepadFirstCheckEvent += DisableNotification;
    }
}

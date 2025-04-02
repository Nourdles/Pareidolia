using System;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class DoorInteraction : ObjectInteraction
{
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private MeshCollider doorCollider;
    [SerializeField] private MeshCollider doorKnobCollider;

    public event Action DoorFirstOpeningEvent;
    public event Action DoorUnlockEvent;
    [SerializeField] EventReference doorOpenSound;
    [SerializeField] EventReference doorCloseSound;
    [SerializeField] EventReference doorLockSound;

    public bool locked = true;
    private bool firstOpen = true;
    private bool doorOpen = false;
    [SerializeField] private string lockedDialogue;

    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
        interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to open door";
    }

    protected override void interactaction(GameObject objectInHand)
    {
        if (!locked)
        {
            if (firstOpen)
            {
                DoorAnimation();
                DoorFirstOpeningEvent?.Invoke();
                firstOpen = false;
            }
            else
            {
                DoorAnimation();
            }
        }
        else
        {
            InvokeDialoguePromptEvent(lockedDialogue);
            AudioManager.instance.PlayOneShot(doorLockSound, this.transform.position);
        }
    }

    private void DoorAnimation()
    {
        DisableColliders(); // disable colliders before animation starts

        if (doorOpen)
        {
            doorAnimator.Play("DoorClose");
            Debug.Log("Door Closing");
            AudioManager.instance.PlayOneShot(doorCloseSound, this.transform.position);
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to open door";
        }
        else
        {
            doorAnimator.Play("DoorOpen");
            Debug.Log("Door Opening");
            AudioManager.instance.PlayOneShot(doorOpenSound, this.transform.position);
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to close door";
        }

        doorOpen = !doorOpen;

        // wait for animation to finish before enabling colliders again
        float animDuration = GetAnimationClipDuration(doorAnimator, doorOpen ? "DoorOpen" : "DoorClose");
        if (!doorOpen)
        {
            Invoke(nameof(EnableColliders), animDuration);
        }
    }

    private void DisableColliders()
    {
        if (doorCollider != null) doorCollider.isTrigger = true;
        if (doorKnobCollider != null) doorKnobCollider.isTrigger = true;
    }

    private void EnableColliders()
    {
        if (doorCollider != null) doorCollider.isTrigger = false;
        if (doorKnobCollider != null) doorKnobCollider.isTrigger = false;
    }

    private float GetAnimationClipDuration(Animator animator, string clipName)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return 1f;

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName) return clip.length;
        }
        return 1f; // default
    }

    public void SetLockedDialogue(string newDialogue)
    {
        lockedDialogue = newDialogue;   
    }

    public void UnlockDoor()
    {
        locked = false;
        Debug.Log("Door has been unlocked");
        DoorUnlockEvent?.Invoke();
    }

    public void LockDoor()
    {
        locked = true;
        Debug.Log("Door has been locked");
    }

    protected override void UpdateInteractText()
    {
        if (doorOpen)
        {
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to close door";
        }
        else
        {
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to open door";
        }
    }
}

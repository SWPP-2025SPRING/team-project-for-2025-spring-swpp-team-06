using UnityEngine;

public static class GameObjectUtils
{
    public static void DestroySiblings(Transform callerTransform)
    {

        // created by chatGPT
        Transform parent = callerTransform.parent;
        if (parent == null || parent.gameObject.CompareTag("Ground")) return;

        foreach (Transform child in parent)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static void OnTriggerActions<T>(Transform transform, Collider other) where T: IPlayerState
    {
        DestroySiblings(transform);

        PlayerControl player = other.GetComponent<PlayerControl>();
        foreach (var state in player.States)
        {
            if (state is T existingGamingState)
            {
                existingGamingState.ResetTimer();
                return;
            }
        }

        player.PushState(new CoffeeState());
    }
}

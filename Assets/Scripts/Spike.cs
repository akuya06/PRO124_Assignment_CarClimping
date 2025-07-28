using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private Collider2D spikeCollider; // Gán collider trong Inspector hoặc tự động lấy

    private void Start()
    {
        if (spikeCollider == null)
            spikeCollider = GetComponent<Collider2D>();
        if (spikeCollider != null)
            spikeCollider.enabled = false;
    }

    // Gọi hàm này để bật collider (ví dụ: Animation Event hoặc script khác)
    public void ActivateSpike()
    {
        if (spikeCollider != null)
            spikeCollider.enabled = true;
    }

    // Gọi hàm này để tắt collider
    public void DeactivateSpike()
    {
        if (spikeCollider != null)
            spikeCollider.enabled = false;
    }
}

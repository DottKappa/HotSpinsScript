using UnityEngine;

public class FullScreenImage : MonoBehaviour
{
    public void CloseImageButton()
    {
        WaifuDetail waifuDetail = Object.FindFirstObjectByType<WaifuDetail>();
        if (waifuDetail != null) {
            // Chiama la funzione CloseImage dal componente WaifuDetail
            waifuDetail.CloseImage();
        } else {
            Debug.LogError("[FullScreenImage.cs] WaifuDetail non trovato nella scena!");
        }
    }
}

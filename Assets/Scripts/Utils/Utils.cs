using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        Debug.Log("*****" + Time.time + "*****");
        Debug.Log(position);
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);

        //return Vector3.zero;
    }
}

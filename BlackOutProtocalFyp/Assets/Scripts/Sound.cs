using UnityEngine;

public class Sound : MonoBehaviour
{
    public Sound(Vector3 _Position, float _Range)
    {
        SoundPosition = _Position;

        SoundRange = _Range; 
    }

    public readonly Vector3 SoundPosition;
    public readonly float SoundRange;  
}

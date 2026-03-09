using UnityEngine;

public static class Sounds 
{
    public static void MakeSound(Sound sound)
    {
        Collider[] col = Physics.OverlapSphere(sound.SoundPosition, sound.SoundRange);

        for (int i = 0; i < col.Length; i++) 
        {
            if (col[i].TryGetComponent(out HearSound hearer))
                hearer.RespondToSound(sound);
        }
    }
}

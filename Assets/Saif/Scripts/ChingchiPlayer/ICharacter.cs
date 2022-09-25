using UnityEngine;

public interface ICharacter
{
    //public bool IsPlayable();
    public string GetName();
    public void SetActivation(bool activationStatus);

    public bool IsPlayable();

}

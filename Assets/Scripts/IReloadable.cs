using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReloadable 
{
    public bool CanReload();
    public bool HasAmmo();
    public void Reload();

}

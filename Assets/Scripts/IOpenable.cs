using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOpenable
{
    public void OpenorClose();

    public void Unlock();

    public void Lock(CodeLock codeLock);

}

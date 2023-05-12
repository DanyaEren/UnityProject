using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLockKey : MonoBehaviour
{
    [SerializeField] private CodeLock _codeLock;

    public enum KeyType { Number, Enter, Clear};
    public KeyType keyType = KeyType.Number;

    [SerializeField] private int _keyValue;

    private void OnMouseDown()
    {
       switch (keyType)
        {
            case KeyType.Number:
                _codeLock.AddNumber(_keyValue.ToString());
                break;
            case KeyType.Enter:
                _codeLock.CheckCode();
                break;
            case KeyType.Clear:
                _codeLock.ClearNumbers();
                break;
            default:
                break;
        }
    }
}

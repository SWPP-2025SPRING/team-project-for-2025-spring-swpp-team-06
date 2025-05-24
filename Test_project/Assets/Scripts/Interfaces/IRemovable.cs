using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRemovable
{
    bool ShouldRemove { get; }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="What is gem Name",menuName = "ScriptableObjects/Grid Item Data")]
public class GridItemData : ScriptableObject
{
    public int _Width = 1;
    public int _Height = 1;

    public GemType _GemType;

    public Sprite _Icon;
}

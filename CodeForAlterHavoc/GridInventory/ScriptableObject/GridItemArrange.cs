using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "What is gem Name", menuName = "ScriptableObjects/Grid Item Arrange Effect")]
public class GridItemArrange : ScriptableObject
{
    public int _Width = 1;
    public int _Height = 1;

    public InventoryArrangeEffect _Effect;
    public GemType[] _GemType;

}

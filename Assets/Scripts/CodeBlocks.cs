using UnityEngine;

public enum CodeType
{
    Go,
    Turn_right,
    Turn_left,
    Place,
    Loop,
    Function
}
public class CodeBlocks : MonoBehaviour
{
    public CodeType type;
}

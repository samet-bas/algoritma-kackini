using UnityEngine;

public enum CodeType
{
    Go,
    Turn_right,
    Turn_left,
    Place,
    Loop,
    Function,
    If,
    pathCheck,
    Fly,
    Land
}
public class CodeBlocks : MonoBehaviour
{
    public CodeType type;
}

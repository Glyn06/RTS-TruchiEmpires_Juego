public struct NodeAdy
{
    public Node node;
    public NodeAdyType type;
}

public struct NodeValue
{
    public int pathValue;
    public int value;
    public bool isDanger;
    public bool isRisky;

    public NodeValue(bool isInMud)
    {
        if (!isInMud)
            value = (int)NodeValueMultipliers.Normal;
        else
            value = (int)NodeValueMultipliers.Mud;

        pathValue = value;
        isDanger = false;
        isRisky = false;
    }

    public void ResetPathValue()
    {
        pathValue = value;
    }

    public bool IsDanger
    {
        get { return isDanger; }
        set
        {
            if (isDanger != value)
            {
                isDanger = value;

                if (isDanger)
                    this.value += (int)NodeValueMultipliers.Danger;
                else
                    this.value -= (int)NodeValueMultipliers.Danger;
            }
        }
    }

    public bool IsRisky
    {
        get { return isRisky; }
        set
        {
            if (isRisky != value)
            {
                isRisky = value;

                if (isRisky)
                    this.value += (int)NodeValueMultipliers.Risky;
                else
                    this.value -= (int)NodeValueMultipliers.Risky;
            }
        }
    }
}
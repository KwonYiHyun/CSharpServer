using System;
using System.Collections.Generic;
using System.Text;

public class GameObject
{
    public GameObjectType ObjectType { get; protected set; }

    public int Id
    {
        get { return Info.objectId; }
        set { Info.objectId = value; }
    }

    public GameRoom Room { get; set; }

    public ObjectInfo Info { get; set; } = new ObjectInfo();

    public PositionInfo PosInfo { get; private set; } = new PositionInfo();

    public GameObject()
    {
        Info.positionInfo = PosInfo;
    }
}
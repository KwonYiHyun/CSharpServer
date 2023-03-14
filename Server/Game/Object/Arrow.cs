using System;
using System.Collections.Generic;
using System.Text;

public class Arrow : Projectile
{
    public GameObject Owner { get; set; }

    long _nextMoveTick = 0;

    public override void Update()
    {
        if (Owner == null || Room == null)
            return;

        if (_nextMoveTick >= Environment.TickCount64)
            return;

        _nextMoveTick = Environment.TickCount + 50;

        /*
         * TODO 앞의 좌표를 확인
         * 갈수있으면 S_Move패킷으로 브로드캐스팅
         * 앞에 객체가있으면 피격판정 후 소멸
         */
    }
}
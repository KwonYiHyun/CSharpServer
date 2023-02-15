﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;

public class PacketHandler
{
    public void S_LoginAction(Session session, IPacket packet)
    {
        S_Login pkt = packet as S_Login;

        Console.WriteLine("클라이언트 액션!");
    }

    public async void C_LoginAction(Session session, IPacket packet)
    {
        C_Login pkt = packet as C_Login;

        Console.WriteLine("C_LoginAction");

        await Task.Delay(100);
    }

    public async void S_UserEnterAction(Session session, IPacket packet)
    {
        Console.WriteLine("S_UserEnterAction");

        await Task.Delay(100);
    }

    public async void C_UserEnterAction(Session session, IPacket packet)
    {
        Console.WriteLine("C_UserEnterAction");

        await Task.Delay(100);
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;

public class PacketHandler
{
    int count = 0;

    public void S_LoginAction(ClientSession session, IPacket packet)
    {
        S_Login pkt = packet as S_Login;
        ClientSession clientSession = session as ClientSession;

        //Console.WriteLine($"S_LoginAction : {pkt.msg}");
        //foreach (var item in pkt.arr)
        //{
        //    Console.WriteLine($"item : {item}");
        //}
        //Console.WriteLine($"isMine : {pkt.isMine}");
        //Console.WriteLine($"pos : {pkt.pos}");
        //Console.WriteLine($"id : {pkt.id}");
        //Console.WriteLine($"type : {pkt.type}");

        session.Room.Push(() => { session.Room.Broadcast(session); });
        Console.WriteLine("count = " + count);
    }

    public async void C_LoginAction(ClientSession session, IPacket packet)
    {
        C_Login pkt = packet as C_Login;

        Console.WriteLine("C_LoginAction");

        await Task.Delay(100);
    }

    public async void S_UserEnterAction(ClientSession session, IPacket packet)
    {
        Console.WriteLine("S_UserEnterAction");

        await Task.Delay(100);
    }

    public async void C_UserEnterAction(ClientSession session, IPacket packet)
    {
        Console.WriteLine("C_UserEnterAction");

        await Task.Delay(100);
    }
}
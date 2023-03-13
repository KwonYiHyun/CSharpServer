using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;

public class PacketHandler
{
    public void C_LoginAction(ClientSession session, IPacket packet)
    {
        C_Login pkt = packet as C_Login;
        ClientSession clientSession = session as ClientSession;

        Console.WriteLine("msg = " + pkt.msg + " / info : " + pkt.positionInfo.posX + " / info : " + pkt.positionInfo.posY);

        S_Login pp = new S_Login();
        pp.msg = "c -> s";

        // await clientSession.SendAsync(pp.Serialize());

        Program.roomManager.Push(async () => await clientSession.SendAsync(pp.Serialize()));
    }

    public async void C_UserEnterAction(ClientSession session, IPacket packet)
    {
        Console.WriteLine("C_UserEnterAction");

        await Task.Delay(100);
    }

    public async void C_PlayerMoveAction(ClientSession session, IPacket packet)
    {
        Console.WriteLine("C_PlayerMoveAction");

        await Task.Delay(100);
    }
}
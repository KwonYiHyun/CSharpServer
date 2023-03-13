START bin/Debug/netcoreapp3.1/PacketTool.exe

XCOPY /Y ServerPacketManager.cs "../Server/Packet"
XCOPY /Y Packets.cs "../Server/Packet"
XCOPY /Y PacketType.cs "../Server/Packet"

XCOPY /Y ClientPacketManager.cs "../DummyClient"
XCOPY /Y Packets.cs "../DummyClient"
XCOPY /Y PacketType.cs "../DummyClient"

cmd/k
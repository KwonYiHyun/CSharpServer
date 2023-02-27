START bin/Debug/netcoreapp3.1/PacketTool.exe

XCOPY /Y ServerPacketManager.cs "../Server"
XCOPY /Y packets.cs "../Server"
XCOPY /Y PacketType.cs "../Server"

XCOPY /Y ClientPacketManager.cs "../DummyClient"
XCOPY /Y packets.cs "../DummyClient"
XCOPY /Y PacketType.cs "../DummyClient"

cmd/k
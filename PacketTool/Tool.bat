START bin/Debug/netcoreapp3.1/PacketTool.exe

XCOPY /Y PacketManager.cs "../Server"
XCOPY /Y packets.cs "../Server"
XCOPY /Y PacketManager.cs "../DummyClient"
XCOPY /Y packets.cs "../DummyClient"

cmd/k
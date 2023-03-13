using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PacketTool
{
    class Program
    {
        static string packets =
@"
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Core;

public class Packets
{{

}}

{0}
";
        static string pakcetclass =
@"
public class {1} : IPacket
{{
    public short Protocol {{ get; set; }} = (short)PacketType.{1};
    {0}
    public int offset = 0;

    public byte[] Serialize()
    {{
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(Protocol));
        
        {2}
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;
        
        {3}
        return buffer;
    }}

    public void DeSerialize(byte[] buffer)
    {{
        offset = 2;

        {4}
    }}
}}
";
        static string bytesLen = "short dataSize = (short)(packetType.Length";
        static string arrayCopy = "";
        static string read = "";
        static string packetclasses = "";

        static string serverPacketManager = @"using System;
using System.Collections.Generic;
using System.Text;
using Core;

public static class PacketManager
{{
    public static Dictionary<PacketType, Action<ClientSession, IPacket>> action = new Dictionary<PacketType, Action<ClientSession, IPacket>>();
    public static Dictionary<PacketType, Func<ClientSession, byte[], IPacket>> packetTypes = new Dictionary<PacketType, Func<ClientSession, byte[], IPacket>>();
    public static PacketHandler packetHandler = new PacketHandler();

    static PacketManager()
    {{
        {0}
    }}

    static T MakePacket<T>(Session session, byte[] buffer) where T : IPacket, new()
    {{
        T pkt = new T();
        pkt.DeSerialize(buffer);
        return pkt;
    }}
}}";
        static string clientPacketManager = @"using System;
using System.Collections.Generic;
using System.Text;
using Core;

public class PacketManager
{{
    public static Dictionary<PacketType, Action<ServerSession, IPacket>> action = new Dictionary<PacketType, Action<ServerSession, IPacket>>();
    public static Dictionary<PacketType, Func<ServerSession, byte[], IPacket>> packetTypes = new Dictionary<PacketType, Func<ServerSession, byte[], IPacket>>();
    public static PacketHandler packetHandler = new PacketHandler();

    static PacketManager _instance = new PacketManager();
    public static PacketManager Instance {{ get {{ return _instance; }} }}

    static PacketManager()
    {{
        {0}
    }}

    static T MakePacket<T>(Session session, byte[] buffer) where T : IPacket, new()
    {{
        T pkt = new T();
        pkt.DeSerialize(buffer);
        return pkt;
    }}

    public void HandlePacket(ServerSession session, IPacket type)
    {{
        Action<ServerSession, IPacket> _action = null;
        if (action.TryGetValue((PacketType)type.Protocol, out _action))
        {{
            _action.Invoke(session, type);
        }}
    }}
}}";

        static string packetManagerClientAction = "";
        static string packetManagerServerAction = "";

        static string packetType = @"
using System;
using System.Collections.Generic;
using System.Text;


public enum PacketType
{{
    INDEX,
    {0}
}}
";

        static void Main(string[] args)
        {
            string fileName = Directory.GetCurrentDirectory();
            fileName = fileName + "\\packet.json";
            string jsonString = File.ReadAllText(fileName);
            Console.WriteLine(jsonString);

            string[] json = jsonString.Split("\n");
            bool isPkt = false;


            string packetName = "";
            string packetVariable = "";
            string packetByte = "";

            string types = "";


            foreach (var item in json)
            {
                if (item.Contains("S_") || item.Contains("C_") || item.Contains("Class_"))
                {
                    if (isPkt)
                    {
                        packetclasses += string.Format(pakcetclass, packetVariable, packetName, packetByte + "\n\t" + bytesLen + ");", arrayCopy, read) + Environment.NewLine;
                    }
                    packetVariable = "";
                    packetByte = "";
                    bytesLen = "short dataSize = (short)(packetType.Length";
                    arrayCopy = "";
                    read = "";
                    packetName = item.Split("\": [")[0].Split("\"")[1];
                    if (packetName.Contains("Class_"))
                        packetName = packetName.Split("Class_")[1];

                    if (!item.Contains("S_PacketEnd"))
                    {
                        if (item.Contains("S_"))
                        {
                            packetManagerClientAction += "action.Add(PacketType." + packetName + ", packetHandler." + packetName + @"Action);
        packetTypes.Add(PacketType." + packetName + ", MakePacket<" + packetName + ">);" + Environment.NewLine + Environment.NewLine + "\t\t";
                        }
                        else if (item.Contains("C_"))
                        {
                            packetManagerServerAction += "action.Add(PacketType." + packetName + ", packetHandler." + packetName + @"Action);
        packetTypes.Add(PacketType." + packetName + ", MakePacket<" + packetName + ">);" + Environment.NewLine + Environment.NewLine + "\t\t";
                        }

                        types += packetName + "," + Environment.NewLine + "\t";
                    }
                }


                string va = getVariable(item);
                if (!va.Equals(""))
                {
                    packetVariable += va + "\n    ";
                }

                string vab = getBytes(item);
                if (!vab.Equals(""))
                {
                    packetByte += vab + Environment.NewLine + "\t";
                }

                if (item.Contains("S_") || item.Contains("C_"))
                {
                    isPkt = true;
                }
            }


            string filePath = Directory.GetCurrentDirectory();
            File.WriteAllText(filePath + "\\Packets.cs", string.Format(packets, packetclasses));
            File.WriteAllText(filePath + "\\ServerPacketManager.cs", string.Format(serverPacketManager, packetManagerServerAction));
            File.WriteAllText(filePath + "\\ClientPacketManager.cs", string.Format(clientPacketManager, packetManagerClientAction));
            File.WriteAllText(filePath + "\\PacketType.cs", string.Format(packetType, types));
        }

        static string getBytes(string str)
        {
            if (!str.Contains("\": "))
            {
                return "";
            }

            if(str.Contains("S_") || str.Contains("C_") || str.Contains("Class_"))
            {
                return "";
            }

            string vari = str.Split("\": \"")[0].Split("\"")[1];
            string varName = str.Split("\":")[1].Split("\"")[1];
            string result = @"";

            switch (vari)
            {
                case "char":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToChar(buffer, offset" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
                    break;
                case "string":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = Encoding.UTF8.GetString(buffer, offset, " + varName + "Size" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
                    break;
                case "bool":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToBoolean(buffer, offset" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
                    break;
                case "short":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToInt16(buffer, offset" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
                    break;
                case "int":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToInt32(buffer, offset" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
                    break;
                case "long":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToInt64(buffer, offset" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
                    break;
                case "float":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToSingle(buffer, offset" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
                    break;
                case "double":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToDouble(buffer, offset" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
                    break;
                case "list":
                    varName = str.Split("\", \"")[1].Split("\"")[0];
                    read += "short " + varName + @"CountSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);

        for (int i = 0; i < " + varName + @"CountSize; i++)
        {
            short itemSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
            offset += sizeof(short);

            short item = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
            offset += itemSize;

            arr.Add(item);
        }

        ";
                    break;
                case "Class":
                    string className = varName.Substring(0, 1).ToLower() + varName.Substring(1, varName.Length - 1);
                    read += "short " + className + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " _" + className + " = new " + varName + @"();
        _" + className + ".DeSerialize(new ArraySegment<byte>(buffer, offset, " + className + @"Size).ToArray());
        " + className + " = " + "_" + className + @";
        offset += " + className + @"Size;
        " + Environment.NewLine + "\t";
                    break;
                default:
                    break;
            }

            switch (vari)
            {
                case "string":
                    #region 추가
                    result = "byte[] " + varName + "Pkt = Encoding.UTF8.GetBytes(" + varName + @");
        byte[] " + varName + "Size = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)" + varName + "Pkt.Length));";
                    bytesLen += " + " + varName + "Pkt.Length + " + varName + "Size.Length";
                    arrayCopy += @"Array.Copy(" + varName + @"Size, 0, buffer, offset, " + varName + @"Size.Length);
        offset += " + varName + @"Size.Length;
        Array.Copy(" + varName + "Pkt, 0, buffer, offset, " + varName + @"Pkt.Length);
        offset += " + varName + "Pkt.Length;" + Environment.NewLine + Environment.NewLine + "\t";
                    #endregion
                    break;
                case "char":
                case "bool":
                case "short":
                case "int":
                case "long":
                case "float":
                case "double":
                    #region 추가
                    result = "byte[] " + varName + "Pkt = BitConverter.GetBytes(" + varName + @");
        byte[] " + varName + "Size = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)" + varName + "Pkt.Length));";
                    bytesLen += " + " + varName + "Pkt.Length + " + varName + "Size.Length";
                    arrayCopy += @"Array.Copy(" + varName + @"Size, 0, buffer, offset, " + varName + @"Size.Length);
        offset += " + varName + @"Size.Length;
        Array.Copy(" + varName + "Pkt, 0, buffer, offset, " + varName + @"Pkt.Length);
        offset += " + varName + "Pkt.Length;" + Environment.NewLine + Environment.NewLine + "\t";
                    #endregion
                    break;
                case "list":
                    #region 추가
                    result = "byte [] " + varName + "CountSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)" + varName + ".Count));";
                    bytesLen += " + " + varName + "CountSize.Length + sizeof(" + str.Split(": [ \"")[1].Split("\"")[0] + ") * " + varName + ".Count";
                    arrayCopy += "Array.Copy(" + varName + @"CountSize, 0, buffer, offset, " + varName + @"CountSize.Length);
        offset += " + varName + @"CountSize.Length;

        foreach (var item in " + varName + @")
        {
            byte[] " + varName + @"Pkt = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)item));
            byte[] " + varName + @"Size = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)" + varName + @"Pkt.Length));

            Array.Copy(" + varName + @"Size, 0, buffer, offset, " + varName + @"Size.Length);
            offset += " + varName + @"Size.Length;

            Array.Copy(" + varName + @"Pkt, 0, buffer, offset, " + varName + @"Pkt.Length);
            offset += " + varName + @"Pkt.Length;
        }" + Environment.NewLine + Environment.NewLine + "\t";
                    #endregion
                    break;
                case "Class":
                    string className = varName.Substring(0, 1).ToLower() + varName.Substring(1, varName.Length - 1);
                    result = "byte[] " + varName + "Pkt = " + className + @".Serialize();
        byte[] " + varName + "Size = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)" + varName + "Pkt.Length));";
                    bytesLen += " + " + varName + "Pkt.Length + " + varName + "Size.Length";
                    // arrayCopy += "\tArray.Copy(" + className + ".Serialize(), 0, buffer, offset, " + className + @".offset);
                    // offset += " + className + ".offset;";
                    arrayCopy += @"Array.Copy(" + varName + @"Size, 0, buffer, offset, " + varName + @"Size.Length);
        offset += " + varName + @"Size.Length;
        Array.Copy(" + varName + "Pkt, 0, buffer, offset, " + varName + @"Pkt.Length);
        offset += " + varName + "Pkt.Length;" + Environment.NewLine + Environment.NewLine + "\t";
                    break;
                default:
                    break;
            }

            return result;
        }

        static string getVariable(string str)
        {
            if (!str.Contains("\": "))
            {
                return "";
            }

            string vari = str.Split("\": \"")[0].Split("\"")[1];
            string result = "";

            switch (vari)
            {
                case "list":
                    result = "public List<" + str.Split(": [ \"")[1].Split("\"")[0] + "> " + str.Split("\", \"")[1].Split("\"")[0] + " = new List<" + str.Split(": [ \"")[1].Split("\"")[0] + ">();";
                    break;
                case "char":
                case "string":
                case "bool":
                case "short":
                case "int":
                case "long":
                case "float":
                case "double":
                    result = "public " + vari + " " + str.Split("\": \"")[1].Split("\"")[0] + ";";
                    break;
                case "Class":
                    string name = str.Split("\": \"")[1].Split("\"")[0];
                    result = "public " + name + " " + name.Substring(0, 1).ToLower() + name.Substring(1, name.Length - 1) + " { get; set; } = new " + name + "();";
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}

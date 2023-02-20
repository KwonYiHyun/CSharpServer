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
    {0}

    public byte[] Serialize()
    {{
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PacketType.{1}));
        
        {2}
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        int offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;
        
        {3}
        return buffer;
    }}

    public void DeSerialize(byte[] buffer)
    {{
        int offset = 2;

        {4}
    }}
}}
";
        static string bytesLen = "short dataSize = (short)(packetType.Length";
        static string arrayCopy = "";
        static string read = "";
        static string packetclasses = "";

        static string packetManager = @"using System;
using System.Collections.Generic;
using System.Text;
using Core;

public static class PacketManager
{{
    public static Dictionary<PacketType, Action<Session, IPacket>> action = new Dictionary<PacketType, Action<Session, IPacket>>();
    public static Dictionary<PacketType, Func<Session, byte[], IPacket>> packetTypes = new Dictionary<PacketType, Func<Session, byte[], IPacket>>();
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

        static string packetManagerAction = "";

        static void Main(string[] args)
        {
            string fileName = Directory.GetCurrentDirectory();
            fileName = Path.GetDirectoryName(fileName);
            fileName = Path.GetDirectoryName(fileName);
            fileName = Path.GetDirectoryName(fileName) + "\\packet.json";
            string jsonString = File.ReadAllText(fileName);
            Console.WriteLine(jsonString);

            string[] json = jsonString.Split("\n");
            bool isPkt = false;


            string packetName = "";
            string packetVariable = "";
            string packetByte = "";


            foreach (var item in json)
            {
                if (item.Contains("S_") || item.Contains("C_"))
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

                    if (!item.Contains("S_PacketEnd"))
                    {
                        packetManagerAction += "action.Add(PacketType." + packetName + ", packetHandler." + packetName + @"Action);
        packetTypes.Add(PacketType." + packetName + ", MakePacket<" + packetName + ">);" + Environment.NewLine + Environment.NewLine + "\t\t";
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
            filePath = Path.GetDirectoryName(filePath);
            filePath = Path.GetDirectoryName(filePath);
            filePath = Path.GetDirectoryName(filePath);
            File.WriteAllText(filePath + "\\packets.cs", string.Format(packets, packetclasses));
            File.WriteAllText(filePath + "\\PacketManager.cs", string.Format(packetManager, packetManagerAction));
        }

        static string getBytes(string str)
        {
            if (!str.Contains("\": "))
            {
                return "";
            }

            if(str.Contains("S_") || str.Contains("C_"))
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
                    //string[] aa = str.Split(": [\"");
                    //string aaa = str.Split(": [\"")[1].Split("\"")[0];
                    //string a = str.Split(": [\"")[1].Split("\"")[0];
                    //string b = str.Split("\", \"")[1].Split("\"")[0];
                    //string c = str.Split(": [\"")[1].Split("\"")[0];
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
                default:
                    break;
            }

            return result;
        }
    }
}

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

            File.WriteAllText("packets.cs", string.Format(packets, packetclasses));
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
        
        " + varName + " = BitConverter.ToChar(buffer, " + varName + "Size" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
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
                case "string":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = Encoding.UTF8.GetString(buffer, offset, " + varName + "Size" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
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
                case "bool":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToBoolean(buffer, " + varName + "Size" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
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
                case "short":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToInt16(buffer, " + varName + "Size" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
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
                case "int":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToInt32(buffer, " + varName + "Size" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
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
                case "long":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToInt64(buffer, " + varName + "Size" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
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
                case "float":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToSingle(buffer, " + varName + "Size" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
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
                case "double":
                    read += "short " + varName + @"Size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);
        
        " + varName + " = BitConverter.ToDouble(buffer, " + varName + "Size" + @");
        offset += " + varName + @"Size;
        " + Environment.NewLine + "\t";
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
                    varName = str.Split("\",\"")[1].Split("\"")[0];
                    read += "short " + varName + @"CountSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);

        for (int i = 0; i < " + varName + @"CountSize; i++)
        {
            short itemSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
            offset += sizeof(short);

            short item = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
            offset += itemSize;

            arr.Add(item);
        }";
                    #region 추가
                    result = "byte [] " + varName + "CountSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)" + varName + ".Count));";
                    bytesLen += " + " + varName + "CountSize.Length + sizeof(" + str.Split(": [\"")[1].Split("\"")[0] + ") * " + varName + ".Count";
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
                    result = "public List<" + str.Split(": [\"")[1].Split("\"")[0] + "> " + str.Split("\",\"")[1].Split("\"")[0] + " = new List<" + str.Split(": [\"")[1].Split("\"")[0] + ">();";
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

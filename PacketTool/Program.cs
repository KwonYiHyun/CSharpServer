using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

public class Packets
{{

}}

{0}
";

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

            foreach (var item in json)
            {
                if (item.Contains("S_") || item.Contains("C_"))
                {
                    isPkt = true;
                    // 클래스 생성
                }else if (item.Contains("]"))
                {
                    isPkt = false;
                }

                if (isPkt)
                {

                }
            }

            List<int> arr = new List<int>();
            arr.Add(1);
            arr.Add(2);
            arr.Add(3);

            Console.WriteLine(arr.Count * sizeof(int));
        }
    }
}

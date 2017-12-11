using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeShadowSocks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("启动中...");

            //关闭ss进程
            Process[] p = Process.GetProcessesByName("Shadowsocks");
            if (p.Length > 0)
            {
                p[0].Kill();
            }

            //抓取免费服务器并生成ss配置文件
            string result = string.Empty;
            string url = "https://1hv.top/"; 
            HtmlWeb htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.Default;
            HtmlDocument htmlDoc = htmlWeb.Load(url);
            HtmlNode nodes = htmlDoc.GetElementbyId("port");

            string server = "139.162.87.254";
            string port = htmlDoc.GetElementbyId("port").InnerText.Trim();
            string pw = htmlDoc.GetElementbyId("pw").InnerText.Trim();
            string method = "aes-256-cfb";
            string config = "{'configs' : [  {'server' : '{server}','server_port' : {port},'password' : '{pw}','method' : '{method}','remarks' : ''}],'strategy' : null,'index' : 0,'global' : false,'enabled' : true,'shareOverLan' : false,'isDefault' : false,'localPort' : 1080,'pacUrl' : null,'useOnlinePac' : false}";
            config = config.Replace("'", "\"").Replace("{server}", server).Replace("{port}", port).Replace("{pw}", pw).Replace("{method}", method);

            Write(config, "gui-config.json");

            Process.Start("Shadowsocks.exe");
        }

        static void Write(string message, string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.Default);

            streamWriter.WriteLine(message);

            streamWriter.Close();
            fileStream.Close();
        }
    }
}

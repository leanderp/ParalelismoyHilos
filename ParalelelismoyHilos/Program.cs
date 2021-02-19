using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParalelismoyHilos
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "result.txt";
            var fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
            Parallel.For(1, 100, new ParallelOptions { MaxDegreeOfParallelism = 10 },
                (i) =>
                {
                    Write(fileName, i, fs);
                });
        }

        static void Write(string path, int id, FileStream fs)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync("https://jsonplaceholder.typicode.com/todos/" + id).Result;
            var content = response.Content.ReadAsStringAsync().Result + Environment.NewLine;

            lock (fs)
            {
                byte[] bContent = new UTF8Encoding(true).GetBytes(content + Environment.NewLine);
                fs.Write(bContent, 0, bContent.Length);
                fs.Flush();
            }
        }
    }
}

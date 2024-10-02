using System;
using System.Net.Sockets;
using Newtonsoft.Json;

TcpClient clientSocket = new TcpClient("127.0.0.1", 7);
Console.WriteLine("client is ready");

Stream ns = clientSocket.GetStream();
StreamReader sr = new StreamReader(ns);
StreamWriter sw = new StreamWriter(ns) { AutoFlush = true };

while(true)
{
    // User inputs the operation and numbers
    Console.WriteLine("Enter calculation method (random, add, subtract):");
    string method = Console.ReadLine();
    Console.WriteLine("Enter first number:");
    string num1 = Console.ReadLine();
    Console.WriteLine("Enter second number:");
    string num2 = Console.ReadLine();

    var request = new
    {
        Operation = method.ToLower(),
        Number1 = num1,
        Number2 = num2
    };

    sw.WriteLine(JsonConvert.SerializeObject(request));

    string serverAnswer = sr.ReadLine();
    var response = JsonConvert.DeserializeObject<dynamic>(serverAnswer);

    if(response.Error != null)
    {
        Console.WriteLine($"{response.Error}, {response.Detail}");
    }
    else
    {
        Console.WriteLine($"Your answer is: {response.Result}");
    }
}
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Calculator");

IPAddress ip = IPAddress.Parse("127.0.0.1");
TcpListener listener = new TcpListener(ip, 7);
listener.Start();

while (true)
{
    TcpClient socket = listener.AcceptTcpClient();
    Task.Run(() => HandleClient(socket));
}

void HandleClient(TcpClient socket)
{
    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns) { AutoFlush = true };

    while (true)
    {
        string input = reader.ReadLine();
        var request = JsonConvert.DeserializeObject<CalculationRequest>(input);
        int result = 0;

        if (int.TryParse(request.Number1, out int num1) && int.TryParse(request.Number2, out int num2))
        {
            if (request.Operation == "random")
            {
                Random rng = new Random();
                result = rng.Next(num1, num2);

                var response = new { Result = result };
                writer.WriteLine(JsonConvert.SerializeObject(response));

            }
            else if (request.Operation == "add")
            {
                result = num1 + num2;

                var response = new { Result = result };
                writer.WriteLine(JsonConvert.SerializeObject(response));
            }
            else if (request.Operation == "subtract")
            {
                result = num1 - num2;

                var response = new { Result = result };
                writer.WriteLine(JsonConvert.SerializeObject(response));
            }
            else
            {
                var errorResponse = new { Error = "Invalid input", Detail = "Incorrect operation." };
                writer.WriteLine(JsonConvert.SerializeObject(errorResponse));
            }
        }
        else
        {
            var errorResponse = new
            {
                Error = "Invalid input",
                Detail = "One or both numbers are not valid integers."
            };
            writer.WriteLine(JsonConvert.SerializeObject(errorResponse));
        }
    }  
}

public class CalculationRequest
{
    public string Operation { get; set; }
    public string Number1 { get; set; }
    public string Number2 { get; set; }
}





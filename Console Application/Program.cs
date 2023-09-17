using System;
using System.IO;
using System.IO.Pipes;
using System.Text;

class Program
{
    static void Main()
    {
        using (NamedPipeServerStream pipeServer = new NamedPipeServerStream("MyPipe", PipeDirection.InOut))
        {
            Console.WriteLine("Сервер ожидает подключения...");
            pipeServer.WaitForConnection();

            Console.WriteLine("Клиент подключен.");

            // Чтение данных от клиента
            byte[] buffer = new byte[256];
            int bytesRead = pipeServer.Read(buffer, 0, buffer.Length);

            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Получено от клиента: " + message);

            // Отправка ответа клиенту
            string response = "Сообщение получено на сервере.";
            byte[] responseBuffer = Encoding.UTF8.GetBytes(response);
            pipeServer.Write(responseBuffer, 0, responseBuffer.Length);

            Console.WriteLine("Ответ отправлен клиенту.");

            pipeServer.WaitForPipeDrain();
        }

        Console.WriteLine("Сервер завершил работу.");
    }
}

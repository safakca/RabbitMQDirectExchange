using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();
factory.Uri = new Uri("amqps://geilrjnf:CFHBVh0ApbH-4IfPGkSEB7-hUFVg1wQK@sparrow.rmq.cloudamqp.com/geilrjnf");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

//1. adim
channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

//2.adim
string queueName = channel.QueueDeclare().QueueName;

//3.adim
channel.QueueBind(queue: queueName,
    exchange: "direct-exchange-example",
    routingKey: "direct-queue-example");

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
    queue: queueName, 
    autoAck: true, 
    consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};

Console.Read();


// 1.Adim : Publisher'da ki exchange ile birebir aynı isim ve type'a sahip bir exxhange tanımlanmalıdır!
// 2.Adim : Publisher tarafindan routing key' de bulunan degerdeki kuyruga gonderilen mesajlari kendi olusturdugumuz kuyruga yonlendirerek tuketmemiz gerekmektedir. Bunun icin oncelikle bir kuyruk olustulmalıdır !
// 3.Adim : Mesajlari Receive edilir.
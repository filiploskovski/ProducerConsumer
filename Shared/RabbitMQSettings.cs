using RabbitMQ.Client;

namespace Shared
{
    public static class RabbitMQSettings
    {
        public static string HOSTNAME = "localhost";
        public static string USERNAME = "guest";
        public static string PASSWORD = "guest";
        public static string QUEUE = "loshko";
    }
}
namespace EmailService
{
    public class EmailConfiguration
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public EmailConfiguration(string from, string smtpServer, int port, string userName, string password)
        {
            From = from;
            SmtpServer = smtpServer;
            Port = port;
            UserName = userName;
            Password = password;
        }
    }
}
namespace Decorator
{
    /// <summary>
    /// Component (as interface)
    /// </summary>
    public interface IMailService
    {
        bool SendMail(string message);
    }

    /// <summary>
    /// ConcreateComponent1
    /// </summary>
    public class OnPremiseMailService : IMailService
    {
        public bool SendMail(string message)
        {
            // Send mail using on-premise service
            Console.WriteLine($"Message \"{message}\" sent via {nameof(OnPremiseMailService)}.");
            return true;
        }
    }

    /// <summary>
    /// ConcreateComponent2
    /// </summary>
    public class CloudMailService : IMailService
    {
        public bool SendMail(string message)
        {
            // Send mail using on-premise service
            Console.WriteLine($"Message \"{message}\" sent via {nameof(CloudMailService)}.");
            return true;
        }
    }

    /// <summary>
    /// Decorator
    /// </summary>
    public class MailServiceDecoratorBase : IMailService
    {
        private readonly IMailService _mailService;

        public MailServiceDecoratorBase(IMailService mailService)
        {
            _mailService = mailService;
        }

        public virtual bool SendMail(string message)
        {
            return _mailService.SendMail(message);
        }
    }

    /// <summary>
    /// ConcreateDecorator1
    /// </summary>
    public class StatisticsDecorator : MailServiceDecoratorBase
    {
        public StatisticsDecorator(IMailService mailService) : base(mailService) { }

        public override bool SendMail(string message)
        {
            // Fake collecting before sending
            Console.WriteLine($"Collecting statistics in {nameof(StatisticsDecorator)}.");
            return base.SendMail(message);
        }
    }

    /// <summary>
    /// ConcreateDecorator2
    /// </summary>
    public class MessageDatabasesDecorator : MailServiceDecoratorBase
    {
        /// <summary>
        /// A list of sent messages in memory database
        /// </summary>
        public List<string> SentMessages { get; private set; } = [];

        public MessageDatabasesDecorator(IMailService mailService) : base(mailService) { }

        public override bool SendMail(string message)
        {
            if (base.SendMail(message))
            {
                SentMessages.Add(message);
                return true;
            }
            return false;
        }
    }
}

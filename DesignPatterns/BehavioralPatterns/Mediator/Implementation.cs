namespace Mediator
{
    /// <summary>
    /// Mediator interface
    /// </summary>
    public interface IChatRoomMediator
    {
        void SendMessage(string message, User sender);
        void RegisterUser(User user);
    }

    /// <summary>
    /// Colleague abstract class
    /// </summary>
    public abstract class User
    {
        protected readonly IChatRoomMediator _mediator;
        public string Name { get; }

        protected User(IChatRoomMediator mediator, string name)
        {
            _mediator = mediator;
            Name = name;
        }

        public void Send(string message)
        {
            Console.WriteLine($"[{Name}] sending: \"{message}\"");
            _mediator.SendMessage(message, this);
        }

        public abstract void Receive(string message, string senderName);
    }

    /// <summary>
    /// Concrete Colleague 1
    /// </summary>
    public class DeveloperUser : User
    {
        public DeveloperUser(IChatRoomMediator mediator, string name) : base(mediator, name) { }

        public override void Receive(string message, string senderName)
        {
            Console.WriteLine($"  -> Developer {Name} received from {senderName}: \"{message}\"");
        }
    }

    /// <summary>
    /// Concrete Colleague 2
    /// </summary>
    public class TesterUser : User
    {
        public TesterUser(IChatRoomMediator mediator, string name) : base(mediator, name) { }

        public override void Receive(string message, string senderName)
        {
            Console.WriteLine($"  -> QA Tester {Name} received from {senderName}: \"{message}\"");
        }
    }

    /// <summary>
    /// Concrete Mediator
    /// </summary>
    public class TeamChatRoom : IChatRoomMediator
    {
        private readonly List<User> _users = [];

        public void RegisterUser(User user)
        {
            if (!_users.Contains(user))
            {
                _users.Add(user);
            }
        }

        public void SendMessage(string message, User sender)
        {
            foreach (var user in _users)
            {
                // Do not broadcast back to the sender
                if (user != sender)
                {
                    user.Receive(message, sender.Name);
                }
            }
        }
    }
}

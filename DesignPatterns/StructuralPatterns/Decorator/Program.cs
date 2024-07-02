using Decorator;

Console.Title = "Decorator Pattern";

// instantiate mail service
var cloudMailService = new CloudMailService();
cloudMailService.SendMail("Hello everyone.");

var onPremiseMailService = new OnPremiseMailService();
onPremiseMailService.SendMail("Hello everyone");

// add behaviour
var statisticsdecorator = new StatisticsDecorator(cloudMailService);
statisticsdecorator.SendMail($"Hello everyone via {nameof(StatisticsDecorator)} wrapper.");

// add behaviour
var messageDatabasesDecorator = new MessageDatabasesDecorator(onPremiseMailService);
messageDatabasesDecorator.SendMail(
    $"Hello everyone via {nameof(MessageDatabasesDecorator)} wrapper, message #1."
);
messageDatabasesDecorator.SendMail(
    $"Hello everyone via {nameof(MessageDatabasesDecorator)} wrapper, message #2."
);

foreach (var message in messageDatabasesDecorator.SentMessages)
{
    Console.WriteLine($"Sent message: \"{message}\"");
}

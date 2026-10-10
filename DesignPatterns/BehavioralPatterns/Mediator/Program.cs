using Mediator;

Console.Title = "Mediator Pattern";

var chatRoom = new TeamChatRoom();

var dev1 = new DeveloperUser(chatRoom, "Alice");
var dev2 = new DeveloperUser(chatRoom, "Bob");
var tester = new TesterUser(chatRoom, "Carol");

chatRoom.RegisterUser(dev1);
chatRoom.RegisterUser(dev2);
chatRoom.RegisterUser(tester);

Console.WriteLine("--- Communication via Mediator ---");
dev1.Send("Feature build is complete and deployed to staging!");
Console.WriteLine();
tester.Send("Starting automated regression test suite on staging.");
Console.WriteLine();
dev2.Send("Thanks Carol, standing by for results.");

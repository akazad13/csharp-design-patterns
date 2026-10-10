using ChainOfResponsibility;

Console.Title = "Chain of Responsibility Pattern";

// Configure chain: TeamLead -> Manager -> VicePresident
var teamLead = new TeamLeadApprover();
var manager = new ManagerApprover();
var vp = new VicePresidentApprover();

teamLead.SetNext(manager).SetNext(vp);

// Process requests through the chain
Console.WriteLine("--- Request 1: Office Supplies ($450) ---");
teamLead.Process(new PurchaseRequest(101, "Office Supplies", 450));

Console.WriteLine("\n--- Request 2: New Developer Workstation ($2,800) ---");
teamLead.Process(new PurchaseRequest(102, "Developer Workstation", 2800));

Console.WriteLine("\n--- Request 3: Annual Cloud Hosting Infrastructure ($15,000) ---");
teamLead.Process(new PurchaseRequest(103, "Cloud Infrastructure", 15000));

Console.WriteLine("\n--- Request 4: Company Vehicle Fleet ($85,000) ---");
teamLead.Process(new PurchaseRequest(104, "Vehicle Fleet", 85000));

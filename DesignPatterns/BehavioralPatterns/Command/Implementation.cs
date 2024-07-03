namespace Command
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Employee(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Manager : Employee
    {
        public List<Employee> Employees = [];

        public Manager(int id, string name) : base(id, name) { }
    }

    /// <summary>
    /// Reciever (interface)
    /// </summary>
    public interface IEmployeeManagerRepository {
        void AddEmploye(int managerId, Employee employee);
        void RemoveEmploye(int managerId, Employee employee);
        bool HasEmployee(int managerId, int employeId);
        void WriteDataStore();
    }

    /// <summary>
    /// Concrete Reciver
    /// </summary>
    /// 
    public class EmployeeManagerRepository : IEmployeeManagerRepository
    {
        private readonly List<Manager> _managers = new() { new Manager(1, "Paul"), new Manager(2, "Sid")};
        public void AddEmploye(int managerId, Employee employee)
        {
            _managers.First(m => m.Id == managerId).Employees.Add(employee);
        }

        public bool HasEmployee(int managerId, int employeId)
        {
            return _managers.First(m => m.Id == managerId).Employees.Exists(e => e.Id == employeId);
        }

        public void RemoveEmploye(int managerId, Employee employee)
        {
            _managers.First(m => m.Id == managerId).Employees.Remove(employee);
        }

        public void WriteDataStore()
        {
            
            foreach (var manager in _managers)
            {
                Console.WriteLine($"Manager {manager.Id}, {manager.Name}");
                if (manager.Employees.Any())
                {
                    foreach (var employee in manager.Employees)
                    {
                        Console.WriteLine($"\t Employee {employee.Id}, {employee.Name}");
                    }
                }
                else
                {
                    Console.WriteLine($"\t No employees.");
                }
            }
        }
    }

    /// <summary>
    /// Command
    /// </summary>
    public interface ICommand {
        void Execute();
        bool CanExecute();
        void Undo();
    }

    /// <summary>
    /// Concrete Command
    /// </summary>
    public class AddEmployeeToManagerList : ICommand
    {
        private readonly IEmployeeManagerRepository _employeeManagerRepository;
        private readonly int _managerId;
        private readonly Employee _employee;

        public AddEmployeeToManagerList(IEmployeeManagerRepository employeeManagerRepository, int managerId, Employee employee)
        {
            _employeeManagerRepository = employeeManagerRepository;
            _managerId = managerId;
            _employee = employee;
        }
        public bool CanExecute()
        {
            if(_employee == null) {
                return false;
            }

            if(_employeeManagerRepository.HasEmployee(_managerId, _employee.Id)) {
                return false;
            }

            return true;
        }

        public void Execute()
        {
            if(_employee == null) {
                return;
            }

            _employeeManagerRepository.AddEmploye(_managerId, _employee);

        }

        public void Undo()
        {
            if(_employee == null) {
                return;
            }

            _employeeManagerRepository.RemoveEmploye(_managerId, _employee);
        }
    }

    /// <summary>
    /// Invoker
    /// </summary>
    public class CommandManager
    {
        private readonly Stack<ICommand> _commands= new();

        public void Invoke(ICommand command) {
            if(command.CanExecute()) {
                command.Execute();
                _commands.Push(command);
            }
        }

        public void Undo() {
            if(_commands.Count > 0) {
                _commands.Pop()?.Undo();
            }
        }

        public void UndoAll() {
            while(_commands.Count > 0) {
                _commands.Pop()?.Undo();
            }
        }
    }
}

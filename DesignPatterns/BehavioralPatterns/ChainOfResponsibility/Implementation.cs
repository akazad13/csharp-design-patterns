namespace ChainOfResponsibility
{
    public record PurchaseRequest(int Id, string Purpose, decimal Amount);

    /// <summary>
    /// Handler Interface
    /// </summary>
    public interface IApprover
    {
        IApprover SetNext(IApprover next);
        void Process(PurchaseRequest request);
    }

    /// <summary>
    /// Base Handler
    /// </summary>
    public abstract class ApproverBase : IApprover
    {
        private IApprover? _next;

        public IApprover SetNext(IApprover next)
        {
            _next = next;
            return next;
        }

        public virtual void Process(PurchaseRequest request)
        {
            if (_next != null)
            {
                _next.Process(request);
            }
            else
            {
                Console.WriteLine($"[End of Chain] Request #{request.Id} (${request.Amount:N0} for '{request.Purpose}') requires Executive Board approval.");
            }
        }
    }

    /// <summary>
    /// Concrete Handler 1 (handles <= $1,000)
    /// </summary>
    public class TeamLeadApprover : ApproverBase
    {
        public override void Process(PurchaseRequest request)
        {
            if (request.Amount <= 1000)
            {
                Console.WriteLine($"[Team Lead] Approved Request #{request.Id} (${request.Amount:N0} for '{request.Purpose}').");
            }
            else
            {
                Console.WriteLine($"[Team Lead] Amount exceeds $1,000 limit. Escalating to Manager...");
                base.Process(request);
            }
        }
    }

    /// <summary>
    /// Concrete Handler 2 (handles <= $5,000)
    /// </summary>
    public class ManagerApprover : ApproverBase
    {
        public override void Process(PurchaseRequest request)
        {
            if (request.Amount <= 5000)
            {
                Console.WriteLine($"[Manager] Approved Request #{request.Id} (${request.Amount:N0} for '{request.Purpose}').");
            }
            else
            {
                Console.WriteLine($"[Manager] Amount exceeds $5,000 limit. Escalating to Vice President...");
                base.Process(request);
            }
        }
    }

    /// <summary>
    /// Concrete Handler 3 (handles <= $20,000)
    /// </summary>
    public class VicePresidentApprover : ApproverBase
    {
        public override void Process(PurchaseRequest request)
        {
            if (request.Amount <= 20000)
            {
                Console.WriteLine($"[Vice President] Approved Request #{request.Id} (${request.Amount:N0} for '{request.Purpose}').");
            }
            else
            {
                Console.WriteLine($"[Vice President] Amount exceeds $20,000 limit. Escalating to Executive Board...");
                base.Process(request);
            }
        }
    }
}

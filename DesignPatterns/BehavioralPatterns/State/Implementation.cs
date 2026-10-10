namespace State
{
    /// <summary>
    /// Context
    /// </summary>
    public class OrderContext
    {
        public OrderState State { get; private set; }

        public OrderContext()
        {
            // Initial state
            State = new PendingPaymentState();
            State.SetContext(this);
        }

        public void TransitionTo(OrderState state)
        {
            Console.WriteLine($"[OrderContext] Transitioning from {State.GetType().Name} -> {state.GetType().Name}");
            State = state;
            State.SetContext(this);
        }

        public void Pay() => State.Pay();
        public void Ship() => State.Ship();
        public void Deliver() => State.Deliver();
        public void Cancel() => State.Cancel();
    }

    /// <summary>
    /// State base class
    /// </summary>
    public abstract class OrderState
    {
        protected OrderContext? _context;

        public void SetContext(OrderContext context)
        {
            _context = context;
        }

        public abstract void Pay();
        public abstract void Ship();
        public abstract void Deliver();
        public abstract void Cancel();
    }

    /// <summary>
    /// Concrete State 1
    /// </summary>
    public class PendingPaymentState : OrderState
    {
        public override void Pay()
        {
            Console.WriteLine("Payment successful.");
            _context?.TransitionTo(new PaidState());
        }

        public override void Ship()
        {
            Console.WriteLine("Cannot ship order: Payment has not been received yet.");
        }

        public override void Deliver()
        {
            Console.WriteLine("Cannot deliver order: Order hasn't been paid or shipped.");
        }

        public override void Cancel()
        {
            Console.WriteLine("Order cancelled before payment.");
            _context?.TransitionTo(new CancelledState());
        }
    }

    /// <summary>
    /// Concrete State 2
    /// </summary>
    public class PaidState : OrderState
    {
        public override void Pay()
        {
            Console.WriteLine("Order is already paid.");
        }

        public override void Ship()
        {
            Console.WriteLine("Order has been packaged and handed over to carrier.");
            _context?.TransitionTo(new ShippedState());
        }

        public override void Deliver()
        {
            Console.WriteLine("Cannot deliver order: It must be shipped first.");
        }

        public override void Cancel()
        {
            Console.WriteLine("Order cancelled after payment. Initiating refund.");
            _context?.TransitionTo(new CancelledState());
        }
    }

    /// <summary>
    /// Concrete State 3
    /// </summary>
    public class ShippedState : OrderState
    {
        public override void Pay()
        {
            Console.WriteLine("Order is already paid.");
        }

        public override void Ship()
        {
            Console.WriteLine("Order is already in transit.");
        }

        public override void Deliver()
        {
            Console.WriteLine("Package handed over to customer successfully.");
            _context?.TransitionTo(new DeliveredState());
        }

        public override void Cancel()
        {
            Console.WriteLine("Cannot cancel order: Package is already in transit. Return policy applies.");
        }
    }

    /// <summary>
    /// Concrete State 4
    /// </summary>
    public class DeliveredState : OrderState
    {
        public override void Pay() => Console.WriteLine("Order already fulfilled and delivered.");
        public override void Ship() => Console.WriteLine("Order has already been delivered.");
        public override void Deliver() => Console.WriteLine("Order is already marked as delivered.");
        public override void Cancel() => Console.WriteLine("Cannot cancel delivered order.");
    }

    /// <summary>
    /// Concrete State 5
    /// </summary>
    public class CancelledState : OrderState
    {
        public override void Pay() => Console.WriteLine("Cannot pay for a cancelled order.");
        public override void Ship() => Console.WriteLine("Cannot ship a cancelled order.");
        public override void Deliver() => Console.WriteLine("Cannot deliver a cancelled order.");
        public override void Cancel() => Console.WriteLine("Order is already cancelled.");
    }
}

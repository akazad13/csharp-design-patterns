namespace Facade
{
    /// <summary>
    /// Subsystem class
    /// </summary>

    public class OrderService
    {
        public bool HasEnoughOrders(int customerId)
        {
            // Check if the customer has enough orders
            // demo calculation
            return customerId > 5;
        }
    }

    /// <summary>
    /// Subsystem class
    /// </summary>
    public class CustomerDiscountBaseService
    {
        public double CalculateDiscountBase(int customerId)
        {
            return customerId > 8 ? 20 : 30;
        }
    }

    /// <summary>
    /// Subsystem class
    /// </summary>
    public class DayOfTheWeekFactorService
    {
        public double CalculateDayOfTheWeekFactor()
        {
            // fake calculation for demo purposes
            return DateTime.UtcNow.DayOfWeek switch
            {
                DayOfWeek.Saturday or DayOfWeek.Sunday => 0.8,
                _ => 1.2,
            };
        }
    }

    /// <summary>
    /// Facade
    /// </summary>

    public class DiscountFacade
    {
        private readonly OrderService _orderService = new();
        private readonly CustomerDiscountBaseService _customerDiscountBaseService = new();
        private readonly DayOfTheWeekFactorService _dayOfTheWeekFactorService = new();

        public double CalculateDiscountPercentage(int customerId)
        {
            if (!_orderService.HasEnoughOrders(customerId))
            {
                return 0;
            }

            return _customerDiscountBaseService.CalculateDiscountBase(customerId)
                * _dayOfTheWeekFactorService.CalculateDayOfTheWeekFactor();
        }
    }
}

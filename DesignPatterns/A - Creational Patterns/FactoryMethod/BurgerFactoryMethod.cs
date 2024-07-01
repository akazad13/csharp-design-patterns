namespace FactoryMethod
{
    /// <summary>
    /// Product
    /// </summary>
    public abstract class Burger
    {
        public int ProductId { get; set; }

        public abstract void Prepare();
    }

    /// <summary>
    /// Concreate Product A
    /// </summary>
    public class BeefBurger : Burger
    {
        public bool Angus { get; set; }

        public override void Prepare()
        {
            Console.WriteLine("Preparing Beef Burger");
        }
    }

    /// <summary>
    /// Concreate Product B
    /// </summary>
    public class VeggieBurger : Burger
    {
        public bool Combo { get; set; }

        public override void Prepare()
        {
            Console.WriteLine("Preparing Veggie Burger");
        }
    }

    /// <summary>
    /// Creator
    /// </summary>
    public abstract class Restaurant
    {
        public Burger OrderBurger()
        {
            Burger burger = CreateBurger();
            burger.Prepare();
            return burger;
        }

        public abstract Burger CreateBurger();
    }

    /// <summary>
    /// Concreate Creator A
    /// </summary>
    public class BeefBurgerRestaurant : Restaurant
    {
        public override Burger CreateBurger()
        {
            return new BeefBurger();
        }
    }

    /// <summary>
    /// Concreate Creator B
    /// </summary>
    public class VeggieBurgerRestaurant : Restaurant
    {
        public override Burger CreateBurger()
        {
            return new VeggieBurger();
        }
    }
}

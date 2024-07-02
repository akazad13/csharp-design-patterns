using AbstractFactory;

Console.Title = "Abstract Factory";

var indiaShoppingCartPurchaseFactory = new IndiaShoppingCartPurchaseFactory();
var shoppingCartForIndia = new ShoppingCart(indiaShoppingCartPurchaseFactory);
shoppingCartForIndia.CalculateCost();

var chinaShoppingCartPurchaseFactory = new ChinaShoppingCartPurchaseFactory();
var shoppingCartForChina = new ShoppingCart(chinaShoppingCartPurchaseFactory);
shoppingCartForChina.CalculateCost();

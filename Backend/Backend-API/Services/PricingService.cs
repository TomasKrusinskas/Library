namespace Backend_API.Services
{
    public class PricingService
    {
        public decimal CalculatePrice(string type, int days, bool quickPickup)
        {
            decimal baseRate = type == "Book" ? 2 : 3;
            decimal total = baseRate * days;

            if (days > 10) total *= 0.8m;
            else if (days > 3) total *= 0.9m;

            total += 3;
            if (quickPickup) total += 5;

            return total;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinDeskBitcoinPrice;

namespace CoinDeskBitcoinPriceDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            try
            {    
                // Instantiate CoinDesk Bitcoin Price Client
                var coinDeskBitcoinPriceClient = new CoinDeskBitcoinPriceClient();

                // Current price BPI in USD, EUR, and GBP, calculated every minute, based on criteria as discussed on the CoinDesk BPI page https://www.coindesk.com/price/. 
                var currentPrice = await coinDeskBitcoinPriceClient.GetCurrentPrice();
                
                // Display data from currentPrice
                if (currentPrice != null)
                {
                    Console.WriteLine("\nCurrent Price as USD:");
                    foreach (var time in currentPrice.Time)
                    {
                        Console.WriteLine(time.Key + ":" + Convert.ToString(time.Value));
                    }
                    Console.WriteLine("disclaimer:" + currentPrice.Disclaimer);
                    Console.WriteLine("chartName:" + currentPrice.ChartName);
                    foreach (var bpi in currentPrice.Bpi)
                    {
                        Console.WriteLine(bpi.Key  + ":" + ShowCurrencyRate(bpi.Value));
                    }
                }

                // Current price BPI converted into in any of CoinDesk supported currencies https://api.coindesk.com/v1/bpi/supported-currencies.json. Parameters set to return BRL Brazilian Real.
                var currentPriceBrl = await coinDeskBitcoinPriceClient.GetCurrentPrice("BRL");

                // Display data from currentPriceBrl
                if (currentPriceBrl != null)
                {
                    Console.WriteLine("\nCurrent Price as BRL:");
                    foreach (var time in currentPriceBrl.Time)
                    {
                        Console.WriteLine(time.Key + ":" + Convert.ToString(time.Value));
                    }
                    Console.WriteLine("disclaimer:" + currentPriceBrl.Disclaimer);
                    Console.WriteLine("chartName:" + currentPriceBrl.ChartName);
                    foreach (var bpi in currentPriceBrl.Bpi)
                    {
                        Console.WriteLine(bpi.Key + ":" + ShowCurrencyRate(bpi.Value));
                    }
                }
                               
                // Historical data from CoinDesk Bitcoin Price Index. By default, this will return the previous 31 days' worth of data as USD United States Dollar.
                var historicalPrice = await coinDeskBitcoinPriceClient.GetHistoricalPrice();

                // Display data from historicalPrice
                if (historicalPrice != null)
                {
                    Console.WriteLine("\nHistorical Price as USD, previous 31 day's:");
                    Console.WriteLine("bpi:");
                    foreach (var bpi in historicalPrice.Bpi)
                    {
                        Console.WriteLine(bpi.Key + ":" + Convert.ToString(bpi.Value));
                    }
                    Console.WriteLine("disclaimer:" + historicalPrice.Disclaimer);
                    foreach (var time in historicalPrice.Time)
                    {
                        Console.WriteLine(time.Key + ":" + Convert.ToString(time.Value));
                    }
                }
                
                // Historical data from CoinDesk Bitcoin Price Index. Parameters set to return the previous 7 days's worth of data as EUR Euro.
                var historicalPriceEur = await coinDeskBitcoinPriceClient.GetHistoricalPrice("USD", "EUR", DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));

                // Display data from historicalPriceEur
                if (historicalPriceEur != null)
                {
                    Console.WriteLine("\nHistorical Price as EUR, previous 7 day's:");
                    Console.WriteLine("bpi:");
                    foreach (var bpi in historicalPriceEur.Bpi)
                    {
                        Console.WriteLine(bpi.Key + ":" + Convert.ToString(bpi.Value));
                    }
                    Console.WriteLine("disclaimer:" + historicalPriceEur.Disclaimer);
                    foreach (var time in historicalPriceEur.Time)
                    {
                        Console.WriteLine(time.Key + ":" + Convert.ToString(time.Value));
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
        
        static string ShowCurrencyRate(CurrencyRate currencyRate)
        {
            string result = string.Empty;
            result += $"code:{currencyRate.Code}\n";
            result += (currencyRate != null) ? result += $"symbol:{currencyRate.Symbol}\n" : result += String.Empty;
            result += $"rate:{currencyRate.Rate}\n";
            result += $"rate_float:{Convert.ToString(currencyRate.Rate_Float)}";
            return result;
        }               
    }
}

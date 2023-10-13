using Newtonsoft.Json;

HttpClient httpClient = new();
const string API = "https://api.wallex.ir/v1/currencies/stats";

HttpResponseMessage response = await httpClient.GetAsync(API);

if (response.IsSuccessStatusCode)
{
    string apiResponse = await response.Content.ReadAsStringAsync();
    ApiResponseWrapper apiWrapper = JsonConvert.DeserializeObject<ApiResponseWrapper>(apiResponse);
    List<DataItem> dataItems = apiWrapper.result;
    Console.WriteLine($"Based on market analysis out of {dataItems.Count()} coins, these ones are gonna be profitable:");
    foreach (var item in dataItems)
    {
        if (buyForProfit(item))
        {
            Console.WriteLine($"{item.name_en,-15} {Math.Round((decimal)item.price, 2),-10} ==>    {pricePrediction(item).ToString("F2"),-10}");
        }
    }
}

bool buyForProfit(DataItem item)
{
    if (item.market_cap > 500_999_999 && item.percent_change_30d > 2)
    {
        return true;
    }
    else
    {
        return false;
    }
}

decimal pricePrediction(DataItem item)
{
    decimal futurePrice;
    futurePrice = (decimal)(item.price * (1 + (item.percent_change_30d / 100)));
    return futurePrice;
}

public class ApiResponseWrapper
{
    public List<DataItem> result { get; set; }
}

public class DataItem
{
    public string? name_en { get; set; }
    public decimal? market_cap { get; set; }
    public decimal? price { get; set; }
    public decimal? percent_change_30d { get; set; }
    public decimal? total_supply { get; set; }
}

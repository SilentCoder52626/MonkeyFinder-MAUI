using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    HttpClient httpClient;

    public MonkeyService()
    {
        this.httpClient = new HttpClient();
    }

    List<Monkey> monkeyList = new();
    public async Task<List<Monkey>> GetMonkeys()
    {
        if(monkeyList.Any())
        {
            return monkeyList;
        }
        var url = "https://montemagno.com/monkeys.json";
        var response = await httpClient.GetAsync(url);
        if(response.IsSuccessStatusCode)
        {
            monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
        }
        else
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();

            monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);
        }



        return monkeyList;
    }
}

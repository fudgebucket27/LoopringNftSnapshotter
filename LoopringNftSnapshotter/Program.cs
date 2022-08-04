using CsvHelper;
using LoopringNftSnapshotter.Models;
using LoopringNftSnapshotter.Services;
using System.Diagnostics;
using System.Globalization;

//Initialize objects
LoopringGraphQLService loopringGraphQLService = new LoopringGraphQLService("https://api.thegraph.com/subgraphs/name/juanmardefago/loopring36");
List<string> metaboyNftIds = new List<string>();
List<NftHolder> metaboyHolders = new List<NftHolder>();
List<NftHolder> errors = new List<NftHolder>();


//Load nfts from text file
using(StreamReader sr = new StreamReader("metaboys.txt"))
{
    string metaboyId;
    while ((metaboyId = sr.ReadLine()!) != null)
    {
        metaboyNftIds.Add(metaboyId);
    }
}

Console.WriteLine("Working...");
//Loop through nft ids and call graph ql service to get holders

Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
foreach(string metaboyNftId in metaboyNftIds)
{
    List<AccountNFTSlot> accountNftSlots = await loopringGraphQLService.GetNftHolders($"0x39dc73b6067f33f1225695059512fa889b8cef6b-0-0x1d006a27bd82e10f9194d30158d91201e9930420-{metaboyNftId}-5");
    if(accountNftSlots.Count == 0) //Issue with the graph
    {
        errors.Add(new NftHolder() { address = "Could not find!", nftId = metaboyNftId });
    }
    else
    {
        foreach (var nftHolder in accountNftSlots)
        {
            metaboyHolders.Add(new NftHolder() { address = nftHolder.account!.address, nftId = metaboyNftId });
        }
    }
    break;
}
stopWatch.Stop();
Console.WriteLine($"Gathered holders in {stopWatch.Elapsed.ToString("hh\\:mm\\:ss\\.ff")}");



//Create CSV report
string dateTime = DateTime.Now.ToString("yyyy-mm-dd hh-mm-ss");
string holderCsvName = $"Holders-{dateTime}.csv";
string errorCsvName = $"Errors-{dateTime}.csv";
using (var writer = new StreamWriter(holderCsvName))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(metaboyHolders);
    Console.WriteLine($"Generated Holder Report and can be found in the following location: {AppDomain.CurrentDomain.BaseDirectory + holderCsvName}");
}

using (var writer = new StreamWriter(errorCsvName))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(errors);
    Console.WriteLine($"Generated Holder Report and can be found in the following location: {AppDomain.CurrentDomain.BaseDirectory + errorCsvName}");
}

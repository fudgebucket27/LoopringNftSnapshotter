using CsvHelper;
using LoopringNftSnapshotter.Models;
using LoopringNftSnapshotter.Services;
using System.Diagnostics;
using System.Globalization;


//Change these
string nftType = "0"; // 0 is ERC1155, 1 is ERC721
string nftMinterAddress = "0x39dc73b6067f33f1225695059512fa889b8cef6b";
string nftTokenAddress = "0x1d006a27bd82e10f9194d30158d91201e9930420";
string nftRoyaltyPercentage = "5";

//Initialize objects
LoopringGraphQLService loopringGraphQLService = new LoopringGraphQLService("https://api.thegraph.com/subgraphs/name/juanmardefago/loopring36");
List<string> nftIds = new List<string>();
List<NftHolder> collectionHolders = new List<NftHolder>();
List<NftHolder> errors = new List<NftHolder>(); //possibly due to being withdrawn to layer 1

//Load nfts from text file
using(StreamReader sr = new StreamReader("metaboys.txt"))
{
    string nftId;
    while ((nftId = sr.ReadLine()!) != null)
    {
        nftIds.Add(nftId);
    }
}

//Loop through nft ids and call graph ql service to get holders
Console.WriteLine("Working...");
Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
foreach(string nftId in nftIds)
{
    string fullNftId = $"{nftMinterAddress}-{nftType}-{nftTokenAddress}-{nftId}-{nftRoyaltyPercentage}";
    List<AccountNFTSlot> accountNftSlots = await loopringGraphQLService.GetNftHolders(fullNftId);
    if(accountNftSlots.Count == 0) //Issue with the graph
    {
        errors.Add(new NftHolder() { address = "Could not find!", nftId = fullNftId });
    }
    else
    {
        foreach (var nftHolder in accountNftSlots)
        {
            collectionHolders.Add(new NftHolder() { address = nftHolder.account!.address, nftId = fullNftId });
        }
    }
    break;
}
stopWatch.Stop();
Console.WriteLine($"Gathered holders in {stopWatch.Elapsed.ToString("hh\\:mm\\:ss\\.ff")}");

//Create CSV report for errors and holders
string dateTime = DateTime.Now.ToString("yyyy-mm-dd hh-mm-ss");
string holderCsvName = $"Holders-{dateTime}.csv";
string errorCsvName = $"Errors-{dateTime}.csv";
using (var writer = new StreamWriter(holderCsvName))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(collectionHolders);
    Console.WriteLine($"Generated Holder Report and can be found in the following location: {AppDomain.CurrentDomain.BaseDirectory + holderCsvName}");
}

using (var writer = new StreamWriter(errorCsvName))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(errors);
    Console.WriteLine($"Generated Error Report and can be found in the following location: {AppDomain.CurrentDomain.BaseDirectory + errorCsvName}");
}

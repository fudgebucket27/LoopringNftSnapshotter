using CsvHelper;
using LoopringNftSnapshotter.Helpers;
using LoopringNftSnapshotter.Models;
using LoopringNftSnapshotter.Services;
using System.Diagnostics;
using System.Globalization;


//Leave this setting to 0 if you want to grab from the latest layer 1 block,
//If this snapshot runs for an extended amount of time it might grab data from a more recent block so you might want to specify the block number for the snapshot for your users
//If you modify this number remember it must be the layer 1 block number, not the layer 2 block number
int layerOneBlockNumber = 0;//

string filePath = "nftIds.txt";

if(args.Length == 2)
{
    layerOneBlockNumber = Int32.Parse(args[0]); //arg 1 is layer one block number

    filePath = args[1]; //arg 2 is filepath to nft ids file
}

//Initialize objects
LoopringGraphQLService loopringGraphQLService = new LoopringGraphQLService("https://gateway.thegraph.com/api/294a874dfcbae25bcca653a7f56cfb63/subgraphs/id/7QP7oCLbEAjejkp7wSLTD1zbRMSiDydAmALksBB5E6i1");
List<string> nftIds = new List<string>();
List<NftHolder> nftHolders = new List<NftHolder>();
List<NftHolder> nftHoldersErrors = new List<NftHolder>();

//Load nfts from text file
using (StreamReader sr = new StreamReader(filePath))
{
    string nftId;
    while ((nftId = sr.ReadLine()!) != null)
    {
        nftIds.Add(nftId.ToLower().Trim());
    }
}

//Loop through nft ids and call graph ql service to get holders
Console.WriteLine("Working...");
Stopwatch stopWatch = new Stopwatch();
stopWatch.Start();
foreach (string nftId in nftIds)
{
    if (nftId.Contains("-") && nftId.Split('-').Length == 5)
    {
        //do nothing
    }
    else
    {
        Console.WriteLine("The full NFT ID needs to be in the form: nftMinterAddress-nftType-nftTokenAddress-nftId-nftRoyaltyPercentage");
        continue;
    }

    //Check for layer 2 transactions
    
    bool hasOriginalNftIdHolders = false;
    int skip = 0;
    while (true)
    {
        IList<AccountNFTSlot>? accountNftSlots = await loopringGraphQLService.GetNftHolders(nftId, skip, 200, "id", "asc",
                   null, layerOneBlockNumber: layerOneBlockNumber)!;
        if ((accountNftSlots == null) || (accountNftSlots.Count == 0)) //No holders or issue with the graph
        {
            break;
        }
        else
        {
            hasOriginalNftIdHolders = true;
            foreach (var nftHolder in accountNftSlots)
            {

                nftHolders.Add(new NftHolder()
                {
                    recieverAddress = nftHolder.account!.address,
                    dateRecieved = TimestampConverter.ToUTCString(nftHolder.createdAtTransaction!.block!.timestamp),
                    transactionId = nftHolder.createdAtTransaction.id,
                    transactionType = nftHolder.createdAtTransaction.typeName,
                    fullNftId = nftId,
                    balance = nftHolder.balance.ToString()
                });
            }
        }
        if (accountNftSlots.Count < 200) break;
        skip += 200;
    }

    //Check for any deposits back into layer 2 from layer 1, these essentially get reminted
    skip = 0;
    bool hasDepositedBackIntoLayer2NftIdHolders = false;
    string[] depositedBackIntoLayer2FullNftIdArray = nftId.Split('-');
    depositedBackIntoLayer2FullNftIdArray[0] = depositedBackIntoLayer2FullNftIdArray[2]; //minter address becomes token address
    depositedBackIntoLayer2FullNftIdArray[4] = "0"; //royalty percentage becomes 0
    string depositedBackIntoLayer2FullNftId = string.Join("-", depositedBackIntoLayer2FullNftIdArray);
    
    if(depositedBackIntoLayer2FullNftId == nftId) //this was already processed
    {
        if (!hasOriginalNftIdHolders)
        {
            nftHoldersErrors.Add(new NftHolder()
            {
                recieverAddress = "N/A",
                fullNftId = nftId
            });
        }
        continue; 
    }

    while (true)
    {
        IList<AccountNFTSlot>? accountNftSlots = await loopringGraphQLService.GetNftHolders(depositedBackIntoLayer2FullNftId, skip, 200, "id", "asc",
                    null, layerOneBlockNumber: layerOneBlockNumber)!;
        if ((accountNftSlots == null) || (accountNftSlots.Count == 0)) //No holders or issue with the graph
        {
            break;
        }
        else
        {
            hasDepositedBackIntoLayer2NftIdHolders = true;
            foreach (var nftHolder in accountNftSlots)
            {
                nftHolders.Add(new NftHolder()
                {
                    recieverAddress = nftHolder.account!.address,
                    dateRecieved = TimestampConverter.ToUTCString(nftHolder.createdAtTransaction!.block!.timestamp),
                    transactionId = nftHolder.createdAtTransaction.id,
                    transactionType = nftHolder.createdAtTransaction.typeName,
                    fullNftId = depositedBackIntoLayer2FullNftId,
                    balance = nftHolder.balance.ToString()
                });
            }

        }
        if (accountNftSlots.Count < 200) break;
        skip += 200;
    } 

    if(!hasOriginalNftIdHolders && !hasDepositedBackIntoLayer2NftIdHolders)
    {
        nftHoldersErrors.Add(new NftHolder()
        {
            recieverAddress = "N/A",
            fullNftId = nftId
        });
    }

}
stopWatch.Stop();
Console.WriteLine($"Gathered holders in {stopWatch.Elapsed.ToString("hh\\:mm\\:ss\\.ff")}");

//Create CSV report for errors and holders
string dateTime = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
string holderCsvName = $"Holders-{dateTime}.csv";
string errorCsvName = $"Errors-{dateTime}.csv";
using (var writer = new StreamWriter(holderCsvName))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(nftHolders);
    Console.WriteLine($"Generated Holder Report and can be found in the following location: {AppDomain.CurrentDomain.BaseDirectory + holderCsvName}");
}

using (var writer = new StreamWriter(errorCsvName))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(nftHoldersErrors);
    Console.WriteLine($"Generated Error Report and can be found in the following location: {AppDomain.CurrentDomain.BaseDirectory + errorCsvName}");
}

using LoopringNftSnapshotter.Models;
using LoopringNftSnapshotter.Services;

LoopringGraphQLService loopringGraphQLService = new LoopringGraphQLService("https://api.thegraph.com/subgraphs/name/juanmardefago/loopring36");
List<AccountNFTSlot> nftHolders = await loopringGraphQLService.GetNftHolders("0x39dc73b6067f33f1225695059512fa889b8cef6b-0-0x1d006a27bd82e10f9194d30158d91201e9930420-0x13a4661f9339947e56a2c16e704ad6b9c77a5a20cd212272c26959fdad0ccd2e-5");
foreach(var nftHolder in nftHolders)
{
    Console.WriteLine(nftHolder.account!.address);
}
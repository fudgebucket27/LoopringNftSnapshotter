# LoopringNftSnapshotter
Snapshot NFT holders on Loopring using the Subgraph. For a 10 000 NFT collection(1/1 mints) expect it to around 5 minutes depending on your internet speed. If doing a snapshot of a large collection I would recommend you give your holders enough time in advance to prepare for it. I would suggest that your holders not do anything with their NFTs in the hours leading up to the snapshot.

This tool can snapshot holders for NFTs minted as 1/1s or minted an X amount of times.

You will need an IDE that can handle .NET 6 like Visual Studio 2022 to compile this. I suggest downloading the latest precompiled release from [here](https://github.com/fudgebucket27/LoopringNftSnapshotter/releases)

## Setup
In the included nftIds.txt file, you can enter the full nft id on each line such as:

```bash 
0x124be95eb2321386360adddaff38f980bae2d33b-0-0xd8e8c807e4b33abfde1eb514e798f700ca4e361b-0xf11780791dfef9ca79a07f046e98ef0efdebecfaa763b24eb61ccaaca3132d32-10
```
Full nft ids on loopring are of the form:

```bash
nftMinterAddress-nftType-nftTokenAddress-nftId-nftRoyaltyPercentage
```
If you run the program without arguments it will default to the latest block and the default included nftIds.txt file. You can pass the block number like below via commandline to target a specific block:

```bash
LoopringNftSnapshotter 15735556
```

You can also specify the full file path of another nft ids file as the second argument via commandline:

```bash
LoopringNftSnapshotter 15735556 "C:\temp\ids.txt"
```

Two CSV reports will be output after the snapshot has run, one with the holders and another with errors. The subgraph will not return holders if the NFT is withdrawn to layer 1 and not deposited back to layer 2 or if the fullNftId can not be found in the specified layer one block number, you will find these in the errors CSV report.

## Credits
ItsMonty.eth for help with the subgraph queries

Modersohn for the boiler plate

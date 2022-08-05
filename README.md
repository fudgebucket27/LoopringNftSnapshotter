# LoopringNftSnapshotter
Snapshot NFT holders on Loopring using the Subgraph. For a 10 000 NFT collection(1/1 mints) expect it to take an hour depending on your internet speed.

This tool can snapshot holders for NFTs minted as 1/1s or minted an X amount of times.

You will need to modify the nftType, nftMinterAddress, nftTokenAddress and nftRoyaltyPercentage to the appropriate values for your collection.

The included nftIds.txt file needs to be modified with your collection's Nft Ids. On each line you can either put in a single nft id such as 0xb34b96e2294f7b79b6af3f576758febcee688977054438328fcf3e76e9fb9742 , which will work as long as you have filled out the nftType, nftMinterAddress, nftTokenAddress and nftRoyaltyPercentage on lines 9-12. You can enter a full nft id on each line such as 0x124be95eb2321386360adddaff38f980bae2d33b-0-0xd8e8c807e4b33abfde1eb514e798f700ca4e361b-0xf11780791dfef9ca79a07f046e98ef0efdebecfaa763b24eb61ccaaca3132d32-10 which will ignore what was put into lines 9-12.

Two CSV reports will be output after the snapshot has run, one with the holders and another with errors. The subgraph will not return holders if the NFT is withdrawn to layer 1 or if the fullNftId can not be found, you will find these in the errors CSV report.

You will need an IDE that can compile .NET 6 to use this tool.



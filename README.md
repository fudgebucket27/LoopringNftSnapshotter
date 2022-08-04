# LoopringNftSnapshotter
Snapshot NFT holders on Loopring using the Subgraph. For a 10 000 NFT collection(1/1 mints) expect it to take an hour depending on your internet speed.

This tool can snapshot holders for NFTs minted as 1/1s or minted an X amount of times.

You will need to modify the nftType, nftMinterAddress, nftTokenAddress and nftRoyaltyPercentage to the appropriate values for your collection.

The included nftIds.txt file needs to be modified with your collection's Nft Ids.

Two CSV reports will be output after the snapshot has run, one with the holders and another with errors. The subgraph will not return holders if the NFT is withdrawn to layer 1 or if the fullNftId can not be found, you will find these in the errors CSV report.

You will need an IDE that can compile .NET 6 to use this tool.



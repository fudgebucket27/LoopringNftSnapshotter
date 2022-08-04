﻿namespace LoopringNftSnapshotter.Helpers
{
    public static class GraphQLFragments
    {
        public static string BlockFragment = @"  
            fragment BlockFragment on Block {
                id
                timestamp
                txHash
                gasLimit
                gasPrice
                height
                blockHash
                blockSize
                gasPrice
                operatorAccount {
                  ...AccountFragment
                }
              }";

        //to avoid recursion in User/PoolFragments for accountCreatedAt, use an un-nested fragment
        //otherwise the transaction belongs to a block, which has an operator, which again is a pool
        public static string AccountCreatedAtFragment = @"
            fragment AccountCreatedAtFragment on Transaction {
                id
                __typename
                block {
                  id
                  timestamp
                }
            }";

        public static string AccountFragment = @"
            fragment AccountFragment on Account {
                id
                address
                __typename
            }";

        public static string UserFragment = @"
            fragment UserFragment on User {
                id
                address
                __typename
                createdAtTransaction {
                  ...AccountCreatedAtFragment
                }
                publicKey
              }";

        public static string PoolFragment = @"
            fragment PoolFragment on Pool {
                id
                address
                __typename
                createdAtTransaction {
                  ...AccountCreatedAtFragment
                }
                feeBipsAMM
              }";

        public static string TokenFragment = @"
            fragment TokenFragment on Token {
                id
                name
                symbol
                decimals
                address
              }";

        public static string NFTFragment = @"
          fragment NFTFragment on NonFungibleToken {
            id
            minter {
              ...AccountFragment
            }
            __typename
            nftID
            nftType
            token
          }
        ";

        public static string AddFragment = @"
        fragment AddFragment on Add {
            id
            account {
              ...AccountFragment
            }
            pool {
              ...PoolFragment
            }
            token {
              ...TokenFragment
            }
            feeToken {
              ...TokenFragment
            }
            amount
            fee
            __typename
          }";

        public static string RemoveFragment = @"
         fragment RemoveFragment on Remove {
            id
            account {
              ...AccountFragment
            }
            pool {
              ...PoolFragment
            }
            token {
              ...TokenFragment
            }
            feeToken {
              ...TokenFragment
            }
            amount
            fee
            __typename
          }";

        public static string SwapFragment = @"
        fragment SwapFragment on Swap {
            id
            account {
              ...AccountFragment
            }
            pool {
              ...PoolFragment
            }
            tokenA {
              ...TokenFragment
            }
            tokenB {
              ...TokenFragment
            }
            pair {
              id
              token0 {
                symbol
              }
              token1 {
                symbol
              }
            }
            tokenAPrice
            tokenBPrice
            fillSA
            fillSB
            fillBA
            fillBB
            protocolFeeA
            protocolFeeB
            feeA
            feeB
            __typename
          }";

        public static string OrderBookTradeFragment = @"
          fragment OrderbookTradeFragment on OrderbookTrade {
            id
            accountA {
              ...AccountFragment
            }
            accountB {
              ...AccountFragment
            }
            tokenA {
              ...TokenFragment
            }
            tokenB {
              ...TokenFragment
            }
            pair {
              id
              token0 {
                symbol
              }
              token1 {
                symbol
              }
            }
            tokenAPrice
            tokenBPrice
            fillSA
            fillSB
            fillBA
            fillBB
            fillAmountBorSA
            fillAmountBorSB
            feeA
            feeB
            __typename
          }";

        public static string DepositFragment = @"
          fragment DepositFragment on Deposit {
            id
            toAccount {
              ...AccountFragment
            }
            token {
              ...TokenFragment
            }
            amount
            __typename
          }";

        public static string WithdrawalFragment = @"
          fragment WithdrawalFragment on Withdrawal {
            id
            fromAccount {
              ...AccountFragment
            }
            token {
              ...TokenFragment
            }
            feeToken {
              ...TokenFragment
            }
            amount
            fee
            __typename
          }";

        public static string TransferFragment = @"
         fragment TransferFragment on Transfer {
            id
            fromAccount {
              ...AccountFragment
            }
            toAccount {
              ...AccountFragment
            }
            token {
              ...TokenFragment
            }
            feeToken {
              ...TokenFragment
            }
            amount
            fee
            __typename
          }";

        public static string AccountUpdateFragment = @"
          fragment AccountUpdateFragment on AccountUpdate {
            id
            user {
              id
              address
              publicKey
            }
            feeToken {
              ...TokenFragment
            }
            fee
            nonce
            __typename
          }";

        public static string AmmUpdateFragment = @"
          fragment AmmUpdateFragment on AmmUpdate {
            id
            pool {
              ...PoolFragment
            }
            tokenID
            feeBips
            tokenWeight
            nonce
            balance
            __typename
          }";

        public static string SignatureVerificationFragment = @"
          fragment SignatureVerificationFragment on SignatureVerification {
            id
            account {
              ...AccountFragment
            }
            verificationData
            __typename
          }";

        public static string TradeNFTFragment = @"
          fragment TradeNFTFragment on TradeNFT {
            id
            accountSeller {
              ...AccountFragment
            }
            accountBuyer {
              ...AccountFragment
            }
            token {
              ...TokenFragment
            }
            nfts {
              ...NFTFragment
            }
            realizedNFTPrice
            feeBuyer
            feeSeller
            fillSA
            fillBA
            fillSB
            fillBB
            tokenIDAS
            protocolFeeBuyer
            __typename
          }";

        public static string SwapNFTFragment = @"
         fragment SwapNFTFragment on SwapNFT {
            id
            accountA {
              ...AccountFragment
            }
            accountB {
              ...AccountFragment
            }
            nfts {
              ...NFTFragment
            }
            __typename
          }";

        public static string WithdrawalNFTFragment = @"
          fragment WithdrawalNFTFragment on WithdrawalNFT {
            id
            fromAccount {
              ...AccountFragment
            }
            fee
            feeToken {
              ...TokenFragment
            }
            nfts {
              ...NFTFragment
            }
            amount
            valid
            __typename
          }";

        public static string TransferNFTFragment = @"
          fragment TransferNFTFragment on TransferNFT {
            id
            fromAccount {
              ...AccountFragment
            }
            toAccount {
              ...AccountFragment
            }
            feeToken {
              ...TokenFragment
            }
            nfts {
              ...NFTFragment
            }
            fee
            amount
            __typename
          }";

        public static string MintNFTFragment = @"
          fragment MintNFTFragment on MintNFT {
            id
            minter {
              ...AccountFragment
            }
            receiver {
              ...AccountFragment
            }
            receiverSlot {
              id
            }
            nft {
              ...NFTFragment
            }
            fee
            feeToken {
              ...TokenFragment
            }
            amount
            __typename
          }";

        public static string MintNFTFragmentWithoutNFT = @"
          fragment MintNFTFragmentWithoutNFT on MintNFT {
            id
            minter {
              ...AccountFragment
            }
            receiver {
              ...AccountFragment
            }
            receiverSlot {
              id
            }
            fee
            feeToken {
              ...TokenFragment
            }
            amount
            __typename
          }";

        public static string DataNFTFragment = @"
        fragment DataNFTFragment on DataNFT {
            id
            __typename
            accountID
            tokenID
            minter
            tokenAddress
            nftID
            nftType
            data
          }";
    }
}

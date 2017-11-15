using System;
using via_jsonrpc;

namespace via_jsonrpc_test {
    class Program {
        static void Main (string[] args) {

            var userId = 1;
            var asset = "BTC";
            var business = "deposit";
            int start_time = 0;
            int end_time = 0;
            int offset = 0;
            int limit = 50;
            string market = "BTCUSD";

            var via = new ViaJsonRpc ("http://10.50.1.2:8080");
            var balance = via.BalanceQuery(userId, asset);
            Console.WriteLine("Your {0} balance is {1} ({2} frozen)", asset, balance.Available, balance.Freeze);
            var balances = via.BalanceQuery(userId);
            foreach (var item in balances)
            {
                Console.WriteLine("Your {0} balance is {1} ({2} frozen)", item.Key, item.Value.Available, item.Value.Freeze);
            }
            // // var status = via.BalanceUpdateQuery(userId, asset, "deposit", 15, "5.5");
            // // Console.WriteLine("Your balance update state is: {0}", status);
            // var balanceHistroy = via.BalanceHistoryQuery(userId, asset, business, start_time, end_time, offset, limit);
            // Console.WriteLine("There is {0} records:", balanceHistroy.limit);
            // foreach(var item in balanceHistroy.records)
            // {
            //     Console.WriteLine(item.asset);
            // }

            //var limitOrder = via.OrderLimitQuery(userId, market, OrderSide.Ask, "1", "9200", "0.002", "0.001", "");

            //var transactionOrder = via.OrderTransactionQuery(14,0,50);
            //Console.WriteLine(transactionOrder.records);

            var orderBook = via.OrderBookQuery(market, OrderSide.Ask, 0, 50);
            orderBook = via.OrderBookQuery(market, OrderSide.Bid, 0, 50);

            var ordersPending = via.OrdersPendingQuery(userId, market, 0, 10);
            Console.WriteLine("{0}: You have {1} total pending orders", market, ordersPending.total);
            var assetA = market.Substring(0, 3);
            var assetB = market.Substring(3, 3);
            foreach (var item in ordersPending.records)
            {
                var feeAsset = item.side == OrderSide.Ask ? assetB : assetA;
                Console.WriteLine($"{item.side} - {item.type} - price: {item.price} {assetB}, amount: {item.amount} {assetA}, remaining: {item.left} {assetA}, traded: {item.deal_stock} {assetA} / {item.deal_money} {assetB}, fee: {item.deal_fee} {feeAsset}");
            }

            // Console.WriteLine(transactionOrder.records);
            //var status = via.TodayMarketStatus(market);
            //Console.WriteLine("Todays Market Status is {0}", status);   

            // var orderDepth = via.OrderDepthQuery(market, 50, "1");
            // var transactionhistory = via.MarketHistoryQuery(market, 50, 0);
            // foreach(var item in transactionhistory)
            // {
            // Console.Write(item.id);
            // }

            Console.WriteLine("Klines...");
            var klines = via.KlineQuery(market, 1, 12000000000, 3600);
            foreach (var kline in klines)
                Console.WriteLine(KlineResponse.ParseKlineList(kline));
        }
    }
}
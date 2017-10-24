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
            string market = "BTCCNY";

            var via = new ViaJsonRpc ("http://10.50.1.2:8080");
            // var balance = via.BalanceQuery(userId, asset);
            // Console.WriteLine("Your balance is {0} ({1} frozen)", balance.Available, balance.Freeze);
            // // var status = via.BalanceUpdateQuery(userId, asset, "deposit", 15, "5.5");
            // // Console.WriteLine("Your balance update state is: {0}", status);
            // var balanceHistroy = via.BalanceHistoryQuery(userId, asset, business, start_time, end_time, offset, limit);
            // Console.WriteLine("There is {0} records:", balanceHistroy.limit);
            // foreach(var item in balanceHistroy.records)
            // {
            //     Console.WriteLine(item.asset);
            // }

            var limitOrder = via.OrderLimitQuery(userId, market, OrderSide.Ask, "1", "9200", "0.002", "0.001", "");

            //var transactionOrder = via.OrderTransactionQuery(14,0,50);
            //Console.WriteLine(transactionOrder.records);

            var orderBook = via.OrderBookQuery(market, OrderSide.Ask, 0, 50);
            orderBook = via.OrderBookQuery(market, OrderSide.Bid, 0, 50);

            // Console.WriteLine(transactionOrder.records);
            //var status = via.TodayMarketStatus("BTCCNY");
            //Console.WriteLine("Todays Market Status is {0}", status);   

            // var orderDepth = via.OrderDepthQuery("BTCCNY",50,"1");
            // var transactionhistory = via.MarketHistoryQuery("BTCCNY",50,0);
            // foreach(var item in transactionhistory)
            // {
            // Console.Write(item.id);
            // }

        }
    }
}
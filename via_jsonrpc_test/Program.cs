using System; 
using Newtonsoft.Json; 
using System.Net.Http; 
using System.Net;
using via_jsonrpc;


namespace via_jsonrpc_test {
 class Program {
        static void Main(string[] args) {

            var userId = 1;
            var asset = "BTC";
            var business = "deposit";
            int start_time = 0;
            int end_time = 0;
            int offset = 0;
            int limit = 50;
            string market = ""
           
            var via = new ViaJsonRpc("http://10.50.1.2:8080");
            var balance = via.BalanceQuery(userId, asset);
            Console.WriteLine("Your balance is {0} ({1} frozen)", balance.Available, balance.Freeze);
            // var status = via.BalanceUpdateQuery(userId, asset, "deposit", 15, "5.5");
            // Console.WriteLine("Your balance update state is: {0}", status);
            var balanceHistroy = via.BalanceHistoryQuery(userId, asset, business, start_time, end_time, offset, limit);
            Console.WriteLine("There is {0} records:", balanceHistroy.limit);

            var limitOrder = via.OrderLimitQuery(userId, "BTCCNY", 1, "1", "8000", "0.002", "0.001", "");

            //var status = via.TodayMarketStatus("BTCCNY");
            //Console.WriteLine("Todays Market Status is {0}", status);   
        }
    }
}

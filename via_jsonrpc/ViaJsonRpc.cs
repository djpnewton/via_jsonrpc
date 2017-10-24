using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace via_jsonrpc {

    public class ViaJsonException : Exception {
        public ViaJsonException (int code, string message) : base (string.Format ("{0}: {1}", code, message)) { }

        public ViaJsonException (string message) : base (message) { }
    }

    public class Error {
        public int Code;
        public string Message;
    }

    public abstract class BaseResponse {
        public Error Error;
        public int Id;

        public void CheckId (int request_id) {
            if (Id != request_id) {
                throw new ViaJsonException (string.Format ("Response id '{0}' does not match request id '{1}'", Id, request_id));
            }
        }
    }

    public class OrderTransactionRecords {

        public float time;
        public string deal;
        public int user;
        public int deal_order_id;
        public int id;
        public int role;
        public string price;
        public string fee;
        public string amount;
    }
    public class OrderTransaction {
        public int offset;
        public int limit;
        public IEnumerable<OrderTransactionRecords> records;
    }

    public class Order {
        public int id;
        public int side;
        public string price;
        public float mtime;
        public string deal_stock;
        public string amount;
        public string market;
        public string maker_fee;
        public float ctime;
        public string source;
        public int type;
        public int user;
        public string taker_fee;
        public string deal_fee;
        public string left;
        public string deal_money;
    }

    public class OrderCompleted {
        public int offset;
        public int limit;
        public IEnumerable<Order> records;
    }

    public class OrderBook {
        public int offset;
        public IEnumerable<Order> orders;

    }

    public class PendingOrder {
        public int limit;
        public int offset;
        public int total;
        public IEnumerable<Order> records;
    }

    public class BalanceHistoryRecords {
        public string asset;
        public float time;
        public Dictionary<string, object> detail;
        public string business;
        public string change;
        public string balance;
    }

    public class BalanceHistory {
        public int offset;
        public int limit;
        public IEnumerable<BalanceHistoryRecords> records;
    }

    public class OrderDepth {
        public IEnumerable<IEnumerable<string>> asks;
        public IEnumerable<IEnumerable<string>> bids;
    }

    public class Balance {
        public string Freeze;
        public string Available;
    }
    public class MarketHistory {
        public int id;
        public string amount;
        public float time;
        public string type;
        public string price;
        public string deal;
        public string fee;
        public int deal_order_id;
    }

    public class ResultStatus {
        public string Status;
    }

    public class MarketTransactionRecord {
        public int id;
        public float time;
        public int user;
        public int side;
        public int role;
        public string amount;
        public string price;
        public string deal;

    }
    public class MarketStatus {
        public string close;
        public int period;
        public string last;
        public string high;
        public string open;
        public string volume;
        public string low;
    }
    public class MarketTransaction {
        public int offset;
        public int limit;

        public IEnumerable<MarketTransactionRecord> records;

    }
    public class TodayMarketStatus {
        public string high;
        public string open;
        public string last;
        public string volume;
        public string low;
        public string deal;
    }

    // Balace
    public class BalanceHistoryResponse : BaseResponse {
        public BalanceHistory Result;

    }
    public class BalanceQueryResponse : BaseResponse {
        public Dictionary<string, Balance> Result;

    }

    public class BalanceUpdateResponse : BaseResponse {
        public ResultStatus Result;
    }

    //Order
    public class OrderTransactionResponse : BaseResponse {
        public OrderTransaction Result;
    }
    public class OrderBookResponse : BaseResponse {
        public OrderBook Result;
    }
    public class OrderDepthResponse : BaseResponse {
        public OrderDepth Result;
    }
    public class OrderPendingResponse : BaseResponse {
        public PendingOrder Result;
    }
    public class ReturnOrderDetailResponse : BaseResponse {
        public Order Result;
    }
    public class OrderCompletedResponse : BaseResponse {
        public OrderCompleted Result;
    }
    public class MarketPriceResponse : BaseResponse {
        public string Result;
    }
    public class MarketHistoryResponse : BaseResponse {
        public IEnumerable<MarketHistory> Result;
    }
    public class MarketTransactionHistoryResponse : BaseResponse {
        public MarketTransaction Result;
    }
    public class KlineResponse : BaseResponse {
        public IEnumerable<IEnumerable<string>> Result;
    }
    public class MarketStatusResponse : BaseResponse {
        public MarketStatus Result;
    }
    public class TodayMarketStatusResponse : BaseResponse {
        public TodayMarketStatus Result;
    }
    public class ViaJsonRpc {
        private int call_id = 1;
        HttpClient client;

        public ViaJsonRpc (string url) {
            client = new HttpClient (url);
        }

        string JsonBody (int call_id, string method, object[] parameters) {
            var dict = new Dictionary<string, object> { { "id", call_id },
                    { "jsonrpc", "2.0" },
                    { "method", method },
                    { "params", parameters }
                };
            return JsonConvert.SerializeObject (dict);
        }

        public Balance BalanceQuery (int user_id, string asset) {
            call_id++;
            var json = JsonBody (call_id, "balance.query", new object[] { user_id, asset });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<BalanceQueryResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result[asset];
        }

        public string BalanceUpdateQuery (int user_id, string asset, string business, int business_id, string change, Dictionary<string, object> source = null) {
            if (source == null)
                source = new Dictionary<string, object> ();

            call_id++;
            var json = JsonBody (call_id, "balance.update", new object[] { user_id, asset, business, business_id, change, source });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<BalanceUpdateResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result.Status;
        }

        public BalanceHistory BalanceHistoryQuery (int user_id, string asset, string business, int start_time, int end_time, int offset, int limit) {
            call_id++;
            var json = JsonBody (call_id, "balance.history", new object[] { user_id, asset, business, start_time, end_time, offset, limit });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<BalanceHistoryResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }
        public Order OrderLimitQuery (int user_id, string market, int side, string amount, string price, string taker_fee_rate, string maker_fee_rate, string source) {
            call_id++;
            var json = JsonBody (call_id, "order.put_limit", new object[] { user_id, market, side, amount, price, taker_fee_rate, maker_fee_rate, source });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<ReturnOrderDetailResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public Order OrderMarketQuery (int user_id, string market, int side, string amount, string taker_fee_rate, string source) {
            call_id++;
            var json = JsonBody (call_id, "order.put_market", new object[] { user_id, market, side, amount, taker_fee_rate, source });
            var resp = JsonConvert.DeserializeObject<ReturnOrderDetailResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public Order OrderCancelQuery (int user_id, string market, int order_id) {
            call_id++;
            var json = JsonBody (call_id, "order.cancel", new object[] { user_id, market, order_id });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<ReturnOrderDetailResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public OrderTransaction OrderTransactionQuery (int order_id, int offset, int limit) {
            call_id++;
            var json = JsonBody (call_id, "order.deals", new object[] { order_id, offset, limit });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<OrderTransactionResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public OrderBook OrderBookQuery (string market, int side, int offset, int limit) {
            call_id++;
            var json = JsonBody (call_id, "order.book", new object[] { market, side, offset, limit });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<OrderBookResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public OrderDepth OrderDepthQuery (string market, int limit, string interval) {
            call_id++;
            var json = JsonBody (call_id, "order.depth", new object[] { market, limit, interval });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<OrderDepthResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public PendingOrder OrderPendingQuery (string market, int limit, string interval) {
            call_id++;
            var json = JsonBody (call_id, "order.pending", new object[] { market, limit, interval });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<OrderPendingResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public Order OrderPendingDetails (string market, int order_id) {
            call_id++;
            var json = JsonBody (call_id, "order.pending_detail", new object[] { market, order_id });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<ReturnOrderDetailResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public OrderCompleted OrderCompletedQuery (int user_id, string market, int start_time, int end_time, int offset, int limit, int side) {
            call_id++;
            var json = JsonBody (call_id, "order.finished", new object[] { user_id, market, start_time, end_time, offset, limit, side });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<OrderCompletedResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public Order OrderCompletedDetails (int order_id) {
            call_id++;
            var json = JsonBody (call_id, "order.finished_detail", new object[] { order_id });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<ReturnOrderDetailResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public string MarketPriceQuery (string market) {
            call_id++;
            var json = JsonBody (call_id, "market.last", new object[] { market });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<MarketPriceResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public IEnumerable<MarketHistory> MarketHistoryQuery (string market, int limit, int last_id) {
            call_id++;
            var json = JsonBody (call_id, "market.deals", new object[] { market, limit, last_id });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<MarketHistoryResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public MarketTransaction MarketTransactionHistoryQuery (int user_id, string market, int offset, int limit) {
            call_id++;
            var json = JsonBody (call_id, "market.user_deals", new object[] { user_id, market, offset, limit });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<MarketTransactionHistoryResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public IEnumerable<IEnumerable<string>> KlineQuery (string market, int start_time, int end_time, int interval) {
            call_id++;
            var json = JsonBody (call_id, "market.kline", new object[] { market, start_time, end_time, interval });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<KlineResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public MarketStatus MarketStatusQuery (string market, int period) {
            call_id++;
            var json = JsonBody (call_id, "market.status", new object[] { market, period });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<MarketStatusResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

        public TodayMarketStatus TodayMarketStatusQuery (string market) {
            call_id++;
            var json = JsonBody (call_id, "market.status_today", new object[] { market });
            json = client.PostJson (json);
            var resp = JsonConvert.DeserializeObject<TodayMarketStatusResponse> (json);
            resp.CheckId (call_id);
            if (resp.Error != null) {
                throw new ViaJsonException (resp.Error.Code, resp.Error.Message);
            }
            return resp.Result;
        }

    }
}
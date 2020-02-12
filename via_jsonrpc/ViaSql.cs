using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace via_jsonrpc.sql
{
    public enum MarketRole
    {
        Maker = 1,
        Taker = 2
    }

    public class Deal
    {
        public double time;
        public int user_id;
        public string market;
        public int deal_id;
        public int order_id;
        public int deal_order_id;
        public OrderSide side;
        public MarketRole role;
        public decimal price;
        public decimal amount;
        public decimal deal;
        public decimal fee;
        public decimal deal_fee;
    }

    public static class ViaSql
    {
        public static readonly string SELECT_USER_DEALS_UNION = @"select * from user_deal_history_0
    union all
    select * from user_deal_history_1
    union all
    select * from user_deal_history_2
    union all
    select * from user_deal_history_3
    union all
    select * from user_deal_history_4
    union all
    select * from user_deal_history_5
    union all
    select * from user_deal_history_6
    union all
    select * from user_deal_history_7
    union all
    select * from user_deal_history_8
    union all
    select * from user_deal_history_9
    union all
    select * from user_deal_history_10
    union all
    select * from user_deal_history_11
    union all
    select * from user_deal_history_12
    union all
    select * from user_deal_history_13
    union all
    select * from user_deal_history_14
    union all
    select * from user_deal_history_15
    union all
    select * from user_deal_history_16
    union all
    select * from user_deal_history_17
    union all
    select * from user_deal_history_18
    union all
    select * from user_deal_history_19
    union all
    select * from user_deal_history_20
    union all
    select * from user_deal_history_21
    union all
    select * from user_deal_history_22
    union all
    select * from user_deal_history_23
    union all
    select * from user_deal_history_24
    union all
    select * from user_deal_history_25
    union all
    select * from user_deal_history_26
    union all
    select * from user_deal_history_27
    union all
    select * from user_deal_history_28
    union all
    select * from user_deal_history_29
    union all
    select * from user_deal_history_30
    union all
    select * from user_deal_history_31
    union all
    select * from user_deal_history_32
    union all
    select * from user_deal_history_33
    union all
    select * from user_deal_history_34
    union all
    select * from user_deal_history_35
    union all
    select * from user_deal_history_36
    union all
    select * from user_deal_history_37
    union all
    select * from user_deal_history_38
    union all
    select * from user_deal_history_39
    union all
    select * from user_deal_history_40
    union all
    select * from user_deal_history_41
    union all
    select * from user_deal_history_42
    union all
    select * from user_deal_history_43
    union all
    select * from user_deal_history_44
    union all
    select * from user_deal_history_45
    union all
    select * from user_deal_history_46
    union all
    select * from user_deal_history_47
    union all
    select * from user_deal_history_48
    union all
    select * from user_deal_history_49
    union all
    select * from user_deal_history_50
    union all
    select * from user_deal_history_51
    union all
    select * from user_deal_history_52
    union all
    select * from user_deal_history_53
    union all
    select * from user_deal_history_54
    union all
    select * from user_deal_history_55
    union all
    select * from user_deal_history_56
    union all
    select * from user_deal_history_57
    union all
    select * from user_deal_history_58
    union all
    select * from user_deal_history_59
    union all
    select * from user_deal_history_60
    union all
    select * from user_deal_history_61
    union all
    select * from user_deal_history_62
    union all
    select * from user_deal_history_63
    union all
    select * from user_deal_history_64
    union all
    select * from user_deal_history_65
    union all
    select * from user_deal_history_66
    union all
    select * from user_deal_history_67
    union all
    select * from user_deal_history_68
    union all
    select * from user_deal_history_69
    union all
    select * from user_deal_history_70
    union all
    select * from user_deal_history_71
    union all
    select * from user_deal_history_72
    union all
    select * from user_deal_history_73
    union all
    select * from user_deal_history_74
    union all
    select * from user_deal_history_75
    union all
    select * from user_deal_history_76
    union all
    select * from user_deal_history_77
    union all
    select * from user_deal_history_78
    union all
    select * from user_deal_history_79
    union all
    select * from user_deal_history_80
    union all
    select * from user_deal_history_81
    union all
    select * from user_deal_history_82
    union all
    select * from user_deal_history_83
    union all
    select * from user_deal_history_84
    union all
    select * from user_deal_history_85
    union all
    select * from user_deal_history_86
    union all
    select * from user_deal_history_87
    union all
    select * from user_deal_history_88
    union all
    select * from user_deal_history_89
    union all
    select * from user_deal_history_90
    union all
    select * from user_deal_history_91
    union all
    select * from user_deal_history_92
    union all
    select * from user_deal_history_93
    union all
    select * from user_deal_history_94
    union all
    select * from user_deal_history_95
    union all
    select * from user_deal_history_96
    union all
    select * from user_deal_history_97
    union all
    select * from user_deal_history_98
    union all
    select * from user_deal_history_99";
        public static readonly string SELECT_USER_DEALS_ALL = $"select * from ({SELECT_USER_DEALS_UNION}) as U";
        public static readonly string SELECT_USER_DEALS_COUNT = $"select count(*) from ({SELECT_USER_DEALS_UNION}) as U";

        //TODO: get rid of this!!! it is not needed
        public static bool EnsureExchangeUserTablesPresent(ILogger logger, string host, string database, string user, string password, int user_id)
        {
            var conn = new MySqlConnection($"host={host};database={database};uid={user};password={password};");
            try
            {
                var sqlCmds = new string[] {
                    $"create table if not exists balance_history_{user_id} like balance_history_example;",
                    $"create table if not exists deal_history_{user_id} like deal_history_example;",
                    $"create table if not exists order_history_{user_id} like order_history_example;",
                    $"create table if not exists order_detail_{user_id} like order_detail_example;",
                    $"create table if not exists user_deal_history_{user_id} like user_deal_history_example;"};
                conn.Open();
                var trans = conn.BeginTransaction();
                foreach (var sqlCmd in sqlCmds)
                {
#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
                    var cmd = new MySqlCommand(sqlCmd, conn, trans);
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public static IList<Deal> ExchangeDeals(ILogger logger, string host, string database, string user, string password, long startUnixtimestamp = 0, long endUnixtimestamp = 0, long offset = 0, long limit = 0)
        {
            var orderBy = "order by U.time desc";
            var limitClause = $"limit {limit}";
            var offsetClause = $"offset {offset}";
            var end = orderBy;
            if (offset != 0 && limit != 0)
                end = $"{orderBy}\n{limitClause}\n{offsetClause}";
            else if (offset != 0)
                end = $"{orderBy}\n{offsetClause}";
            else if (limit != 0)
                end = $"{orderBy}\n{limitClause}";
            var sqlCmd = $"{SELECT_USER_DEALS_ALL}\n{end}";
            if (startUnixtimestamp != 0 && endUnixtimestamp != 0)
                sqlCmd = $"{SELECT_USER_DEALS_ALL}\nwhere U.time >= {startUnixtimestamp} and U.time < {endUnixtimestamp}\n{end}";
            else if (startUnixtimestamp != 0)
                sqlCmd = $"{SELECT_USER_DEALS_ALL}\nwhere U.time >= {startUnixtimestamp}\n{end}";
            else if (endUnixtimestamp != 0)
                sqlCmd = $"{SELECT_USER_DEALS_ALL}\nwhere U.time < {endUnixtimestamp}\n{end}";
            var result = new List<Deal>();
            var conn = new MySqlConnection($"host={host};database={database};uid={user};password={password};");
            conn.Open();
            var cmd = new MySqlCommand(sqlCmd, conn);
            var reader = cmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    var deal = new Deal
                    {
                        time = reader.GetDouble("time"),
                        user_id = reader.GetInt32("user_id"),
                        market = reader.GetString("market"),
                        deal_id = reader.GetInt32("deal_id"),
                        order_id = reader.GetInt32("order_id"),
                        deal_order_id = reader.GetInt32("deal_order_id"),
                        side = (OrderSide)reader.GetInt32("side"),
                        role = (MarketRole)reader.GetInt32("role"),
                        price = reader.GetDecimal("price"),
                        amount = reader.GetDecimal("amount"),
                        deal = reader.GetDecimal("deal"),
                        fee = reader.GetDecimal("fee"),
                        deal_fee = reader.GetDecimal("deal_fee"),
                    };
                    result.Add(deal);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            finally
            {
                reader.Close();
            }
            conn.Close();
            return result;
        }

        public static int ExchangeDealsCount(ILogger logger, string host, string database, string user, string password, long startUnixtimestamp = 0, long endUnixtimestamp = 0)
        {
            var sqlCmd = $"{SELECT_USER_DEALS_COUNT}";
            if (startUnixtimestamp != 0 && endUnixtimestamp != 0)
                sqlCmd = $"{SELECT_USER_DEALS_COUNT}\nwhere U.time >= {startUnixtimestamp} and U.time < {endUnixtimestamp}";
            else if (startUnixtimestamp != 0)
                sqlCmd = $"{SELECT_USER_DEALS_COUNT}\nwhere U.time >= {startUnixtimestamp}";
            else if (endUnixtimestamp != 0)
                sqlCmd = $"{SELECT_USER_DEALS_COUNT}\nwhere U.time < {endUnixtimestamp}";
            var conn = new MySqlConnection($"host={host};database={database};uid={user};password={password};");
            conn.Open();
            var cmd = new MySqlCommand(sqlCmd, conn);
            var result = cmd.ExecuteScalar();
            conn.Close();
            if (result != null)
                return Convert.ToInt32(result);
            logger.LogError("ExchangeDealsCount sql result is null");
            return 0;
        }
    }
}
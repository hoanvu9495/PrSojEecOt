using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.eAita;
using Business.Business;
using Web.ServiceReference1;

namespace Web.Common
{
    public class SmsHelper
    {
        public static void sendSMS(string to_phone, string content){
            Entities context = new Entities();
            TransactionSmsBusiness ITransactionSmsBusiness = new TransactionSmsBusiness(context);
            SendMTPortTypeClient ws = new SendMTPortTypeClient();
            //string username = "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAoYOdKfWI4ka1w/PJDQBZ7QAAAAACAAAAAAAQZgAAAAEAACAAAACOrRzunopEd4SdLySQAn4FovCnKXiYHTm0W1RKMO9lzAAAAAAOgAAAAAIAACAAAACknvo6G/lWzOI8ozdjE+SaiJA//II2wFqnmUOUkwqi7RAAAADqe5zXOpjk+uuIB7ZfbpprQAAAAHHSZ/V0mEaR++uWAZJxvRWqcZ1b0ICLaztISlHfPgFlq3b/07g02qFoXhpTALnMGQXRc23e9OKkILTUNCHqb5w=";
            //string password = "AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAoYOdKfWI4ka1w/PJDQBZ7QAAAAACAAAAAAAQZgAAAAEAACAAAADQNlvmvQilyG67H2y8HjUaRaovVCGQcZlqOG5HIBq5SQAAAAAOgAAAAAIAACAAAABUvgFywQdjyb0FHZkvTr61StF6jL4yUcojhsAs+/4laxAAAAAVQYieAss91mxIgX1Q2EvJQAAAABB6O/zrxbW17HleFzb029zxg8O/tcAjVUj6ogKOFaNpWDP+11tRzUwJJ7NEuEuo9AMSyHqaYvZjMPoJmIpFRkg=";
            string username = "hinet";
            string password = "111111";
            var result = ws.sendSMS(username, password, to_phone, content, 1, "", "1");

            TRANSACTION_SMS TRAN_SMS = new TRANSACTION_SMS();
            TRAN_SMS.CONTENT_MESS = content;
            TRAN_SMS.RECEIVER = to_phone;
            TRAN_SMS.RESPONSE = result.ToString();
            TRAN_SMS.LOAISP = 1;
            TRAN_SMS.BRANDNAME = "";
            TRAN_SMS.TARGET = "1";
            ITransactionSmsBusiness.Save(TRAN_SMS);
        }    
    }
}

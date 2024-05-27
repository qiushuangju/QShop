using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qs.App.Wx;
using Qs.Repository.Vm;

namespace WxPayAPI
{
    /// <summary>
    /// 回调处理基类
    /// 主要负责接收微信支付后台发送过来的数据，对数据进行签名验证
    /// 子类在此类基础上进行派生并重写自己的回调处理过程
    /// </summary>
    public class WxNotify
    {

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetNotifyData(string xml, VmSettingBasicWxApp setting)
        {
            StringBuilder builder = new StringBuilder();
            builder = new StringBuilder(xml);


            WxLog.Info(this.GetType().ToString(), "Receive data from WeChat : " + builder.ToString());

            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString(),  setting);
            }
            catch(WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", ex.Message);
                WxLog.Error(this.GetType().ToString(), "Sign check error : " + res.ToXml());
                 //page.Response.Write(res.ToXml());
                 //page.Response.End();
            }

            WxLog.Info(this.GetType().ToString(), "Check sign success");
            return data;
        }

        //派生类需要重写这个方法，进行不同的回调处理
        public virtual void ProcessNotify()
        {

        }
    }
}
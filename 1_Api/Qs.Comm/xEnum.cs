using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Qs.Comm
{ 
    /// <summary>
    /// 枚举
    /// </summary>
    public class xEnum
    {
        /// <summary>
        /// 页面类型(10:首页 20:活动页)
        /// </summary>
        public enum PageType
        {
            /// <summary>
            ///  首页
            /// </summary>
            [Description("首页")]
            HomePage = 10,
            ///// <summary>
            /////  活动页
            ///// </summary>
            //[Description("活动页")]
            //Trial = 20,

            /// <summary>
            ///  自定义
            /// </summary>
            [Description("自定义")]
            Custom = 100,
        }
        /// <summary>
        /// 店铺状态 状态(-10:闭店 10:试用 20:正常)
        /// </summary>
        public enum StoreStatus
        {
            /// <summary>
            ///  闭店
            /// </summary>
            [Description("闭店")]
            Closed = -10,
            /// <summary>
            ///  试用
            /// </summary>
            [Description("试用")]
            Trial = 10,
            /// <summary>
            ///  正常
            /// </summary>
            [Description("正常")]
            Normal = 20,
        }
        /// <summary>
        /// 店铺地址 10:发货地址 20:退货地址
        /// </summary>
        public enum StoreAddressType
        {
            /// <summary>
            ///  发货地址
            /// </summary>
            [Description("发货地址")]
            SendAddress = 10,
            /// <summary>
            ///  退货地址
            /// </summary>
            [Description("退货地址")]
            ReturnAddress = 20,
        
        }
        /// <summary>
        /// 验证码类型
        /// </summary>
        public enum CodeType
        {
            /// <summary>
            ///  图片验证码
            /// </summary>
            [Description("纯数字验证码")]
            Image = 0,
            /// <summary>
            ///  短信验证码
            /// </summary>
            [Description("短信验证码")]
            Sms = 1,
        }

        /// <summary>
        /// 验证码类型
        /// </summary>
        public enum VerifyCodeType
        {
            /// <summary>
            ///  纯数字验证码
            /// </summary>
            [Description("纯数字验证码")]
            Num = 0,
            /// <summary>
            ///   数字加字母验证码
            /// </summary>
            [Description("数字加字母验证码")]
            Char = 1,
            /// <summary>
            /// 数字运算验证码
            /// </summary>
            [Description("数字运算验证码")]
            Arith = 2,
        }

        /// <summary>
        ///计费方式(10:按件数 20:按重量)
        /// </summary>
        public enum DeliveryMethod
        {
            /// <summary>
            ///  按件数
            /// </summary>
            [Description("按件数")] PerUnit = 10,
            /// <summary>
            ///  按重量
            /// </summary>
            [Description("按重量")] PerWeight = 20
        }
        /// <summary>
        /// 快递公司
        /// </summary>
        public enum ExpressName
        {
            /// <summary>
            ///  快递鸟
            /// </summary>
            [Description("快递鸟")] Kdn = 10,
            /// <summary>
            ///  快递100
            /// </summary>
            [Description("快递100")] Kd100 = 20
        }

        /// <summary>
        /// 售后类型 (10:退货退款 40:仅退款)
        /// </summary>
        public enum RefundType
        {
            /// <summary>
            ///  退货退款
            /// </summary>
            [Description("退货退款")] ReturnGoods = 10,

            /// <summary>
            ///  换货
            /// </summary>
            [Description("换货")] Exchanged = 20
        }



        /// <summary>
        ///  退货方式(10:上门取货 20:自行送回)
        /// </summary>
        public enum RefundReturnWay
        {
            /// <summary>
            /// 上门取货
            /// </summary>
            [Description("上门取货")] Visit = 10,
            /// <summary>
            /// 自行送回
            /// </summary>
            [Description("自行送回")] BackSelf = 20,
        }

        /// <summary>
        /// 类型(10:销售佣金 20:开通唯粉佣金 30:开通经销商佣金)
        /// </summary>
        public enum IncomeType
        {
            /// <summary>
            /// 销售佣金
            /// </summary>
            [Description("销售佣金")] Sales = 10,
            /// <summary>
            /// 开通唯粉佣金
            /// </summary>
            [Description("开通唯粉佣金")] OpenVip = 20,
            /// <summary>
            /// 开通经销商佣金
            /// </summary>
            [Description("开通经销商佣金")] OpenWbFamily = 30,
        }

        /// <summary>
        /// 支付方式(10:微信支付 20:余额支付 30:云闪付)
        /// </summary>
        public enum PayType
        {
            /// <summary>
            /// 微信支付
            /// </summary>
            [Description("微信支付")] Wx = 10,
            /// <summary>
            /// 余额支付
            /// </summary>
            [Description("余额支付")] Balance = 20,
           
            // [Description("云闪付")] YunShanFu = 30,
            // [Description("找人代付")] Friend = 50,
        }
      
        /// <summary>
        /// 消息类型10:公告;20:消息;
        /// </summary>
        public enum MessageType
        {
            [Description("公告")] PublicNotice = 10,
            [Description("消息")] Message = 20,
        }
        /// <summary>
        /// 消息子类型 10:平台消息;20:订单消息;
        /// </summary>
        public enum MessageSubType
        {
            [Description("平台消息")] MsgSys = 10,
            [Description("订单消息")] MsgOrder = 20,
           
        }
        
        /// <summary>
        /// 提成状态(-30：已取消 10:已下单 20:未结算 50:已计算)
        /// </summary>
        public enum IncomeStatus
        {
            /// <summary>
            /// 已取消
            /// </summary>
            [Description("已取消")] Cancel = -30,
            /// <summary>
            /// 已下单（订单未完成）
            /// </summary>
            [Description("已下单")] Paid = 10,
            /// <summary>
            /// 未结算（订单已完成）
            /// </summary>
            [Description("未结算")] CanIncome = 20,
            /// <summary>
            /// 已结算（销售佣金已结算到用户销售佣金余额）
            /// </summary>
            [Description("已结算")] Done = 50,
        }

        /// <summary>
        /// 用户提现状态（-10：提现失败 10：已申请  20：正在处理  50：提现成功）
        /// </summary>
        public enum DrawMoneyStatus
        {
            /// <summary>
            /// 提现失败
            /// </summary>
            [Description("提现失败")] Failure = -10,
            /// <summary>
            /// 已申请
            /// </summary>
            [Description("已申请")] Apply = 10,
            /// <summary>
            /// 正在处理
            /// </summary>
            [Description("正在处理")] DealWith = 20,
            /// <summary>
            /// 提现成功
            /// </summary>
            [Description("提现成功")] Success = 50,
            
        }           

        /// <summary>
        /// 邀请状态
        /// </summary>
        public enum InviteRecordStatus
        {
            [Description("已注册")] Reg = 10,
            [Description("已充Vip")] BuyVip = 20,
        }

        /// <summary>
        /// 支付状态(10:待支付 20:已支付)
        /// </summary>
        public enum PayStatus
        {
            [Description("待支付")] Unpaid = 10,
            [Description("已支付")] Paid = 20,
        }

        /// <summary>
        ///充值方式(10:自定义金额 20:套餐充值)
        /// </summary>
        public enum RechargeType
        {
            [Description("自定义金额")] Custom = 10,
            [Description("套餐充值")] Package = 20,
        }

        /// <summary>
        ///余额变动场景(10:用户充值 11:管理员充值 20:用户消费 21:管理员扣减 31:管理员设置余额 40:订单退款) 
        /// </summary>
        public enum BalanceType
        {
            [Description("用户充值")] Recharge = 10,
            [Description("管理员充值")] AdminRecharge = 11,
            [Description("用户消费")] ConsumeOrder = 20,
            [Description("管理员扣减")] AdminConsume = 21,
            [Description("管理员设置余额")] AdminOp = 31,
            [Description("订单退款")] Refund = 40,
            [Description("服务定金")] Deposit = 50,
            [Description("服务尾款")] Final = 60,
        }

        /// <summary>
        /// 消费金额变动
        /// </summary>
        public enum ConsumptionMoneyType
        {
            /// <summary>
            /// 取消订单
            /// </summary>
            [Description("取消订单")] Cancel = -30,
            /// <summary>
            /// 开通会员（唯粉，经销商）
            /// </summary>
            [Description("开通会员")] OpenMember = 10,
            /// <summary>
            /// 用户消费
            /// </summary>
            [Description("用户消费")] Consume = 20,

            /// <summary>
            /// 支付运费
            /// </summary>
            [Description("支付运费")] Freight = 30,
            /// <summary>
            /// 订单退款
            /// </summary>
            [Description("订单退款")] Refund = 40,
        }


        /// <summary>
        ///积分变动场景(10:订单得积分 20:下单抵扣 30:管理员操作 40:消费退款) 
        /// </summary>
        public enum PointType
        {
            [Description("不操作")] NotOp = 0,
            [Description("订单得积分")] OrderAdd = 10,
            [Description("下单抵扣")] ConsumeOrder = 20,
            [Description("消费退款")] Refund = 40,
        }
     
        /// <summary>
        /// 商品类型(10:自有商品 20:唯品尚普通商品 22:唯品尚特卖商品) 
        /// </summary>
        public enum GoodsType
        {
            [Description("自有商品")] Self = 10,
            [Description("普通")] Normal = 20,
            [Description("特卖")] Special = 22,
        }

        /// <summary>
        /// 库存计算方式(10:下单减库存 20:付款减库存) 
        /// </summary>
        public enum DeductStockType
        {
            [Description("下单减库存")] Order = 10,
            [Description("付款减库存")] Paid = 20,
        }  

        /// <summary>
        /// 运输类型（10:销售订单，20:退货订单 30:采购订单）
        /// </summary>
        public enum TransportType
        {
            /// <summary>
            /// 销售订单
            /// </summary>
            [Description("销售订单")] Order = 10,
        }
        
        /// <summary>
        /// 订单来源（10：小程序  20：ERP）
        /// </summary>
        public enum OrderSource
        {
            [Description("小程序")] APP = 10,
            [Description("EPR")] ERP = 20,
        }
        /// <summary>
        /// 订单大状态
        /// </summary>
        public enum OrderBigStatus
        {
            /// <summary>
            /// 待付款
            /// </summary>
            [Description("待支付")] NotPaid = 10,
            /// <summary>
            /// 待发货
            /// </summary>
            [Description("待发货")] WaitDeliver = 20,
            /// <summary>
            /// 待收货
            /// </summary>
            [Description("待收货")] WaitReceiving = 30,
            /// <summary>
            /// 待评价
            /// </summary>
            [Description("待评价")] WaitComment = 40,
            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")] Done = 80,
            /// <summary>
            /// 售后
            /// </summary>
            [Description("售后")] Refund = -10,
        }


        /// <summary>
        /// 订单状态(-30:已取消 10:待付款 30:待发货 40:待收货 50:已送达 60:已收货 80:已完成)
        /// </summary>
        public enum OrderStatus
        {
            /// <summary>
            /// 已取消
            /// </summary>
            [Description("已取消")] Cancel = -30,
            /// <summary>
            /// 申请取消
            /// </summary>
            [Description("申请取消")] ApplyCancel = -20,
            /// <summary>
            /// 待付款
            /// </summary>
            [Description("待付款")] NotPaid = 10,
            /// <summary>
            ///待发货 
            /// </summary>
            [Description("待发货")] WaitDeliver = 20,
            /// <summary>
            /// 待收货
            /// </summary>
            [Description("待收货")] WaitReceiving = 30,
            // /// <summary>
            // /// 待评价
            // /// </summary>
            // [Description("待评价")] WaitComment = 40,
          
            /// <summary>
            /// 已完成
            /// </summary>
            [Description("已完成")] Done = 80,
        }

        /// <summary>
        /// 订单跟跟踪类型
        /// </summary>
        public enum OrderTrackType
        {
            /// <summary>
            /// 订单
            /// </summary>
            [Description("订单")] Order = 10,
            /// <summary>
            /// 物流
            /// </summary>
            [Description("物流")] Express = 20,
            /// <summary>
            /// 售后
            /// </summary>
            [Description("售后")] Refund = 50
        }

        /// <summary>
        /// 售后状态(-10:审核拒绝 10:等待审核 20:审核通过 30:已发货 40:已收货 80:已退款)
        /// </summary>
        public enum RefundStatus
        {
            /// <summary>
            ///  审核拒绝
            /// </summary>
            [Description("审核拒绝")] UnApprove = -10,

            /// <summary>
            ///  待审核
            /// </summary>
            [Description("待审核")] WaitAudit = 10,

            /// <summary>
            /// 审核通过
            /// </summary>
            [Description("审核通过")] Approve = 20,

            /// <summary>
            /// 用户已发货
            /// </summary>
            [Description("用户已发货")] Shipped = 30,
            /// <summary>
            ///  已退款
            /// </summary>
            [Description("已退款")]
            RefundedMoney= 80,

           
        }

        /// <summary>
        /// 到期类型(10:领取后生效 20:固定时间)
        /// </summary>
        public enum ExpireType
        {
            [Description("领取天数")] Days = 10,
            [Description("固定时间")] FixedTime = 20,
        }

        /// <summary>
        /// 优惠券状态((-20:已使用 -10:已过期 10:可使用))
        /// </summary>
        public enum CouponStatus
        {
            [Description("已使用")] Used = -20,
            [Description("已过期")] Expired = -10,
            [Description("可使用")] Usable = 10,
        }

        /// <summary>
        /// 优惠券类型(10:满减券 20:折扣券)
        /// </summary>
        public enum CouponType
        {
            [Description("满减券")] FullFree = 10,
            [Description("折扣券")] Discount = 20,
        }

        /// <summary>
        /// 优惠券适用范围(10:全部商品 20:指定商品 30:排除商品)
        /// </summary>
        public enum  CouponRange
        {
            [Description("全部商品")] All = 10,
            [Description("指定商品")] Goods = 20,
            [Description("排除商品")] Exclude = 30,
        }

        /// <summary>
        /// 首页推荐类型(10:推荐产品 20:特价产品 30:热销产品)
        /// </summary>
        [Description("首页推荐类型")]
        public enum HomeRecommendType
        {    
            [Description("推荐产品")] Recommed = 10,
            [Description("特价产品")] SpecialPrice = 20 ,
            [Description("热销产品")] Hot = 30 ,
        }

        [Description("规格类型")]
        public enum SpecType
        {
            //状态(-10:禁用 10:正常)
            [Description("单规格")] One = 10,
            [Description("多规格")] More = 20
        }

        [Description("通用状态(禁用/正常)")]
        public enum ComStatus
        {
            //状态(-10:禁用 10:正常)
            [Description("禁用")] Disable = -10,
            [Description("正常")] Normal = 10
        }
       
       
        
        /// <summary>
        ///     行政区划等级
        /// </summary>
        public enum DivisionLevel
        {
            /// <summary>
            ///     省
            /// </summary>                                                                                  PersonalArtLive
            [Description("省")] Province = 1,

            /// <summary>
            ///     市
            /// </summary>
            [Description("市")] City = 2,

            /// <summary>
            ///     区县
            /// </summary>
            [Description("区县")] District = 3,

            /// <summary>
            ///     乡镇
            /// </summary>
            [Description("乡镇")] Town = 4,

            /// <summary>
            ///     村社区
            /// </summary>
            [Description("村社区")] Village = 5
        }

        /// <summary>
        /// 日志类型 (10:操作日志 20:接口日志 30:错误日志)
        /// </summary>
        [Description("日志类型")]
        public enum LogType
        { 
            [Description("操作日志")] Op = 10,
            [Description("警告")] Api = 20,
            [Description("错误日志")] Error =30
        }
        /// <summary>
        /// 日志级别(20:消息 30:警告 40:错误 50:严重错误)
        /// </summary>
        [Description("日志级别")]
        public enum LogLevel
        {
            [Description("消息")] Info = 20,
            [Description("警告")] Warn = 30,
            [Description("错误")] Error = 40,
            [Description("重大错误")] Fatal = 50
        }

        /// <summary>
        ///   商品状态
        /// </summary>
        [Description("商品状态")]
        public enum GoodsStatus
        {
            [Description("下架")] NotOn = -10, 
            [Description("上架")] OnSale = 10,
            [Description("已开售")] AvailableSale = 20,
        }
        /// <summary>
        ///   评论状态 (10:好评 20:中评 30:差评)
        /// </summary>
        [Description("评论状态")]
        public enum CommentType
        {
            [Description("好评")] Praise = 10,
            [Description("中评")] Middle = 20,
            [Description("差评")] Bad = 30,
        }

        /// <summary>
        /// Token状态
        /// </summary>
        [Description("Token状态")]
        public enum TokenStatus
        {
            /// <summary>
            ///正常
            /// </summary>
            [Description("正常")] Normal = 200,

            /// <summary>
            /// 未登录
            /// </summary>
            [Description("未登录")] LogOut = 401,

            /// <summary>
            ///未绑定手机
            /// </summary>
            [Description("未绑定手机")] UnboundPhone = 402
        } 

        [Description("商品标签")]
        public enum GoodsLabel                                             
        {
            [Description("推荐产品")] Recommend = 10,
            [Description("特价产品")] Special = 20,
            [Description("热销产品")] Hot = 30
        }
     
        [Description("用户状态")]                                   
        public enum UserStatus
        {
            [Description("已拒绝")] Refused = -20,
            [Description("无效")] Disable = -10,
            [Description("正常")] Normal = 10,
        }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Description("用户类型")]
        public enum UserType
        {
            // 用户类型 10:店铺管理员 20:店铺管理员 100:用户 150:分销代理 
            /// <summary>
            ///   店铺管理员
            /// </summary>
            [Description("店铺管理员")] StoreAdmin = 10,

            /// <summary>
            ///    平台管理员
            /// </summary>
            [Description("平台管理员")] SysAdmin = 20,

            /// <summary>
            ///  用户
            /// </summary>
            [Description("用户")] Customer = 100,
            /// <summary>
            ///  分销代理
            /// </summary>
            [Description("分销代理")]Agent = 150,

        }

        [Description("类型(10:平台 20:店铺)")]
        public enum RoleType
        {
            /// <summary>
            /// 平台
            /// </summary>
            [Description("平台")] SysAdmin = 10,
            /// <summary>
            ///   店铺管理员
            /// </summary>
            [Description("店铺")] StoreAdmin = 20,

        }
        

      [Description("是否")]
        public enum YesOrNo
        {
            [Description("是")] Yes = 1,
            [Description("否")] No = 0
        }  

        [Description("文件类型")]
        public enum FileType
        {
            [Description("图片")] Image = 10,
            [Description("附件")] Annex = 20,
            [Description("视频")] Video = 30,
            [Description("支付证书")] PayCert = 80,
        } 

        #region 枚举常用操作

        /// <summary>
        ///     根据数值获取枚举描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string GetEnumDescription(Type enumType, int? value)
        {
            var description = "";
            var cnNamedic = new Dictionary<string, string>();
            var fieldinfos = enumType.GetFields();
            foreach (var field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    if (value == (int) enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null))
                    {
                        var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        description = ((DescriptionAttribute) objs[0]).Description;
                    }
                }
            }

            return description;
        }

        /// <summary>
        ///     根据数值获取枚举描述
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<string> ListEnumDescription(Type enumType, List<int> listValue)
        {
            var list = new List<string>();
            var cnNamedic = new Dictionary<string, string>();
            var fieldinfos = enumType.GetFields();
            foreach (var value in listValue)
            {
                var description = "无";
                foreach (var field in fieldinfos)
                {
                    if (field.FieldType.IsEnum)
                    {
                        if (value == (int) enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null))
                        {
                            var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                            description = ((DescriptionAttribute) objs[0]).Description;
                        }
                    }
                }

                list.Add(description);
            }

            return list;
        }

        /// <summary>
        ///     获取枚举名称和值
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static List<EnumEntity> ListEnumModel(Type enumType,bool isAddAll=false)
        {
            var list = new List<EnumEntity>();
            if (isAddAll)
            {
                list.Add(new EnumEntity(){EnName = "All",Text = "全部",Value = 0});
            }
            var fieldinfos = enumType.GetFields();
            foreach (var field in fieldinfos)
            {
                if (field.FieldType.IsEnum)
                {
                    var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    var model = new EnumEntity();
                    model.Value = (int) enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                    model.EnName = field.Name;
                    model.Text = ((DescriptionAttribute) objs[0]).Description;
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        ///  获取枚举英文名
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string GetEnName(Type enumType,int value)
        {
            return ListEnumModel(enumType).FirstOrDefault(p => p.Value == value)?.EnName;
             
        }

        /// <summary>
        ///     获取所有描述
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> GetEnumDescriptionItems<T>()
        {
            var obj = default(T);
            var type = obj.GetType();
            return type.GetEnumNames().Select(s => Enum.Parse(typeof(T), s, true)).ToDictionary<object, int, string>(
                enumType => Convert.ToInt16((T) enumType), enumType => GetDescriptionFromValue((T) enumType));
        }

        /// <summary>
        ///     根据枚举值获取对应的枚举注释
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <returns></returns>
        public static string GetDescriptionFromValue<T>(T enumValue)
        {
            var type = typeof(T);
            return GetDescriptionFromValue(type, enumValue);
        }

        private static string GetDescriptionFromValue(Type type, object enumValue)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            var field = type.GetField(enumValue.ToString());

            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                as DescriptionAttribute;

            return attribute != null ? attribute.Description : enumValue.ToString();
        }

        /// <summary>
        /// 获取枚举项上的<see cref="DescriptionAttribute"/>特性的文字描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(Enum enumValue)
        {
            string value = enumValue.ToString();
            FieldInfo field = enumValue.GetType().GetField(value);
            object[] objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);    //获取描述属性
            if (objs.Length == 0)    //当描述属性没有时，直接返回名称
                return value;
            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)objs[0];
            return descriptionAttribute.Description;
        }


        /// <summary>
        /// 根据枚举名称获取枚举列表
        /// </summary>
        /// <param name="enumName">枚举名称</param>
        /// <returns></returns>
        public static List<EnumEntity> GetEnumListByName(string enumName,bool isAddAll= false)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var enumInfo = assembly.CreateInstance($"Qs.Comm.xEnum+{enumName}", false);
            if (enumInfo == null) return new List<EnumEntity>();
            var enumType = enumInfo.GetType();
            var enums = Enum.GetValues(enumType);
            List<EnumEntity> list = new List<EnumEntity>();
            
            foreach (Enum item in enums)
            {
                list.Add(new EnumEntity()
                {
                    EnName = item.ToString(),
                    Value = Convert.ToInt32(item),
                    Text = xEnum.ToDescription(item)
                });
            }
            list= list.OrderBy(p => p.Value).ToList();
            if (isAddAll)
            {
                list.Insert(0,new EnumEntity() { EnName = "All", Text = "全部", Value = 0 });
            }
            return list.ToList();
        }

        #endregion
    }

    [Description("枚举实体")]
    public class EnumEntity
    {
        /// <summary>
        ///     枚举项英文名
        /// </summary>
        public string EnName { get; set; }

        /// <summary>
        ///     枚举项值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        ///     枚举项中文名
        /// </summary>
        public string Text { get; set; }
    }
}
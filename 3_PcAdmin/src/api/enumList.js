import * as comm from '@/api/commApi'
export default {
    //店铺状态
    getListStoreStatus(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'StoreStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },

    //通用状态
    getComStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'ComStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //商品状态
    getGoodsStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'GoodsStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //首页推荐类型
    getHomeRecommendTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'HomeRecommendType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //库存计算方式
    getDeductStockTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'DeductStockType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //订单状态
    getListOrderStatus(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'OrderStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //支付类型
    getPayTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'PayType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //配送方式
    getDeliveryTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'DeliveryType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //订单类型
    getOrderTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'OrderType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //优惠券适用范围
    getCouponRangeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'CouponRange', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //优惠券到期类型
    getExpireTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'ExpireType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //退货类型
    getRefundTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'RefundType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //类型(销售佣金 开卡佣金)
    getIncomeTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'IncomeType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //积分商品类型
    getPointGoodsTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'PointGoodsType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //消息类型
    getMessageTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'MessageType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //消息子类型
    getMessageSubTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'MessageSubType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },

    //地址类型
    getAddressTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'AddressType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //邀请状态
    getInviteRecordStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'InviteRecordStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //提成状态
    getIncomeStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'IncomeStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //支付状态
    getPayStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'PayStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //充值方式
    getRechargeTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'RechargeType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //余额变动场景
    getBalanceTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'BalanceType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },

    //积分变动场景
    getointTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'ointType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //积分订单状态
    getPointStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'PointStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //拍照订单状态
    getPhotoOrderStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'PhotoOrderStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //优惠券状态
    getCouponStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'CouponStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //行政区划等级
    getDivisionLevelList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'DivisionLevel', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //日志类型
    getLogTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'LogType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //日志级别
    getLogLevelList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'LogLevel', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //
    //自提点类型
    getTsTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'TsType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //商品标签
    getGoodsLabelList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'GoodsLabel', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //用户代码状态
    getUserCodeStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'UserCodeStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //用户性别
    getUserSexList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'UserSex', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //用户状态
    getUserStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'UserStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //用户类型
    getUserTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'UserType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //是否
    getYesOrNoList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'YesOrNo', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //售后状态
    getRefundStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'RefundStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //采购单状态
    getStorageOrderStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'StorageOrderStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //文章状态
    getArticleStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'ArticleStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //采购支付方式
    getPurchasePayTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'PurchasePayType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //运输类型
    getTransportTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'TransportType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //运输状态
    getStatusOrderCarList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'StatusOrderCar', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },

    //商品类型（10:销售商品 20:服务商品）
    getGoodsTypeList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'GoodsType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //服务订单状态
    getServiceOrderStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'ServiceOrderStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //服务订单售后状态
    getServiceOrderRefundStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'ServiceOrderStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //提现申请状态
    getDrawMoneyStatusList(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'DrawMoneyStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //申请代理状态
    listApplyAgentStatus(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'ApplyAgentStatus', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    },
    //申请代理状态
    listCommentType(isAddAll = false) {
        return new Promise((resolve, reject) => {
            comm.ListEnum({ name: 'CommentType', isAddAll: isAddAll }).then(res => {
                resolve(res.result)
            })
        })
    }


}
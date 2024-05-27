import _ from 'lodash'
import CategoryModel from '../../common/model/Category'
import * as GoodsApi from '@/api/goods/goods'
import * as GradeApi from '@/api/user/usergrade'
import * as ServiceApi from '@/api/goods/goodsservice'
import * as DeliveryApi from '@/api/setting/delivery'

/**
 * 商品 model类
 * GoodsModel
 */
export default {

    // 当前商品ID
    goodsId: null,

    // 表单数据
    formData: {
        // 当前商品记录
        goods: {},
        // 分类列表
        categoryList: [],
        // 运费模板
        deliveryList: [],
        // 服务与承诺
        serviceList: [],
        defaultServiceIds: [],
        // 会员等级列表
        userGradeList: [],
        defaultUserGradeValue: {}
    },

    // 获取form所需的数据
    getFromData(goodsId = null) {
        // 记录商品ID (编辑时)
        this.goodsId = goodsId
        return new Promise((resolve, reject) => {
            Promise.all([
                // 获取分类列表
                this.getCategoryList(),
                // 获取商品详情信息(编辑时)
                this.getGoodsDetail(goodsId),
                // 获取运费模板列表
                // this.getDeliveryList(),
                // 获取服务与承诺
                this.getServiceList(),
                // 获取会员等级列表
                // this.getUserGradeList()
            ]).then(() => {
                // 设置默认数据
                this.setDefaultData()
                resolve({ formData: this.formData })
            })
        })
    },

    // 获取商品详情
    getGoodsDetail(goodsId = null) {
        if (!goodsId) return false
        return new Promise((resolve, reject) => {
            GoodsApi.get({ id: goodsId })
                .then(res => {
                    const categorylist = [];
                    // res.result.categoryId.forEach(p => {
                    //     categorylist.push(this.formData.categoryList.find(o => o.key == p));
                    // });
                    //是否设置首页推荐类型

                    //状态为已开售在编辑中为已上架(开售由已开售时间决定)
                    res.result.status = res.result.status == 20 ? 10 : res.result.status
                        // res.result.detailPics = res.result.detailPics.split(',').map(el => el.trim()).filter(item => item.trim() != '')
                        // res.result.categoryId = categorylist
                    this.formData.goods = res.result
                    resolve()
                })
        })
    },
    // 获取表单默认值(用于form.setFieldsValue的数据)
    getFieldsValue() {
        // 商品详情信息
        const goodsInfo = this.formData.goods
            // 格式化categoryIds
            // goodsInfo.categoryId = this.formatCategoryIds(goodsInfo.categoryId)
            // 商品基本数据
        const goodsFormData = _.pick(goodsInfo, [
                'goodsName', 'goodsNo', 'categoryId', 'sort', 'storeId',
                'recommendType', 'isRecommend',
                'sellStartTime',
                'purchasePriceMin', 'purchasePriceMax',
                'stockTotal', 'goodsWeight',
                'deliveryId', 'status', 'specType', 'deductStockType',
                'subTitle', 'listServiceId', 'salesInitial', 'isPointsGift',
                'isPointsDiscount', 'entryPrice', 'floorPrice', 'goodsType'
                // 'isEnableGrade', 'isAloneGrade'
            ])
            // 单规格数据
        const skuOne = _.pick(goodsInfo.listSku[0], ['afterSaleDeadline', 'unit', 'salePrice', 'linePrice', 'commission1', 'commission2', 'commission3', 'goodsWeight'])
        return {
            ...goodsFormData,
            ...skuOne
        }
    },

    /**
     * 格式化categoryIds (用于表单元素选中)
     * @param {*} categoryId
     */
    formatCategoryIds(categoryId) {
        return categoryId.map(id => { return { value: id } })
    },

    // 获取分类列表
    getCategoryList() {
        return new Promise((resolve, reject) => {
            CategoryModel.getCategoryTreeSelect()
                .then(list => {
                    this.formData.categoryList = list
                    resolve()
                })
        })
    },

    // 获取运费模板列表
    // getDeliveryList() {
    //     return new Promise((resolve, reject) => {
    //         DeliveryApi.listByWhere()
    //             .then(res => {
    //                 this.formData.deliveryList = res.result
    //                 resolve()
    //             })
    //     })
    // },

    // 获取服务与承诺
    getServiceList() {
        return new Promise((resolve, reject) => {
            ServiceApi.listByWhere()
                .then(res => {
                    this.formData.serviceList = res.result
                    resolve()
                })
        })
    },

    // 获取会员等级列表
    getUserGradeList() {
        return new Promise((resolve, reject) => {
            GradeApi.listByWhere({ onlyMy: false })
                .then(res => {
                    this.formData.userGradeList = res.result
                    console.log('this.formData.userGradeList ', this.formData.userGradeList)
                    resolve()
                })
        })
    },

    // 设置默认的数据(无法用于form.setFieldsValue的数据)
    setDefaultData() {
        // 默认的商品服务与承诺
        this.setDefaultServiceIds()
            // 默认的等级折扣
        this.setDefaultUserGradeValue()
    },

    // 默认的商品服务与承诺
    setDefaultServiceIds() {
        // 服务与承诺列表
        const serviceList = this.formData.serviceList
        if (!this.goodsId) {
            // 默认选中的id集
            const defaultServiceItems = serviceList.filter(item => item.isDefault)
            this.formData.defaultServiceIds = defaultServiceItems.map(item => item.id)
        }
    },

    // 默认的等级折扣
    setDefaultUserGradeValue() {
        // 会员等级列表
        const userGradeList = this.formData.userGradeList
            // 单独设置折扣的配置 (已保存的)
        const aloneGradeEquity = (this.goodsId && this.formData.goods.aloneGradeEquity) ?
            this.formData.goods.aloneGradeEquity : {}

        // 生成的默认数据
        const defaultData = {}

        userGradeList.forEach(item => {
            var discount = JSON.parse(item.equity).discount;
            defaultData[item.id] = aloneGradeEquity[item.id] || discount
        })
        this.formData.defaultUserGradeValue = {...defaultData }
    }

}
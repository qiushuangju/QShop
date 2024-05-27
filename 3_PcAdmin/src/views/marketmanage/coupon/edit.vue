<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit">
                <a-form-item label="优惠券名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input placeholder="请输入优惠券名称" v-decorator="['name', { rules: [{ required: true, min: 2, message: '请输入至少2个字符' }] }]" />
                </a-form-item>
                <a-form-item label="发放方式" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['sendType', { initialValue: 10, rules: [{ required: true }] }]">
                        <a-radio :value="10">用户领取</a-radio>
                        <!-- <a-radio :value="20">系统发放</a-radio> -->
                    </a-radio-group>
                </a-form-item>
                <a-form-item v-show="form.getFieldValue('sendType') == 10" label="减免金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input-number :min="0.01" :precision="2" v-decorator="['reducePrice', { rules: [{ required: true, message: '请输入减免金额' }] }]" />
                    <span class="ml-5">元</span>
                </a-form-item>
                <!-- <a-form-item
          v-show="form.getFieldValue('sendType') == sendTypeEnum.DISCOUNT.value"
          label="折扣率"
          :labelCol="labelCol"
          :wrapperCol="wrapperCol"
        >
          <a-input-number
            :min="0"
            :max="9.9"
            :precision="1"
            v-decorator="['discount', { initialValue: 9.9, rules: [{ required: true, message: '请输入折扣率' }] }]"
          />
          <span class="ml-5">%</span>
          <p class="form-item-help">
            <small>折扣率范围 0-9.9，8代表打8折，0代表不折扣</small>
          </p>
        </a-form-item> -->
                <a-form-item label="最低消费金额" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input-number :min="0.01" :precision="2" v-decorator="['minPrice', { rules: [{ required: true, message: '请输入最低消费金额' }] }]" />
                    <span class="ml-5">元</span>
                </a-form-item>
                <a-form-item label="到期类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['expireType', { initialValue: 10, rules: [{ required: true }] }]">
                        <a-radio v-for="item in ExpireTypeList" :key="item.value" :value="item.value">{{item.text}}</a-radio>
                    </a-radio-group>
                    <a-form-item v-show="form.getFieldValue('expireType') == 10" class="expire_type-10">
                        <InputNumberGroup addonBefore="有效期" addonAfter="天" :inputProps="{ min: 1, precision: 0 }" v-decorator="['expireDay', { initialValue: 7}]" />
                    </a-form-item>
                    <a-form-item v-show="form.getFieldValue('expireType') == 20" class="expire_type-20">
                        <a-range-picker format="YYYY-MM-DD" v-decorator="['betweenTime', { initialValue: defaultDate}]" />
                    </a-form-item>
                </a-form-item>
                <a-form-item label="券适用范围" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['applyRange', { initialValue: 10, rules: [{ required: true }] }]">
                        <a-radio v-for="item in CouponRangeList" :key="item.value" :value="item.value">{{item.text}}</a-radio>
                    </a-radio-group>
                    <a-form-item v-if="form.getFieldValue('applyRange') == 20||form.getFieldValue('applyRange') == 30">
                        <SelectGoods :defaultList="containGoodsList" v-decorator="['applyRangeConfig']" />
                    </a-form-item>
                </a-form-item>
                <a-form-item label="发放总数量" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input-number :min="-1" :precision="0" v-decorator="['totalNum', { initialValue: -1, rules: [{ required: true, message: '请输入发放总数量' }] }]" />
                    <span class="ml-5">张</span>
                    <p class="form-item-help">
                        <small>发放的优惠券总数量，-1为不限制</small>
                    </p>
                </a-form-item>
                <a-form-item label="限领次数" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-input-number :min="-1" :precision="0" v-decorator="['limitQuantity', { initialValue: 1, rules: [{ required: true, message: '请输入限领次数' }] }]" />
                    <span class="ml-5">张</span>
                </a-form-item>
                <a-form-item label="状态" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['status', { initialValue: 10, rules: [{ required: true,message: '请选择状态'  }] }]">
                        <a-radio v-for="item in ComStatusList" :key="item.value" :value="item.value">{{item.text}}</a-radio>
                    </a-radio-group>
                    <!-- <p class="form-item-help">
            <small>如果设为隐藏将不会展示在用户端页面</small>
          </p> -->
                </a-form-item>
                <a-form-item label="优惠券描述" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-textarea :autoSize="{ minRows: 4 }" v-decorator="['describe', {rules: [{ required: true,message: '请输入优惠券描述'}] }]" />
                </a-form-item>
                <a-form-item label="排序" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="数字越小越靠前">
                    <a-input-number :min="0" v-decorator="['sort', {initialValue: 100, rules:[{required: true, message: '请输入排序值'}]}]" />
                </a-form-item>
                <a-form-item label="首页弹出" :labelCol="labelCol" :wrapperCol="wrapperCol">
                    <a-radio-group v-decorator="['isShowHome', {initialValue: -10}]" @change="onForceUpdate()">
                        <a-radio :value="-10">否</a-radio>
                        <a-radio :value="10">是</a-radio>
                    </a-radio-group>
                </a-form-item>
                <div v-show="form.getFieldValue('isShowHome') == 10">
                    <a-form-item label="弹出图片" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议尺寸：750*750像素">
                        <SelectImage :maxNum="1" :defaultList="defaultImgList" v-decorator="['showImg']" />
                    </a-form-item>
                </div>
                <a-form-item class="mt-20" :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
                    <a-button type="primary" html-type="submit" :loading="isBtnLoading">提交</a-button>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-card>
</template>

<script>
import moment from 'moment'
import { pick, get } from 'lodash'
import enums from '@/api/enumList'
import * as coupon from '@/api/marketManage/coupon'
import * as goods from '@/api/goods/goods'
import { isEmpty } from '@/utils/util'
import { InputNumberGroup, SelectGoods, SelectImage } from '@/components'
import { SendTypeEnum } from '@/common/enum/coupon'
import { async } from 'q'

export default {
    components: {
        SelectGoods,
        InputNumberGroup,
        SelectImage,
    },
    data() {
        return {
            // 枚举类
            SendTypeEnum,
            CouponRangeList: [],
            ExpireTypeList: [],
            ComStatusList: [], //状态
            // 正在加载
            isLoading: false,
            isBtnLoading: false,
            // 标签布局属性
            labelCol: { span: 3 },
            // 输入框布局属性
            wrapperCol: { span: 10 },
            // 当前表单元素
            form: this.$form.createForm(this),
            // 默认日期范围
            defaultDate: [moment(), moment()],
            // 优惠券ID
            couponId: null,
            // 当前记录
            record: {},
            // 适用范围：指定的商品
            containGoodsList: [],
            //商城列表
            storeList: [],
            defaultImgList: [], //弹出图片
        }
    },
    created() {
        new Promise((resolve, reject) => {
            Promise.all([
                enums.getCouponRangeList().then((res) => {
                    this.CouponRangeList = res
                }),
                enums.getExpireTypeList().then((res) => {
                    this.ExpireTypeList = res
                }),
                enums.getComStatusList().then((res) => {
                    this.ComStatusList = res
                }),
            ]).then(() => {
                // 记录优惠券ID
                this.couponId = this.$route.params && this.$route.params.id

                console.log("1234561213",this.couponId )
                //获取所有商城
                this.getStoreList()
                // 获取当前记录
                if (this.couponId != null && this.couponId.trim() != '')
                    this.getDetail()
            })
        })
    },
    methods: {
        getStoreList() {},
        // 手动强制更新页面
        onForceUpdate(bool = false) {
            this.$forceUpdate()
            // bool为true时再执行一次 $forceUpdate, 特殊情况下需执行两次，原因如下：
            // 第一次执行 $forceUpdate时, 新元素绑定v-decorator无法获取到form.getFieldValue
            bool &&
                setTimeout(() => {
                    this.$forceUpdate()
                }, 10)
        },
        // 获取当前记录
        getDetail() {
            console.log("1111",this.couponId)
            this.isLoading = true
            coupon
                .get({ id: this.couponId })
                .then((res) => {
                    // 当前记录
                    if (
                        res.result.applyRangeConfig != null &&
                        res.result.applyRangeConfig != ''
                    )
                        res.result.applyRangeConfig =
                        res.result.applyRangeConfig.split(',')
                    this.record = res.result
                    if (
                        res.result.showImg != null &&
                        res.result.showImg != ''
                    ) {
                        this.defaultImgList.push({
                            id: res.result.showImg,
                            thumbnail: res.result.urlHomeImg,
                        })
                    }
                    // 设置表单默认值
                    this.setFieldsValue()
                    // 获取指定的商品列表
                    this.getContainGoodsList()
                })
                .finally(() => {
                    this.isLoading = false
                })
        },

        // 获取指定的商品列表
        getContainGoodsList() {
            this.isLoading = true
            const ids =
                this.record.applyRangeConfig == null
                    ? '0'
                    : this.record.applyRangeConfig.join(',')
            goods
                .listByWhere({ goodsIds: ids })
                .then((res) => {
                    this.containGoodsList = res.result
                })
                .finally((res) => {
                    this.isLoading = false
                })
        },

        // 设置表单默认值
        setFieldsValue() {
            const { record, form, $nextTick } = this
            // 设置表单内容
            !isEmpty(form.getFieldsValue()) &&
                $nextTick(() => {
                    // 表单数据
                    const data = pick(record, [
                        'name',
                        'sendType',
                        'storeId',
                        'reducePrice',
                        'discount',
                        'minPrice',
                        'status',
                        'limitQuantity',
                        'expireType',
                        'expireDay',
                        'applyRange',
                        'totalNum',
                        'describe',
                        'sort',
                        'isShowHome',
                    ])
                    // 时间范围
                    data.betweenTime = this.getBetweenTime(record)
                    form.setFieldsValue(data)
                })
        },

        // 格式化时间范围
        getBetweenTime(record) {
            if (record.expireType === 20) {
                return [
                    moment(new Date(record.startTime)),
                    moment(new Date(record.endTime)),
                ]
            }
            return this.defaultDate
        },

        // 确认按钮
        handleSubmit(e) {
            e.preventDefault()
            // 表单验证
            const {
                form: { validateFields },
                onFormSubmit,
            } = this
            validateFields((errors, values) => {
                if (!errors) {
                    if (
                        values.applyRangeConfig != undefined &&
                        values.applyRangeConfig != 'undefined'
                    )
                        values.applyRangeConfig =
                            values.applyRangeConfig.join(',')
                    if (this.couponId != null && this.couponId.trim() != '') {
                        values.id = this.couponId
                        values.createTime = this.record.createTime
                    }
                    for (var i = 0; i < values.betweenTime.length; i++) {
                        switch (i) {
                            case 0:
                                values.startTime = values.betweenTime[i].format(
                                    'YYYY-MM-DD HH:mm:ss'
                                )
                                break
                            case 1:
                                values.endTime = values.betweenTime[i].format(
                                    'YYYY-MM-DD HH:mm:ss'
                                )
                                break
                        }
                    }
                    if (
                        values.isShowHome == 10 &&
                        (values.showImg == undefined || values.showImg == '')
                    ) {
                        this.$message.error('请选择弹出图片')
                        return
                    }
                    !errors && onFormSubmit(values)
                }
            })
        },

        /**
         * 提交到后端api
         */
        onFormSubmit(values) {
            this.isLoading = true
            this.isBtnLoading = true
            coupon
                .addOrUpdate(values)
                .then((result) => {
                    // 显示提示信息
                    this.$message.success(result.message, 1.5)
                    // 跳转到列表页
                    setTimeout(() => {
                        this.$store.state.tagsView.visitedViews.splice(
                            this.$store.state.tagsView.visitedViews.findIndex(
                                (item) => item.path === this.$route.path
                            ),
                            1
                        )
                        this.$router.push(
                            this.$store.state.tagsView.visitedViews[
                                this.$store.state.tagsView.visitedViews.length -
                                    1
                            ].path
                        )
                    }, 1500)
                })
                .catch(() => {
                    this.isBtnLoading = false
                })
                .finally((result) => {
                    this.isLoading = false
                })
        },
    },
}
</script>
<style lang="less" scoped>
.ant-form-item {
    .ant-form-item {
        margin-bottom: 0;
    }
}
</style>

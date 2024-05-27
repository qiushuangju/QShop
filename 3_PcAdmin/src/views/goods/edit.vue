<template>
    <a-card :bordered="false">
        <div class="card-title">{{ $route.meta.title }}</div>
        <a-spin :spinning="isLoading">
            <a-form :form="form" @submit="handleSubmit" :selfUpdate="true">
                <a-tabs :activeKey="tabKey" :tabBarStyle="{marginBottom: '30px'}" @change="handleTabs">
                    <a-tab-pane :key="0" tab="基本信息"></a-tab-pane>
                    <a-tab-pane :key="1" tab="规格/库存"></a-tab-pane>
                    <a-tab-pane :key="2" tab="商品详情"></a-tab-pane>
                    <a-tab-pane :key="3" tab="更多设置"></a-tab-pane>
                </a-tabs>
                <div class="tabs-content">
                    <!-- 基本信息 -->
                    <div class="tab-pane" v-show="tabKey == 0">
                        <div v-show="form.getFieldValue('isRecommend') == true">
                            <a-form-item label="首页推荐类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
                                <a-select style="width: 300px" v-decorator="['recommendType']" placeholder="请选择首页推荐类型">
                                    <a-select-option v-for="item in HomeRecommendType" :key="item.value" :value="item.value">{{ item.text }}</a-select-option>
                                </a-select>
                            </a-form-item>
                        </div>
                        <a-form-item label="商品名称" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-input placeholder="请输入商品名称" v-decorator="['goodsName', { rules: [{ required: true, min: 2, message: '请输入至少2个字符' }] }]" />
                        </a-form-item>
                        <a-form-item label="商品分类" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-tree-select placeholder="请选择商品分类" :dropdownStyle="{ maxHeight: '500px', overflow: 'auto' }" :treeData="formData.categoryList" allowClear
                                treeDefaultExpandAll v-decorator="['categoryId', { rules: [{required: true, message: '请至少选择1个商品分类'}]}]"></a-tree-select>
                            <div class="form-item-help">
                                <router-link target="_blank" :to="{ path: '/goodscate/index' }">去新增</router-link>
                                <a href="javascript:;" @click="onReloadCategoryList">刷新</a>
                            </div>
                        </a-form-item>
                        <a-form-item label="商品图片" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议尺寸：750*750像素, 最多上传10张, 可拖拽图片调整顺序, 第1张将作为商品主图">
                            <SelectImage multiple :maxNum="10" :defaultList="formData.goods.listImg"
                                v-decorator="['imagesIds', { rules: [{ required: true, message: '请至少上传1张商品图片' }] }]" />
                        </a-form-item>

                        <a-form-item label="商品状态" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-radio-group v-decorator="['status', {initialValue: 10, rules: [{ required: true }]}]">
                                <a-radio v-for="item in GoodsStatusList" :key="item.value" :value="item.value">{{item.text}}</a-radio>
                            </a-radio-group>
                        </a-form-item>

                        <a-form-item label="运费模板" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-select style="width: 300px" v-decorator="['deliveryId', {rules: [{required: true, message: '请选择运费模板'}]}]" placeholder="请选择运费模板">
                                <a-select-option v-for="(item, index) in formData.deliveryList" :key="index" :value="item.id">{{ item.name }}</a-select-option>
                            </a-select>
                            <div class="form-item-help">
                                <router-link target="_blank" :to="{ path: '/setting/delivery/template/create' }">新增模板</router-link>
                                <a href="javascript:;" @click="onReloadDeliveryList">刷新</a>
                            </div>
                        </a-form-item>

                        <a-form-item label="商品编码" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-input placeholder="请输入商品编码" v-decorator="['goodsNo',{ }]" />
                        </a-form-item>

                        <a-form-item label="商品排序" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="数字越小越靠前">
                            <a-input-number :min="0" v-decorator="['sort', {initialValue: 100, rules:[{ required: true,message: '请设置排序' }]}]" />
                        </a-form-item>
                    </div>

                    <!-- 规格/库存 -->
                    <div class="tab-pane" v-show="tabKey == 1">
                        <a-form-item label="规格类型" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-radio-group v-decorator="['specType', {initialValue: 10, rules: [{ required: true }]}]" @change="onForceUpdate()">
                                <a-radio :value="10">单规格</a-radio>
                                <a-radio :value="20">多规格</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <!-- 多规格的表单内容 -->
                        <div v-show="form.getFieldValue('specType') == 20">
                            <MultiSpec ref="MultiSpec" :defaultSpecList="formData.goods.specList" :defaultSkuList="formData.goods.listSku" />
                        </div>
                        <!-- 单规格的表单内容 -->
                        <div v-show="form.getFieldValue('specType') == 10">
                            <a-form-item label="售价" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="商品的售价格，最低0.01">
                                <a-input-number :min="0.01" :precision="2"
                                    v-decorator="['salePrice', {initialValue: 1, rules:[{ required: true, message: '请输入价格' }] }]" />
                                <span class="ml-10">元</span>
                            </a-form-item>
                            <a-form-item label="当前库存数量" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="商品的实际库存数量，为0时用户无法下单">
                                <a-input-number :min="0" :precision="0"
                                    v-decorator="['stockTotal', { initialValue: 1, rules:[{ required: true, message: '请输入库存数量' }] }]" />
                                <span class="ml-10">件</span>
                            </a-form-item>
                            <a-form-item label="商品重量" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="商品的实际重量，用于计算运费">
                                <a-input-number :min="0.1" :precision="2"
                                    v-decorator="['goodsWeight', { initialValue: 0, rules:[{ required: true, message: '请输入库存数量' }] }]" />
                                <span class="ml-10">千克 (Kg)</span>
                            </a-form-item>
                            <a-form-item label="划线价" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="商品的划线价格,为0不显示">
                                <a-input-number :min="0.00" :precision="2" v-decorator="['linePrice', {initialValue: 0 }]" />
                                <span class="ml-10">元</span>
                            </a-form-item>
                            <a-form-item label="一级提成" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="注:一级提成为顾客的直接上级(如果开启分销内购,分销商自己购买商品，享受一级提成)">
                                <a-input-number :min="0.00" :precision="2" v-decorator="['commission1', { initialValue: 0}]" />
                                <span class=" ml-10">元</span>
                            </a-form-item>
                            <a-form-item label="二级提成" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="注:二级为顾客的上上级">
                                <a-input-number :min="0.00" :precision="2" v-decorator="['commission2', { initialValue: 0}]" />
                                <span class="ml-10">元</span>
                            </a-form-item>
                            <a-form-item label="三级提成" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="注:三级为顾客的上上上级">
                                <a-input-number :min="0.00" :precision="2" v-decorator="['commission3', { initialValue: 0 }]" />
                                <span class="ml-10">元</span>
                            </a-form-item>
                            <a-form-item label="采购价" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="采购价格">
                                <a-input-number :min="0.01" :precision="2" v-decorator="['purchasePrice', {initialValue: 0, rules:[] }]" />
                                <span class="ml-10">元</span>
                            </a-form-item>
                        </div>
                    </div>

                    <!-- 商品详情 -->
                    <div class="tab-pane" v-show="tabKey == 2">
                        <!-- <a-form-item label="商品详情" :labelCol="labelCol" :wrapperCol="{span: 16}">
              <Ueditor v-decorator="['content', {rules: [{required: true, message: '商品详情不能为空'}]}]" />
            </a-form-item> -->
                        <a-row>
                            <a-col :span="12">
                                <!-- 11111111111 -->
                                <a-form-item label="商品详情" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议尺寸：750*750像素, 最多上传20张, 可拖拽图片调整顺序, 第1张将作为商品首图">
                                    <SelectImage multiple :maxNum="20" :defaultList="formData.goods.listImgDetail" @change="selectDetailImg"
                                        v-decorator="['listImgDetail', { rules: [{ required: true, message: '请至少上传1张商品图片' }] }]" />
                                </a-form-item>
                            </a-col>
                            <a-col :span="12">
                                <a-form-item label="详情预览" :labelCol="labelCol" :wrapperCol="{span: 16}">
                                    <quill-editor v-model="detailPicsContent" ref="myQuillEditor" :options="editorOption">
                                    </quill-editor>
                                </a-form-item>
                            </a-col>
                        </a-row>
                    </div>

                    <!-- 更多设置 -->
                    <div class="tab-pane" v-show="tabKey == 3">
                        <a-form-item label="主图视频" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议视频宽高比1:1，建议时长8-45秒">
                            <SelectVideo :multiple="false" :defaultList="formData.goods.urlVideo ? [formData.goods.urlVideo] : []" v-decorator="['videoId']" />
                        </a-form-item>
                        <a-form-item label="视频封面" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="建议尺寸：750像素*750像素">
                            <SelectImage :multiple="false" :defaultList="formData.goods.urlVideoCover ? [formData.goods.urlVideoCover] : []"
                                v-decorator="['videoCoverId']" />
                        </a-form-item>
                        <a-form-item label="商品卖点" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="一句话简述，例如：此款商品美观大方 性价比较高 不容错过">
                            <a-input placeholder="请输入小标题(卖点)" v-decorator="['subTitle']" />
                        </a-form-item>
                        <a-form-item label="服务与承诺" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-select v-if="formData.serviceList" mode="multiple" v-decorator="['listServiceId',{initialValue: formData.defaultServiceIds} ]"
                                placeholder="请选择服务与承诺">
                                <a-select-option v-for="(item, index) in formData.serviceList" :key="index" :value="item.id">{{ item.serviceName }}</a-select-option>
                            </a-select>
                        </a-form-item>
                        <a-form-item label="初始销量" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="用户端展示的销量 = 初始销量 + 实际销量">
                            <a-input-number v-decorator="['salesInitial', {initialValue: 0}]" />
                        </a-form-item>

                        <a-divider orientation="left">积分设置</a-divider>
                        <a-form-item label="积分赠送" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="开启后用户购买此商品将获得积分">
                            <a-radio-group v-decorator="['isPointsGift', {initialValue: 1, rules: [{ required: true }]}]">
                                <a-radio :value="1">开启</a-radio>
                                <a-radio :value="0">关闭</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item label="积分抵扣" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="开启后用户购买此商品可以使用积分进行抵扣">
                            <a-radio-group v-decorator="['isPointsDiscount', {initialValue: 1, rules: [{ required: true }]}]">
                                <a-radio :value="1">开启</a-radio>
                                <a-radio :value="0">关闭</a-radio>
                            </a-radio-group>
                        </a-form-item>

                        <!-- <a-divider orientation="left">会员折扣设置</a-divider>
                        <a-form-item label="会员折扣" :labelCol="labelCol" :wrapperCol="wrapperCol" extra="开启后会员折扣，会员购买此商品可以享受会员等级折扣价">
                            <a-radio-group v-decorator="['isEnableGrade', {initialValue: 1, rules: [{ required: true }]}]" @change="onForceUpdate(true)">
                                <a-radio :value="1">开启</a-radio>
                                <a-radio :value="0">关闭</a-radio>
                            </a-radio-group>
                        </a-form-item>
                        <a-form-item rm-item v-show="form.getFieldValue('isEnableGrade')" label="会员折扣设置" :labelCol="labelCol" :wrapperCol="wrapperCol">
                            <a-radio-group v-decorator="['isAloneGrade', {initialValue: 0, rules: [{ required: true }]}]" @change="onForceUpdate(true)">
                                <a-radio :value="0">默认等级折扣</a-radio>
                                <a-radio :value="1">单独设置折扣</a-radio>
                            </a-radio-group>
                            会员等级列表
                            <div v-show="form.getFieldValue('isAloneGrade')">
                                <a-form-item v-for="item in formData.userGradeList" :key="item.id">
                                    <InputNumberGroup :addonBefore="item.name" addonAfter="折" :inputProps="{ min: 0, max: 9.9 }" v-decorator="[`aloneGradeEquity[gradeId:${item.id}]`,
                                {initialValue: formData.defaultUserGradeValue[item.id], rules: [{required: true, message: '折扣率不能为空'}]
                    }]" />
                                </a-form-item>
                            </div>
                            <div class="form-item-help">
                                <p class="extra" v-if="form.getFieldValue('isAloneGrade')">单独折扣：折扣率范围0.0-9.9，例如: 9.8代表98折，0代表不折扣</p>
                                <p class="extra" v-else>默认折扣：默认为用户所属会员等级的折扣率</p>
                            </div>
                        </a-form-item> -->
                    </div>
                </div>

                <a-form-item class="mt-20" :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
                    <a-button type="primary" html-type="submit" :loading="isBtnLoading">提交</a-button>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-card>
</template>

<script>
import * as goodsApi from '@/api/goods/goods'
import * as deliveryApi from '@/api/setting/delivery'
import * as serviceApi from '@/api/goods/goodsservice'

import { SelectImage, SelectVideo } from '@/components'
import goodsIndex from '@/api/goods/Index'
import * as commApi from '@/api/commApi'
import MultiSpec from './modules/MultiSpec'
import { quillEditor } from 'vue-quill-editor'
import { isEmptyObject } from '@/utils/util'
import FileTypeEnum from '@/common/enum/file/FileType'
import '@riophae/vue-treeselect/dist/vue-treeselect.css'

import moment from 'moment'
import 'moment/locale/zh-cn'
moment.locale('zh-cn')

export default {
    components: {
        // FilesModal,
        SelectImage,
        SelectVideo,
        //Ueditor,
        // InputNumberGroup,
        MultiSpec,
        // Treeselect,
    },
    data() {
        return {
            GoodsStatusList: [], //商品状态
            HomeRecommendType: [], //首页推荐类型

            listStore: [], //商户
            //测试
            treeId: '4',
            // 默认的标签索引
            tabKey: 0,
            // 标签布局属性
            labelCol: { span: 3 },
            // 输入框布局属性
            wrapperCol: { span: 10 },
            // loading状态
            isLoading: false,
            isBtnLoading: false,
            // 当前表单元素
            form: this.$form.createForm(this),
            // 商品ID
            goodsId: null,
            // 表单数据
            formData: goodsIndex.formData,
            content: '',
            filesModalType: FileTypeEnum.IMAGE.value,
            //商品详情图片
            listImgDetail: [],
            //商品详情展示
            detailPicsContent: '',
            editorOption: {
                modules: {
                    toolbar: [
                        // ["bold", "italic", "underline", "strike"], // toggled buttons
                        // ["blockquote", "code-block"],
                        // [image],
                    ],
                },
            },
            pickerOptions: {
                shortcuts: [
                    {
                        text: '今天',
                        onClick(picker) {
                            picker.$emit('pick', new Date())
                        },
                    },
                    {
                        text: '明天',
                        onClick(picker) {
                            const date = new Date()
                            date.setTime(date.getTime() + 3600 * 1000 * 24)
                            picker.$emit('pick', date)
                        },
                    },
                    {
                        text: '一周后',
                        onClick(picker) {
                            const date = new Date()
                            date.setTime(date.getTime() + 3600 * 1000 * 24 * 7)
                            picker.$emit('pick', date)
                        },
                    },
                ],
            },
        }
    },
    watch: {
        listImgDetail: {
            //监控listUrlDetail改变
            handler(val) {
                this.clickRefresh()
            },
            deep: true,
        },
    },
    created() {
        new Promise((resolve, reject) => {
            Promise.all([
                this.initData(), // 初始化数据
                this.getGoodsStatusList(),

                this.getDeliveryList(),
                this.getServiceList(),
            ]).then(() => {})
        })
    },
    beforeDestroy() {
        // 销毁商品详情
        goodsIndex.formData.goods = {}
    },
    methods: {
        // 初始化数据
        initData() {
            // 记录商品ID
            this.goodsId = this.$route.params && this.$route.params.id
            //获取form所需的数据
            this.isLoading = true
            goodsIndex.getFromData(this.goodsId).then(() => {
                // // 商品表单数据
                if (
                    this.goodsId != '' &&
                    !isEmptyObject(this.form.getFieldsValue())
                ) {
                    this.form.setFieldsValue(goodsIndex.getFieldsValue()) // loadsh的pick方法
                    this.listImgDetail = this.formData.goods.listImgDetail
                }

                this.isLoading = false
            })
        },
        getServiceList() {
            // serviceApi.listByWhere({}).then((res) => {
            //     this.formData.serviceList = res.result
            // })
        },
        getDeliveryList() {
            deliveryApi.listByWhere({ onlyStore: true }).then((res) => {
                this.formData.deliveryList = res.result
            })
        },
        getGoodsStatusList() {
            //商品状态
            return new Promise((resolve, reject) => {
                commApi.ListEnum({ name: 'GoodsStatus' }).then((res) => {
                    var arrTemp = res.result
                    //去除已开售状态
                    arrTemp = arrTemp.filter((item) => item.value != 20)
                    this.GoodsStatusList = arrTemp
                    resolve()
                })
            })
        },
        selectDetailImg(ids, items) {
            this.listImgDetail = []
            if (items != null) {
                items.forEach((element) => {
                    this.listImgDetail.push(element)
                })
            }
        },

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

        // 切换tab选项卡
        handleTabs(key) {
            this.tabKey = key
        },

        // 刷新分类列表
        onReloadCategoryList() {
            this.isLoading = true
            goodsIndex.getCategoryList().then(() => {
                this.isLoading = false
            })
        },

        // 刷新服务与承诺列表
        onReloadServiceList() {
            this.isLoading = true
            goodsIndex.getServiceList().then(() => {
                this.isLoading = false
            })
        },

        // 刷新配送模板列表
        onReloadDeliveryList() {
            // this.isLoading = true;
            // goodsIndex.getDeliveryList().then(() => {
            //     this.isLoading = false;
            // });
        },
        //刷新详情
        clickRefresh() {
            var arrImg = this.listImgDetail
            this.detailPicsContent = ''
            arrImg.forEach((elem, index) => {
                this.detailPicsContent +=
                    "<img style='width: 100%;' src='" + elem.thumbnail + "'/>"
            })
        },
        /**
         * 确认按钮
         */
        handleSubmit(e) {
            e.preventDefault()
            // 表单验证
            const {
                form: { validateFields },
            } = this
            validateFields((errors, values) => {
                // 定位到错误的tab选项卡
                if (errors) {
                    this.onTargetTabError(errors)
                    return false
                }
                // 验证多规格
                if (values.specType === 20) {
                    const MultiSpec = this.$refs.MultiSpec
                    if (!MultiSpec.verifyForm()) {
                        this.tabKey = 1
                        return false
                    }
                    // 记录多规格数据
                    values.specData = MultiSpec.getFromSpecData()
                    values.specData.skuList.forEach((item) => {
                        item.afterSaleDeadline = values.afterSaleDeadline
                        item.unit = values.unit
                    })
                }
                // return false;
                //商品详情
                values.Content = this.detailPicsContent
                //组装单规格数据
                if (values.specType == 10) {
                    values.purchasePriceMax = values.purchasePriceMin
                    const spec = {
                        imageId: values.imagesIds[0], //规格图片
                        salePrice: values.salePrice, //原价
                        linePrice: values.linePrice, //划线价
                        goodsWeight: values.goodsWeight, //重量
                        goodsSkuNo: 1, //编码
                        stockNum: values.stockTotal, //库存
                        purchasePrice: values.purchasePrice, //采购价
                        commission1: values.commission1, //一级提成
                        commission2: values.commission2, //二级提成
                        commission3: values.commission3, //三级提成
                        afterSaleDeadline: values.afterSaleDeadline, //售后期限
                    }
                    values.specData = { skuList: [spec], specList: [] }
                }

                values.id = this.goodsId
                // 提交到后端api
                this.onFormSubmit(values)
                return true
            })
        },

        /**
         * 定位到错误的tab选项卡
         */
        onTargetTabError(errors) {
            // 表单字段与tabKey对应关系
            // 只需要必填字段就可
            const tabsFieldsMap = [
                [
                    'goodsName',
                    'goodsNo',
                    'categoryId',
                    'imagesIds',
                    'storeId',
                    'afterSaleDeadline',
                ],
                ['specType', 'purchasePrice'],
                ['listImgDetail'],
                ['aloneGradeEquity'],
            ]
            const field = Object.keys(errors).shift()
            for (const key in tabsFieldsMap) {
                if (tabsFieldsMap[key].indexOf(field) > -1) {
                    this.tabKey = parseInt(key)
                    break
                }
            }
            var err = errors[field].errors[0]
            this.$message({
                message: err.field + ': ' + err.message,
                type: 'error',
            })
        },

        /**
         * 提交到后端api
         */
        onFormSubmit(values) {
            // this.isLoading = true;
            this.isBtnLoading = true
            // return;
            goodsApi
                .addOrUpdate(values)
                .then((result) => {
                    // 显示提示信息
                    this.$message.success(result.message, 1.5)
                })

                .catch(() => {
                    this.isBtnLoading = false
                })
                .finally(() => {
                    this.isLoading = false
                    this.isBtnLoading = false
                })
        },
    },
}
</script>
<style lang="less" scoped>
@import './style.less';
</style>

<template>
    <a-card :bordered="false">
      <div class="card-title">{{ $route.meta.title }}</div>
      <a-spin :spinning="isLoading">
        <a-form :form="form" @submit="handleSubmit">
          <a-form-item label="是否开启满额包邮" :labelCol="labelCol" :wrapperCol="wrapperCol">
            <a-radio-group v-decorator="['isOpen', { rules: [{ required: true }] }]">
              <a-radio :value=10>开启</a-radio>
              <a-radio :value=-10>关闭</a-radio>
            </a-radio-group>
          </a-form-item>
          <a-form-item label="单笔订单满" :labelCol="labelCol" :wrapperCol="wrapperCol">
            <a-input-number
              :min="0"
              :precision="2"
              v-decorator="['money', { rules: [{ required: true, message: '请输入包邮的订单额度' }] }]"
            />
            <span class="ml-10">元包邮</span>
            <div class="form-item-help">
              <small>如设置0为全场包邮</small>
            </div>
          </a-form-item>
          <a-form-item label="不参与包邮的商品" :labelCol="labelCol" :wrapperCol="wrapperCol">
            <SelectGoods :defaultList="excludedGoodsList" v-decorator="['excludedGoodsIds']" />
          </a-form-item>
          <a-form-item label="不参与包邮的地区" :labelCol="labelCol" :wrapperCol="wrapperCol">
            <a-button @click="handleAreasModal">选择地区</a-button>
            <p class="content">
              <span v-for="(province, pidx) in excludedRegions.selectedText" :key="pidx">
                <span>{{ province.name }}</span>

                <!-- {{ JSON.stringify(province.chilren) }}{{ "萨达大萨达" }} -->
                <template v-if="province.chilren.length">
                  <span>[</span>
                  <span
                    class="city-name"
                    v-for="(city, cidx) in province.chilren"
                    :key="cidx"
                  >{{ city.name }}{{ province.chilren.length > cidx + 1 ? '、': '' }}</span>
                  <span>]</span>
                </template>
                <span class="mr-5"></span>
              </span>
            </p>
          </a-form-item>
          <a-form-item label="满额包邮说明" :labelCol="labelCol" :wrapperCol="wrapperCol">
            <a-textarea
              :autoSize="{ minRows: 4 }"
              v-decorator="['describe', { rules: [{ required: true, message: '请输入满额包邮说明' }] }]"
            />
          </a-form-item>
          <a-form-item :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
            <a-button type="primary" html-type="submit">提交</a-button>
          </a-form-item>
        </a-form>
        <AreasModal ref="AreasModal" @handleSubmit="handleAreaSubmit" />
      </a-spin>
    </a-card>
  </template>
  
  <script>

  import { pick, get } from 'lodash'
  import * as Api from '@/api/setting/storesetting'
  import * as GoodsApi from '@/api/goods/goods'
  import { SelectGoods } from '@/components'
  import { AreasModal } from '@/components/Modal'
  
  export default {
    components: {
      SelectGoods,
      AreasModal
    },
    data () {
      return {
        // 当前设置项的key
        key: 'fullFree',
        // 标签布局属性
        labelCol: { span: 4 },
        // 输入框布局属性
        wrapperCol: { span: 12 },
        // loading状态
        isLoading: false,
        // 当前表单元素
        form: this.$form.createForm(this),
        // 当前记录详情
        record: {},
        // 不参与包邮的地区
        excludedRegions: {
          cityIds: [],
          selectedText: []
        },
        // 不参与包邮的商品列表
        excludedGoodsList: []
      }
    },
    // 初始化数据
    async created () {
      // 获取当前详情记录
      await this.getDetail()
      // 获取不参与包邮的商品列表
      await this.getExcludedGoodsList()
    },
    methods: {
  
      // 获取当前详情记录
      async getDetail () {
              this.isLoading = true
              
        await Api.GetDetail({ key: this.key })
          .then(res => {
            // 当前记录
            this.record = res.result
            // 设置默认值
            this.setFieldsValue()
          })
          .finally(res => {
            this.isLoading = false
          })
      },
  
      // 获取不参与包邮的商品列表
      async getExcludedGoodsList () {
            const { record: { excludedGoodsIds } } = this

            console.log("xxxxxxxx", excludedGoodsIds)

            const ids = excludedGoodsIds== null? '0': excludedGoodsIds.join(',')
        if (excludedGoodsIds.length > 0) {
          this.isLoading = true
              await GoodsApi.listByWhere({ goodsIds: ids })
            .then(res => {
              this.excludedGoodsList = res.result
            })
            .finally(res => {
              this.isLoading = false
            })
        }
      },
  
      /**
       * 设置默认值
       */
      setFieldsValue () {
        const { record, $nextTick, form: { setFieldsValue } } = this
        $nextTick(() => {
          this.excludedRegions = record.excludedRegions
          setFieldsValue(pick(record, ['isOpen', 'money', 'describe']))
        })
      },
  
      // 显示选择地区对话框
      handleAreasModal () {
        this.$refs.AreasModal.handle({}, this.excludedRegions.cityIds)
      },
  
      // 选择地区后的回调
      handleAreaSubmit (result) {
        this.excludedRegions = {
          cityIds: result.selectedCityIds,
          selectedText: result.selectedText
        }
      },
  
      /**
       * 确认按钮
       */
      handleSubmit (e) {
        e.preventDefault()
        // 表单验证
        const { form: { validateFields }, excludedRegions } = this
        validateFields((errors, values) => {
          // 提交到后端api
          if (!errors) {
            values.excludedRegions = excludedRegions
            this.onFormSubmit(values)
          }
        })
      },
  
      // 提交到后端api
      onFormSubmit (values) {
        this.isLoading = true
         Api.addOrUpdate({ key: this.key, data: values })
          .then((result) => {
            // 显示提示信息
            this.$message.success(result.message, 1.5)
          })
          .finally((result) => {
            this.isLoading = false
          })
      }
  
    }
  }
  </script>
  <style lang="less" scoped>
  .ant-form-item {
    .ant-form-item {
      margin-bottom: 0;
    }
  }
  
  :v-deep(.ant-form-item-control ){
    padding-left: 10px;
    .ant-form-item-control {
      padding-left: 0;
    }
  }
  .content {
    color: #505050;
    line-height: 1.6;
    .city-name {
      font-size: 12.5px;
      color: #7b7b7b;
    }
  }
  </style>
  
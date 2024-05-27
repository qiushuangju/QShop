<template>
  <a-card :bordered="false">
    <a-spin :spinning="isLoading">
      <div class="card-title">{{ $route.meta.title }}</div>
      <div class="table-operator">
        <!-- 搜索板块 -->
        <a-row class="row-item-search">
          <a-form class="search-form" :form="searchForm" layout="inline" @submit="handleSearch">
            <a-form-item label="关键词查询">
              <a-input style="width: 342px" placeholder="请输入关键词" v-decorator="['searchValue']">
                <a-select slot="addonBefore" v-decorator="['searchType', { initialValue: 10 }]" style="width: 100px">
                  <a-select-option v-for="(item, index) in SearchTypeEnum" :key="index" :value="item.value">{{ item.name }}</a-select-option>
                </a-select>
              </a-input>
            </a-form-item>
            <a-form-item label="售后类型">
              <a-select v-decorator="['refundType', { initialValue: -1 }]">
                <a-select-option :value="-1">全部</a-select-option>
                <a-select-option v-for="(item, index) in RefundTypeEnum.data" :key="index" :value="item.value">{{ item.name }}</a-select-option>
              </a-select>
            </a-form-item>
            <a-form-item label="售后单状态">
              <a-select v-decorator="['refundStatus', { initialValue: -1 }]">
                <a-select-option :value="-1">全部</a-select-option>
                <a-select-option v-for="(item, index) in RefundStatusEnum.data" :key="index" :value="item.value">{{ item.name }}</a-select-option>
              </a-select>
            </a-form-item>
            <a-form-item label="申请时间">
              <a-range-picker format="YYYY-MM-DD" v-decorator="['betweenTime']" />
            </a-form-item>
            <a-form-item class="search-btn">
              <a-button type="primary" icon="search" html-type="submit">搜索</a-button>
            </a-form-item>
            <a-form-item class="search-btn">
              <a-button @click="handleReset">重置</a-button>
            </a-form-item>
          </a-form>
        </a-row>
      </div>
      <!-- 列表内容 -->
      <div class="ant-table ant-table-scroll-position-left ant-table-default ant-table-bordered">
        <div class="ant-table-content">
          <div class="ant-table-body">
            <table>
              <thead class="ant-table-thead">
                <tr>
                  <th v-for="(item, index) in columns" :key="index">
                    <span class="ant-table-header-column">
                      <div>
                        <span class="ant-table-column-title">{{ item.title }}</span>
                      </div>
                    </span>
                  </th>
                </tr>
              </thead>
              <tbody class="ant-table-tbody">
                <template v-for="item in refundList.data">
                  <tr class="order-empty" :key="`refund_${item.order_refund_id}_1`">
                    <td colspan="8"></td>
                  </tr>
                  <tr :key="`refund_${item.order_refund_id}_2`">
                    <td colspan="8">
                      <span class="mr-20">{{ item.create_time }}</span>
                      <span>订单号：{{ item.order_no }}</span>
                    </td>
                  </tr>
                  <tr :key="`refund_${item.order_refund_id}_3`">
                    <td>
                      <GoodsItem :data="{
                          image: item.orderGoods.goods_image,
                          imageAlt: '商品图片',
                          title: item.orderGoods.goods_name,
                          goodsProps: item.orderGoods.goods_props
                        }" />
                    </td>
                    <td>
                      <p>￥{{ item.orderGoods.goods_price }}</p>
                      <p>×{{ item.orderGoods.total_num }}</p>
                    </td>
                    <td>
                      <p>￥{{ item.orderGoods.total_pay_price }}</p>
                    </td>
                    <td>
                      <UserItem :user="item.user" />
                    </td>
                    <td>
                      <a-tag>{{ RefundTypeEnum[item.type].name }}</a-tag>
                    </td>
                    <td>
                      <p class="mtb-2">
                        <span class="f-13">商家审核：</span>
                        <a-tag :color="renderAuditStatusColor(item.audit_status)">{{ AuditStatusEnum[item.audit_status].name }}</a-tag>
                      </p>
                      <p v-if="item.type == RefundTypeEnum.RETURN.value" class="mtb-2">
                        <span class="f-13">用户发货：</span>
                        <a-tag :color="item.is_user_send ? 'green' : ''">{{ item.is_user_send ? '已发货' : '待发货' }}</a-tag>
                      </p>
                      <p v-if="item.type == RefundTypeEnum.RETURN.value" class="mtb-2">
                        <span class="f-13">商家收货：</span>
                        <a-tag :color="item.is_receipt ? 'green' : ''">{{ item.is_receipt ? '已收货' : '待收货' }}</a-tag>
                      </p>
                    </td>
                    <td>
                      <a-tag :color="renderRefundStatusColor(item.status)">{{ RefundStatusEnum[item.status].name }}</a-tag>
                    </td>
                    <td>
                      <div class="actions">
                        <router-link v-if="$auth('/order/refund/detail')" :to="{ path: '/order/refund/detail', query: { orderRefundId: item.order_refund_id } }">详情</router-link>
                        <a v-action:audit v-if="item.audit_status == AuditStatusEnum.WAIT.value" @click="handleAudit(item)">审核</a>
                        <a v-action:receipt v-if="(
                            item.type == RefundTypeEnum.RETURN.value
                              && item.audit_status == AuditStatusEnum.REVIEWED.value
                              && item.is_user_send
                              && !item.is_receipt
                          )" @click="handleReceipt(item)">确认收货</a>
                      </div>
                    </td>
                  </tr>
                </template>
              </tbody>
            </table>
          </div>
          <!-- 空状态 -->
          <a-empty v-if="!refundList.data.length" :image="simpleImage" />
        </div>
      </div>
      <!-- 分页器 -->
      <div v-if="refundList.data.length" class="pagination">
        <a-pagination :current="page" :pageSize="refundList.per_page" :total="refundList.total" @change="onChangePage" />
      </div>
      <AuditForm ref="AuditForm" @handleSubmit="handleRefresh" />
      <ReceiptForm ref="ReceiptForm" @handleSubmit="handleRefresh" />
    </a-spin>
  </a-card>
</template>

<script>
import { Empty } from 'ant-design-vue'
import { inArray, assignment } from '@/utils/util'
import * as Api from '@/api/order/orderRefundSku'
import { GoodsItem, UserItem } from '@/components/Table'
import {
  AuditStatusEnum,
  RefundStatusEnum,
  RefundTypeEnum,
} from '@/common/enum/order/refund'
import { AuditForm, ReceiptForm } from './modules'

// 表格字段
const columns = [
  {
    title: '商品信息',
    align: 'center',
  },
  {
    title: '单价/数量',
    align: 'center',
    scopedSlots: { customRender: 'unit_price' },
  },
  {
    title: '付款金额',
    align: 'center',
    dataIndex: 'total_pay_price',
    scopedSlots: { customRender: 'pay_price' },
  },
  {
    title: '买家',
    dataIndex: 'user',
    scopedSlots: { customRender: 'user' },
  },
  {
    title: '售后类型',
    dataIndex: 'type',
    scopedSlots: { customRender: 'type' },
  },
  {
    title: '处理进度',
    dataIndex: 'progress',
    scopedSlots: { customRender: 'progress' },
  },
  {
    title: '售后单状态',
    dataIndex: 'status',
    scopedSlots: { customRender: 'status' },
  },
  {
    title: '操作',
    dataIndex: 'action',
    width: '180px',
    scopedSlots: { customRender: 'action' },
  },
]

// 搜索关键词类型枚举
const SearchTypeEnum = [
  { name: '订单号', value: 10 },
  { name: '会员昵称', value: 20 },
  { name: '会员ID', value: 30 },
]

export default {
  name: 'Index',
  components: {
    GoodsItem,
    UserItem,
    AuditForm,
    ReceiptForm,
  },
  data() {
    return {
      // 当前表单元素
      searchForm: this.$form.createForm(this),
      // 查询参数
      queryParam: {},
      // 正在加载
      isLoading: false,
      // 表格字段
      columns,
      // 当前页码
      page: 1,
      // 列表数据
      refundList: { data: [], total: 0, per_page: 10 },
    }
  },
  beforeCreate() {
    // 批量给当前实例赋值
    assignment(this, {
      inArray,
      AuditStatusEnum,
      RefundStatusEnum,
      RefundTypeEnum,
      SearchTypeEnum,
      simpleImage: Empty.PRESENTED_IMAGE_SIMPLE,
    })
  },
  created() {
    // 初始化页面
    this.init()
  },
  methods: {
    // 初始化页面
    init() {
      this.searchForm.resetFields()
      this.queryParam = {}
      this.handleRefresh(true)
    },

    // 获取列表数据
    getList() {
      const { queryParam, page } = this
      this.isLoading = true
      return Api.load({ ...queryParam, page })
        .then((response) => {
          this.refundList = response.data.list
        })
        .finally(() => {
          this.isLoading = false
        })
    },

    // 渲染商家审核状态标签颜色
    renderAuditStatusColor(status) {
      const { AuditStatusEnum } = this
      const ColorEnum = {
        [AuditStatusEnum.WAIT.value]: '',
        [AuditStatusEnum.REVIEWED.value]: 'green',
        [AuditStatusEnum.REJECTED.value]: 'red',
      }
      return ColorEnum[status]
    },

    // 渲染售后单状态标签颜色
    renderRefundStatusColor(status) {
      const { RefundStatusEnum } = this
      const ColorEnum = {
        [RefundStatusEnum.NORMAL.value]: '',
        [RefundStatusEnum.REJECTED.value]: 'red',
        [RefundStatusEnum.COMPLETED.value]: 'green',
        [RefundStatusEnum.CANCELLED.value]: 'red',
      }
      return ColorEnum[status]
    },

    /**
     * 刷新列表
     * @param Boolean bool 强制刷新到第一页
     */
    handleRefresh(bool = false) {
      bool && (this.page = 1)
      this.getList()
    },

    // 确认搜索
    handleSearch(e) {
      e.preventDefault()
      this.searchForm.validateFields((error, values) => {
        if (!error) {
          this.queryParam = { ...this.queryParam, ...values }
          this.handleRefresh(true)
        }
      })
    },

    // 重置搜索表单
    handleReset() {
      this.searchForm.resetFields()
    },

    // 翻页事件
    onChangePage(current) {
      this.page = current
      this.handleRefresh()
    },

    // 商家审核
    handleAudit(record) {
      this.$refs.AuditForm.show(record)
    },

    // 确认收货
    handleReceipt(record) {
      this.$refs.ReceiptForm.show(record)
    },
  },
}
</script>
<style lang="less" scoped>
// 分页器
.pagination {
  margin-top: 16px;
  .ant-pagination {
    float: right;
  }
}

.ant-table {
  table {
    border: none;
    border-collapse: collapse;
  }
  .ant-table-thead > tr {
    border: 1px solid #e8e8e8;
  }
  tr.order-empty {
    height: 15px;
    border: 1px solid #fff;
    td {
      padding: 0;
      border-right: none;
      border-left: none;
      background: none !important;
    }
  }
}

.ant-table-thead > tr > th {
  border-right: none;
  border-bottom: none;
  padding: 12px 12px;
  font-weight: bold;
}
.ant-table-tbody > tr > td {
  border-right: 1px solid #e8e8e8;
  border-left: 1px solid #e8e8e8;
  padding: 12px 12px;
  // text-align: center;
}
</style>

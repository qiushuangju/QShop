<template>
    <div>
        <!-- 加载中 -->
        <a-spin :spinning="isLoading" />
        <!-- 售后单内容 -->
        <div v-if="!isLoading" class="detail-content">
            <!-- 售后单信息 -->
            <a-card :bordered="false">
                <!-- 订单操作 -->
                <div v-if="record.status == RefundStatusEnum.NORMAL.value" class="detail-actions">
                    <div class="ant-descriptions-title">售后单操作</div>
                    <!-- 提示栏 -->
                    <div class="alerts mt-10 mb-15">
                        <a-alert v-if="record.audit_status == AuditStatusEnum.WAIT.value" message="当前买家已发起售后申请，请及时审核处理" banner />
                    </div>
                    <!-- 操作栏 -->
                    <div class="actions mt-10">
                        <div v-if="$auth('/order/refund/index.audit')">
                            <a-button v-if="record.audit_status == AuditStatusEnum.WAIT.value" type="primary" @click="handleAudit">商家审核</a-button>
                        </div>
                        <div v-if="$auth('/order/refund/index.receipt')">
                            <a-button v-if="(
                  record.type == RefundTypeEnum.RETURN.value
                    && record.audit_status == AuditStatusEnum.REVIEWED.value
                    && record.is_user_send
                    && !record.is_receipt
                )" type="primary" @click="handleReceipt">确认收货</a-button>
                        </div>
                    </div>
                    <a-divider class="o-divider" />
                </div>
                <!-- 售后单信息 -->
                <a-descriptions title="售后单信息">
                    <a-descriptions-item label="订单号">
                        <router-link title="查看订单详情" :to="{ path: '/order/detail', query: { orderId: record.order_id } }"
                            target="_blank">{{ record.orderData.order_no }}</router-link>
                    </a-descriptions-item>
                    <a-descriptions-item label="买家信息">
                        <a-tooltip>
                            <template slot="title">会员ID: {{ record.user.user_id }}</template>
                            <span class="c-p">{{ record.user.nick_name }}</span>
                        </a-tooltip>
                    </a-descriptions-item>
                    <a-descriptions-item label="订单支付总额">
                        <span class="c-p">
                            <span>￥</span>
                            <span>{{ record.orderData.pay_price }}</span>
                        </span>
                    </a-descriptions-item>
                    <a-descriptions-item label="售后类型">
                        <a-tag>{{ RefundTypeEnum[record.type].name }}</a-tag>
                    </a-descriptions-item>
                    <a-descriptions-item label="售后单状态">
                        <a-tag :color="renderRefundStatusColor(record.status)">{{ RefundStatusEnum[record.status].name }}</a-tag>
                    </a-descriptions-item>
                    <a-descriptions-item label="申请时间">{{ record.create_time }}</a-descriptions-item>
                </a-descriptions>
                <a-divider class="o-divider" />
                <!-- 售后单信息 -->
                <a-descriptions title="处理进度">
                    <a-descriptions-item label="审核状态 (商家)">
                        <a-tag :color="renderAuditStatusColor(record.audit_status)">{{ AuditStatusEnum[record.audit_status].name }}</a-tag>
                    </a-descriptions-item>
                    <a-descriptions-item v-if="record.type == RefundTypeEnum.RETURN.value" label="发货状态 (买家)">
                        <a-tag :color="record.is_user_send ? 'green' : ''">{{ record.is_user_send ? '已发货' : '待发货' }}</a-tag>
                    </a-descriptions-item>
                    <a-descriptions-item v-if="record.type == RefundTypeEnum.RETURN.value" label="收货状态 (商家)">
                        <a-tag :color="record.is_receipt ? 'green' : ''">{{ record.is_receipt ? '已收货' : '待收货' }}</a-tag>
                    </a-descriptions-item>
                    <a-descriptions-item v-if="record.audit_status == AuditStatusEnum.REJECTED.value" label="拒绝原因">
                        <span>{{ record.refuse_desc }}</span>
                    </a-descriptions-item>
                </a-descriptions>
            </a-card>

            <!-- 买家申请原因 -->
            <a-card class="mt-20" :bordered="false">
                <a-descriptions title="买家申请原因">
                    <a-descriptions-item label="售后描述">{{ record.apply_desc ? record.apply_desc : '--' }}</a-descriptions-item>
                </a-descriptions>
                <template v-if="record.images.length">
                    <a-divider class="o-divider" />
                    <a-descriptions title="申请凭证">
                        <a-descriptions-item>
                            <div class="image-list">
                                <div v-for="item in record.images" :key="item.image_id" class="file-item">
                                    <a :href="item.image_url" target="_blank">
                                        <div class="img-cover" :style="{ backgroundImage: `url('${item.image_url}')` }"></div>
                                    </a>
                                </div>
                            </div>
                        </a-descriptions-item>
                    </a-descriptions>
                </template>
            </a-card>

            <!-- 商品信息 -->
            <a-card class="mt-20" :bordered="false">
                <div class="ant-descriptions-title">售后商品</div>
                <div class="goods-list">
                    <a-table rowKey="order_goods_id" :columns="goodsColumns" :dataSource="[record.orderGoods]" :pagination="false">
                        <!-- 商品信息 -->
                        <template slot="goodsInfo" slot-scope="text, item">
                            <GoodsItem :data="{
                  image: item.goods_image,
                  imageAlt: '商品图片',
                  title: item.goods_name,
                  goodsProps: item.goods_props
                }" />
                        </template>
                        <!-- 商品编码 -->
                        <span slot="goods_no" slot-scope="text">{{ text ? text : '--' }}</span>
                        <!-- 单价	 -->
                        <template slot="goods_price" slot-scope="text">￥{{ text }}</template>
                        <!-- 购买数量	 -->
                        <span slot="total_num" slot-scope="text">x{{ text }}</span>
                        <!-- 实际付款价 -->
                        <span slot="total_pay_price" slot-scope="text">￥{{ text }}</span>
                    </a-table>
                </div>
            </a-card>

            <!-- 商家退货地址 -->
            <a-card v-if="record.audit_status == AuditStatusEnum.REVIEWED.value" class="mt-20" :bordered="false">
                <a-descriptions title="商家退货地址">
                    <a-descriptions-item label="收货人姓名">{{ record.address.name }}</a-descriptions-item>
                    <a-descriptions-item label="联系电话">{{ record.address.phone }}</a-descriptions-item>
                    <a-descriptions-item label="所在地区">
                        <span class="region mr-5" v-for="(region, idx) in record.address.region" :key="idx">{{ region }}</span>
                    </a-descriptions-item>
                    <a-descriptions-item label="详细地址">{{ record.address.detail }}</a-descriptions-item>
                </a-descriptions>
            </a-card>

            <!-- 用户发货信息 -->
            <a-card v-if="(
          record.type == RefundTypeEnum.RETURN.value
            && record.audit_status == AuditStatusEnum.REVIEWED.value
            && record.is_user_send
        )" class="mt-20" :bordered="false">
                <a-descriptions title="退货物流信息">
                    <a-descriptions-item label="物流公司">{{ record.express.express_name }}</a-descriptions-item>
                    <a-descriptions-item label="物流单号">{{ record.express_no }}</a-descriptions-item>
                    <a-descriptions-item label="发货状态">
                        <a-tag :color="record.is_user_send ? 'green' : ''">{{ record.is_user_send ? '已发货' : '待发货' }}</a-tag>
                    </a-descriptions-item>
                    <a-descriptions-item label="发货时间">{{ record.send_time }}</a-descriptions-item>
                </a-descriptions>
            </a-card>

        </div>
        <AuditForm ref="AuditForm" @handleSubmit="handleRefresh" />
        <ReceiptForm ref="ReceiptForm" @handleSubmit="handleRefresh" />
    </div>
</template>

<script>
import { assignment } from '@/utils/util'
import * as Api from '@/api/order/orderRefundSku'
import { GoodsItem, UserItem } from '@/components/Table'
import {
    AuditStatusEnum,
    RefundStatusEnum,
    RefundTypeEnum,
} from '@/common/enum/order/refund'
import { AuditForm, ReceiptForm } from './modules'

// 商品内容表头
const goodsColumns = [
    {
        title: '商品信息',
        scopedSlots: { customRender: 'goodsInfo' },
    },
    {
        title: '商品编码',
        dataIndex: 'goods_no',
        scopedSlots: { customRender: 'goods_no' },
    },
    {
        title: '重量(Kg)',
        dataIndex: 'goods_weight',
        scopedSlots: { customRender: 'goods_weight' },
    },
    {
        title: '单价',
        dataIndex: 'goods_price',
        scopedSlots: { customRender: 'goods_price' },
    },
    {
        title: '购买数量',
        dataIndex: 'total_num',
        scopedSlots: { customRender: 'total_num' },
    },
    {
        title: '实际付款价',
        dataIndex: 'total_pay_price',
        scopedSlots: { customRender: 'total_pay_price' },
    },
]

export default {
    name: 'Index',
    components: {
        GoodsItem,
        // UserItem,
        AuditForm,
        ReceiptForm,
    },
    data() {
        return {
            // 正在加载
            isLoading: true,
            // 售后单ID
            orderRefundId: null,
            // 售后单详情
            record: {},
            // 商品内容表头
            goodsColumns,
            // 商品列表数据
            goodsList: [],
        }
    },
    beforeCreate() {
        // 批量给当前实例赋值
        assignment(this, {
            AuditStatusEnum,
            RefundStatusEnum,
            RefundTypeEnum,
        })
    },
    created() {
        // 记录售后单ID
        this.orderRefundId = this.$route.query.orderRefundId
        // 刷新页面
        this.handleRefresh()
    },
    methods: {
        // 刷新页面
        handleRefresh() {
            // 获取当前记录
            this.getDetail()
        },

        // 获取当前记录
        getDetail() {
            const { orderRefundId } = this
            this.isLoading = true
            Api.getDetail({ orderRefundId })
                .then((result) => {
                    // 当前记录
                    this.record = result.data.detail
                    // 商品列表
                    this.goodsList = [this.record.orderGoods]
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

        // 商家审核
        handleAudit() {
            const { record } = this
            this.$refs.AuditForm.show(record)
        },

        // 确认收货
        handleReceipt() {
            const { record } = this
            this.$refs.ReceiptForm.show(record)
        },
    },
}
</script>
<style lang="less" scoped>
// 售后单详情页
.detail-content {
    margin-bottom: 70px;

    /deep/.ant-descriptions-item > span {
        vertical-align: middle;
    }

    /deep/.ant-descriptions-item-content {
        padding-left: 3px;
    }

    // 商品列表
    .goods-list {
        /deep/table {
            table-layout: auto;
        }
    }
    // 操作栏
    .actions {
        button {
            margin-right: 8px;
        }
    }

    // 预览图列表
    .image-list {
        // 文件元素
        .file-item {
            position: relative;
            float: left;
            width: 120px;
            height: 120px;
            position: relative;
            padding: 2px;
            border: 1px solid #ddd;
            background: #fff;
            margin-right: 12px;
            .img-cover {
                display: block;
                width: 100%;
                height: 100%;
                background: no-repeat center center / 100%;
            }
            &:hover {
                border: 1px solid #a7c3de;
            }
        }
    }
}
</style>

<template>
    <a-card :bordered="false">
        <a-spin :spinning="isLoading">
            <a-form>
                <div class="tabs-content">
                    <!-- 详情 -->
                    <div id="printarea" class="printInfo">
                        <!-- 用户信息 -->
                        <div class="details">
                            <div class="box">
                                <el-row>
                                    <el-col :span="8">用户昵称：{{temp.orderInfo==null?'':
                                            temp.orderInfo.userName}}</el-col>
                                    <el-col :span="16">联系电话：{{temp.orderInfo==null?'':
                                            temp.orderInfo.userPhone}}</el-col>
                                </el-row>
                                <el-row>
                                    <el-col :span="8">客户经理：{{temp.businessUser==null?'':
                                            temp.businessUser.name}}</el-col>
                                    <el-col :span="16">联系电话：{{temp.businessUser==null?'':
                                            temp.businessUser.phone}}</el-col>
                                </el-row>
                            </div>
                        </div>
                        <!-- 订单信息 -->
                        <div class="details">
                            <div class="title">售后信息</div>
                            <div class="box">
                                <el-row>
                                    <el-col :span="8">订单号：{{temp.orderInfo==null?'':temp.orderInfo.orderNo}}</el-col>
                                    <el-col :span="8">发起时间：{{temp.orderInfo==null?'':temp.orderInfo.createTime}}</el-col>
                                    <el-col :span="8">售后状态：{{temp.orderInfo==null?'':temp.strStatus}}</el-col>
                                </el-row>
                                <el-row>
                                    <!-- <el-col :span="8">订单金额：{{temp.orderInfo==null?'':temp.orderInfo.orderPrice}}</el-col>
                                <el-col :span="8">运费总计：{{temp.orderInfo==null?'':temp.orderInfo.totalFreightPrice}}</el-col>
                                <el-col :span="8">实际支付：{{temp.orderInfo==null?'':temp.orderInfo.payPrice}}</el-col> -->
                                </el-row>
                                <el-row>
                                    <el-col :span="8">退货类型：{{temp.orderInfo==null?'':temp.orderInfo.strRefundType}}</el-col>
                                    <el-col :span="16">退货原因：{{temp.orderInfo==null?'':temp.orderInfo.refundReason}}</el-col>
                                </el-row>
                                <el-row>
                                    <el-col :span="8">申请退款金额：{{temp.orderInfo==null?'':temp.orderInfo.expectedMoney}}</el-col>
                                    <el-col v-if="temp.status==80" :span="8">实际退款金额：{{temp.orderInfo==null?'':temp.orderInfo.refundMoney}}</el-col>
                                </el-row>
                                <el-row>
                                    <el-col>退款备注：{{temp.orderInfo==null?'':temp.orderInfo.refundMoneyRemark}}</el-col>
                                </el-row>
                            </div>
                        </div>
                        <!-- 售后凭证 -->
                        <div class="details">
                            <div class="title">售后凭证</div>
                            <div class="box">
                                <el-image :previewSrcList="[item]" style="padding:0px 30px;" v-for="(item,index) in temp.listUrlRefundProof" :key="index" :src="item"
                                    fit></el-image>
                                <a class="c-a" @click="downloadIamgeArr(temp.listUrlRefundProof,'退货凭证')">下载</a>
                            </div>
                        </div>
                        <!-- 退货退款 -->
                        <div class="refundType" v-if="temp.orderInfo.refundType>10">
                            <!-- 商品清单 -->
                            <div class="details">
                                <div class="title">退货清单</div>
                                <div class="box">
                                    <el-row :key="index" v-for="(item,index) in temp.goodsListing">
                                        <el-col :span="8" :key="index" v-for="(child,index) in item">
                                            <el-col :span="6">
                                                <el-image :src="child.urlImg" fit></el-image>
                                            </el-col>
                                            <el-col :span="18">
                                                <el-row>
                                                    {{child.goodsName}}
                                                </el-row>
                                                <el-row>
                                                    <el-col :span="4">{{child.goodsName}}</el-col>
                                                    <el-col :span="20">￥{{child.payPrice}}X{{child.totalNum}}</el-col>
                                                </el-row>
                                                <el-row v-if="temp.status>50">
                                                    <el-col>实际入库数量：{{child.actualNum}}</el-col>
                                                </el-row>
                                            </el-col>
                                        </el-col>
                                    </el-row>
                                </div>
                            </div>
                            <!-- 平台取货需要展示取货地址、运费、运费支付、取货司机、送货凭证-->
                            <!-- 平台配送 -->
                            <div class="delivery" v-if="temp.returnWay==10">
                                <!-- 取货地址 -->
                                <div class="details">
                                    <div class="title">取货地址</div>
                                    <div class="box">
                                        <el-row>
                                            <el-col :span="8">
                                                联系人：{{temp.addressInfo==null?"":temp.addressInfo.linkMan}}
                                            </el-col>
                                            <el-col :span="8">
                                                联系电话：{{temp.addressInfo==null?"":temp.addressInfo.phone}}
                                            </el-col>
                                        </el-row>
                                        <el-row>
                                            <el-col :span="24">
                                                地址：{{temp.addressInfo==null?"":temp.addressInfo.fullAddress}}
                                            </el-col>
                                        </el-row>
                                    </div>
                                </div>
                                <!-- 退货运费  -->
                                <div class="details">
                                    <div class="title">退货运费</div>
                                    <div class="box">
                                        <el-row>
                                            <el-col :span="8">运输费：{{temp.orderInfo==null?"":temp.orderInfo.freightPrice}}</el-col>
                                            <el-col :span="8">上楼费：{{temp.orderInfo==null?"":temp.orderInfo.floorPrice}}</el-col>
                                            <el-col :span="8">入户费：{{temp.orderInfo==null?"":temp.orderInfo.entryPrice}}</el-col>
                                        </el-row>
                                    </div>
                                </div>
                                <!-- 运费支付信息 -->
                                <div class="details">
                                    <div class="title">运费支付信息</div>
                                    <div class="box" v-if="temp.status>30">
                                        <el-row>
                                            <el-col :span="8">支付方式：{{temp.strPayType}}</el-col>
                                            <el-col :span="8">支付金额：{{temp.orderInfo==null?'':temp.orderInfo.totalFreightPrice}}</el-col>
                                            <el-col :span="8">支付时间：{{temp.payTime}}</el-col>
                                        </el-row>
                                    </div>
                                </div>
                                <!-- 司机信息 30:待分配-->
                                <div class="details" v-if="temp.status>30">
                                    <div class="title">司机信息</div>
                                    <div class="box">
                                        <el-row>
                                            <el-col :span="8">司机姓名：{{temp.carInfo==null?"":temp.carInfo.name}}</el-col>
                                            <el-col :span="8">联系电话：{{temp.carInfo==null?"":temp.carInfo.phone}}</el-col>
                                        </el-row>
                                        <el-row>
                                            <el-col :span="8">司机类型：{{temp.carInfo==null?"":temp.carInfo.strDriverType}}</el-col>
                                            <el-col :span="8">配送状态：{{temp.carInfo==null?"":temp.carInfo.strStatus}}</el-col>
                                        </el-row>
                                    </div>
                                </div>
                                <!--取货凭证 (已出发：40)-->
                                <div class="details" v-if="temp.status>32">
                                    <div class="title">取货凭证</div>
                                    <div class="box">
                                        <el-image :previewSrcList="[item]" style="padding:0px 30px;" v-for="(item,index) in temp.listUrlTakeGoodsProof" :key="index"
                                            :src="item" fit></el-image>
                                        <a class="c-a" @click="downloadIamgeArr(temp.listUrlTakeGoodsProof,'取货凭证')">下载</a>
                                    </div>
                                </div>
                                <!-- 送达凭证 (已送达状态：50)-->
                                <div class="details" v-if="temp.status>40">
                                    <div class="title">送达凭证</div>
                                    <div class="box">
                                        <el-image :previewSrcList="[item]" style="padding:0px 30px;" v-for="(item,index) in temp.listUrlReceiptProof" :key="index"
                                            :src="item" fit></el-image>
                                        <a class="c-a" @click="downloadIamgeArr(temp.listUrlReceiptProof,'送达凭证')">下载</a>
                                    </div>
                                </div>
                            </div>
                            <!-- 收货仓库 -->
                            <div class="details" v-if="this.temp.status>10">
                                <div class="title">收货仓库</div>
                                <div class="box">
                                    <el-row>
                                        <el-col :span="8">仓库名称：{{temp.storeStorage==null?'':temp.storeStorage.name}}</el-col>
                                        <el-col :span="16">仓库地址：{{temp.storeStorage==null?'':temp.storeStorage.address}}</el-col>
                                    </el-row>
                                    <el-row>
                                        <el-col :span="8">联系人：{{temp.storeStorage==null?'':temp.storeStorage.linkMan}}</el-col>
                                        <el-col :span="16">联系电话：{{temp.storeStorage==null?'':temp.storeStorage.phone}}</el-col>
                                    </el-row>
                                </div>
                            </div>
                            <!-- 入库凭证 -->
                            <div class="details" v-if="this.temp.status>60">
                                <div class="title">入库凭证</div>
                                <div class="box">
                                    <el-image :previewSrcList="[item]" style="padding:0px 30px;" v-for="(item,index) in temp.listUrlPutStorageProof" :key="index"
                                        :src="item" fit></el-image>
                                    <a class="c-a" @click="downloadIamgeArr(temp.listUrlPutStorageProof,'入库凭证')">下载</a>
                                </div>
                            </div>
                        </div>
                        <!-- 退款信息 -->
                        <div class="details" v-if="temp.status==80">
                            <div class="title">退款信息</div>
                            <div class="box">
                                <el-row>
                                    <el-col :span="8">处理人：{{temp.refundOperatorName}}</el-col>
                                    <el-col :span="8">处理时间：{{temp.refundTime}}</el-col>
                                </el-row>
                            </div>
                        </div>
                    </div>
                    <!-- 售后详情 -->
                    <div class="details">
                        <div class="title">售后详情</div>
                        <div class="box">
                            <el-row :key="index" v-for="(item,index) in temp.listRecord">
                                <el-col :span="8">
                                    {{item.createTime}}
                                </el-col>
                                <el-col :span="16">
                                    {{item.info}}
                                </el-col>
                            </el-row>
                        </div>
                    </div>
                </div>
                <a-form-item class="mt-20" :wrapper-col="{ span: wrapperCol.span, offset: labelCol.span }">
                    <el-row>
                        <el-col :span="6">
                            <a-button v-print="print" type="primary">打印清单</a-button>
                        </el-col>
                    </el-row>
                </a-form-item>
            </a-form>
        </a-spin>
    </a-card>
</template>
<script>
import _ from 'lodash'
import * as orderRefund from '@/api/order/orderRefundSku'
import { debuglog, debug } from 'util'

export default {
    data() {
        return {
            // loading状态
            isLoading: false,
            tabKey: 0,
            isBtnLoading: false,
            labelCol: { span: 3 },
            wrapperCol: { span: 10 },
            orderId: null,
            temp: {
                id: '',
                orderNo: '', //订单编号
                addressInfo: {
                    //收货地址信息
                    contactInfo: '',
                    expectTimeSlot: '',
                    fullAddress: '',
                    name: '',
                    linkMan: '',
                    phone: '',
                },
                orderInfo: {
                    orderNo: '',
                    refundType: '',
                },
                listRefundSku: [], //商品清单
                goodsListing: [], //清单
            },
            print: {
                id: 'printarea',
                popTitle: this.$route.meta.title, // 打印配置页上方标题
                extraHead: '', //最上方的头部文字，附加在head标签上的额外标签,使用逗号分隔
                preview: '', // 是否启动预览模式，默认是false（开启预览模式，可以先预览后打印）
                previewTitle: '', // 打印预览的标题（开启预览模式后出现）,
                previewPrintBtnLabel: '', // 打印预览的标题的下方按钮文本，点击可进入打印（开启预览模式后出现）
                zIndex: '', // 预览的窗口的z-index，默认是 20002（此值要高一些，这涉及到预览模式是否显示在最上面）
                previewBeforeOpenCallback() {}, //预览窗口打开之前的callback（开启预览模式调用）
                previewOpenCallback() {}, // 预览窗口打开之后的callback（开启预览模式调用）
                beforeOpenCallback() {}, // 开启打印前的回调事件
                openCallback() {}, // 调用打印之后的回调事件
                closeCallback(vue) {}, //关闭打印的回调事件（无法确定点击的是确认还是取消）
                url: '',
                standard: '',
                extraCss: '',
            },
        }
    },
    created() {
        this.orderId = this.$route.params && this.$route.params.id
        if (this.orderId != null && this.orderId.trim() != '') {
            this.loadData()
        }
    },
    methods: {
        //加载数据
        loadData() {
            orderRefund.getDetail({ orderId: this.orderId }).then((res) => {
                // this.form.resetFields()
                this.$nextTick(() => {
                    this.temp = res.result
                    this.temp.goodsListing = []
                    let list = []
                    for (var i = 0; i < this.temp.listRefundSku.length; i++) {
                        if (list.length == 2) {
                            list.push(this.temp.listRefundSku[i])
                            this.temp.goodsListing.push(list)
                            list = []
                        } else {
                            list.push(this.temp.listRefundSku[i])
                        }
                        if (
                            i == this.temp.listRefundSku.length - 1 &&
                            list.length > 0
                        )
                            this.temp.goodsListing.push(list)
                    }
                })
            })
        },
        //确认退款
        refundMoney() {
            const orderNo = this.temp.orderInfo.orderNo
            const that = this
            this.$confirm(
                `此操作将确认退款，货款将原路径返还用户, 是否继续?`,
                `确认退款`,
                {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning',
                    beforeClose: (action, instance, done) => {
                        if (action === 'confirm') {
                            orderRefund
                                .refundMoney({ orderNo: orderNo })
                                .then((res) => {
                                    that.$message.success(res.message, 1.5)
                                    setTimeout(() => {
                                        that.$store.state.tagsView.visitedViews.splice(
                                            that.$store.state.tagsView.visitedViews.findIndex(
                                                (item) =>
                                                    item.path ===
                                                    that.$route.path
                                            ),
                                            1
                                        )
                                        that.$router.push(
                                            that.$store.state.tagsView
                                                .visitedViews[
                                                that.$store.state.tagsView
                                                    .visitedViews.length - 1
                                            ].path
                                        )
                                    }, 1500)
                                })
                            done()
                        } else {
                            done()
                        }
                    },
                }
            )
        },
        //批量下载
        downloadIamgeArr(imgsrcArr, name) {
            // 下载图片地址和图片名
            const orderNo = this.temp.orderInfo.orderNo
            for (var i = 0; i < imgsrcArr.length; i++) {
                const imgsrc = imgsrcArr[i]
                const index = i + 1
                const imgname = orderNo + name + index
                this.downloadIamge(imgsrc, imgname)
            }
        },
        //下载图片
        downloadIamge(imgsrc, name) {
            var image = new Image()
            // 解决跨域 Canvas 污染问题
            image.setAttribute('crossOrigin', 'anonymous')
            image.onload = function () {
                var canvas = document.createElement('canvas')
                canvas.width = image.width
                canvas.height = image.height
                var context = canvas.getContext('2d')
                context.drawImage(image, 0, 0, image.width, image.height)
                var url = canvas.toDataURL('image/png') // 得到图片的base64编码数据
                var a = document.createElement('a') // 生成一个a元素
                var event = new MouseEvent('click') // 创建一个单击事件
                // a.download = name || 'photo' // 设置图片名称
                a.download = name // 设置图片名称
                a.href = url // 将生成的URL设置为a.href属性
                a.dispatchEvent(event) // 触发a的单击事件
            }
            image.src = imgsrc
        },
    },
}
</script>

<style scoped>
.box {
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
    padding: 10px;
}
.el-row {
    margin: 10px 0px;
}
.title {
    padding: 20px 0px;
}
.mt-20 {
    padding: 20px 0px;
}
.el-input__inner {
    height: 30px !important;
}
.el-image__inner {
    width: 50px;
    height: 50px;
}
</style>
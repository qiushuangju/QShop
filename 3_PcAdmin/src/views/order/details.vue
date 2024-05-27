<template>
  <a-card :bordered="false">
    <div class="card-title"></div>
    <a-spin :spinning="isLoading">
      <a-form>
        <div class="tabs-content">
          <!-- 打印区 -->
          <div id="printarea" class="printInfo">
            <!-- 用户信息 -->
            <div class="details">
              <div class="box">
                <el-row>
                  <el-col :span="8">用户昵称：{{temp.orderUser==null?"":temp.orderUser.name}}</el-col>
                  <el-col :span="16">联系电话：{{temp.orderUser==null?"":
                                         temp.orderUser.phone}}</el-col>
                </el-row>
              </div>
            </div>
            <!-- 订单信息 -->
            <div class="details">
              <div class="title">订单信息</div>
              <div class="box">
                <!-- 订单号、下单时间、状态 -->
                <el-row>
                  <el-col :span="8">订单号： {{temp.orderNo}}</el-col>
                  <el-col :span="8">下单时间： {{temp.createTime}}</el-col>
                  <el-col :span="8">
                    <el-col :span="12">
                      订单状态： {{temp.orderStatusStr}}
                    </el-col>
                    <el-col :span="12" v-if="this.temp.orderStatus<=-50">
                      <el-button type="text" @click="openRefundDetails(scope.row)">查看售后详情</el-button>
                      <!-- <a class="c-a" @click="openRefundDetails()">查看售后详情</a> -->
                    </el-col>

                  </el-col>
                </el-row>
                <!-- 订单、、优惠券金额 -->
                <el-row>
                  <el-col :span="8">订单金额： {{temp.orderMoney==null?'0':temp.orderMoney.totalGoodsPrice}}</el-col>
                  <el-col :span="8">优惠劵金额：{{temp.orderMoney==null?'0':temp.orderMoney.couponMoney}}</el-col>
                </el-row>
                <!-- 配送方式 -->
                <el-row>
                  <el-col :span="8">配送方式： {{DeliveryTypeList.find(p=>p.value==temp.deliveryType)==null?'':
                                DeliveryTypeList.find(p=>p.value==temp.deliveryType).text}}
                  </el-col>
                  <el-col :span="16">买家留言：{{temp.buyerRemark}}</el-col>
                </el-row>
              </div>
            </div>
            <!-- 商品清单 -->
            <div class="details">
              <div class="title">商品清单</div>
              <div class="box">
                <el-row :key="index" v-for="(item,index) in temp.goodsListing">
                  <el-col :span="8" :key="index" v-for="(child,index) in item">
                    <el-col :span="6">
                      <el-image :src="child.urlImg" fit></el-image>
                    </el-col>
                    <el-col :span="18">
                      <el-row>
                        <el-col>{{child.goodsName}}</el-col>
                      </el-row>
                      <el-row>
                        <el-col :span="4">{{child.goodsProps}}</el-col>
                        <el-col :span="20">￥{{child.salePrice}}X{{child.totalNum}}</el-col>
                      </el-row>
                    </el-col>
                  </el-col>
                </el-row>
              </div>
            </div>
            <!-- 支付信息 -->
            <div class="details" v-if="temp.orderStatus<-10||temp.orderStatus>20">
              <div class="title">支付信息</div>
              <div class="box">
                <el-row>
                  <el-col :span="8">支付方式：{{temp.strPayType}}</el-col>
                  <el-col :span="8">支付金额：{{temp.orderMoney==null?'0':temp.orderMoney.payPrice}}</el-col>
                  <el-col :span="8">支付时间：{{temp.payTime}}</el-col>
                </el-row>
              </div>
            </div>
            <!-- 自提仓库 -->
            <div class="details" v-if="temp.deliveryType==20">
              <div class="title">自提仓库</div>
              <div class="box">
                <el-row>
                  <el-col :span="8">仓库名称：{{temp.addressInfo==null?"":temp.addressInfo.name}}</el-col>
                  <el-col :span="16">仓库地址：{{temp.addressInfo==null?"":temp.addressInfo.fullAddress}}</el-col>
                </el-row>
                <el-row>
                  <el-col :span="8">联系人：{{temp.addressInfo==null?"":temp.addressInfo.linkMan}}</el-col>
                  <el-col :span="16">联系电话：{{temp.addressInfo==null?"":temp.addressInfo.phone}}</el-col>
                </el-row>
              </div>
            </div>
            <!-- 出库凭证 -->
            <!-- <div class="details" v-if="temp.outBoundStatus!=null&&temp.outBoundStatus==10">
                            <div class="title">出库凭证</div>
                            <div class="box">
                                <el-image style="padding:0px 30px;" v-for="(item,index) in temp.listOutboundProofImg" :key="index" :src="item" :preview-src-list="[item]" fit>
                                </el-image>
                                <a  class="c-a" @click="downloadIamgeArr(temp.listOutboundProofImg,'出库凭证')">下载</a>
                            </div>
                        </div> -->
            <!-- 平台配送需要展示取货地址、运费、送货司机、送货凭证-->
            <div class="delivery" v-if="temp.deliveryType==10">
              <!-- 送货地址 -->
              <div class="details">
                <div class="title">送货地址</div>
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
              <!-- 运费  -->
              <div class="details">
                <div class="title">运费</div>
                <div class="box">
                  <el-row>
                    <el-col :span="8">运费：
                      {{temp.truckage.freightPrice}}
                    </el-col>
                    <el-col :span="8">上楼费：
                      {{temp.truckage.floorPrice}}
                    </el-col>
                    <el-col :span="8">入户费：
                      {{temp.truckage.entryPrice}}
                    </el-col>
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
              <!--取货凭证（单据） -->
              <div class="details" v-if="this.temp.orderStatus>40||this.temp.orderStatus<=-50">
                <div class="title">单据</div>
                <div class="box">
                  <el-image style="padding:0px 30px;" v-for="(item,index) in temp.listUrlTakeGoodsProof" :key="index" :src="item" :preview-src-list="temp.listUrlTakeGoodsProof" fit></el-image>
                  <a class="c-a" @click="downloadIamgeArr(temp.listUrlTakeGoodsProof,'单据')">下载</a>
                </div>
              </div>
              <!-- 送达凭证 -->
              <div class="details" v-if="this.temp.orderStatus>40||this.temp.orderStatus<=-50">
                <div>
                  <el-row>
                    <el-col>
                      送达时间：{{temp.deliveryProof==null?'':temp.deliveryProof.arrivedTime}}
                    </el-col>
                  </el-row>
                </div>
                <div class="title">送达凭证</div>
                <div class="box">
                  <el-image style="padding:0px 30px;" v-for="(item,index) in temp.deliveryProof.listUrlProof" :key="index" :src="item" :preview-src-list="temp.deliveryProof.listUrlProof" fit></el-image>
                  <a class="c-a" @click="downloadIamgeArr(temp.deliveryProof.listUrlProof,'送达凭证')">下载</a>
                </div>
              </div>
            </div>
          </div>
          <!-- 订单流程详情 -->
          <div class="details">
            <div class="title">流程详情</div>
            <div class="box">
              <el-row :key="index" v-for="(item,index) in temp.listTrackRecord">
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
              <a-button @click="printOrderDetails" type="primary">生成打印单</a-button>
            </el-col>
          </el-row>
        </a-form-item>
      </a-form>
    </a-spin>
  </a-card>
</template>
<script>
import _ from 'lodash'
import Layout from '@/views/layout/Layout'
import enums from '@/api/enumList'
import { debuglog, debug } from 'util'
import * as order from '@/api/order/orderSku'

export default {
  data() {
    return {
      OrderTypeList: [], //订单类型
      PayTypeList: [], //支付方式
      OrderStatusList: [], //订单状态
      DeliveryTypeList: [], //配送方式
      PayStatusList: [],
      // loading状态
      isLoading: false,
      tabKey: 0,
      isBtnLoading: false,
      orderId: null,
      labelCol: { span: 10 },
      wrapperCol: { span: 10 },
      temp: {
        orderNo: '',
        applyStorageId: '',
        sendStorageId: '',
        applyAddress: '',
        sendAddress: '',
        storeId: '',
        userId: '',
        createTime: '',
        name: '',
        userName: '',
        UserPhone: '',
        oderTotalPrice: '', //订单总价
        listStorageOrderSku: [],
        applyStorageInfo: null, //申请仓库
        sendStorageInfo: null, //送货仓库
        goodsListing: [], //商品清单
      },
      storestorageList: [], //仓库
      print: {
        id: 'printarea',
        popTitle: this.$route.meta.title, // 打印配置页上方标题
        extraHead: '', //最上方的头部文字，附加在head标签上的额外标签,使用逗号分隔
        preview: '', // 是否启动预览模式，默认是false（开启预览模式，可以先预览后打印）
        previewTitle: '', // 打印预览的标题（开启预览模式后出现）,
        previewPrintBtnLabel: '', // 打印预览的标题的下方按钮文本，点击可进入打印（开启预览模式后出现）
        zIndex: '', // 预览的窗口的z-index，默认是 20002（此值要高一些，这涉及到预览模式是否显示在最上面）
        previewBeforeOpenCallback() { }, //预览窗口打开之前的callback（开启预览模式调用）
        previewOpenCallback() { }, // 预览窗口打开之后的callback（开启预览模式调用）
        beforeOpenCallback() { }, // 开启打印前的回调事件
        openCallback() { }, // 调用打印之后的回调事件
        closeCallback(vue) { }, //关闭打印的回调事件（无法确定点击的是确认还是取消）
        url: '',
        standard: '',
        extraCss: '',
      },
    }
  },
  created() {
    this.orderId = this.$route.params && this.$route.params.id
    new Promise((resolve, reject) => {
      Promise.all([
        enums.getPayStatusList().then((res) => {
          this.PayStatusList = res
        }),
        enums.getListOrderStatus(true).then((res) => {
          this.OrderStatusList = res
        }),
        enums.getPayTypeList(true).then((res) => {
          this.PayTypeList = res
        }),
        enums.getDeliveryTypeList().then((res) => {
          this.DeliveryTypeList = res
        }),
        enums.getOrderTypeList().then((res) => {
          this.OrderTypeList = res
        }),
      ]).then(() => {
        if (this.orderId != null && this.orderId.trim() != '')
          this.loadData()
      })
    })
  },
  methods: {
    //加载数据
    loadData() {
      this.isLoading = true
      order.getDetail({ id: this.orderId }).then((res) => {
        this.$nextTick(() => {
          this.temp = res.result
          this.temp.goodsListing = []
          let list = []
          for (var i = 0; i < this.temp.listGoods.length; i++) {
            if (list.length == 2) {
              list.push(this.temp.listGoods[i])
              this.temp.goodsListing.push(list)
              list = []
            } else {
              list.push(this.temp.listGoods[i])
            }
            if (
              i == this.temp.listGoods.length - 1 &&
              list.length > 0
            )
              this.temp.goodsListing.push(list)
          }
          this.isLoading = false
        })
      })
    },
    //批量下载
    downloadIamgeArr(imgsrcArr, name) {
      // 下载图片地址和图片名
      const orderNo = this.temp.orderNo
      const address = this.temp.addressInfo.fullAddress.replace(
        /.+?(省|市|自治区|自治州|县|区)/g,
        ''
      )
      for (var i = 0; i < imgsrcArr.length; i++) {
        const imgsrc = imgsrcArr[i]
        const index = i + 1
        const imgname = orderNo + address + name + index
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
    //查看售后详情
    openRefundDetails() {
      var id = this.temp.id
      this.$router.push({ name: 'refundDetails', params: { id: id } })
    },
    //生成打印单
    printOrderDetails() {
      this.$router.push({
        name: 'printOrderDetails',
        params: { id: this.orderId },
      })
    },
  },
}
</script>

<style lang="less" scoped>
.box {
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.12), 0 0 6px rgba(0, 0, 0, 0.04);
  padding: 10px;
}
.c-a {
  color: #0076c8;
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
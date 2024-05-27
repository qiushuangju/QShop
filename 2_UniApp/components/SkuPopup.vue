<template>

  <goods-sku-popup :value="value" @input="onChangeValue" border-radius="20" :localdata="goodsInfo" :mode="skuMode"
    :maskCloseAble="true" @open="openSkuPopup" @close="closeSkuPopup" @add-cart="addCart" @buy-now="buyNow"
    buyNowText="立即购买" />
</template>

<script>
  import { setCartTotalNum } from '@/core/app'
  import * as CartApi from '@/api/cart'
  import GoodsSkuPopup from '@/components/goods-sku-popup'

  export default {
    components: {
      GoodsSkuPopup
    },
    model: {
      prop: 'value',
      event: 'input'
    },
    props: {
      // true 组件显示 false 组件隐藏
      value: {
        Type: Boolean,
        default: false
      },
      // 模式 1:都显示 2:只显示购物车 3:只显示立即购买
      skuMode: {
        type: Number,
        default: 1
      },
      // 商品详情信息
      goods: {
        type: Object,
        default: {}
      },
    },

    data() {
      return {
        goodsInfo: {}
      }
    },

    created() {
      const app = this
	  console.log("33331111",JSON.stringify(app.goodsInfo.listSku))
      const { goods, skuMode} = app
	  console.log("skuMode",app.skuMode)
		var listSku=app.getListSpec();
		
		
      app.goodsInfo = {
        id: goods.id,
        name: goods.goodsName,
        urlImageMain: goods.urlImageMain,
        listSku: app.getlistSku(),
        listSpec: app.getListSpec(),
      }
	   console.log("333333333333",JSON.stringify(app.goodsInfo.listSku))
    },

    methods: {

      // 监听组件显示隐藏
      onChangeValue(val) {
        this.$emit('input', val)
      },

      // 整理商品SKU列表
      getlistSku() {
        const app = this
        const { goods: { goodsName, urlImageMain, listSku } } = app
        const skuData = []
        listSku.forEach(item => {
          skuData.push({
            // id: item.id,
            skuId: item.id,
            goodsId: item.goodsId,
            goodsName: goodsName,
            image: item.fileSkuImg == null ? urlImageMain: item.fileSkuImg.thumbnail ,
            price: item.salePrice * 100,
            stock: item.stockNum,
            specValueIds: item.specValueIds,
            arrSkuName:app.getArrSkuName( item.skuName)
          })
        })
        return skuData
      },

      // 获取sku记录的规格值列表
      // getSkuNameArr(specValueIds) {
      //   const app = this
      //   const defaultData = ['默认']
      //   const skuNameArr = []
      //   if (specValueIds) {
      //     specValueIds.forEach((valueId, groupIndex) => {
      //       const specValueName = app.getSpecValueName(valueId, groupIndex)
      //       skuNameArr.push(specValueName)
      //     })
      //   }
      //   return skuNameArr.length ? skuNameArr : defaultData
      // },
	  
	  getArrSkuName(strSkuName){
		 return strSkuName.split(",")
	  },

      // 获取指定的规格值名称
      getSpecValueName(valueId, groupIndex) {
        const app = this
        const { goods: { specList } } = app
        const res = specList[groupIndex].valueList.find(specValue => {
          return specValue.specValueId == valueId
        })
        return res.specValue
      },

      // 整理规格数据
      getListSpec() {
        const { goods: { specList } } = this
        const defaultData = [{ name: '默认', list: [{ name: '默认' }] }]
        const specData = []
		console.log("spec",specList)
        specList.forEach(group => {
          const children = []
          group.valueList.forEach(specValue => {
            children.push({valueId:specValue.specValueId, name: specValue.specValue })
          })
          specData.push({
			specId:group.specId,
            name: group.specName,
            list: children
          })
        })
        return specData.length ? specData : defaultData
      },

      // sku组件 开始-----------------------------------------------------------
      openSkuPopup() {
        // console.log("监听 - 打开sku组件")
      },

      closeSkuPopup() {
        // console.log("监听 - 关闭sku组件")
      },

      // 加入购物车按钮
      addCart(selectShop) {
        const app = this
        const { goodsId, skuId, buy_num } = selectShop
        CartApi.addCart(goodsId, skuId, buy_num)
          .then(res => {
            // 显示成功
            app.$toast(res.message)
            // 隐藏当前弹窗
            app.onChangeValue(false)
            // 购物车商品总数量
            const cartTotal = res.result.cartTotal
            // 缓存购物车数量
            setCartTotalNum(cartTotal)
            // 传递给父级
            app.$emit('addCart', cartTotal)
          })
      },

      // 立即购买
      buyNow(selectShop) {
        // 跳转到订单结算页
        this.$navTo('pages/checkout/index', {
          mode: 'buyNow',
          goodsId: selectShop.goodsId,
          goodsSkuId: selectShop.skuId,
          goodsNum: selectShop.buy_num
        })
        // 隐藏当前弹窗
        this.onChangeValue(false)
      }

    }
  }
</script>

<style lang="scss" scoped>

</style>

<template>
	<view class="goods-sku-popup" catchtouchmove="true" :class="getValue() && complete ? 'show' : 'none'"
		@touchmove.stop.prevent="moveHandle" @click.stop="stop">
		<!-- 页面内容开始 -->
		<view class="mask" @click="close('mask')"></view>
		<view class="layer attr-content" :style="{borderRadius: borderRadius + 'rpx ' + borderRadius + 'rpx 0 0'}">
			<view class="specification-wrapper">
				<scroll-view class="specification-wrapper-content" scroll-y="true">
					<view class="specification-header">
						<view class="specification-left">
							<image class="product-img"
								:src="selectShop.image ? selectShop.image : goodsInfo[urlImageMain]"
								:style="{backgroundColor: goodsThumbBackgroundColor}" mode="aspectFill"
								@click="previewImage"></image>
						</view>
						<view class="specification-right">
							<view class="price-content" :style="{color: themeColorFn('priceColor')}">
								<text class="sign">¥</text>
								<text class="price" :class="priceCom.length > 16 ? 'price2' : ''">{{ priceCom }}</text>
							</view>
							<view class="inventory" v-if="!hideStock">{{ stockText }}：{{ stockCom }}</view>
							<view class="inventory" v-else></view>
							<view class="choose" v-show="isManyCom">
								<text v-if="!selectArr.every(val => val == '')">已选：{{ selectArr.join(' ') }}</text>
							</view>
						</view>
					</view>

					<view class="specification-content">
						<view v-show="isManyCom" class="specification-item"
							v-for="(itemSpec, indexSpec) in goodsInfo[specListName]" :key="indexSpec">
							<!-- 规格项 -->
							<view class="item-title">{{ itemSpec.name }}</view>
							<view class="item-wrapper">
								<!--  <view class="item-content" v-for="(skuItem, indexSku) in item.list" :key="indexSku"
					:class="[skuItem.ishow ? '' : 'noactived',subIndex[indexSpec] == indexSku ? 'actived' : '']" 
					:style="[skuItem.ishow ? '' : themeColorFn('disableStyle'),skuItem.ishow ? themeColorFn('btnStyle') : ''
							,subIndex[indexSpec] == indexSku ? themeColorFn('activedStyle') : '']" @click="clickSku(skuItem, indexSpec, indexSku)">
                  {{ skuItem.name }}
                </view> -->
								<view class="item-content" v-for="(itemSpecValue, indexSpecValue) in itemSpec.list"
									:key="indexSpecValue"
									:class="[subIndex[indexSpec] == indexSpecValue ? 'actived' : '']"
									:style="[themeColorFn('btnStyle'),subIndex[indexSpec] == indexSpecValue ? themeColorFn('activedStyle') : '']"
									@click="clickSku(itemSpecValue, indexSpec, indexSpecValue)">
									{{ itemSpecValue.name }}
								</view>
							</view>
						</view>
						
						<view class="number-box-view">
							<view style="flex: 1;">数量</view>
							<view style="flex: 4;text-align: right;">
								<number-box v-model="selectNum" :min="minBuyNum || 1" :max="maxBuyNumCom"
									:step="stepBuyNum || 1" :step-strictly="stepStrictly" :positive-integer="true">
								</number-box>
							</view>
						</view>
					</view>
				</scroll-view>
				<view class="close" @click="close('close')" v-if="showClose != false">
					<image class="close-item" :src="closeImage"></image>
				</view>
			</view>

			<view class="btn-wrapper" v-if="outFoStock || mode == 4">
				<view class="sure" style="color:#ffffff;background-color:#cccccc">{{ noStockText }}</view>
			</view>
			<view class="btn-wrapper" v-else-if="mode == 1">
				<view class="sure add-cart" style="border-radius:38rpx 0rpx 0rpx 38rpx;" @click="addCart"
					:style="{color: themeColorFn('addCartColor'),backgroundColor: themeColorFn('addCartBackgroundColor')}">
					{{ addCartText }}
				</view>

				<view class="sure" style="border-radius:0rpx 38rpx 38rpx 0rpx;" @click="buyNow"
					:style="{color: themeColorFn('buyNowColor'),backgroundColor: themeColorFn('buyNowBackgroundColor')}">
					{{ buyNowText }}
				</view>
			</view>
			<view class="btn-wrapper" v-else-if="mode == 2">
				<view class="sure add-cart" @click="addCart"
					:style="{color: themeColorFn('addCartColor'),backgroundColor: themeColorFn('addCartBackgroundColor')}">
					{{ addCartText }}
				</view>
			</view>
			<view class="btn-wrapper" v-else-if="mode == 3">
				<view class="sure" @click="buyNow" :style="{
						color: themeColorFn('buyNowColor'),
						backgroundColor: themeColorFn('buyNowBackgroundColor')
					}">
					{{ buyNowText }}
				</view>
			</view>
		</view>
		<!-- 页面内容结束 -->
	</view>
</template>

<script>
	import NumberBox from './number-box'
	var vk; // vk依赖
	var goodsCache = {}; // 本地商品缓存
	export default {
		name: "GoodsSkuPopup",
		components: {
			NumberBox
		},
		emits: ["update:modelValue", "input", "update-goods", "open", "close", "add-cart", "buy-now"],
		props: {
			// true 组件显示 false 组件隐藏
			value: {
				Type: Boolean,
				default: false
			},
			modelValue: {
				Type: Boolean,
				default: false
			},
			// vk云函数路由模式参数开始-----------------------------------------------------------
			// 商品id
			goodsId: {
				Type: String,
				default: ""
			},
			// vk路由模式框架下的云函数地址
			action: {
				Type: String,
				default: ""
			},
			// vk云函数路由模式参数结束-----------------------------------------------------------
			// 该商品已抢完时的按钮文字
			noStockText: {
				Type: String,
				default: "该商品已抢完"
			},
			// 库存文字
			stockText: {
				Type: String,
				default: "库存"
			},
			// 商品表id的字段名
			goodsIdName: {
				Type: String,
				default: "id"
			},
			// sku表id的字段名
			skuIdName: {
				Type: String,
				default: "skuId"
			},
			// ListSku的字段名
			listSkuName: {
				Type: String,
				default: "listSku"
			},
			// specList的字段名
			specListName: {
				Type: String,
				default: "listSpec"
			},
			// 库存的字段名 默认 stock
			stockName: {
				Type: String,
				default: "stock"
			},
			// sku组合路径的字段名
			arrSkuName: {
				Type: String,
				default: "arrSkuName"
			},
			// 默认单规格时的规格组名称
			defaultSingleSkuName: {
				Type: String,
				default: "默认"
			},
			// 模式 1:都显示  2:只显示购物车 3:只显示立即购买 4:显示缺货按钮 默认 1
			mode: {
				Type: Number,
				default: 1
			},
			// 点击遮罩是否关闭组件 true 关闭 false 不关闭 默认true
			maskCloseAble: {
				Type: Boolean,
				default: true
			},
			// 顶部圆角值
			borderRadius: {
				Type: [String, Number],
				default: 0
			},
			// 商品缩略图字段名(未选择sku时)
			urlImageMain: {
				Type: [String],
				default: "urlImageMain"
			},
			// 商品缩略图背景颜色，如#999999
			goodsThumbBackgroundColor: {
				Type: String,
				default: "#999999"
			},
			// 最小购买数量 默认 1
			minBuyNum: {
				Type: [Number, String],
				default: 1
			},
			// 最大购买数量 默认 100000
			maxBuyNum: {
				Type: [Number, String],
				default: 100000
			},
			// 步进器步长 默认 1
			stepBuyNum: {
				Type: [Number, String],
				default: 1
			},
			// 是否只能输入 step 的倍数
			stepStrictly: {
				Type: Boolean,
				default: false
			},
			// 自定义获取商品信息的函数,支付宝小程序不支持该属性,请使用localdata属性
			customAction: {
				Type: [Function],
				default: null
			},
			// 本地数据源
			localdata: {
				type: Object
			},
			// 价格的字体颜色
			priceColor: {
				Type: String
			},
			// 立即购买按钮的文字
			buyNowText: {
				Type: String,
				default: "立即购买"
			},
			// 立即购买按钮的字体颜色
			buyNowColor: {
				Type: String
			},
			// 立即购买按钮的背景颜色
			buyNowBackgroundColor: {
				Type: String
			},
			// 加入购物车按钮的文字
			addCartText: {
				Type: String,
				default: "加入购物车"
			},
			// 加入购物车按钮的字体颜色
			addCartColor: {
				Type: String
			},
			// 加入购物车按钮的背景颜色
			addCartBackgroundColor: {
				Type: String
			},
			// 不可点击时,按钮的样式
			disableStyle: {
				Type: Object,
				default: null
			},
			// 按钮点击时的样式
			activedStyle: {
				Type: Object,
				default: null
			},
			// 按钮常态的样式
			btnStyle: {
				Type: Object,
				default: null
			},
			// 是否显示右上角关闭按钮
			showClose: {
				Type: Boolean,
				default: true
			},
			// 关闭按钮的图片地址 https://img.alicdn.com/imgextra/i1/121022687/O1CN01ImN0O11VigqwzpLiK_!!121022687.png
			closeImage: {
				Type: String,
				default: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACoAAAAqCAYAAADFw8lbAAAEyUlEQVR42sSZeWwNURTGp4OqtBo7sSXELragdkpQsRRJ1Zr4hyJiJ9YgxNIg1qANiT+E1i5IY0kVVWtQEbuEKLFGUSH27/ANN5PXmTvzupzkl/tm8t6b7517lnvvC0lKSjJ8WmnQAUSDFqABqALKgl8gD7wE90E2SAeXwFf1SxISErQeVtKHwCgwFsSDSIf3hYFKoCkYDBaDdyAViHdueHmoF6FtwDLQ23b/E7gM7oIcejIERIDaoBFoC8qA8mA8SQNz6W1XC9GY+nCQCCYAk/c+gF0gBZwH312+IxR0BCPBUIaH2A+wHsxHCHxx+gLT5QGN6a2JfG8uvVCDws9oiDQYlxkMGfHyQvARlADTwcXk5OT6foV2kS8ATXidymlcyen1a/Jjl9IJh3hPkjELYqO8Cu0KjjNZvtETw5jFBWXPmGSTGQKSeOn5iQ0kVLL0CINfPNcPbDMKyRCbGzEMBJ+ZD8cChYFdqGTqfsWT8otPGoVsEHsMwxDFs3shNsxJ6BrQ0Po8OGUUkVHsNCVml+cntB1jUWwn2GEUsTEMrASbDK+2CCQ0kYX6nfLLisMmKqUr0S60M+jG10vAm+JSCa8+x7CKlzHwaktV6DiObzUzPJIxFO1BQ12wGtTReO9GetVgY/kjNJzZbcWmTjHfxw51AsRqvL8eOAtmsJuFu3g1l+1ZLB5eDTVZ3K0P7tL0TkWOpSg61kVkBtuuNRthGs+wtJST5aQI7cEbkkRXNYVKgX6kIdYuUhYzMQwxN8tiExCLFqHNeSF9/aem0BzGp5PYQCJ7c/Gsk1RfuSD6U1dNpcDf9ZigTmKbMRZ9iVTsHscGJluW2FMf1SSQWGnBmaB6kCJVTVVNJZE++Cx9drEllS1KMCINpURFmEbBWA63Fz9s95cGIdJgp/zXmT4pZcOvSUzuZttTbblmnc3PIjjmidDXvKgdhMh0JdbzuCjWrbNOVovjS5P7bkPJ/mBESkz2BO0166ybNeJ431S2q+01NntuIq3E0amzjiZtk9tssWyTDzO4525bACK9NAUn68TtkNhpEXpOSagRml+S6iLSSeweHv242Qhl13rRyvoDvDlKyTQny/ZQJ+1iH7vVbEx7OR5UiKVIO7VicgvHCtwrudloMIV7/0uadVYW57O4Wvvi8v4pymlKkrpwvsDeLLZAY2pkwbAB3PSQfC+4cH7l4k1ZH8zkZRq8ecO+Z5rN40JJqnXFuGfaxPCTLjcn0OZOpnArXw8HY4paIbw5CcMgXq6HN2/mt6+XGLrN15tBryIUGavMpCTrfKcDCKkAceA9S8nhAOehhSUyhXpkBxxnP4YM1InugP7cBkjBPcqVUWFYCEROxXiQz5JlXV+IfKh7mpfJac+lZ6V87QXVClBkTc7YWsWTPSDyitfzUTlJlj8TbvE6jluDOdwZ+jX57GLO3ADeuyZrDYi86vV81FD2UVGsmT+5Zl0BnkhoseOEaogL46pqO4v/IqUEyalIR4h85BgjHv6+aUWRMbb7EstX6O0cpT1Gco0ry8fWygLDMjmDnQeBt3Qe7uVfkeugDwVLcsVzGsuwLXbV+I63XNAkG5r/hvgRqgqWs6pJPKrsbvz/Q6yyun0w/h6lP+BnzrCpfPMT2L8FGAA7k1GZ/vnaqAAAAABJRU5ErkJggg=="
			},
			// 是否隐藏库存显示
			hideStock: {
				Type: Boolean,
				default: false
			},
			// 颜色主题
			theme: {
				Type: String,
				default: "default"
			},
			// 请求中的提示
			actionTips: {
				Type: String,
				default: "请求中..."
			},
			// 默认选中的SKU
			defaultSelect: {
				Type: Object
			},
			// 是否使用缓存
			useCache: {
				Type: Boolean,
				default: true
			},
			/**
			 * 默认商品,设置该值可快速展示商品
			 * 逻辑: 先展示 defaultGoods 信息,再取数据库,再更新页面(通常为更新库存)
			 */
			defaultGoods: {
				Type: Object
			},
			/**
			 * 金额是否需要除以100
			 * 1:金额会除以100
			 * 0:金额不会除以100
			 */
			amountType: {
				Type: Number,
				default: 1
			},
			// 每次选择完SKU后，购买数量归1，如果有最小购买数量，则设置为最小购买数量
			selectedInit: {
				Type: Boolean,
				default: false
			},
		},
		data() {
			return {
				complete: false, // 组件是否加载完成
				goodsInfo: {}, // 商品信息
				isShow: false, // true 显示 false 隐藏
				initKey: true, // 是否已初始化
				shopItemInfo: {}, // 存放要和选中的值进行匹配的数据
				selectArr: [], // 存放被选中的值
				subIndex: [], // 是否选中 因为不确定是多规格还是单规格，所以这里定义数组来判断
				selectShop: {}, // 存放最后选中的商品
				selectNum: this.minBuyNum || 1, // 选中数量
				outFoStock: false, // 是否全部sku都缺货
				openTime: 0,
				themeColor: {
					// 默认主题
					default: {
						priceColor: "rgb(254, 86, 10)",
						buyNowColor: "#ffffff",
						buyNowBackgroundColor: "rgb(254, 86, 10)",
						addCartColor: "#ffffff",
						addCartBackgroundColor: "rgb(255, 148, 2)",
						btnStyle: {
							color: "#333333",
							borderColor: "#f4f4f4",
							backgroundColor: "#ffffff"
						},
						activedStyle: {
							color: "rgb(254, 86, 10)",
							borderColor: "rgb(254, 86, 10)",
							backgroundColor: "rgba(254,86,10,0.1)"
						},
						disableStyle: {
							color: "#c3c3c3",
							borderColor: "#f6f6f6",
							backgroundColor: "#f6f6f6"
						}
					},
					// 红黑主题
					"red-black": {
						priceColor: "rgb(255, 68, 68)",
						buyNowColor: "#ffffff",
						buyNowBackgroundColor: "rgb(255, 68, 68)",
						addCartColor: "#ffffff",
						addCartBackgroundColor: "rgb(85, 85, 85)",
						activedStyle: {
							color: "rgb(255, 68, 68)",
							borderColor: "rgb(255, 68, 68)",
							backgroundColor: "rgba(255,68,68,0.1)"
						}
					},
					// 黑白主题
					"black-white": {
						priceColor: "rgb(47, 47, 52)",
						buyNowColor: "#ffffff",
						buyNowBackgroundColor: "rgb(47, 47, 52)",
						addCartColor: "rgb(47, 47, 52)",
						addCartBackgroundColor: "rgb(235, 236, 242)",
						// btnStyle:{
						// 	color:"rgb(47, 47, 52)",
						// 	borderColor:"rgba(235,236,242,0.5)",
						// 	backgroundColor:"rgba(235,236,242,0.5)",
						// },
						activedStyle: {
							color: "rgb(47, 47, 52)",
							borderColor: "rgba(47,47,52,0.12)",
							backgroundColor: "rgba(47,47,52,0.12)"
						}
					},
					// 咖啡色主题
					coffee: {
						priceColor: "rgb(195, 167, 105)",
						buyNowColor: "#ffffff",
						buyNowBackgroundColor: "rgb(195, 167, 105)",
						addCartColor: "rgb(195, 167, 105)",
						addCartBackgroundColor: "rgb(243, 238, 225)",
						activedStyle: {
							color: "rgb(195, 167, 105)",
							borderColor: "rgb(195, 167, 105)",
							backgroundColor: "rgba(195, 167, 105,0.1)"
						}
					},
					// 浅绿色主题
					green: {
						priceColor: "rgb(99, 190, 114)",
						buyNowColor: "#ffffff",
						buyNowBackgroundColor: "rgb(99, 190, 114)",
						addCartColor: "rgb(99, 190, 114)",
						addCartBackgroundColor: "rgb(225, 244, 227)",
						activedStyle: {
							color: "rgb(99, 190, 114)",
							borderColor: "rgb(99, 190, 114)",
							backgroundColor: "rgba(99, 190, 114,0.1)"
						}
					}
				}
			};
		},
		created() {
			let that = this;
			vk = that.vk;
			if (that.getValue()) {
				that.open();
			}
		},
		mounted() {},
		methods: {
			// 初始化
			init(notAutoClick) {
				let that = this;
				// 清空之前的数据
				that.selectArr = [];
				that.subIndex = [];
				that.selectShop = {};
				that.selectNum = that.minBuyNum || 1;
				that.outFoStock = false;
				that.shopItemInfo = {};
				let specListName = that.specListName;
				console.log("init", JSON.stringify(that.goodsInfo))
				that.goodsInfo[specListName].map(item => {
					that.selectArr.push("");
					that.subIndex.push(-1);
				});
				that.checkItem(); // 计算sku里面规格形成路径
				that.checkInpath(-1); // 传-1是为了不跳过循环
				if (!notAutoClick) that.autoClickSku(); // 自动选择sku策略
			},
			getValue() {
				// #ifndef VUE3
				return this.value;
				// #endif

				// #ifdef VUE3
				return this.modelValue;
				// #endif
			},
			// 使用vk路由模式框架获取商品信息
			findGoodsInfo(obj = {}) {
				let that = this;
				let {
					useCache
				} = obj;
				if (typeof vk == "undefined") {
					that.toast("custom-action必须是function", "none");
					return false;
				}
				let {
					actionTips
				} = that;
				let actionTitle = "";
				let actionAoading = false;
				if (actionTips !== "custom") {
					actionTitle = useCache ? "" : "请求中...";
				} else {
					actionAoading = useCache ? false : true;
				}
				vk.callFunction({
					url: that.action,
					title: actionTitle,
					loading: actionAoading,
					data: {
						goods_id: that.goodsId
					},
					success(data) {
						that.updateGoodsInfo(data.goodsInfo);
						// 更新缓存
						goodsCache[that.goodsId] = data.goodsInfo;
						that.$emit("update-goods", data.goodsInfo);
					},
					fail() {
						that.updateValue(false);
					}
				});
			},
			updateValue(value) {
				let that = this;
				if (value) {
					that.$emit("open", true);
					that.$emit("input", true);
					that.$emit("update:modelValue", true);
				} else {
					that.$emit("input", false);
					that.$emit("close", "close");
					that.$emit("update:modelValue", false);
				}
			},
			// 更新商品信息(库存、名称、图片)
			updateGoodsInfo(goodsInfo) {
				let that = this;
				// goodsInfo.sku_list.map((item, index) => {
				// 	item.arrSkuName = ["20ml/瓶"];
				// });
				let {
					listSkuName
				} = that;
				if (
					JSON.stringify(that.goodsInfo) === "{}" ||
					that.goodsInfo[that.goodsIdName] !== goodsInfo[that.goodsIdName]
				) {
					that.goodsInfo = goodsInfo;
					that.initKey = true;
				} else {
					that.goodsInfo[listSkuName] = goodsInfo[listSkuName];
				}
				if (that.initKey) {
					that.initKey = false;
					that.init(that.isManyCom);
				}
				// 更新选中sku的库存信息
				let select_sku_info = that.getListItem(
					that.goodsInfo[listSkuName],
					that.skuIdName,
					that.selectShop[that.skuIdName]
				);
				Object.assign(that.selectShop, select_sku_info);
				that.defaultSelectSku();
				that.complete = true;
			},
			async open() {
				let that = this;
				that.openTime = new Date().getTime();
				let findGoodsInfoRun = true;
				let listSkuName = that.listSkuName;
				// 先获取缓存中的商品信息
				let useCache = false;
				let goodsInfo = goodsCache[that.goodsId];
				if (goodsInfo && that.useCache) {
					useCache = true;
					that.updateGoodsInfo(goodsInfo);
				} else {
					that.complete = false;
				}
				if (that.customAction && typeof that.customAction === "function") {
					try {
						goodsInfo = await that
							.customAction({
								useCache,
								goodsId: that.goodsId,
								goodsInfo,
								close: function() {
									setTimeout(function() {
										that.close();
									}, 500);
								}
							})
							.catch(err => {
								setTimeout(function() {
									that.close();
								}, 500);
							});
					} catch (err) {
						let {
							message = ""
						} = err;
						if (message.indexOf(".catch is not a function") > -1) {
							that.toast("custom-action必须返回一个Promise", "none");
							setTimeout(function() {
								that.close();
							}, 500);
							return false;
						}
					}
					// 更新缓存
					goodsCache[that.goodsId] = goodsInfo;
					if (goodsInfo && typeof goodsInfo == "object" && JSON.stringify(goodsInfo) != "{}") {
						findGoodsInfoRun = false;
						that.updateGoodsInfo(goodsInfo);
						that.updateValue(true);
					} else {
						that.toast("未获取到商品信息", "none");
						that.$emit("input", false);
						return false;
					}
				} else if (typeof that.localdata !== "undefined" && that.localdata !== null) {
					goodsInfo = that.localdata;
					if (goodsInfo && typeof goodsInfo == "object" && JSON.stringify(goodsInfo) != "{}") {
						findGoodsInfoRun = false;
						that.updateGoodsInfo(goodsInfo);
						that.updateValue(true);
					} else {
						that.toast("未获取到商品信息", "none");
						that.$emit("input", false);
						return false;
					}
				} else {
					if (findGoodsInfoRun) that.findGoodsInfo({
						useCache
					});
				}
			},
			// 监听 - 弹出层收起
			close(s) {
				let that = this;
				if (new Date().getTime() - that.openTime < 400) {
					return false;
				}
				if (s == "mask") {
					if (that.maskCloseAble !== false) {
						that.$emit("input", false);
						that.$emit("close", "mask");
						that.$emit("update:modelValue", false);
					}
				} else {
					that.$emit("input", false);
					that.$emit("close", "close");
					that.$emit("update:modelValue", false);
				}
			},
			moveHandle() {
				//禁止父元素滑动
			},
			// sku按钮的点击事件
			// clickSku(itemSpecValue, indexSpec{

			clickSku(itemSpecValue, indexSpec, indexSpecValue) {
				let that = this;
				// if (skuItem.ishow) {
console.log("xxx",JSON.stringify(that.selectArr)+itemSpecValue.name+indexSpec)
				if (that.selectArr[indexSpec] != itemSpecValue.name) {
					that.$set(that.selectArr, indexSpec, itemSpecValue.name);
					that.$set(that.subIndex, indexSpec, indexSpecValue);
				} else {
					that.$set(that.selectArr, indexSpec, "");
					that.$set(that.subIndex, indexSpec, -1);
				}

				console.log("clickSku", JSON.stringify(that.selectArr) + JSON.stringify(that.subIndex))
				that.checkInpath(indexSpec);
				// 如果全部选完
				that.checkSelectShop();
				// }
			},
			// 检查路径
			checkInpath(clickIndex) {
				let that = this;
				let specListName = that.specListName;
				//console.time('筛选可选路径需要的时间是');
				//循环所有属性判断哪些属性可选
				//当前选中的兄弟节点和已选中属性不需要循环
				let specList = that.goodsInfo[specListName];
				for (let i = 0, len = specList.length; i < len; i++) {
					if (i == clickIndex) {
						continue;
					}
					let len2 = specList[i].list.length;
					for (let j = 0; j < len2; j++) {
						if (that.subIndex[i] != -1 && j == that.subIndex[i]) {
							continue;
						}
						let choosed_copy = [...that.selectArr];
						that.$set(choosed_copy, i, specList[i].list[j].name);
						let choosed_copy2 = choosed_copy.filter(
							item => item !== "" && typeof item !== "undefined"
						);
						if (that.shopItemInfo.hasOwnProperty(that.getArrayToSting(choosed_copy2))) {
							specList[i].list[j].ishow = true;
						} else {
							specList[i].list[j].ishow = false;
						}
					}
				}
				that.$set(that.goodsInfo, specListName, specList);
				console.log("checkInpath",that.goodsInfo);
			},

			// 检测是否已经选完sku
			checkSelectShop() {
				let that = this;
				// 如果全部选完
				if (that.selectArr.every(item => item != "")) {

					console.log("1111", that.selectArr + "" + that.getArrayToSting(that.selectArr));

					that.selectShop = that.shopItemInfo[that.getArrayToSting(that.selectArr)];

					console.log("2222", JSON.stringify(that.shopItemInfo));
					console.log("3333", that.stockName);

					let stock = that.selectShop[that.stockName];
					if (typeof stock !== "undefined" && that.selectNum > stock) {
						that.selectNum = stock;
					}
					if (that.selectNum > that.maxBuyNum) {
						that.selectNum = that.maxBuyNum;
					}
					if (that.selectNum < that.minBuyNum) {
						that.selectNum = that.minBuyNum;
					}
					if (that.selectedInit) {
						that.selectNum = that.minBuyNum || 1;
					}
				} else {
					that.selectShop = {};
				}
			},

			// 计算sku里面规格形成路径
			checkItem() {
				let that = this;
				// console.time('计算有多小种可选路径需要的时间是');
				let { stockName } = that;
				let listSkuName = that.listSkuName;
				// 去除库存小于等于0的商品sku
				let originallistSku = that.goodsInfo[listSkuName];
				let listSku = [];
				let stockNum = 0;
				originallistSku.map((skuItem, index) => {
				  if (skuItem[stockName] > 0) {
				    listSku.push(skuItem);
				    stockNum += skuItem[stockName];
				  }
				});
				if (stockNum <= 0) {
				  that.outFoStock = true;
				}
				// 计算有多小种可选路径
				
				console.log("listSku",listSku)
				let result = listSku.reduce(
				  (arrs, items) => {
					  console.log("111",items[that.arrSkuName])
				    return arrs.concat(
				      items[that.arrSkuName].reduce(
				        (arr, item) => {
				          return arr.concat(
				            arr.map(item2 => {
				              // 利用对象属性的唯一性实现二维数组去重
				              console.log(1,that.shopItemInfo,that.getArrayToSting([...item2, item]),item2,item,items);
				              if (!that.shopItemInfo.hasOwnProperty(that.getArrayToSting([...item2, item]))) {
				                that.shopItemInfo[that.getArrayToSting([...item2, item])] = items;
				              }
				              return [...item2, item];
				            })
				          );
				        },
				        [
				          []
				        ]
				      )
				    );
				  },
				  [
				    []
				  ]
				);
				console.timeEnd('计算有多小种可选路径需要的时间是');
			},
			getArrayToSting(arr) {
				let str = "";
				arr.map((item, index) => {
					item = item.replace(/\./g, "。");
					if (index == 0) {
						str += item;
					} else {
						str += "," + item;
					}
				});
				return str;
			},
			// 检测sku选项是否已全部选完,且有库存
			checkSelectComplete(obj = {}) {
				let that = this;
				let clickTime = new Date().getTime();
				if (that.clickTime && clickTime - that.clickTime < 400) {
					return false;
				}
				that.clickTime = clickTime;
				let {
					selectShop,
					selectNum,
					stockText,
					stockName
				} = that;

				console.log("checkSelectComplete", JSON.stringify(selectShop) + "," + that.skuIdName)
				if (!selectShop || !selectShop[that.skuIdName]) {
					that.toast("请先选择对应规格", "none");
					return false;
				}
				if (selectNum <= 0) {
					that.toast("购买数量必须>0", "none");
					return false;
				}
				// 判断库存
				if (selectNum > selectShop[stockName]) {
					that.toast(stockText + "不足", "none");
					return false;
				}
				if (typeof obj.success == "function") obj.success(selectShop);
			},
			// 加入购物车
			addCart() {
				let that = this;
				that.checkSelectComplete({
					success: function(selectShop) {
						selectShop.buy_num = that.selectNum;
						that.$emit("add-cart", selectShop);
						// setTimeout(function() {
						// 	that.init();
						// }, 300);
					}
				});
			},
			// 立即购买
			buyNow() {
				let that = this;
				that.checkSelectComplete({
					success: function(selectShop) {
						selectShop.buy_num = that.selectNum;
						that.$emit("buy-now", selectShop);
					}
				});
			},
			// 弹窗
			toast(title, icon) {
				uni.showToast({
					title: title,
					icon: icon
				});
			},
			// 获取对象数组中的某一个item,根据指定的键值
			getListItem(list, key, value) {
				let that = this;
				let item;
				for (let i in list) {
					if (typeof value == "object") {
						if (JSON.stringify(list[i][key]) === JSON.stringify(value)) {
							item = list[i];
							break;
						}
					} else {
						if (list[i][key] === value) {
							item = list[i];
							break;
						}
					}
				}
				return item;
			},
			getListIndex(list, key, value) {
				let that = this;
				let index = -1;
				for (let i = 0; i < list.length; i++) {
					if (list[i][key] === value) {
						index = i;
						break;
					}
				}
				return index;
			},
			// 自动选择sku前提是只有一组sku,默认自动选择最前面的有库存的sku
			autoClickSku() {
				let that = this;
				let listSku = that.goodsInfo[that.listSkuName];
				let specListArr = that.goodsInfo[that.specListName];
				if (specListArr.length == 1) {
					let specList = specListArr[0].list;
					for (let i = 0; i < specList.length; i++) {
						let sku = that.getListItem(listSku, that.arrSkuName, [specList[i].name]);
						if (sku) {
							that.clickSku(specList[i], 0, i);
							break;
						}
					}
				}
			},
			// 主题颜色
			themeColorFn(name) {
				let that = this;
				let {
					theme,
					themeColor
				} = that;
				let color = that[name] ? that[name] : themeColor[theme][name];
				return color;
			},
			defaultSelectSku() {
				let that = this;
				let {
					defaultSelect
				} = that;
				if (defaultSelect && defaultSelect.sku && defaultSelect.sku.length > 0) {
					that.selectSku(defaultSelect);
				}
			},
			/**
				 * 主动方法 - 设置sku
				that.$refs.skuPopup.selectSku({
					sku:["红色","256G","公开版"],
					num:5
				});
				 */
			selectSku(obj = {}) {
				let that = this;
				let {
					sku: skuArr,
					num: selectNum
				} = obj;
				let specListArr = that.goodsInfo[that.specListName];
				if (skuArr && specListArr.length === skuArr.length) {
					// 先清空
					let clickSkuArr = [];
					let clickKey = true;
					for (let index = 0; index < skuArr.length; index++) {
						let skuName = skuArr[index];
						let specList = specListArr[index].list;
						let indexSpec = index;
						let indexSku = that.getListIndex(specList, "name", skuName);
						if (indexSku == -1) {
							clickKey = false;
							break;
						}
						clickSkuArr.push({
							spec: specList[indexSku],
							indexSpec: indexSpec,
							indexSku: indexSku
						});
					}
					if (clickKey) {
						that.init(true);
						clickSkuArr.map(item => {
							that.clickSku(item.spec, item.indexSpec, item.indexSku);
						});
					}
				}
				if (selectNum > 0) that.selectNum = selectNum;
			},
			priceFilter(n = 0) {
				let that = this;
				if (typeof n == "string") {
					n = parseFloat(n);
				}
				if (that.amountType === 0) {
					return n.toFixed(2);
				} else {
					return (n / 100).toFixed(2);
				}
			},
			pushGoodsCache(goodsInfo) {
				let that = this;
				let {
					goodsIdName
				} = that;
				goodsCache[goodsInfo[goodsIdName]] = goodsInfo;
			},
			// 用于阻止冒泡
			stop() {},
			// 图片预览
			previewImage() {
				let that = this;
				let {
					selectShop,
					goodsInfo,
					urlImageMain
				} = that;
				let src = selectShop.image ? selectShop.image : goodsInfo[urlImageMain];
				if (src) {
					uni.previewImage({
						urls: [src]
					});
				}
			}
		},
		// 计算属性
		computed: {
			// 最大购买数量
			maxBuyNumCom() {
				let that = this;
				let max = that.maxBuyNum || 100000;
				let stockName = that.stockName;
				if (that.selectShop && typeof that.selectShop[stockName] !== "undefined") {
					// 最大购买量不能超过当前商品的库存
					if (max > that.selectShop[stockName]) {
						max = that.selectShop[stockName];
					}
				}
				return max;
			},
			// 是否是多规格
			isManyCom() {
				let that = this;
				let {
					goodsInfo,
					defaultSingleSkuName,
					specListName
				} = that;
				let isMany = true;
				if (
					goodsInfo[specListName] &&
					goodsInfo[specListName].length === 1 &&
					goodsInfo[specListName][0].list.length === 1 &&
					goodsInfo[specListName][0].name === defaultSingleSkuName
				) {
					isMany = false;
				}
				return isMany;
			},
			// 默认价格区间计算
			priceCom() {
				let str = "";
				let that = this;
				let {
					selectShop = {}, goodsInfo = {}, listSkuName, skuIdName
				} = that;
				if (selectShop[skuIdName]) {
					str = that.priceFilter(selectShop.price);
				} else {
					let listSku = goodsInfo[listSkuName];
					if (listSku && listSku.length > 0) {
						let valueArr = [];
						listSku.map((skuItem, index) => {
							valueArr.push(skuItem.price);
						});
						let min = that.priceFilter(Math.min(...valueArr));
						let max = that.priceFilter(Math.max(...valueArr));
						if (min === max) {
							str = min + "";
						} else {
							str = `${min} - ${max}`;
						}
					}
				}
				return str;
			},
			// 库存显示
			stockCom() {
				let str = "";
				let that = this;
				let {
					selectShop = {}, goodsInfo = {}, listSkuName, stockName
				} = that;
				if (selectShop[stockName]) {
					str = selectShop[stockName];
				} else {
					let listSku = goodsInfo[listSkuName];
					if (listSku && listSku.length > 0) {
						let valueArr = [];
						listSku.map((skuItem, index) => {
							valueArr.push(skuItem[stockName]);
						});
						let min = Math.min(...valueArr);
						let max = Math.max(...valueArr);
						if (min === max) {
							str = min;
						} else {
							str = `${min} - ${max}`;
						}
					}
				}
				return str;
			},
		},
		watch: {
			value(newVal, oldValue) {
				let that = this;
				if (newVal) {
					that.open();
				}
			},
			modelValue(newVal, oldValue) {
				let that = this;
				if (newVal) {
					that.open();
				}
			},
			defaultGoods: {
				immediate: true,
				handler: function(newVal, oldValue) {
					let that = this;
					let {
						goodsIdName
					} = that;
					if (
						typeof newVal === "object" &&
						newVal &&
						newVal[goodsIdName] &&
						!goodsCache[newVal[goodsIdName]]
					) {
						that.pushGoodsCache(newVal);
					}
				}
			}
		}
	};
</script>

<style lang="scss" scoped>
	/*  sku弹出层 */
	.goods-sku-popup {
		position: fixed;
		left: var(--window-left);
		right: var(--window-right);
		bottom: var(--window-bottom);
		top: var(--window-top);
		z-index: 21;
		overflow: hidden;

		&.show {
			display: block;

			.mask {
				animation: showPopup 0.2s linear both;
			}

			.layer {
				animation: showLayer 0.2s linear both;
				bottom: var(--window-bottom);
			}
		}

		&.hide {
			.mask {
				animation: hidePopup 0.2s linear both;
			}

			.layer {
				animation: hideLayer 0.2s linear both;
			}
		}

		&.none {
			display: none;
		}

		.mask {
			position: fixed;
			left: var(--window-left);
			right: var(--window-right);
			top: 0;
			height: 100%;
			z-index: 1;
			background-color: rgba(0, 0, 0, 0.65);
		}

		.layer {
			display: flex;
			flex-direction: column;
			position: fixed;
			left: var(--window-left);
			right: var(--window-right);
			bottom: var(--window-bottom);
			z-index: 99;
			border-radius: 10rpx 10rpx 0 0;
			background-color: #fff;
			// 设置ios刘海屏底部横线安全区域
			padding-bottom: constant(safe-area-inset-bottom);
			padding-bottom: env(safe-area-inset-bottom);

			.specification-wrapper {
				width: 100%;
				padding: 30rpx 25rpx;
				box-sizing: border-box;

				.specification-wrapper-content {
					width: 100%;
					max-height: 900rpx;
					min-height: 300rpx;

					&::-webkit-scrollbar {
						/*隐藏滚轮*/
						display: none;
					}

					.specification-header {
						width: 100%;
						display: flex;
						flex-direction: row;
						position: relative;
						margin-bottom: 40rpx;

						.specification-left {
							width: 180rpx;
							height: 180rpx;
							flex: 0 0 180rpx;

							.product-img {
								width: 180rpx;
								height: 180rpx;
							}
						}

						.specification-right {
							flex: 1;
							padding: 0 35rpx 10rpx 28rpx;
							box-sizing: border-box;
							display: flex;
							flex-direction: column;
							justify-content: flex-end;
							font-weight: 500;

							.price-content {
								color: #fe560a;
								margin-bottom: 10rpx;

								.sign {
									font-size: 28rpx;
									margin-right: 4rpx;
								}

								.price {
									margin-left: 4rpx;
									font-size: 48rpx;
								}

								.price2 {
									margin-left: 4rpx;
									font-size: 36rpx;
								}
							}

							.inventory {
								font-size: 24rpx;
								color: #525252;
								margin-bottom: 14rpx;
							}

							.choose {
								font-size: 24rpx;
								color: #525252;
								min-height: 32rpx;
							}
						}
					}

					.specification-content {
						font-weight: 500;

						.specification-item {
							margin-bottom: 40rpx;

							&:last-child {
								margin-bottom: 0;
							}

							.item-title {
								margin-bottom: 20rpx;
								font-size: 28rpx;
								color: #999999;
							}

							.item-wrapper {
								display: flex;
								flex-direction: row;
								flex-flow: wrap;
								margin-bottom: -20rpx;

								.item-content {
									display: inline-block;
									padding: 10rpx 35rpx;
									font-size: 24rpx;
									border-radius: 10rpx;
									background-color: #ffffff;
									color: #333333;
									margin-right: 20rpx;
									margin-bottom: 20rpx;
									border: 2rpx solid #f4f4f4;
									box-sizing: border-box;

									&.actived {
										border-color: #fe560a;
										color: #fe560a;
									}

									&.noactived {
										// background-color: #f6f6f6;
										// border-color: #f6f6f6;
										// color: #c3c3c3;
										color: #c8c9cc;
										background: #f2f3f5;
										border-color: #f2f3f5;
									}
								}
							}
						}

						.number-box-view {
							display: flex;
							padding-top: 20rpx;
						}
					}
				}

				.close {
					position: absolute;
					top: 30rpx;
					right: 25rpx;
					width: 50rpx;
					height: 50rpx;
					text-align: center;
					line-height: 50rpx;

					.close-item {
						width: 40rpx;
						height: 40rpx;
					}
				}
			}

			.btn-wrapper {
				display: flex;
				width: 100%;
				height: 120rpx;
				flex: 0 0 120rpx;
				align-items: center;
				justify-content: space-between;
				padding: 0 26rpx;
				box-sizing: border-box;

				.layer-btn {
					width: 335rpx;
					height: 76rpx;
					border-radius: 38rpx;
					color: #fff;
					line-height: 76rpx;
					text-align: center;
					font-weight: 500;
					font-size: 28rpx;

					&.add-cart {
						background: #ffbe46;
					}

					&.buy {
						background: #fe560a;
					}
				}

				.sure {
					margin: 0 auto;
					width: 95%;
					max-width: 1200rpx;
					height: 80rpx;
					border-radius: 38rpx;
					color: #fff;
					line-height: 80rpx;
					text-align: center;
					font-weight: 500;
					font-size: 28rpx;
					background: #fe560a;
				}

				.sure.add-cart {
					background: #ff9402;
				}
			}
		}

		@keyframes showPopup {
			0% {
				opacity: 0;
			}

			100% {
				opacity: 1;
			}
		}

		@keyframes hidePopup {
			0% {
				opacity: 1;
			}

			100% {
				opacity: 0;
			}
		}

		@keyframes showLayer {
			0% {
				transform: translateY(120%);
			}

			100% {
				transform: translateY(0%);
			}
		}

		@keyframes hideLayer {
			0% {
				transform: translateY(0);
			}

			100% {
				transform: translateY(120%);
			}
		}
	}
</style>
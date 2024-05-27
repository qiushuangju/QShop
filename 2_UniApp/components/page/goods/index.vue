<template>
	<!-- 商品组 -->
	<view class="diy-goods">

		<view class="goods-list" :style="{ background: itemStyle.background }"
			:class="[`display__${itemStyle.display}`, `column__${itemStyle.column}`]">

			<view class="module-item-title" v-if="itemStyle.titleShow">
				<view class="tit">{{itemStyle.titleText}}</view>
				<view class="mo" @click="onLink(itemStyle.titleMoreLink)">
					{{itemStyle.titleMoreText}} <text class="iconfont icon-arrow-right"></text>
				</view>
			</view>
			<scroll-view :scroll-x="itemStyle.display === 'slide'">
				<view class="goods-item" v-for="(dataItem, index) in dataListData" :key="index"
					@click="onTargetGoods(dataItem.id)">

					<!-- 单列商品 -->
					<block v-if="itemStyle.column === 1">
						<view class="dis-flex">
							<!-- 商品图片 -->
							<view class="goods-item_left">
								<image class="image" :src="dataItem.urlImageMain"></image>
							</view>
							<view class="goods-item_right">
								<!-- 商品名称 -->
								<view v-if="itemStyle.show.includes('goodsName')" class="goods-name">
									<text class="twoline-hide">{{ dataItem.goodsName }}</text>
								</view>
								<view class="goods-item_desc">
									<!-- 商品卖点 -->
									<view v-if="itemStyle.show.includes('sellingPoint')"
										class="desc-selling_point dis-flex">
										<text class="oneline-hide">{{ dataItem.subTitle }}</text>
									</view>
									<!-- 商品销量 -->
									<view v-if="itemStyle.show.includes('goodsSales')"
										class="desc-goods_sales dis-flex">
										<text>已售{{ dataItem.goodsSales }}件</text>
									</view>
									<!-- 商品价格 -->
									<view class="desc_footer">
										<text v-if="itemStyle.show.includes('salePrice')"
											class="price_x">¥{{dataItem.salePrice }}</text>
										<text class="price_y col-9"
											v-if="itemStyle.show.includes('linePrice') && dataItem.linePrice > 0">¥{{ dataItem.linePrice }}</text>
									</view>
								</view>
							</view>
						</view>
					</block>
					<!-- 多列商品 -->
					<block v-else>
						<!-- 商品图片 -->
						<view class="goods-image">
							<image class="image" mode="aspectFill" :src="dataItem.urlImageMain"></image>
						</view>
						<view class="detail">
							<!-- 商品标题 -->
							<view v-if="itemStyle.show.includes('goodsName')" class="goods-name">
								<text class="twoline-hide">{{ dataItem.goodsName }}</text>
							</view>
							<!-- 商品价格 -->
							<view class="detail-price oneline-hide">
								<text v-if="itemStyle.show.includes('salePrice')"
									class="goods-price f-30 col-m">￥{{ dataItem.salePrice }}</text>
								<text v-if="itemStyle.show.includes('linePrice') && dataItem.linePrice > 0"
									class="line-price col-9 f-24">￥{{ dataItem.linePrice }}</text>
							</view>
						</view>
					</block>

				</view>
			</scroll-view>
		</view>
	</view>
</template>

<script>
	import mixin from '../mixin';
	import * as goodsApi from '@/api/goods/goods';
	export default {
		name: "Goods",
		/**
		 * 组件的属性列表
		 * 用于组件自定义设置
		 */
		props: {
			itemIndex: String,
			itemStyle: Object,
			params: Object,
			dataList: Array
		},
		mixins: [mixin],
		data() {
			return {
				paramsData: this.params, // 当前banne所在滑块指针
				dataListData: this.dataList,
				queryParams: {}
			};
		},
		created() {
			if (this.paramsData.source == "auto") {
				var autoData = this.paramsData.auto;
				var cateId = this.paramsData.auto.category

				this.queryParams = {
					cateIdAllLevel: autoData.category,
					SortCol: autoData.goodsSort,
					Page: 1,
					Limit: autoData.showNum
				}
				this.getAutoGoods()
			}
			// console.log("111111",this.paramsData.source)

		},
		/**
		 * 组件的方法列表
		 * 更新属性和数据的方法与更新页面数据的方法类似
		 */
		methods: {

			/**
			 * 跳转商品详情页
			 */
			onTargetGoods(goodsId) {
				console.log("goodsId", goodsId)
				this.$navTo(`pages/goods/detail`, {
					goodsId
				})
			},
			/**
			 * 加载页面数据
			 * @param {Object} callback
			 */
			getAutoGoods(callback) {
				const app = this
				const pageType = 10
				goodsApi.load(this.queryParams)
					.then(res => {
						console.log("res", res)
						this.dataListData = res.result;
					})
					.finally(() => callback && callback())
			},

		}

	}
</script>

<style lang="scss" scoped>
	.module-item-title {
		background: #fff;
		z-index: 1;
		font-size: 14px;
		padding: 14px 12px;
		// border-bottom: 1px solid #eee;
		display: flex;
		justify-content: space-between;
		align-items: center;
		border-radius: 20rpx;

		.tit {
			font-size: 34rpx;
			line-height: 34rpx;
			font-weight: bold;
		}

		.mo {
			font-size: 24rpx;
			font-weight: 400;
			color: #999;
		}
	}

	.icon-arrow-right {
		// position: absolute;
		// top: 26rpx;
		// right: 6rpx;
	}

	.diy-goods {
		.goods-list {
			margin: 15rpx;
			width: 96%;
			box-sizing: border-box;
			border-radius: 20rpx;

			.goods-item {
				box-sizing: border-box;
				padding: 6rpx;

				.goods-image {
					position: relative;
					width: 100%;
					height: 0;
					padding-bottom: 100%;
					overflow: hidden;
					background: #fff;

					&:after {
						content: '';
						display: block;
						margin-top: 100%;
					}

					.image {
						position: absolute;
						width: 100%;
						height: 100%;
						top: 0;
						left: 0;
						-o-object-fit: cover;
						object-fit: cover;
					}
				}

				.detail {
					padding: 8rpx;
					background: #fff;

					.goods-name {
						min-height: 68rpx;
						line-height: 1.3;
						white-space: normal;
						color: #484848;
						font-size: 26rpx;
					}

					.detail-price {
						.goods-price {
							margin-right: 8rpx;
						}

						.line-price {
							text-decoration: line-through;
						}
					}
				}
			}

			&.display__slide {
				white-space: nowrap;
				font-size: 0;

				.goods-item {
					display: inline-block;
				}
			}

			&.display__list {
				.goods-item {
					float: left;
				}
			}

			&.column__2 {
				.goods-item {
					width: 50%;
				}
			}

			&.column__3 {
				.goods-item {
					width: 33.33333%;
				}
			}

			&.column__1 {
				.goods-item {
					width: 100%;
					height: 280rpx;
					margin-bottom: 12rpx;
					padding: 20rpx;
					box-sizing: border-box;
					background: #fff;
					line-height: 1.6;

					&:last-child {
						margin-bottom: 0;
					}
				}

				.goods-item_left {
					display: flex;
					width: 40%;
					background: #fff;
					align-items: center;

					.image {
						display: block;
						width: 240rpx;
						height: 240rpx;
					}
				}

				.goods-item_right {
					position: relative;
					width: 60%;

					.goods-name {
						margin-top: 20rpx;
						min-height: 68rpx;
						line-height: 1.3;
						white-space: normal;
						color: #484848;
						font-size: 26rpx;
					}
				}

				.goods-item_desc {
					margin-top: 8rpx;
				}

				.desc-selling_point {
					width: 400rpx;
					font-size: 24rpx;
					color: #e49a3d;
				}

				.desc-goods_sales {
					color: #999;
					font-size: 24rpx;
				}

				.desc_footer {
					font-size: 24rpx;

					.price_x {
						margin-right: 16rpx;
						color: #f03c3c;
						font-size: 30rpx;
					}

					.price_y {
						text-decoration: line-through;
					}
				}
			}
		}
	}
</style>

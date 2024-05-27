<template>
    <div class="goods-info clearfix">
        <!-- 商品图片 -->
        <div class="in-left">
            <img :src="data.image" :alt="data.imageAlt" />
        </div>
        <div class="in-right">
            <!-- 商品名称 -->
            <p class="title twoline-hide">{{ data.title }}</p>
            <!-- 副标题 -->
            <p v-if="isEmpty(data.goodsProps)" class="subtitle" :class="{ 'c-p': subTitleColor }">{{ data.subtitle }}</p>
            <!-- 商品规格 -->
            <div v-else class="goods-props clearfix">
                <div class="goods-props-item" v-for="(props, idx) in data.goodsProps" :key="idx">
                    <span>{{ props.goodsProps }}</span>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import PropTypes from "ant-design-vue/es/_util/vue-types";
import { isEmpty } from "@/utils/util";

// Table内容元素: 商品信息
export default {
    name: "GoodsItem",
    props: {
        // 商品信息
        data: PropTypes.object.def({
            image: "",
            imageAlt: "",
            title: "",
            subtitle: "",
            goodsProps: [],
        }),
        // 副标题颜色
        subTitleColor: PropTypes.bool.def(false),
    },
    data() {
        return {
            isEmpty,
        };
    },
    methods: {},
};
</script>

<style lang="less" scoped>
@size: 60px;
.goods-info {
    width: 270px;
    line-height: 1.3;
    white-space: normal;
    .in-left {
        float: left;
        margin-right: 8px;

        img {
            width: @size;
            height: @size;
        }
    }
    .in-right {
        display: flex;
        flex-direction: column;
        justify-content: center;
        float: left;
        width: 200px;
        height: @size;
        .title {
            font-size: 13px;
            margin-bottom: 5px;
        }
        .subtitle {
            font-size: 12px;
            color: #7b7b7b;
        }
    }
    // 商品规格

    .goods-props {
        color: #8a8a8a;
        font-size: 12px;
        overflow: hidden;

        &-item {
            display: inline-block;
            margin-right: 4px;
        }
    }
}
</style>

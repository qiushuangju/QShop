/**
 * 图片上传 公共组件
 */
<template>
    <div class="uploadWrapper">
        <vuedraggable class="vue-draggable" :class="{ maxHidden: isMaxHidden }" v-model="arrUrlThumbnail" tag="ul" draggable=".draggable-item" @start="onDragStart"
            @end="onDragEnd">
            <!-- 拖拽元素 -->
            <li v-for="(item, index) in arrUrlThumbnail" :key="item + index" class="draggable-item" :style="{ width: width + 'px', height: height + 'px' }">
                <span>
                    <l-pic :picurl="item" :size="{ width: width, height: height }" @delPic="delPic(item)" />
                </span>
            </li>

            <div class="img-box-button" :width="width + 'px'" :height="height + 'px'" @click="disUrlThumbnail = true">
                <i class="el-icon-plus">
                    <span class="limitTxt">最多{{ arrUrlThumbnail.length }}/{{ limit }}张</span>
                </i>
            </div>

            <!-- 图片选择面板 -->
            <!-- <Modal v-model="disUrlThumbnail" width="1128" :footer-hide="true">

                <PhotoAlbum v-if="disUrlThumbnail" :maxcount="10" @selectPicData="selectThumbnail" />
            </Modal> -->
            <!-- 图片选择面板 -->
            <el-dialog :visible.sync="disUrlThumbnail" width="1328px">
                <PhotoAlbum v-if="disUrlThumbnail" :maxcount="10" @selectPicData="selectThumbnail" />
            </el-dialog>

        </vuedraggable>
        <div class="el-upload__tip" slot="tip">{{ textExplain }}</div>
    </div>
</template>

<script>
import * as vuedraggable from "vuedraggable";
import PhotoAlbum from "@/components/PhotoAlbum";
import LPic from "@/components/lPic";
export default {
    name: "imgSelect",
    components: {
        vuedraggable,
        PhotoAlbum,
        LPic,
    },

    data() {
        return {
            disUrlThumbnail: false,
            // arrUrlThumbnail: [],
        };
    },
    props: {
        // 图片数据(图片url组成的数组) 通过v-model传递
        value: {
            type: Array,
            default() {
                return [];
            },
        },
        //说明文字
        textExplain: {
            type: String,
            default: "只能上传jpg/png文件，且不超过500Kb",
        },
        // 限制上传的图片数量
        limit: {
            type: Number,
            default: 10,
        },
        // 限制上传图片的文件大小(kb)
        size: {
            type: Number,
            default: 500,
        },
        width: {
            type: Number,
            default: 90,
        },
        // 图片显示的高度(px)
        height: {
            type: Number,
            default: 90,
        },
    },

    computed: {
        // 图片数组数据
        arrUrlThumbnail: {
            get() {
                return this.value;
            },
            set(val) {
                if (val.length < this.arrUrlThumbnail.length) {
                    // 判断是删除图片时同步el-upload数据
                    this.syncElUpload(val);
                }
                // 同步v-model
                this.$emit("input", val);
            },
        },
        // 控制达到最大限制时隐藏上传按钮
        isMaxHidden() {
            return this.arrUrlThumbnail.length >= this.limit;
        },
    },

    watch: {
        value: {
            handler(val) {
                if (this.isFirstMount && this.value.length > 0) {
                    this.syncElUpload();
                }
            },

            deep: true,
        },
    },

    mounted() {
        if (this.value.length > 0) {
            this.syncElUpload();
        }
    },

    methods: {
        /**
         * 创建唯一的字符串
         * @return {string} ojgdvbvaua40
         */
        createUniqueString() {
            const timestamp = +new Date() + "";
            const randomNum = parseInt((1 + Math.random()) * 65536) + "";
            return (+(randomNum + timestamp)).toString(32);
        },
        // 同步el-upload数据
        syncElUpload(val) {
            const arrUrlThumbnail = val || this.arrUrlThumbnail;
            this.isFirstMount = false;
        },

        // 移除单张图片
        onRemoveHandler(index) {
            this.$confirm("确定删除该图片?", "提示", {
                confirmButtonText: "确定",
                cancelButtonText: "取消",
                type: "warning",
            })
                .then(() => {
                    this.arrUrlThumbnail = this.arrUrlThumbnail.filter(
                        (v, i) => {
                            return i !== index;
                        }
                    );
                   })
                .catch(() => {});
        },
        //选择图片
        selectThumbnail(data) {
            this.disUrlThumbnail = false;

            if (this.arrUrlThumbnail.length + data.length > this.limit) {
                this.$message({
                    type: "error",
                    message: `图片超限，最多可上传${this.limit}张图片`,
                });
                return false;
            }

            data.forEach((item) => {
                this.arrUrlThumbnail.push(item.filePath);
            });
        },
        delPic(val) {
            this.arrUrlThumbnail.forEach((item, index) => {
                if (item == val) {
                    this.arrUrlThumbnail.splice(index, 1);
                }
            });
        },
        // 超限
        onExceed() {
            this.$refs.uploadRef.abort(); // 取消剩余接口请求
            this.syncElUpload();
            this.$message({
                type: "warning",
                message: `图片超限，最多可上传${this.limit}张图片`,
            });
        },
        onDragStart(e) {
            e.target.classList.add("hideShadow");
        },
        onDragEnd(e) {
            e.target.classList.remove("hideShadow");
        },
    },
};
</script>

<style lang="less" scoped>
:v-deep(.el-upload) {
    width: 100%;
    height: 100%;
}

// 上传按钮
.uploadIcon {
    width: 100%;
    height: 100%;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;
    border: 1px dashed #c0ccda;
    background-color: #fbfdff;
    border-radius: 6px;
    font-size: 20px;
    color: #999;

    .limitTxt,
    .uploading {
        position: absolute;
        bottom: 10%;
        left: 0;
        width: 100%;
        font-size: 14px;
        text-align: center;
    }
}

// 拖拽
.vue-draggable {
    display: flex;
    flex-wrap: wrap;

    .draggable-item {
        margin-right: 5px;
        margin-bottom: 5px;
        border: 1px solid #ddd;
        border-radius: 6px;
        position: relative;
        overflow: hidden;

        .el-image {
            width: 100%;
            height: 100%;
        }
        .shadow {
            position: absolute;
            top: 0;
            right: 0;
            background-color: rgba(0, 0, 0, 0.5);
            opacity: 0;
            transition: opacity 0.3s;
            color: #fff;
            font-size: 20px;
            line-height: 20px;
            padding: 2px;
            cursor: pointer;
        }
        &:hover {
            .shadow {
                opacity: 1;
            }
        }
    }
    &.hideShadow {
        .shadow {
            display: none;
        }
    }
    &.single {
        overflow: hidden;
        position: relative;

        .draggable-item {
            position: absolute;
            left: 0;
            top: 0;
            z-index: 1;
        }
    }
    &.maxHidden {
        .uploadBox {
            display: none;
        }
    }
}
// el-image
.el-image-viewer__wrapper {
    .el-image-viewer__mask {
        opacity: 0.8;
    }
    .el-icon-circle-close {
        color: #fff;
    }
}
.el-upload__tip {
    margin-left: 0px;
}
.img-box-button {
    display: inline-block;

    border: 2px dashed #eee;
    border-radius: 4px;
    // margin: 5px 0 0 10px;
    cursor: pointer;
    width: 90px;
    height: 90px;
}

.img-box-button .el-icon-plus {
    position: relative;
    padding: 30px;
    font-size: 24px;
    font-weight: 600;
    color: #999;
}

.img-box-button .el-icon-plus .limitTxt {
    position: absolute;
    bottom: 10%;
    left: 0;
    width: 100%;
    font-weight: normal;
    font-size: 12px;
    text-align: center;
}
</style>


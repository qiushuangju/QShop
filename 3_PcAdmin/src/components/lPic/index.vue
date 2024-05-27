<template>
    <div class="pic-box" :style="{ margin: picmargin, height: boxHeight, width: size.width + 'px' }">
        <div class="photoalbum-img" :style="{
        width: size.width + 'px',
        height: size.height + 'px',
        'background-image': 'url(' + picurl + ')',
      }" @click="selectPic" @mouseenter="isHover = true" @mouseleave="isHover = false">
            <i v-if="isSelect" class="el-icon-check select-pic-icon" :style="{
          width: size.width + 'px',
          height: size.height + 'px',
          'line-height': size.height + 'px',
        }" />
            <div v-if="isHover && !isSelect">
                <i class="el-icon-delete del-pic-icon" v-if="disdel" :style="{
            top: size.height - 35 + 'px',
            'font-size': size.width / 5 + 'px',
          }" @click.stop="delPic" />
                <i class="el-icon-zoom-in del-pic-icon" v-if="disview" :style="{
            top: size.height - 35 + 'px',
            'font-size': size.width / 5 + 'px',
          }" @click.stop="lookPic" />
            </div>
            <div class="pic-size" :style="{ top: size.height - 25 + 'px' }" v-if="picsize && !isHover && !isSelect">
                {{ picsize.width | intpart }} × {{ picsize.height | intpart }}
            </div>
        </div>
        <div v-if="picname" class="picbox-text" :style="{ width: size.width + 'px', top: size.height + 'px' }">
            {{ picname }}
        </div>

        <!-- <pic-view :isOpen.sync="isPicView"/> -->
        <img-view v-if="isPicView" :urlList="piclist" :url="picurl" :onClose="closeView" />
    </div>
</template>

<script>
// import PicView from '../PicView'
import ImgView from "../ImgView";

export default {
    name: "LImg",
    components: {
        ImgView,
    },
    props: {
        picmargin: {
            type: String,
            required: false,
            default: "0",
        },
        picname: {
            type: String,
            required: false,
            default: undefined,
        },
        size: {
            //尺寸
            type: Object,
            required: false,
            default: () => {
                return { width: 115, height: 115 };
            },
        },
        pickey: {
            type: [String, Object, Number],
            required: false,
            // default: () => { return null }
        },
        picurl: {
            //url
            type: String,
            required: true,
            default: "images/2.jpg",
        },
        piclist: {
            type: Array,
            required: false,
            default: () => {
                return [];
            },
        },
        picsize: {
            type: Object,
            required: false,
        },
        picdata: {
            type: [String, Object],
            required: false,
            default: () => {
                return null;
            },
        },
        ischecked: {
            //单击图片是否可选择图片，默认不可，为false时，单击图片查看图片
            type: Boolean,
            required: false,
            default: false,
        },
        disdel: {
            //是否显示删除按钮，默认显示
            type: Boolean,
            required: false,
            default: true,
        },
        disview: {
            //是否显示查看按钮，默认显示
            type: Boolean,
            required: false,
            default: true,
        },
    },
    computed: {
        boxHeight() {
            if (this.picname) {
                return this.size.height + 30 + "px";
            } else {
                return this.size.height + "px";
            }
        },
    },
    data() {
        return {
            // index: this.piclist.indexOf(this.picurl) > 0 ? this.piclist.indexOf(this.picurl) : 0,
            isSelect: false,
            isHover: false,
            isPicView: false,
        };
    },
    methods: {
        selectPic() {
            if (this.ischecked) {
                this.isSelect = !this.isSelect;
                this.$emit("selectPic", {
                    checked: this.isSelect,
                    picData: this.picdata,
                });
            } else {
                if (this.isSelect) {
                    this.isSelect = !this.isSelect;
                    this.$emit("selectPic", {
                        checked: this.isSelect,
                        picData: this.picdata,
                    });
                } else {
                    this.lookPic();
                }
            }
        },
        delPic() {
            this.$emit("delPic", this.picdata);
        },
        lookPic() {
            if (this.piclist.length === 0) {
                // this.piclist = new Array();
                this.piclist.push(this.picurl);
            }
            this.isPicView = true;
        },
        closeView() {
            this.isPicView = false;
        },
    },
    filters: {
        intpart(value) {
            if (!value) return value;
            return value;
        },
    },
};
</script>

<style lang="css" scoped>
.pic-box {
    display: inline-block;
    text-align: center;
    /* background-color: #ccc; */
}
.photoalbum-img {
    position: absolute;
    border: 1px solid #d9d9d9;
    border-radius: 2px;
    background-size: 100%;
    background-repeat: no-repeat;
    background-position: center;
}
.picbox-text {
    font-size: 12px;
    margin-top: 5px;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
    position: relative;
}
.select-pic-icon {
    position: relative;
    height: 115px;
    width: 115px;
    line-height: 115px;
    font-size: 40px;
    color: #fff;
    background: rgba(0, 0, 0, 0.534);
    border: 2px solid #286dfd;
}
.del-pic-icon {
    color: #fff;
    /* font-size: 22px; */
    position: relative;
    /* left: 43px;
  top: 2px; */
    border-radius: 50%;
    padding: 5px;
    cursor: pointer;
    /* border: 1px solid #ccc; */
    background-color: rgba(0, 0, 0, 0.534);
}
.pic-size {
    height: 25px;
    line-height: 25px;
    position: relative;
    color: #c0c4cc;
    background-color: #00000087;
}
</style>
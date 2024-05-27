<template>
  <div class="photoalbum-container">
    <el-row class="photoalbum-title">
      <el-col :span="6" style="text-align: left">
        <el-button type="primary" size="mini" @click="dialogPicGMa = true">分组管理</el-button>
        <el-upload style="display: inline-block" :action="baseURL + '/Files/upload'" :headers="tokenHeader" multiple :show-file-list="false" :on-change="imgChange" name="files" :on-success="uploadFileSuccess">
          <el-button type="primary" size="mini" slot="trigger" style="margin-left: 15px">
            <span>上传图片</span>
          </el-button>
        </el-upload>
      </el-col>
      <el-col :span="12" class="photoalbum-title-text">选择图片</el-col>
      <el-col :span="6">
        <el-input placeholder="请输入搜索内容" size="mini" style="" v-model="searchName"></el-input>
        <el-button icon="el-icon-search" circle class="search-button" @click="search"></el-button>
      </el-col>
    </el-row>
    <el-row class="photoalbum-main">
      <el-col :span="4">
        <div class="photoalbum-main-menu">
          <el-menu default-active="1">
            <el-menu-item class="menu-item" index="1" @click="switchPhoto('')">
              <span style="
                  display: inline-block;
                  width: 80%;
                  color: #606266;
                  font-size: 14px;
                  font-weight: 500;
                  letter-spacing: 3px;
                ">{{ "未分组" }}</span>
              <!-- <span style="display: inline-block; width: 19%">{{
                defaultCount
              }}</span> -->
            </el-menu-item>
            <el-menu-item class="menu-item" v-for="item in picGroup" :key="item.id" @click="
                switchPhoto(item.id);
                number = item.count;
              ">
              <span style="
                  display: inline-block;
                  width: 80%;
                  color: #606266;
                  font-size: 14px;
                  font-weight: 500;
                  letter-spacing: 3px;
                ">{{ item.groupName }}</span>
              <!-- <span style="display: inline-block; width: 19%">{{
                item.count
              }}</span> -->
            </el-menu-item>
          </el-menu>
        </div>
      </el-col>
      <el-col :span="20">
        <div class="photoalbum-main-pic">
          <l-pic v-for="item in imgList" :key="item.id" :picurl="item.filePath" :pickey="item.id" :picdata="item" :picmargin="'0 16px 15px 16px'" :size="{ width: 115, height: 115 }" :picname="item.fileName"
            :picsize="{ width: item.picWidth, height: item.picHeight }" :ischecked="isChecked && maxcount !== 0" :piclist="viewUrlList" @delPic="delPic" @selectPic="selectPic" />
        </div>
      </el-col>
    </el-row>

    <el-pagination class="photoalbum-page" background layout="total, prev, pager, next, jumper" :total="number" :page-size="18" :current-page="page" @size-change="pageChange" @current-change="pageChange">
    </el-pagination>

    <div style="margin-top: 80px">
      <el-button type="primary" @click="submit" style="
          padding: 10px 40px;
          font-size: 16px;
          font-weight: 600;
          letter-spacing: 8px;
        ">确 定</el-button>
    </div>

    <el-dialog title="分组管理" :visible.sync="dialogPicGMa" width="640px" :modal="false" :before-close="beforeGroup">
      <vPicGroup />
    </el-dialog>
  </div>
</template>

<script>
import LPic from "@/components/lPic";
import * as picGroup from "@/api/uploadgroup";
import vPicGroup from "@/components/PicGroup";
import * as pic from "@/api/uploadFiles";
import { getToken } from "@/utils/auth";
export default {
  name: "PhotoAlbum",
  // mixins: [picMixin],
  components: {
    LPic,
    vPicGroup,
  },
  props: {
    maxcount: {
      type: Number,
      default: -1,
    },
  },
  data() {
    return {
      tokenHeader: { "X-Token": getToken() },
      baseURL: process.env.VUE_APP_BASE_API, // api的base_url
      picGroup: [],
      picGroupId: "", //相册列表Id
      imgList: [], //照片列表
      // upLoadPicData: {},//上传图片信息

      number: 0, //相册中照片数量
      defaultCount: 0, //未分组中照片数量
      selectPicList: new Array(), //选中的图片数据
      // activeApp: this.$store.state.app.activeApp, //当前应用信息
      page: 1, //页码
      limit: 21, //每页数量
      // viewUrlList: [],//查看照片列表
      isChecked: true, //控制照片是否可以选择
      imgurl: "images/2.jpg",
      dialogPicGMa: false,
      searchName: "",
    };
  },
  computed: {
    viewUrlList() {
      const list = new Array();
      this.imgList.forEach((item) => {
        list.push(item.filePath);
      });
      return list;
    },
  },
  created() {
    this.getPicGroup(this.searchName);
  },
  mounted() {
    this.getPic(); //获取未分组相册列表
  },

  methods: {
    beforeGroup() {
      this.dialogPicGMa = false;
      console.log(2121212112);
      this.getPicGroup(this.searchName);
    },
    //上传成功
    uploadFileSuccess(response, file, fileList) {
      if (response.code == 200) {
        console.log(response.result[0]);
        var data = response.result[0];
        data.groupId = this.picGroupId;
        console.log("reqData", data);
        pic.updateGroup(data).then((res) => {
          if (response.code === 200) {
            this.$message.success("上传成功！");
            this.page = 1;
            this.getPic();
          }
        });
      } else {
        this.$message.error(response.result.message); //文件上传错误提示
      }
    },
    getPicGroup(searchKey) {
      const params = { status: "1", limit: 100 };
      picGroup.listByWhere(params).then((response) => {
        if (response.code === 200) {
          this.picGroup = response.result;
          this.picGroup.forEach((element) => {
            if (element.status === "1") {
              element.status = true;
            } else {
              element.status = false;
            }
          });
        } else {
          this.pitcGroup = [];
          this.$message(response.message);
        }
      });
    },
    /**
     * 获取相应相册中的照片
     */
    getPic() {
      this.selectPicList = new Array();
      this.isChecked = true;
      console.log("this.picGroupId", this.picGroupId);
      var groupId =
        this.picGroupId == null || this.picGroupId == ""
          ? "default"
          : this.picGroupId;
      const params = {
        GroupId: groupId,
        key: this.searchName,
        limit: this.limit,
        page: this.page,
      };
      pic.load(params).then((response) => {
        if (response.code === 200) {
          this.imgList = response.result;
          this.setNum(parseInt(response.count));
        } else if (response.code === 204) {
          this.imgList = [];
          this.setNum(0);
        } else {
          this.$message.error(response.message);
        }
      });
    },
    /**
     * 获取照片或上传照片后，设置相册中照片的数量
     */
    setNum(num) {
      this.number = num;
      if (this.picGroupId === "") {
        this.defaultCount = num;
      } else {
        this.picGroup.some((item) => {
          if (item.id === this.picGroupId) {
            item.number = num;
            return true;
          }
        });
      }
    },
    /**
     * 切换相册
     */
    switchPhoto(id) {
      this.picGroupId = id;
      this.page = 1;
      this.getPic();
    },
    /**
     * 删除照片
     */
    delPic(data) {
      pic.del(new Array(data.id)).then((response) => {
        if (response.code === 200) {
          this.getPic();
        } else {
          this.$message.error(response.message);
        }
      });
    },
    selectPic(data) {
      if (data.checked) {
        if (this.maxcount > 0) {
          if (this.selectPicList.length <= this.maxcount) {
            this.selectPicList.push(data.picData);
            if (this.selectPicList.length === this.maxcount) {
              this.isChecked = false;
              this.$message(
                `最多可选${this.maxcount}张，已选择${this.maxcount}张`
              );
            }
          }
        } else if (this.maxcount === 0) {
          this.isChecked = false;
          this.$message(
            `最多可选${this.maxcount}张，已选择${this.maxcount}张`
          );
        } else {
          this.selectPicList.push(data.picData);
        }
      } else {
        this.selectPicList.splice(
          this.selectPicList.findIndex(
            (item) => item.id === data.picData.id
          ),
          1
        );
        if (this.selectPicList.length < this.maxcount) {
          this.isChecked = true;
        }
      }
    },
    pageChange(page) {
      this.page = page;
      this.getPic();
    },
    /**
     * 向父组件发送选择的图片数据
     */
    submit() {
      if (this.selectPicList.length === 0) {
        this.$message.warning("未选中任何图片");
      }
      this.$emit("selectPicData", this.selectPicList);
    },
    imgChange(file) { },
    /**
     * 查询图片
     */
    search() {
      this.getPic();
    },
  },
};
</script>

<style lang="css" scoped>
.photoalbum-container {
  width: 1288px;
  height: 720px;
  box-sizing: border-box;
  padding: 20px 0 0 10px;
  text-align: center;
  z-index: 3000;
}
.photoalbum-title {
  height: 33px;
  width: 100%;
  line-height: 33px;
  /* background-color: #ccc; */
}
.photoalbum-title-text {
  font-size: 18px;
  font-weight: 900;
}
.search-button {
  border: none;
  position: relative;
  top: -34px;
  right: -116px;
  background-color: rgba(255, 255, 255, 0);
}
.photoalbum-main {
  height: 505px;
  width: 100%;
  margin: 10px auto;
  /* background-color: #ccc; */
}
.photoalbum-main > div {
  position: relative;
  top: -30px;
  padding: 0 2px;
  height: 100%;
}
.photoalbum-main-menu {
  height: 100%;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  overflow-y: auto;
  /* background-color: #F6FAFF; */
}
.photoalbum-main-pic {
  height: 100%;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  padding: 20px 0 0 0;
  text-align: start;
  /* background-color: #F6FAFF; */
}
.menu-item {
  height: 40px;
  line-height: 40px;
}
.photoalbum-page {
  position: absolute;
  right: 20px;
}
</style>

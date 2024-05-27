<template>
    <div class="picgroup-container">
        <el-row class="picgroup-title">
            <el-col :span="4">
                <el-button type="primary" class="picgroup-title-add" size="mini" @click="add">新 增</el-button>
            </el-col>
            <el-col :span="20">
                <el-input v-model="groupName" placeholder="请输入搜索内容" size="mini" style="width: 200px; height: 30px; float: right"></el-input>
                <el-button icon="el-icon-search" @click="searchGroup" circle class="search-button"></el-button>
            </el-col>
        </el-row>
        <div>
            <el-table :data="picGroup" stripe style="width: 100%">
                <el-table-column align="center" label="分组名称">
                    <template slot-scope="scope">
                        <div>
                            <div v-if="scope.row.id !== editId">
                                {{ scope.row.groupName }}
                            </div>
                            <div v-if="scope.row.id === editId">
                                <el-input v-model="scope.row.groupName" placeholder="请输入名称" size="mini" style="width: 100px; text-align: center"></el-input>
                                <el-button class="action-button" type="text"><i class="el-icon-check" @click="submit(scope.row)" /></el-button>
                            </div>
                        </div>
                    </template>
                </el-table-column>
                <el-table-column align="center" label="创建时间" width="100px">
                    <template slot-scope="scope">
                        <div>{{ scope.row.createTime | formatTime }}</div>
                    </template>
                </el-table-column>
                <el-table-column label="操作" align="center">
                    <template slot-scope="scope">
                        <el-button class="action-button" type="text" @click="eidt(scope.row.id)"><i class="el-icon-edit" /></el-button>
                        <el-button class="action-button" @click="
                isDel = true;
                pic = scope.row;
              " type="text" v-if="scope.row.id !== editPic.id">
                            <svg-icon icon-class="shanchu" />
                        </el-button>
                    </template>
                </el-table-column>
            </el-table>
        </div>

        <el-dialog :visible.sync="isDel" width="200px" :modal="false" :show-close="false" center>
            <p>确定要删除该分组吗？</p>
            <div slot="footer">
                <el-button size="mini" type="primary" @click="isDel = false">取消</el-button>
                <el-button size="mini" type="text" @click="del(pic)">确定</el-button>
            </div>
        </el-dialog>
    </div>
</template>

<script>
// import picMixin from "../mixin/picture"; //混入
import { parseTime } from "@/utils/index";

import * as picGroup from "@/api/uploadgroup";

export default {
    name: "PicGroup",
    // mixins: [picMixin],
    data() {
        return {
            editId: "",
            isEdit: false,
            isDel: false,
            groupName: "",
            picGroup: [],
            editPic: {
                id: "",
                createTime: parseTime(new Date()),
                status: true,
                groupName: "",
            },
            pic: {},
        };
    },
    created() {
        this.searchGroup();
    },
    methods: {
        eidt(id) {
            //编辑
            this.editId = id;
            this.isEdit = true;
        },
        resetData() {
            this.editPic = {
                id: "",
                createTime: parseTime(new Date()),
                status: true,
                groupName: "",
            };
        },
        add(obj) {
            if (this.isEdit) {
                this.$message.error("当前有分组正在编辑，请提交后再添加");
            } else {
                this.picGroup.push(this.editPic);
                this.isEdit = true;
            }
        },
        /**
         * 提交新增或修改的分组信息
         */
        submit(data) {
            if (data.groupName.trim() === "" || data.groupName === undefined) {
                this.$message.error("分组名称不可为空");
                return;
            }
            const params = {
                id: data.id,
                groupName: data.groupName,
            };
            picGroup.addOrUpdate(params).then((response) => {
                if (response.code === 200) {
                    this.$message.success("操作成功!");
                    this.isEdit = false;
                    this.editId = "";
                    this.resetData();
                    this.searchGroup();
                } else {
                    this.$message.error(response.message);
                }
            });
        },
        /**
         * 删除分组
         */
        del(data) {
            this.isDel = false;
            var arr = new Array(data.id);
            picGroup.del(arr).then((response) => {
                if (response.code === 200) {
                    this.$message.success("删除成功");
                    this.searchGroup();
                    this.isEdit = false;
                    this.editId = "";
                } else {
                    this.$message.error(response.message);
                }
            });
        },
        /**
         * 查询分组
         */
        searchGroup() {
            const params = {
                limit: 100,
                key: this.groupName,
            };
            picGroup.listByWhere(params).then((response) => {
                if (response.code === 200) {
                    this.picGroup = response.result;
                } else {
                    this.picGroup = [];
                    this.$message(response.message);
                }
            });
        },
    },
    filters: {
        formatTime(value) {
            return parseTime(value, "{y}-{m}-{d}");
        },
    },
};
</script>

<style lang="css" scoped>
.picgroup-container {
    padding: 0 10px;
}
.picgroup-title {
    text-align: initial;
    height: 40px;
    line-height: 40px;
    padding: 5px 0;
}
.picgroup-title-add {
    padding: 8px 30px;
}
.search-button {
    border: none;
    position: relative;
    left: 450px;
    background-color: rgba(255, 255, 255, 0);
}
.action-button {
    padding: 0 5px;
}
</style>
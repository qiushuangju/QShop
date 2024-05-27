<template>
  <div class="compent-dialog-body">
    <div class="content">
      <el-input size="small" @keyup.enter.native="handleFilter" style="width: 200px;" class="filter-item" :placeholder="'名称'"
        v-model="listQuery.key">
      </el-input>

      <el-select clearable size="small" class="filter-item" style="width: 130px" v-model="listQuery.appId" :placeholder="'所属应用'">
        <el-option v-for="item in  apps" :key="item.id" :label="item.name+'('+item.id+')'" :value="item.id">
        </el-option>
      </el-select>

      <el-button class="filter-item" size="small" v-waves icon="el-icon-search" @click="handleFilter">搜索</el-button>

      <el-table class="table" ref="mainTable" :key='tableKey' :data="list" v-loading="listLoading" border fit
        highlight-current-row style="width: 100%;" @row-click="rowClick" @selection-change="handleSelectionChange">
        <el-table-column align="center" type="selection" width="55">
        </el-table-column>

        <el-table-column :label="'Id'" min-width="120px">
          <template slot-scope="scope">
            <span>{{scope.row.id}}</span>
          </template>
        </el-table-column>

        <el-table-column min-width="80px" :label="'名称'">
          <template slot-scope="scope">
            <span>{{scope.row.name}}</span>
          </template>
        </el-table-column>

        <el-table-column width="120px" :label="'所属应用'">
          <template slot-scope="scope">
            <span>{{scope.row.appName}}</span>
          </template>
        </el-table-column>
        <el-table-column min-width="150px" :label="'描述'">
          <template slot-scope="scope">
            <span style='color:red;'>{{scope.row.description}}</span>
          </template>
        </el-table-column>
      </el-table>
      <pagination v-show="total>0" :total="total" :page.sync="listQuery.page" :limit.sync="listQuery.limit" @pagination="handleCurrentChange" />
    </div>
    <div class="el-dialog__footer">
      <el-button size="mini" @click="close">取消</el-button>
      <el-button size="mini" type="primary" @click="submit">确认</el-button>
    </div>
  </div>

</template>

<script>
  import * as applications from '@/api/applications'
  import waves from '@/directive/waves' // 水波纹指令
  import * as accessObjs from '@/api/accessObjs'
  import Pagination from '@/components/Pagination'

  export default {
    name: 'assign-res',
    components: {
      Pagination
    },
    directives: {
      waves
    },
    props: ['roleId'],
    data() {
      return {
        multipleSelection: [], // 列表checkbox选中的值
        currentRoleId: this.roleId,
        tableKey: 0,
        list: null,
        total: 0,
        listLoading: true,
        listQuery: { // 查询条件
          page: 1,
          limit: 20,
          key: undefined,
          appId: undefined
        },
        apps: []
      }
    },
    created() {
      this.getList()
      applications.getList().then(response => {
        this.apps = response.result
      })
    },
    methods: {
      rowClick(row) {
        this.$refs.mainTable.clearSelection()
        this.$refs.mainTable.toggleRowSelection(row)
      },
      handleSelectionChange(val) {
        this.multipleSelection = val
      },

      getList() {
        const _this = this
        this.listLoading = true
      },
      handleFilter() {
        this.listQuery.page = 1
        this.getList()
      },
      handleSizeChange(val) {
        this.listQuery.limit = val
        this.getList()
      },
      handleCurrentChange(val) {
        this.listQuery.page = val
        this.getList()
      },
      close() {
        this.$emit('close')
      },
      submit() {
        // 提交结果
        accessObjs.assign({
          type: 'RoleResource',
          firstId: this.roleId,
          secIds: this.multipleSelection.map(u => u.id)
        }).then(() => {
          this.$notify({
            title: '成功',
            message: '分配成功',
            type: 'success',
            duration: 2000
          })
        })

        this.close()
      }
    }
  }

</script>

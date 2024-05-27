<template>
  <el-table ref="subTable" :data="formatData" :row-key="handleRowKey" :default-expand-all="expandAll" v-bind="$attrs"  @row-click="rowClick" @selection-change="handleSelectionChange">
    <!-- <el-table-column align="center" type="selection" width="30"></el-table-column> -->
    <el-table-column v-if="columns.length===0" width="150">
      <template slot-scope="scope">
        <!-- <span v-for="space in scope.row._level" class="ms-tree-space" :key="space">1</span> -->
        <!-- <span class="tree-ctrl" v-if="iconShow(0,scope.row)" @click="toggleExpanded(scope.$index)">
          <i v-if="!scope.row._expanded" class="el-icon-plus"></i>
          <i v-else class="el-icon-minus"></i>
        </span> -->
        {{scope.$index}}
      </template>
    </el-table-column>
    <el-table-column v-else v-for="(column, index) in columns" :key="column.value" :label="column.text" :width="column.width">
      <template slot-scope="scope">
     
        <!-- <span v-if="index === 0" v-for="space in scope.row._level" class="ms-tree-space" :key="space">
        </span> -->
        <!-- <span class="tree-ctrl" v-if="iconShow(index,scope.row)" @click="toggleExpanded(scope.$index)">
          <i v-if="!scope.row._expanded" class="el-icon-plus"></i>
          <i v-else class="el-icon-minus"></i>
        </span> -->
        <template>
           <span v-if="index === 0 && scope.row.parentId" >
            <el-radio v-model="radioCheck" :label="scope.row.id">{{scope.row[column.value]}}</el-radio>
           </span>
            <template v-else>
              {{scope.row[column.value]}}
            </template>
        </template>
        
       
      </template>
    </el-table-column>
    <slot></slot>
  </el-table>
</template>

<script>
/**
  Auth: Lei.j1ang
  Created: 2018/1/19-13:59
*/
import treeToArray from './eval'
export default {
  name: 'treeTable',
  data() {
    return {
      radioCheck: {}
    }
  },
  props: {
    data: {
      type: [Array, Object],
      required: true
    },
    columns: {
      type: Array,
      default: () => []
    },
    evalFunc: Function,
    evalArgs: Array,
    expandAll: {
      type: Boolean,
      default: true
    }
  },
  computed: {
    // 格式化数据源
    formatData: function() {
      let tmp
      if (!Array.isArray(this.data)) {
        tmp = [this.data]
      } else {
        tmp = this.data
      }
      const func = this.evalFunc || treeToArray
      const args = this.evalArgs ? Array.concat([tmp, this.expandAll], this.evalArgs) : [tmp, this.expandAll]
      return func.apply(null, args)
    }
  },
  methods: {
    rowClick(row) {
      this.radioCheck = row.id
      this.$refs.subTable.clearSelection()
      this.$refs.subTable.toggleRowSelection(row)
      this.$emit('row-click', row)
    },
    handleSelectionChange(val) {
      this.$emit('selection-change', val)
    },
    // showRow: function(row) {
    //   const show = (row.row.parent ? (row.row.parent._expanded && row.row.parent._show) : true)
    //   row.row._show = show
    //   return show ? 'animation:treeTableShow 1s;-webkit-animation:treeTableShow 1s;' : 'display:none;'
    // },
    // 切换下级是否展开
    // toggleExpanded: function(trIndex) {
    //   const record = this.formatData[trIndex]
    //   record._expanded = !record._expanded
    // },
    // 图标显示
    // iconShow(index, record) {
    //   return (index === 0 && record.children && record.children.length > 0)
    // },
    handleRowKey(row) {
      return row.id
    }
  }
}
</script>
<style rel="stylesheet/css">
  /* @keyframes treeTableShow {
    from {opacity: 0;}
    to {opacity: 1;}
  }
  @-webkit-keyframes treeTableShow {
    from {opacity: 0;}
    to {opacity: 1;}
  } */
</style>

<style lang="scss" rel="stylesheet/scss" scoped>
  $color-blue: #2196F3;
  $space-width: 18px;
  .ms-tree-space {
    position: relative;
    top: 1px;
    display: inline-block;
    font-style: normal;
    font-weight: 400;
    line-height: 1;
    width: $space-width;
    height: 14px;
    &::before {
      content: ""
    }
  }
  .processContainer{
    width: 100%;
    height: 100%;
  }
  table td {
    line-height: 26px;
  }

  .tree-ctrl{
    position: relative;
    cursor: pointer;
    color: $color-blue;
    margin-left: -$space-width;
  }
</style>

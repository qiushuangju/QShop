let extension = {
    components: {
      //组件扩展
    },
   
    methods: {//事件扩展
      delrows(objs, rows, callback){
        var _this = this
        _this.$confirm('是否确认删除?', '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          objs.del(rows.map(u => u.id)).then(() => {
            _this.$notify({
              title: '成功',
              message: '删除成功',
              type: 'success',
              duration: 2000
            })
            rows.forEach(row => {
              const index = _this.list.indexOf(row)
              _this.list.splice(index, 1)
            })
            if(callback != undefined){
              callback()
            }
          })
        }).catch(() => {
          _this.$message({
            type: 'info',
            message: '已取消'
          });          
        });
      }
    }
  };
  export default extension;
  
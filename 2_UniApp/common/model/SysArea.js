import * as Api from '@/api/sys/sysArea'
import storage from '@/utils/storage'
import * as util from '@/utils/util'
const AreaTree = 'AreaTree'

/**
 * 商品分类 model类
 * RegionModel
 */
export default {

  // 从服务端获取全部地区数据(树状)
  getTreeDataFromApi () {
    return new Promise((resolve, reject) => {
      Api.listByWhere({maxLevel:3}).then(res => {
		  var list=res.result;
		 var treeData= this.formatRegionTree(list) 
		  resolve(treeData)
	  })
    })
  },

  // 获取所有地区(树状)
  getTreeData () {
    return new Promise((resolve, reject) => {
      // 判断缓存中是否存在
      const data = storage.get(AreaTree)
      // 从服务端获取全部地区数据
      if (data) {
        resolve(data)
      } else {
        this.getTreeDataFromApi().then(list => {
          // 缓存24小时
          storage.set(AreaTree, list, 24 * 60 * 60)
          resolve(list)
        })
      }
    })
  },
  
  /**
        * 格式化树结构数据
        * @param {Object} result
        */
       formatRegionTree(list) {
  		   var arr = list.map(function (item) {
  		                  return {
  		                      id: item.id,
  		                      parentId:item.parentId=='0'?null: item.parentId ,
  		                      label: item.name,
							  value: item.id,
  							  level:item.level
  		                  };
  		              });
  					  
  		  var listTree=util.listToTree(arr)
  		  
  		  console.log("listTree",listTree)
         return listTree
       },
	   

  // 获取所有地区的总数
  getCitysCount () {
    return new Promise((resolve, reject) => {
      // 获取所有地区(树状)
      this.getTreeData().then(data => {
        const cityIds = []
        // 遍历省份
        for (const pidx in data) {
          const province = data[pidx]
          // 遍历城市
          for (const cidx in province.city) {
            const cityItem = province.city[cidx]
            cityIds.push(cityItem.id)
          }
        }
        resolve(cityIds.length)
      })
    })
  }

}

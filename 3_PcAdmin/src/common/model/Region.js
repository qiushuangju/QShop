import * as Api from '@/api/sysArea'
import storage from 'store'
import * as util from '@/utils/util'
const AreaTree = 'AreaTree'

/**
 * 商品分类 model类
 * RegionModel
 */
export default {

    // 从服务端获取全部地区数据(树状)
    getTreeDataFromApi() {
        return new Promise((resolve, reject) => {
            Api.listByWhere({ maxLevel: 3 }).then(res => {
                var list = res.result;
                var treeData = this.formatRegionTree(list)
                resolve(treeData)
            })
        })
    },

    // 获取所有地区(树状)
    getTreeData() {
        return new Promise((resolve, reject) => {
            // // 判断缓存中是否存在
            // const data = storage.get(AreaTree)
            // console.log("daddd", data)
            //     // 从服务端获取全部地区数据
            // if (data) {
            //     resolve(data)
            // } else {
            this.getTreeDataFromApi().then(list => {
                    // 缓存24小时
                    storage.set(AreaTree, list, 24 * 60 * 60)
                    resolve(list)
                })
                // }
        })
    },
    /**
     * 格式化树结构数据
     * @param {Object} result
     */
    formatRegionTree(list) {
        var arr = list.map(function(item) {
            return {
                id: item.id,
                parentId: item.parentId == '0' ? null : item.parentId,
                label: item.name,
                value: item.id,
                level: item.level
            };
        });

        var listTree = util.listToTree(arr)

        // console.log("listTree", listTree)
        return listTree
    },
    // 获取所有地区的总数
    getCitysCount() {
        return new Promise((resolve, reject) => {
            // 获取所有地区(树状)
            this.getTreeData().then(list => {
                var arrCity = list.filter((item) => item.level >= 2);
                resolve(arrCity.length)
            })
        })
    }

}
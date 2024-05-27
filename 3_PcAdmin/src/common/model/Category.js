import * as Api from '@/api/goods/goodscate'
import { listToTreeSelect } from "@/utils";

/**
 * 商品分类 model类
 * CategoryModel
 */
export default {
    // 获取商品分类列表 (用于添加商品的form)
    getCategoryTreeSelect() {
        return new Promise((resolve, reject) => {
            Api.listByWhere()
                .then(res => {
                    const categoryList = res.result
                    var cates = res.result.map(function(item) {
                        return {
                            id: item.id,
                            title: item.name,
                            key: item.id,
                            value: item.id,
                            parentId: item.parentId == "" ? null : item.parentId,
                        };
                    });
                    var cateTmp = JSON.parse(JSON.stringify(cates));
                    //console.log(111, JSON.stringify(cateTmp));
                    var cateTree = listToTreeSelect(cateTmp);
                    //console.log(222, JSON.stringify(cateTree));
                    resolve(cateTree)
                        // 格式化分类列表
                        // const treeData = this.formatTreeData(categoryList)
                        // resolve(treeData)
                })
        })
    },

    // 获取商品分类列表 (用于筛选select)
    getListFromScreen() {
        return new Promise((resolve, reject) => {
            Api.listByWhere().then(res => {
                // 格式化分类列表
                const resultList = res.result
                    // 格式化为 select列表数据
                const selectList = [{
                    title: '全部分类',
                    key: 0,
                    value: 0
                }].concat(this.formatTreeData(resultList))
                resolve(selectList)
            })
        })
    },

    /**
     * 格式化分类列表
     * @param {*} list 分类数据源
     * @param {*} disabled
     */
    formatTreeData(list, disabledParentId = null, disabled = false) {
        const data = []
        list.forEach(item => {
            // 新的元素
            const netItem = {
                    title: item.name,
                    key: item.id,
                    value: item.id
                }
                // 禁用的分类
            if (
                [item.id, item.parentId].includes(disabledParentId) ||
                disabled === true
            ) {
                netItem.disabled = true
            }
            // 递归整理子集
            if (item.children && item.children.length) {
                netItem['children'] = this.formatTreeData(item['children'], disabledParentId, netItem.disabled)
            }
            data.push(netItem)
        })
        return data
    }

}
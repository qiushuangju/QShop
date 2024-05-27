import request from '@/utils/request'

// 订单-日统计
export function loadOrderByDay(params) {
    return request({
        url: '/statistic/loadOrderByDay',
        method: 'get',
        params
    })
}
//订单-月统计
export function loadOrderByMonth(params) {
    return request({
        url: '/statistic/loadOrderByMonth',
        method: 'get',
        params
    })
}
//订单-年统计
export function loadOrderByYear(params) {
    return request({
        url: '/statistic/loadOrderByYear',
        method: 'get',
        params
    })
}

// 服务订单-日统计
export function loadOrderServiceByDay(params) {
    return request({
        url: '/statistic/loadOrderServiceByDay',
        method: 'get',
        params
    })
}
//服务订单-月统计
export function loadOrderServiceByMonth(params) {
    return request({
        url: '/statistic/loadOrderServiceByMonth',
        method: 'get',
        params
    })
}
//服务订单-年统计
export function loadOrderServiceByYear(params) {
    return request({
        url: '/statistic/loadOrderServiceByYear',
        method: 'get',
        params
    })
}
//售后-日统计
export function loadRefundOrderByDay(params) {
    return request({
        url: '/statistic/loadRefundOrderByDay',
        method: 'get',
        params
    })
}
//售后-月统计
export function loadRefundOrderByMonth(params) {
    return request({
        url: '/statistic/loadRefundOrderByMonth',
        method: 'get',
        params
    })
}
//售后-年统计
export function loadRefundOrderByYear(params) {
    return request({
        url: '/statistic/loadRefundOrderByYear',
        method: 'get',
        params
    })
}
//服务售后-日统计
export function loadRefundOrderServiceByDay(params) {
    return request({
        url: '/statistic/loadRefundOrderServiceByDay',
        method: 'get',
        params
    })
}
//服务售后-月统计
export function loadRefundOrderServiceByMonth(params) {
    return request({
        url: '/statistic/loadRefundOrderServiceByMonth',
        method: 'get',
        params
    })
}
//服务售后-年统计
export function loadRefundOrderServiceByYear(params) {
    return request({
        url: '/statistic/loadRefundOrderServiceByYear',
        method: 'get',
        params
    })
}
export function loadFreightByDriver(params) {
    return request({
        url: '/statistic/loadFreightByDriver',
        method: 'get',
        params
    })
}
export function loadFreight(params) {
    return request({
        url: '/statistic/loadFreight',
        method: 'get',
        params
    })
}
//订单首页
export function loadPcHomePageData(params) {
    return request({
        url: '/statistic/loadPcHomePageData',
        method: 'get',
        params
    })
}
//服务首页
export function loadPcHomePageDataService(params) {
    return request({
        url: '/statistic/loadPcHomePageDataService',
        method: 'get',
        params
    })
}

export function loadPartner(params) {
    return request({
        url: '/statistic/loadPartner',
        method: 'get',
        params
    })
}
export function ListLinqUserIncomeByUser(params) {
    return request({
        url: '/statistic/ListLinqUserIncomeByUser',
        method: 'get',
        params
    })
}
// 经营统计-代理经销商数据
export function StatisticOperate(params) {
    return request({
        url: '/statistic/StatisticOperate',
        method: 'get',
        params
    })
}
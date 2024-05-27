const ZHCN = {
  common: {
    inputPlaceholder: 'Corn表达式',
    every: '每',
    specified: '固定的',
    fromThe: '从第',
    start: '开始',
    between: '在',
    and: '到',
    end: '之间的',
    current: '本',
    nearest: '最近的',
    placeholderMulti: '请选择(支持多选)',
    help: '帮助',
    placeholder: '请选择',
    use: '使用',
    valTip: '值为',
    symbolTip: '通配符支持',
    wordNumError: '格式不正确，必须有6或7位',
    nth: '第',
    index: '个'
  },
  second: {
    title: '秒',
    val: '0 1 2 ... 59'
  },
  minute: {
    title: '分',
    val: '0 1 2 ... 59'
  },
  hour: {
    title: '时',
    val: '0 1 2 ... 23'
  },
  dayOfMonth: {
    title: '日',
    val: '1 2 ... 31',
    timeUnit: '日'
  },
  month: {
    title: '月',
    val: '1 2 ... 12，或12个月的缩写(JAN ... DEC) '
  },
  dayOfWeek: {
    title: '周',
    val: '1 2 ... 7或星期的缩写(SUN ... SAT)',
    timeUnit: '日'
  },
  year: {
    title: '年',
    val: '2020 ... 2099'
  },
  custom: {
    unspecified: '不固定',
    latestWorkday: '最后一个工作日',
    lastTh: '倒数第',
    workDay: '工作日',
    empty: '不配置',
    lastOne: '最后一个'
  },
  cases: [
    {
      label: '每秒',
      value: '* * * * * ?'
    },
    {
      label: '每30分钟',
      value: '0 */30 * * * ?'
    },
    {
      label: '在每小时的第15,30,45分钟',
      value: '0 15,30,45 * * * ?'
    },
    {
      label: '每个偶数小时',
      value: '0 0 0/2 * * ?'
    },
    {
      label: '每个奇数小时',
      value: '0 0 1/2 * * ?'
    },
    {
      label: '每天凌晨12点(12am)',
      value: '0 0 0 * * ?'
    },
    {
      label: '每天中午12点(12pm)',
      value: '0 0 12 * * ?'
    },
    {
      label: '每周一12点',
      value: '0 0 12 * * MON'
    },
    {
      label: '每周一至周五12点',
      value: '0 0 12 * * MON-FRI'
    },
    {
      label: '每月1号开始每隔4天的中午12点',
      value: '0 0 12 1/4 * ?'
    },
    {
      label: '每月最后一天的中午12点',
      value: '0 0 12 L * ?'
    },
    {
      label: '每月最后一天前两天(倒数第三天)中午12点',
      value: '0 0 12 L-2 * ?'
    },
    {
      label: '每月最后一个工作日的12点',
      value: '0 0 12 LW * ?'
    },
    {
      label: '最接近每月1号的那个工作日的12点',
      value: '0 0 12 1W * ?'
    },
    {
      label: '每月最后一个星期天12点',
      value: '0 0 12 ? * 1L'
    },
    {
      label: '每月第一个星期五的12点',
      value: '0 0 12 ? * 6#1'
    },
    {
      label: '1月和6月的每天中午12点',
      value: '0 0 12 * JAN,JUN ?'
    }
  ],
  daysOfWeek: [
    {
      label: '星期天',
      value: 1
    },
    {
      label: '星期一',
      value: 2
    },
    {
      label: '星期二',
      value: 3
    },
    {
      label: '星期三',
      value: 4
    },
    {
      label: '星期四',
      value: 5
    },
    {
      label: '星期五',
      value: 6
    },
    {
      label: '星期六',
      value: 7
    }
  ]
}
export default ZHCN

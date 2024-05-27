// 获取url二级目录
const pathname = window.location.pathname

// 默认配置
export const defaultConfig = {
  // 编辑器层级的基数,默认是900
  zIndex: 1000,
  // 编辑器自动被内容撑高
  autoHeightEnabled: false,
  // 初始容器高度
  initialFrameHeight: 540,
  // 初始容器宽度
  initialFrameWidth: 375,
  // 上传文件接口（这个地址是我为了方便各位体验文件上传功能搭建的临时接口，请勿在生产环境使用！！！部署在国外的服务器，如果无法访问，请自备梯子）
  // serverUrl: 'http://35.201.165.105:8000/controller.php',
  // UEditor 资源文件的存放路径，如果你使用的是 vue-cli 生成的项目，通常不需要设置该选项，vue-ueditor-wrap 会自动处理常见的情况，如果需要特殊配置，参考下方的常见问题2
  UEDITOR_HOME_URL: `${pathname}static/UEditor/`,
  // 给编辑区域的iframe引入一个css文件
  iframeCssUrl: `${pathname}static/UEditor/themes/iframe.css`,
  // 图片操作的浮层开关
  imagePopup: false,
  // 打开右键菜单功能
  enableContextMenu: false,
  // 是否保持toolbar的位置不动,默认true
  autoFloatEnabled: false,
  // 工具栏上的所有的功能按钮和下拉框
  toolbars: [[
    'source', '|', 'undo', 'redo', '|',
    'bold', 'italic', 'underline', 'strikethrough', 'removeformat',
    // 纯文本粘贴
    'pasteplain', '|', 'forecolor', 'backcolor', 'selectall', 'cleardoc', '|',
    'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
    'fontsize', '|',
    'indent', 'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|', 'touppercase', 'tolowercase', '|',
    'unlink',
    //  'link', 
    // 'simpleupload',
    // 'insertvideo'
  ]]
  // 当鼠标放在工具栏上时显示的tooltip提示
  // labelMap: {
  //   simpleupload: '插入图片'
  // }
}

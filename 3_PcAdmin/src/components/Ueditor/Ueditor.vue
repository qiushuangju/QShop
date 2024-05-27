<template>
  <div>
    <!-- 富文本编辑器 -->
    <VueUeditorWrap
      ref="Ueditor"
      v-model="content"
      :config="myConfig"
      @before-init="beforeUeditorInit"
    />
    <!-- 文件选择器 -->
    <FilesModal
      ref="FilesModal"
      :fileType="filesModalType"
      :multiple="true"
      @handleSubmit="handleFilesSelect"
    />
    <!-- 链接选择器 -->
    <LinkModal ref="LinkModal" @handleSubmit="handleLinkSelect" />
  </div>
</template>

<script>
import _ from 'lodash'
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import VueUeditorWrap from 'vue-ueditor-wrap'
import { defaultConfig } from './config'
import { FilesModal, LinkModal } from '@/components/Modal'
import FileTypeEnum from '@/common/enum/file/FileType'

export default {
  name: 'Ueditor',
  components: {
    FilesModal,
    LinkModal,
    VueUeditorWrap
  },
  model: {
    prop: 'value',
    event: 'change'
  },
  props: {
    // 内容
    // eslint-disable-next-line vue/require-default-prop
    value: PropTypes.string,
    // 编辑器配置
    config: PropTypes.object.def({})
  },
  data () {
    const myConfig = _.merge(defaultConfig, this.config)
    return {
      myConfig,
      content: '',
      filesModalType: FileTypeEnum.IMAGE.value
    }
  },
  watch: {
    value: {
      // 首次加载的时候执行函数
      immediate: true,
      handler (val) {
        this.content = val
      }
    },
    content (newVal) {
      // 使用setTimeout防止热更新时报错
      setTimeout(() => {
        this.$emit('change', newVal)
      }, 10)
    }
  },
  created () { },
  methods: {

    // Ueditor组件初始化
    beforeUeditorInit (editorId) {
      this.registerLink(editorId)
      this.registerSimpleupload(editorId)
      this.registerInsertvideo(editorId)
    },

    // 注册图片选择按钮
    registerSimpleupload (editorId) {
      const app = this
      window.UE.registerUI('simpleupload', function (editor, uiName) {
        // 创建一个 button
        var btn = new window.UE.ui.Button({
          // 按钮的名字
          name: uiName,
          // 提示
          title: '插入图片',
          // 需要添加的额外样式，可指定 icon 图标，图标路径参考常见问题 2
          cssRules: '',
          // 点击时执行的命令
          onclick: () => {
            app.filesModalType = FileTypeEnum.IMAGE.value
            app.$nextTick(() => {
              app.$refs.FilesModal.show()
            })
          }
        })
        // 因为你是添加 button，所以需要返回这个 button
        return btn
      }, undefined /* 指定添加到工具栏上的哪个位置，默认时追加到最后 */, editorId /* 指定这个 UI 是哪个编辑器实例上的，默认是页面上所有的编辑器都会添加这个按钮 */)
    },

    // 注册视频选择按钮
    registerInsertvideo (editorId) {
      const app = this
      window.UE.registerUI('insertvideo', function (editor, uiName) {
        // 创建一个 button
        var btn = new window.UE.ui.Button({
          // 按钮的名字
          name: uiName,
          // 提示
          title: '插入视频',
          // 需要添加的额外样式，可指定 icon 图标，图标路径参考常见问题 2
          cssRules: '',
          // 点击时执行的命令
          onclick: () => {
            app.filesModalType = FileTypeEnum.VIDEO.value
            app.$nextTick(() => {
              app.$refs.FilesModal.show()
            })
          }
        })
        // 因为你是添加 button，所以需要返回这个 button
        return btn
      }, undefined /* 指定添加到工具栏上的哪个位置，默认时追加到最后 */, editorId /* 指定这个 UI 是哪个编辑器实例上的，默认是页面上所有的编辑器都会添加这个按钮 */)
    },

    // 注册链接选择按钮
    registerLink (editorId) {
      const app = this
      window.UE.registerUI('link', function (editor, uiName) {
        // 创建一个 button
        var btn = new window.UE.ui.Button({
          // 按钮的名字
          name: uiName,
          // 提示
          title: '超链接',
          // 需要添加的额外样式，可指定 icon 图标，图标路径参考常见问题 2
          cssRules: '',
          // 点击时执行的命令
          onclick: () => {
            app.$refs.LinkModal.handle()
          }
        })
        // 因为你是添加 button，所以需要返回这个 button
        return btn
      }, undefined /* 指定添加到工具栏上的哪个位置，默认时追加到最后 */, editorId /* 指定这个 UI 是哪个编辑器实例上的，默认是页面上所有的编辑器都会添加这个按钮 */)
    },

    // 文件库modal确认回调
    handleFilesSelect (selectedItems) {
      const app = this
      if (selectedItems.length > 0) {
        let content = ''
        if (app.filesModalType == FileTypeEnum.IMAGE.value) {
          content = selectedItems.map(item => `<p><img src="${item.thumbnail}" /></p>`)
        }
        if (app.filesModalType == FileTypeEnum.VIDEO.value) {
          content = selectedItems.map(item => `<p><video style="width: 100%; height: 240px;" src="${item.external_url}" controls>${item.file_id}</video></p>`)
        }
        app.inserthtml(content.join(''))
      }
    },

    // 链接选择器modal 确认回调
    handleLinkSelect (result) {
      const app = this
      if (result.type === 'PAGE') {
        const attr = { href: result.param.url }
        if (!app.getSelectionText()) {
          attr.textValue = result.title
        }
        app.getEditor().execCommand('link', attr)
      }
    },

    // 插入内容到编辑器
    inserthtml (content) {
      this.getEditor().execCommand('inserthtml', content)
    },

    // 获取选中的文本
    getSelectionText () {
      //当点击按钮时编辑区域已经失去了焦点，如果直接用getText将不会得到内容，所以要在选回来，然后取得内容
      this.getEditor().selection.getRange().select()
      return this.getEditor().selection.getText()
    },

    getEditor () {
      return this.$refs.Ueditor.editor
    }

  }
}
</script>

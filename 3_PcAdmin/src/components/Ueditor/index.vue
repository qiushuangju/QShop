<template>
  <div style="width: 100%;height: 100%;padding: 10px;box-sizing: border-box;">
    <script id="editor" type="text/plain"></script>
  </div>
</template>
<script>
import './leipiFormDesign'
export default {
  name: 'Ueditor',
  data() {
    return {
      editor: null,
      editorContent: this.content
    }
  },
  props: {
    content: {
      type: String
    },
    config: {
      type: Object
    },
    fileds: {
      default: 0
    },
    formType: {
      type: [String, Number]
    }
  },
  mounted() {
    var _this = this
    if (window.ue !== undefined) {
      window.ue.destroy()
    }

    this.$nextTick(() => {
    // this.editor = window.UE.getEditor('editor', this.config) // 初始化UE
    // 表单设计器
      _this.editor = window.UE.getEditor('editor', {
        initialFrameWidth: '100%',
        initialFrameHeight: 450,
        toolleipi: true, // 是否显示，设计器的 toolbars
        textarea: 'design_content',
        // 这里可以选择自己需要的工具按钮名称,此处仅选择如下五个
        toolbars: [
          [
            'fullscreen',
            'source',
            '|',
            'undo',
            'redo',
            '|',
            'bold',
            'italic',
            'underline',
            'fontborder',
            'strikethrough',
            'removeformat',
            '|',
            'forecolor',
            'backcolor',
            'insertorderedlist',
            'insertunorderedlist',
            '|',
            'fontfamily',
            'fontsize',
            '|',
            'indent',
            '|',
            'justifyleft',
            'justifycenter',
            'justifyright',
            'justifyjustify',
            'horizontal',
            '|',
            'inserttable',
            'deletetable',
            'mergecells',
            'splittocells'
          ]
        ],
        wordCount: false,
        elementPathEnabled: false,
        autoHeightEnabled: true
        // initialFrameHeight: 430
      })
      window.ue = _this.editor
      _this.editor.addListener('ready', function() {
        _this.$emit('ready') // 通知父节点编辑器准备完毕
      })
    })
  },
  watch: {
    content: function(val) {
      if (val !== undefined && val !== '') {
        console.log(val)
        window.ue.setContent(val)
      }
    }
  },
  methods: {
    getObj() {
      // 获取内容方法
      var content = this.editor.getContent()
      if (this.formType === 'element-ui') {
        var dom = document.createElement('div')
        dom.innerHTML = content
        var iframes = dom.querySelectorAll('iframe')
        var fileds = iframes.length
        iframes.forEach(item => {
          if (item && !item.getAttribute('name')) {
            item.setAttribute('name', `data_${fileds}`)
            fileds++
          }
        })
        var returnOBj = {
          html: dom.innerHTML,
          content: dom.innerHTML
        }
        return returnOBj
      }
      return window.leipiFormDesign.parse_form(content, this.fileds)
    },
    destroyed() {
      this.editor.destroy()
    }
  }
}
</script>
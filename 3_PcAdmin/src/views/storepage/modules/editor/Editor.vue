<template>
  <div class="editor">
    <!-- 标题 -->
    <div class="editor-title">
      <span>{{
        selectedIndex === "page" ? data.page.name : curItem.name
      }}</span>
    </div>

    <!-- 编辑器: 标题栏 -->
    <div v-if="selectedIndex === 'page'" class="editor-content">
      <a-tabs>
        <a-tab-pane key="1" tab="页面设置">
          <div class="block-box">
            <div class="block-title">基本信息</div>
            <div class="block-item">
              <span class="label">页面名称</span>
              <div class="flex-box">
                <a-input v-model="data.page.paramsData.name" />
                <div class="tips">页面名称仅用于后台管理</div>
              </div>
            </div>
            <div class="block-item">
              <span class="label">分享标题</span>
              <div class="flex-box">
                <a-input v-model="data.page.paramsData.shareTitle" />
                <div class="tips">用户端转发时显示的标题</div>
              </div>
            </div>
          </div>
        </a-tab-pane>
        <a-tab-pane key="2" tab="标题栏设置">
          <div class="block-box">
            <div class="block-title">标题栏设置</div>
            <div class="block-item">
              <span class="label">标题名称</span>
              <div class="flex-box">
                <a-input v-model="data.page.paramsData.title" />
                <div class="tips">用户端端顶部显示的标题</div>
              </div>
            </div>
            <div class="block-item">
              <span class="label">文字颜色</span>
              <a-radio-group buttonStyle="solid" v-model="data.page.style.titleTextColor">
                <a-radio-button value="white">白色</a-radio-button>
                <a-radio-button value="black">黑色</a-radio-button>
              </a-radio-group>
            </div>
            <div class="block-item">
              <span class="label">标题栏背景</span>
              <div class="item-colorPicker">
                <span class="rest-color" @click="
                    onEditorResetColor(
                      data.page.style,
                      'titleBackgroundColor',
                      '#fff'
                    )
                  ">重置</span>
                <colorPicker v-model="data.page.style.titleBackgroundColor" defaultColor="#fff" />
              </div>
            </div>
          </div>
        </a-tab-pane>
      </a-tabs>
    </div>

    <template v-if="data.items.length && curItem">
      <!-- 搜索栏 -->
      <div v-if="curItem.type == 'search'" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="block-box">
              <div class="block-title">功能设置</div>
              <div class="block-item">
                <span class="label">提示文字</span>
                <a-input v-model="curItem.paramsData.placeholder" />
              </div>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">内容样式</div>
              <div class="block-item">
                <span class="label">搜索框样式</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.style.searchStyle">
                  <a-radio-button value="square">方形</a-radio-button>
                  <a-radio-button value="radius">圆角</a-radio-button>
                  <a-radio-button value="round">圆弧</a-radio-button>
                </a-radio-group>
              </div>
              <div class="block-item">
                <span class="label">文字对齐</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.style.textAlign">
                  <a-radio-button value="left">居左</a-radio-button>
                  <a-radio-button value="center">居中</a-radio-button>
                  <a-radio-button value="right">居右</a-radio-button>
                </a-radio-group>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>

      <!-- 辅助空白 -->
      <div v-if="curItem.type == 'blank'" class="editor-content">
        <div class="block-box">
          <div class="block-title">样式设置</div>
          <div class="block-item">
            <span class="label">组件高度</span>
            <div class="item-slider">
              <a-slider v-model="curItem.style.height" :min="5" :max="200" />
              <span class="unit-text">
                <span>{{ curItem.style.height }}</span>
                <span>像素</span>
              </span>
            </div>
          </div>
          <div class="block-item">
            <span class="label">背景颜色</span>
            <div class="item-colorPicker">
              <span class="rest-color" @click="onEditorResetColor(curItem.style, 'background', '#fff')">重置</span>
              <colorPicker v-model="curItem.style.background" defaultColor="#fff" />
            </div>
          </div>
        </div>
      </div>

      <!-- 辅助线 -->
      <div v-if="curItem.type == 'guide'" class="editor-content">
        <div class="block-box">
          <div class="block-title">样式设置</div>
          <div class="block-item">
            <span class="label">线条样式</span>
            <a-radio-group buttonStyle="solid" v-model="curItem.style.lineStyle">
              <a-radio-button value="solid">实线</a-radio-button>
              <a-radio-button value="dashed">虚线</a-radio-button>
              <a-radio-button value="dotted">点状</a-radio-button>
            </a-radio-group>
          </div>
          <div class="block-item">
            <span class="label">线条颜色</span>
            <div class="item-colorPicker">
              <span class="rest-color" @click="onEditorResetColor(curItem.style, 'lineColor', '#000')">重置</span>
              <colorPicker v-model="curItem.style.lineColor" defaultColor="#000" />
            </div>
          </div>
          <div class="block-item">
            <span class="label">线条高度</span>
            <div class="item-slider">
              <a-slider v-model="curItem.style.lineHeight" :min="1" :max="20" />
              <span class="unit-text">
                <span>{{ curItem.style.lineHeight }}</span>
                <span>像素</span>
              </span>
            </div>
          </div>
          <div class="block-item">
            <span class="label">上下边距</span>
            <div class="item-slider">
              <a-slider v-model="curItem.style.paddingTop" :min="0" :max="50" />
              <span class="unit-text">
                <span>{{ curItem.style.paddingTop }}</span>
                <span>像素</span>
              </span>
            </div>
          </div>
          <div class="block-item">
            <span class="label">背景颜色</span>
            <div class="item-colorPicker">
              <span class="rest-color" @click="onEditorResetColor(curItem.style, 'background', '#fff')">重置</span>
              <colorPicker v-model="curItem.style.background" defaultColor="#fff" />
            </div>
          </div>
        </div>
      </div>

      <!-- 富文本 -->
      <div v-if="curItem.type == 'richText'" :style="{ width: '395px' }" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="block-box">
              <div class="block-title">文本内容</div>
              <div class="ueditor">
                <Ueditor v-model="curItem.paramsData.content" :config="{ initialFrameWidth: 375 }" />
              </div>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">组件样式</div>
              <div class="block-item">
                <span class="label">上下边距</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.paddingTop" :min="0" :max="50" />
                  <span class="unit-text">
                    <span>{{ curItem.style.paddingTop }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
              <div class="block-item">
                <span class="label">左右边距</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.paddingLeft" :min="0" :max="50" />
                  <span class="unit-text">
                    <span>{{ curItem.style.paddingLeft }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
              <div class="block-item">
                <span class="label">背景颜色</span>
                <div class="item-colorPicker">
                  <span class="rest-color" @click="
                      onEditorResetColor(curItem.style, 'background', '#fff')
                    ">重置</span>
                  <colorPicker v-model="curItem.style.background" defaultColor="#fff" />
                </div>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>

      <!-- 店铺公告 -->
      <div v-if="curItem.type == 'notice'" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="block-box">
              <div class="block-title">公告文案</div>
              <div class="block-item">
                <span class="label">内容</span>
                <a-input v-model="curItem.paramsData.text" />
              </div>
              <div class="block-item">
                <span class="label">链接</span>
                <div class="flex-box">
                  <SLink v-model="curItem.paramsData.link" />
                </div>
              </div>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">内容样式</div>
              <div class="block-item">
                <span class="label">文字颜色</span>
                <div class="item-colorPicker">
                  <span class="rest-color" @click="
                      onEditorResetColor(curItem.style, 'textColor', '#000')
                    ">重置</span>
                  <colorPicker v-model="curItem.style.textColor" defaultColor="#000" />
                </div>
              </div>
              <div class="block-item">
                <span class="label">背景颜色</span>
                <div class="item-colorPicker">
                  <span class="rest-color" @click="
                      onEditorResetColor(curItem.style, 'background', '#fff')
                    ">重置</span>
                  <colorPicker v-model="curItem.style.background" defaultColor="#fff" />
                </div>
              </div>
            </div>
            <div class="block-box">
              <div class="block-title">组件样式</div>
              <div class="block-item">
                <span class="label">上下边距</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.paddingTop" :min="0" :max="50" />
                  <span class="unit-text">
                    <span>{{ curItem.style.paddingTop }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>

      <!-- 文章 -->
      <div v-if="curItem.type == 'article'" class="editor-content">
        <div class="block-box">
          <div class="block-title">文章内容</div>
          <div class="block-item">
            <span class="label">文章分类</span>
            <SArticleCate v-model="curItem.paramsData.auto.category" />
          </div>
          <div class="block-item">
            <span class="label">显示数量</span>
            <div class="block-item-right">
              <a-input-number v-model="curItem.paramsData.auto.showNum" :min="1" :max="20" />
              <span class="unit-text">
                <span>篇</span>
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- 在线客服 -->
      <div v-if="curItem.type == 'service'" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="block-box">
              <div class="block-title">功能设置</div>
              <div class="block-item">
                <span class="label">客服类型</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.paramsData.type">
                  <a-radio-button value="chat">在线聊天</a-radio-button>
                  <a-radio-button value="phone">拨打电话</a-radio-button>
                </a-radio-group>
              </div>
              <div v-show="curItem.paramsData.type == 'phone'" class="block-item">
                <span class="label">电话号码</span>
                <a-input v-model="curItem.paramsData.tel" />
              </div>
              <div class="block-item">
                <span class="label">客服图标</span>
                <span class="tips-wrap">建议尺寸：90×90</span>
                <SImage v-model="curItem.paramsData.image" :width="60" :height="60" />
              </div>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">组件样式</div>
              <div class="block-item">
                <span class="label">底边距</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.bottom" :min="0" :max="100" />
                  <span class="unit-text">
                    <span>{{ curItem.style.bottom }}</span>
                    <span>%</span>
                  </span>
                </div>
              </div>
              <div class="block-item">
                <span class="label">右边距</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.right" :min="0" :max="100" />
                  <span class="unit-text">
                    <span>{{ curItem.style.right }}</span>
                    <span>%</span>
                  </span>
                </div>
              </div>
              <div class="block-item">
                <span class="label">不透明度</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.opacity" :min="0" :max="100" />
                  <span class="unit-text">
                    <span>{{ curItem.style.opacity }}</span>
                    <span>%</span>
                  </span>
                </div>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>

      <!-- 视频 -->
      <div v-if="curItem.type == 'video'" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="block-box">
              <div class="block-title">功能设置</div>
              <div class="block-item">
                <span class="label">视频地址</span>
                <div class="flex-box">
                  <a-input v-model="curItem.paramsData.videoUrl" />
                  <div class="tips">仅支持.mp4格式的视频源地址</div>
                </div>
              </div>
              <div class="block-item">
                <span class="label">视频封面</span>
                <div class="flex-box">
                  <SImage v-model="curItem.paramsData.poster" :width="160" :height="90" />
                  <div class="tips">建议封面图片尺寸与视频比例一致</div>
                </div>
              </div>
            </div>
            <div class="block-box">
              <div class="block-title">播放设置</div>
              <div class="block-item">
                <span class="label">自动播放</span>
                <span class="tips-wrap">仅支持小程序</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.paramsData.autoplay">
                  <a-radio-button :value="1">开启</a-radio-button>
                  <a-radio-button :value="0">关闭</a-radio-button>
                </a-radio-group>
              </div>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">
                <span>内容样式</span>
                <span class="tips">视频宽度为750像素</span>
              </div>
              <div class="block-item">
                <span class="label">视频高度</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.height" :min="50" :max="400" />
                  <span class="unit-text">
                    <span>{{ curItem.style.height }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
            </div>
            <div class="block-box">
              <div class="block-title">组件样式</div>
              <div class="block-item">
                <span class="label">上下边距</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.paddingTop" :min="0" :max="50" />
                  <span class="unit-text">
                    <span>{{ curItem.style.paddingTop }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>

      <!-- 图片 -->
      <div v-if="curItem.type == 'image'" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="sub-title">添加图片 (最多10张，可拖动排序）</div>
            <draggable :list="curItem.paramsData" v-bind="{
                animation: 120,
                filter: 'input',
                preventOnFilter: false,
              }">
              <div v-for="(item, index) in curItem.paramsData" :key="index" class="block-box drag">
                <div class="block-title">
                  <span class="left">图片 {{ index + 1 }}</span>
                  <a class="link" @click="handleDeleleData(curItem, index)">删除</a>
                </div>
                <div class="block-item">
                  <div class="block-item-common">
                    <div class="block-item-common-row">
                      <span class="label">图片</span>
                      <span class="label value">{{ item.imgName }}</span>
                    </div>
                    <div class="block-item-common-row">
                      <span class="label">链接</span>
                      <SLink v-model="item.link" />
                    </div>
                  </div>
                  <div class="block-item-custom">
                    <SImage v-model="item.imgUrl" tips="建议尺寸：宽750" @update="item.imgName = $event.file_name" />
                  </div>
                </div>
              </div>
            </draggable>
            <div v-if="curItem.paramsData.length < 10" class="data-add">
              <a-button icon="plus" @click="handleAddData(10)">添加图片</a-button>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">组件样式</div>
              <div class="block-item">
                <span class="label">上下边距</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.paddingTop" :min="0" :max="50" />
                  <span class="unit-text">
                    <span>{{ curItem.style.paddingTop }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
              <div class="block-item">
                <span class="label">左右边距</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.paddingLeft" :min="0" :max="50" />
                  <span class="unit-text">
                    <span>{{ curItem.style.paddingLeft }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
              <div class="block-item">
                <span class="label">背景颜色</span>
                <div class="item-colorPicker">
                  <span class="rest-color" @click="
                      onEditorResetColor(curItem.style, 'background', '#fff')
                    ">重置</span>
                  <colorPicker v-model="curItem.style.background" defaultColor="#fff" />
                </div>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>

      <!-- 轮播图 -->
      <div v-if="curItem.type == 'banner'" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="sub-title">添加图片 (最多10张，可拖动排序）</div>
            <draggable :list="curItem.paramsData" v-bind="{
                animation: 120,
                filter: 'input',
                preventOnFilter: false,
              }">
              <div v-for="(item, index) in curItem.paramsData" :key="index" class="block-box drag">
                <div class="block-title">
                  <span class="left">图片 {{ index + 1 }}</span>
                  <a class="link" @click="handleDeleleData(curItem, index)">删除</a>
                </div>
                <div class="block-item">
                  <div class="block-item-common">
                    <div class="block-item-common-row">
                      <span class="label">图片</span>
                      <span class="label value">{{ item.imgName }}</span>
                    </div>
                    <div class="block-item-common-row">
                      <span class="label">链接</span>
                      <SLink v-model="item.link" />
                    </div>
                  </div>
                  <div class="block-item-custom">
                    <SImage v-model="item.imgUrl" tips="建议尺寸：750×400" @update="item.imgName = $event.file_name" />
                  </div>
                </div>
              </div>
            </draggable>
            <div v-if="curItem.paramsData.length < 10" class="data-add">
              <a-button icon="plus" @click="handleAddData(10)">添加图片</a-button>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">内容样式</div>
              <div class="block-item">
                <span class="label">指示点形状</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.style.btnShape">
                  <a-radio-button value="round">圆形</a-radio-button>
                  <a-radio-button value="square">正方形</a-radio-button>
                  <a-radio-button value="rectangle">长方形</a-radio-button>
                </a-radio-group>
              </div>
              <div class="block-item">
                <span class="label">指示点颜色</span>
                <div class="item-colorPicker">
                  <span class="rest-color" @click="
                      onEditorResetColor(curItem.style, 'btnColor', '#fff')
                    ">重置</span>
                  <colorPicker v-model="curItem.style.btnColor" defaultColor="#fff" />
                </div>
              </div>
              <div class="block-item">
                <span class="label">切换时间</span>
                <div class="item-slider" style="width: 190px">
                  <a-slider v-model="curItem.style.interval" :step="1" :min="1" :max="20" />
                  <span class="unit-text">
                    <span>{{ curItem.style.interval }}</span>
                    <span>秒</span>
                  </span>
                </div>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>

      <!-- 商品组 -->
      <div v-if="curItem.type == 'goods'" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="block-box">
              <div class="block-title">
                <span>商品来源</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.paramsData.source">
                  <a-radio-button value="auto">自动获取</a-radio-button>
                  <a-radio-button value="choice">手动选择</a-radio-button>
                </a-radio-group>
              </div>
            </div>
            <!-- 手动选择 -->
            <div v-if="curItem.paramsData.source === 'choice'" class="block-box">
              <div class="block-title">
                选择商品 ({{ curItem.defaultData.length }})
              </div>
              <SGoods v-model="curItem.defaultData" />
            </div>
            <!-- 自动获取 -->
            <div v-if="curItem.paramsData.source === 'auto'" class="block-box">
              <div class="block-title">商品内容</div>
              <div class="block-item">
                <span class="label">商品分类</span>
                <SGoodsCate v-model="curItem.paramsData.auto.category" />
              </div>
              <div class="block-item">
                <span class="label">商品排序</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.paramsData.auto.goodsSort">
                  <a-radio-button value="all">默认</a-radio-button>
                  <a-radio-button value="sales">销量</a-radio-button>
                  <a-radio-button value="price">价格</a-radio-button>
                </a-radio-group>
              </div>
              <div class="block-item">
                <span class="label">显示数量</span>
                <div class="block-item-right">
                  <a-input-number v-model="curItem.paramsData.auto.showNum" :min="1" :max="50" />
                  <span class="unit-text">
                    <span>件</span>
                  </span>
                </div>
              </div>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">模块头样式</div>
              <div class="block-item">
                <span class="label">模块标题</span>
                <a-input v-model="curItem.style.titleText" />
                <el-checkbox style="margin-left:5px" v-model="curItem.style.titleShow">显示</el-checkbox>

              </div>
              <div class="block-item" v-if="curItem.style.titleShow">
                <span class="label">更多文字</span>
                <a-input v-model="curItem.style.titleMoreText" />
              </div>
              <div class="block-item-common-row" v-if="curItem.style.titleShow">
                <span class="label">链接</span>
                <SLink v-model="curItem.style.titleMoreLink" />
              </div>
              <div class="item-colorPicker" v-if="curItem.style.titleShow">
                <span class="label">背景颜色</span>
                <span class="rest-color" @click="onEditorResetColor(curItem.style, 'titlebackground', '#fff')">重置</span>
                <colorPicker v-model="curItem.style.titlebackground" defaultColor="#fff" />
              </div>

            </div>

            <div class="block-box">
              <div class="block-title">内容样式</div>
              <div class="block-item">
                <span class="label">显示类型</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.style.display">
                  <a-radio-button value="list">列表平铺</a-radio-button>
                  <a-radio-button :disabled="curItem.style.column === 1" value="slide">横向滑动</a-radio-button>
                </a-radio-group>
              </div>
              <div class="block-item">
                <span class="label">分列数量</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.style.column">
                  <a-radio-button :disabled="curItem.style.display !== 'list'" :value="1">单列</a-radio-button>
                  <a-radio-button :value="2">两列</a-radio-button>
                  <a-radio-button :value="3">三列</a-radio-button>
                </a-radio-group>
              </div>
              <div class="block-item">
                <span class="label">显示内容</span>
                <div class="item-checkbox" :style="{ width: '180px' }">
                  <a-checkbox-group v-model="curItem.style.show">
                    <a-checkbox value="goodsName">商品名称</a-checkbox>
                    <a-checkbox value="linePrice">划线价</a-checkbox>
                    <a-checkbox value="salePrice">零售价</a-checkbox>
                    <a-checkbox value="goodsSales">商品销量</a-checkbox>
                    <a-checkbox value="subTitle">小标题(卖点)</a-checkbox>
                  </a-checkbox-group>
                </div>
              </div>
            </div>
            <div class="block-box">
              <div class="block-title">组件样式</div>
              <div class="block-item">
                <span class="label">背景颜色</span>
                <div class="item-colorPicker">
                  <span class="rest-color" @click="onEditorResetColor(curItem.style, 'background', '#fff')
                    ">重置</span>
                  <colorPicker v-model="curItem.style.background" defaultColor="#fff" />
                </div>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>

      <!-- 导航 -->
      <div v-if="curItem.type == 'navBar'" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="sub-title">
              添加导航 (最少4个，最多10个，可拖动排序)
            </div>
            <draggable :list="curItem.paramsData" v-bind="{animation: 120,filter: 'input',preventOnFilter: false,}">
              <div v-for="(item, index) in curItem.paramsData" :key="index" class="block-box drag">
                <div class="block-title">
                  <span class="left">导航 {{ index + 1 }}</span>
                  <a class="link" @click="handleDeleleData(curItem, index)">删除</a>
                </div>
                <div class="block-item">
                  <div class="block-item-common">
                    <div class="block-item-common-row">
                      <span class="label">名称</span>
                      <a-input v-model="item.text" />
                    </div>
                    <div class="block-item-common-row">
                      <span class="label">链接</span>
                      <SLink v-model="item.link" />
                    </div>
                  </div>
                  <div class="block-item-custom">
                    <SImage v-model="item.imgUrl" tips="建议尺寸：100×100" />
                  </div>
                </div>
              </div>
            </draggable>
            <div v-if="curItem.paramsData.length < 10" class="data-add">
              <a-button icon="plus" @click="handleAddData(10)">添加导航</a-button>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">内容样式</div>
              <div class="block-item">
                <span class="label">每行数量</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.style.rowsNum">
                  <a-radio-button :value="3">3个</a-radio-button>
                  <a-radio-button :value="4">4个</a-radio-button>
                  <a-radio-button :value="5">5个</a-radio-button>
                </a-radio-group>
              </div>
              <div class="block-item">
                <span class="label">文字颜色</span>
                <div class="item-colorPicker">
                  <span class="rest-color" @click="
                      onEditorResetColor(curItem.style, 'textColor', '#000')
                    ">重置</span>
                  <colorPicker v-model="curItem.style.textColor" defaultColor="#000" />
                </div>
              </div>
            </div>
            <div class="block-box">
              <div class="block-title">组件样式</div>
              <div class="block-item">
                <span class="label">上下边距</span>
                <div class="item-slider">
                  <a-slider v-model="curItem.style.paddingTop" :min="0" :max="50" />
                  <span class="unit-text">
                    <span>{{ curItem.style.paddingTop }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
              <div class="block-item">
                <span class="label">背景颜色</span>
                <div class="item-colorPicker">
                  <span class="rest-color" @click="
                      onEditorResetColor(curItem.style, 'background', '#fff')
                    ">重置</span>
                  <colorPicker v-model="curItem.style.background" defaultColor="#fff" />
                </div>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>
      <!-- 橱窗 -->
      <div v-if="curItem.type == 'window'" class="editor-content">
        <a-tabs>
          <a-tab-pane key="1" tab="内容设置">
            <div class="sub-title">添加图片 (最多10个，可拖动排序)</div>
            <draggable :list="curItem.paramsData" v-bind="{ animation: 120, filter: 'input', preventOnFilter: false,}">
              <div v-for="(item, index) in curItem.paramsData" :key="index" class="block-box drag">
                <div class="block-title">
                  <span class="left">图片 {{ index + 1 }}</span>
                  <a class="link" @click="handleDeleleData(curItem, index)">删除</a>
                </div>
                <div class="block-item">
                  <div class="block-item-common">
                    <div class="block-item-common-row">
                      <span class="label">名称</span>
                      <span class="label value">{{ item.imgName }}</span>
                    </div>
                    <div class="block-item-common-row">
                      <span class="label">链接</span>
                      <SLink v-model="item.link" />
                    </div>
                  </div>
                  <div class="block-item-custom">
                    <SImage v-model="item.imgUrl" @update="item.imgName = $event.file_name" />
                  </div>
                </div>
              </div>
            </draggable>
            <div v-if="curItem.paramsData.length < 10" class="data-add">
              <a-button icon="plus" @click="handleAddData(10)">添加图片</a-button>
            </div>
          </a-tab-pane>
          <a-tab-pane key="2" tab="样式设置">
            <div class="block-box">
              <div class="block-title">内容样式</div>
              <div class="block-item">
                <span class="label">每行数量</span>
                <a-radio-group buttonStyle="solid" v-model="curItem.style.layout">
                  <a-radio-button :value="2">2列</a-radio-button>
                  <a-radio-button :value="3">3列</a-radio-button>
                  <a-radio-button :value="4">4列</a-radio-button>
                  <a-radio-button :value="-1">橱窗</a-radio-button>
                </a-radio-group>
              </div>
              <div class="block-item">
                <span class="label">上下边距</span>
                <div class="item-slider" :style="{ width: '210px' }">
                  <a-slider v-model="curItem.style.paddingTop" :min="0" :max="50" />
                  <span class="unit-text">
                    <span>{{ curItem.style.paddingTop }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
              <div class="block-item">
                <span class="label">左右边距</span>
                <div class="item-slider" :style="{ width: '210px' }">
                  <a-slider v-model="curItem.style.paddingLeft" :min="0" :max="50" />
                  <span class="unit-text">
                    <span>{{ curItem.style.paddingLeft }}</span>
                    <span>像素</span>
                  </span>
                </div>
              </div>
            </div>
            <div class="block-box">
              <div class="block-title">组件样式</div>
              <div class="block-item">
                <span class="label">背景颜色</span>
                <div class="item-colorPicker">
                  <span class="rest-color" @click="
                      onEditorResetColor(curItem.style, 'background', '#fff')
                    ">重置</span>
                  <colorPicker v-model="curItem.style.background" defaultColor="#fff" />
                </div>
              </div>
            </div>
          </a-tab-pane>
        </a-tabs>
      </div>
    </template>
  </div>
</template>

<script>
import Vue from 'vue'
import vcolorpicker from 'vcolorpicker'
import _ from 'lodash'
import PropTypes from 'ant-design-vue/es/_util/vue-types'
import draggable from 'vuedraggable'
import { Ueditor } from '@/components'
import * as Api from '@/api/commApi'
// import * as sysMessages from '@/api/sysmessages'
import { SImage, SArticleCate, SGoods, SGoodsCate, SLink } from './modules'

Vue.use(vcolorpicker)

export default {
  props: {
    // 页面装修默认数据
    defaultData: PropTypes.object.def({}),
    // 页面数据
    data: PropTypes.object.def({}),
    // 当前选中的元素
    curItem: PropTypes.object.def({}),
    // 当前选中的元素索引
    selectedIndex: PropTypes.oneOfType([
      PropTypes.number,
      PropTypes.string,
    ]).def(0),
  },
  components: {
    draggable,
    Ueditor,
    SImage,
    SArticleCate,
    SGoods,
    SGoodsCate,
    SLink,
  },
  data() {
    return {
      arrLabel: [],
    }
  },
  created() {
    this.ddlGoodsLabel()
  },
  methods: {
    // 新增数据
    handleAddData(max = 1) {
      const { defaultData, curItem } = this
      const newDataItem = defaultData.items[curItem.type].paramsData[0]
      curItem.paramsData.push(_.cloneDeep(newDataItem))
    },

    /**
     * 编辑器：删除data元素
     * @param curItem
     * @param index
     */
    handleDeleleData(curItem, index) {
      if (curItem.paramsData.length <= 1) {
        this.$message.warning('至少保留一个')
        return false
      }
      curItem.paramsData.splice(index, 1)
    },

    /**
     * 重置颜色
     * @param holder
     * @param attribute
     * @param color
     */
    onEditorResetColor(holder, attribute, color) {
      holder[attribute] = color
    },
    ddlGoodsLabel() {
      Api.ddlGoodsLabel().then((res) => {
        if (res.code == 200) {
          this.arrLabel = res.result
        }
      })
    },
  },
}
</script>
<style lang="less" scoped>
@import "./style.less";
</style>

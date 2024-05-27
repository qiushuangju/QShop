<template>
  <div id="divBody" class="divBody">
    <div class="viewContainer">
      <div class="loginbox boxall">
        <div class="logo">
          <img class="imgLogo" src="../../assets/img/logo-name.png" alt="">
          <!-- <h1>QShop管理登录页</h1> -->
        </div>
        <!-- <div class="logintit">QShop商城系统</div> -->

        <el-form class="login-form" autoComplete="on" :model="loginForm" :rules="loginRules" ref="loginForm" label-position="left">
          <el-form-item prop="username">

            <el-input prefix-icon="iconfont icon-yonghu_zhanghao_wode" name="username" type="text" v-model="loginForm.username" autoComplete="on" placeholder="请输入登录账号" />
          </el-form-item>
          <el-form-item prop="password">
            <el-input placeholder="请输入密码" prefix-icon="iconfont icon-xianshi_chakan" name="password" v-model="loginForm.password" @keyup.enter.native="handleLogin" show-password autoComplete="on"></el-input>
          </el-form-item>
          <el-form-item prop="verifyCode">
            <el-input name="verifyCode" prefix-icon="iconfont icon-erweima" @keyup.enter.native="handleLogin" v-model="loginForm.verifyCode" autoComplete="off" placeholder="请输入验证码">
            </el-input>
            <img class="imgCode" @click="changeVerifyCode" :src="'data:image/png;base64,'+ imageData" />
          </el-form-item>
          <el-form-item>
            <el-button style="width:100%;background-color: #1890ff;border-color: #1890ff ; box-shadow: 0 9px 16px 0 rgba(34,185,255,.25) !important; font-size: 18px;height:50px;color:#fff" :loading="loading"
              @click.native.prevent="handleLogin">
              登 录
            </el-button>
          </el-form-item>
        </el-form>
      </div>
      <div class="copyright">版权所有：All Right By <a href="http://baidu.com">QS</a> 2024-2030</div>
    </div>
    <vue-canvas-nest :config="config" :el="'#divBody'"></vue-canvas-nest>
  </div>
</template>

<script>
import waves from '@/directive/waves' // 水波纹指令
// import '@/assets/js/canvas-nest.min'

import vueCanvasNest from 'vue-canvas-nest'
import { mapGetters, mapActions } from 'vuex'
import * as loginApi from '@/api/login'
export default {
  name: 'login',
  components: {
    vueCanvasNest,
  },
  directives: {
    waves,
  },
  data() {
    const validateUsername = (rule, value, callback) => {
      if (value.length <= 0) {
        callback(new Error('用户名不能为空'))
      } else {
        callback()
      }
    }
    const validatePass = (rule, value, callback) => {
      if (value.length <= 0) {
        callback(new Error('密码不能为空'))
      } else {
        callback()
      }
    }
    return {
      //配置属性
      config: {
        color: '133, 133, 133',
        opacity: 0.9,
        zIndex: 5,
        count: 90,
      }, //配置炫酷效果
      tenant: this.tenantid || 'QsDBContext', //当前选择的租户
      tenants: [
        {
          value: 'QsDBContext',
          label: '默认租户',
        },
        {
          value: 'ErrorId404',
          label: '模拟一个不存在的租户',
        },
      ],
      loginForm: {
        username: 'qshop',
        password: 'qshop',
      },
      loginRules: {
        username: [
          {
            required: true,
            trigger: 'blur',
            validator: validateUsername,
          },
        ],
        password: [
          {
            required: true,
            trigger: 'blur',
            validator: validatePass,
          },
        ],
        verifyCode: [
          {
            required: true,
            trigger: 'blur',
            message: '验证码不能为空',
          },
        ],
      },
      loading: false,
      pwdType: 'password',
      imageData: '',
      verifyCodeId: '',
      identifyCode: '',
    }
  },
  computed: {
    ...mapGetters(['tenantid']),
  },
  created() {
    this.getVerifyCode()
  },
  methods: {
    ...mapActions([
      'setTenantId', //
    ]),
    getVerifyCode() {
      loginApi.getImageCaptcha().then((res) => {
        this.imageData = res.result.base64Str
        this.verifyCodeId = res.result.verifyCodeId
      })
    },
    tenantChange(e) {
      this.setTenantId(e)
    },
    changeVerifyCode(e) {
      this.getVerifyCode()
    },
    handleLogin() {
      this.loginForm.verifyCodeId = this.verifyCodeId
      this.$refs.loginForm.validate((valid) => {
        if (valid) {
          this.loading = true
          this.$store
            .dispatch('Login', this.loginForm)
            .then(() => {
              this.loading = false
              this.$router.push({
                path: '/',
              })
            })
            .catch(() => {
              this.getVerifyCode()
              this.loading = false
            })
        } else {
          return false
        }
      })
    },
  },
}
</script>

<style rel="stylesheet/scss" lang="scss">
$bg: #2d3a4b;
$light_gray: #eee;
$color_balck: #333;

/* reset element-ui css */
.viewContainer {
  text-align: center;
  .el-input {
    display: inline-block;
    height: 47px;
    width: 85%;

    input {
      background: transparent;
      border: 0px;
      -webkit-appearance: none;
      border-radius: 0px;
      padding: 12px 5px 12px 30px;
      color: $color_balck;
      height: 47px;

      &:-webkit-autofill {
        transition: background-color 5000s ease-in-out 0s;
      }
    }
  }

  .el-form-item {
    margin-bottom: 35px;
    border-radius: 5px;
    color: #454545;

    .el-form-item__content {
      background: #fff;
      border: 1px solid rgba(223, 223, 223, 1);
    }
  }
}

.divBody {
  width: 100%;
  height: 100%;
}
.viewContainer {
  // max-width: 1920px;
  height: 100%;
  width: 100%;
  font-family: "PingFang SC", "Lantinghei SC", "Microsoft YaHei", "HanHei SC",
    "Helvetica Neue", "Open Sans", Arial, "Hiragino Sans GB", "微软雅黑",
    STHeiti, "WenQuanYi Micro Hei", SimSun, sans-serif;
}

.loginbox {
  display: inline-block;
  margin-top: 200px;
  text-align: center;
  vertical-align: middle;
  justify-content: center;
  min-width: 260px;
  width: 460px;
  padding: 40px 60px;
  background: hsla(0, 0%, 100%, 0.48);
  text-align: left;
}
.imgCode {
  position: absolute;
  right: 0px;
}
li {
  list-style-type: none;
}
a {
  text-decoration: none;
  color: #fff;
}

input::input-placeholder {
  color: #fff;
}
::-webkit-input-placeholder {
  color: #fff;
}

.logo {
  // margin-left: 10%;
  // width: 80%;
  // height: 70px;
}
.imgLogo {
  width: 100%;
  height: 42px;
  margin-bottom: 30px;
}

.boxall {
  position: relative;
  transform: scale(0.7);
  opacity: 0;
  animation: shows 1s forwards;
}
@keyframes shows {
  to {
    transform: scale(1);
    opacity: 1;
  }
}

.copyright {
  position: fixed;
  width: 100%;
  left: 0;
  bottom: 10px;
  text-align: center;
  color: #666666;
  font-size: 12px;
}
.copyright a {
  padding: auto 20px;
  color: #20a3f5;
}

// .boxfoot:before {
//     border-left: 3px solid #20a3f5;
//     left: -2px;
// }
// .boxfoot:after {
//     border-right: 3px solid #20a3f5;
//     right: -2px;
// }

// .boxfoot {
//     position: absolute;
//     bottom: 0;
//     width: 100%;
//     left: 0;
// }
// .boxfoot:before,
// .boxfoot:after {
//     position: absolute;
//     width: 30px;
//     height: 30px;
//     content: '';
//     border-bottom: 3px solid #20a3f5;
//     bottom: -2px;
// }

@media (max-width: 1200px) {
  .loginav {
  }
  .video {
    display: none;
  }
  .loginbox {
    width: 96%;
  }
  .logo h1 {
    font-size: 30px;
  }
  .logo h2 {
    font-size: 20px;
  }
  .loginbox {
    padding: 30px;
  }
}
</style>
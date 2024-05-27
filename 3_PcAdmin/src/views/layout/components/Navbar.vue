<template>
  <el-menu class="navbar" mode="horizontal">
    <div class="logo">
      <!-- <img class="user-avatar" :src="logo"> -->
      <!-- &nbsp; -->
      <span style="font-size:18px;line-height: 40px;">QShop商城系统</span>

    </div>
    <hamburger class="hamburger-container" :toggleClick="toggleSideBar" :isActive="sidebar.opened"></hamburger>
    <el-dropdown class="avatar-container" @command="handleCommand" trigger="click">
      <div class="avatar-wrapper">
        欢迎您，{{name}}
        <i class="el-icon-caret-bottom"></i>
      </div>
      <el-dropdown-menu class="user-dropdown" slot="dropdown">
        <el-dropdown-item command="handleGoProfile">
          <span>个人中心</span>
        </el-dropdown-item>
        <el-dropdown-item>
          <span>切换主题
            <el-switch :active-value="1" :inactive-value="0" style="margin-left: 5px;" v-model="theme" />
          </span>
        </el-dropdown-item>
        <el-dropdown-item command="logout" divided>
          <span>退出</span>
        </el-dropdown-item>
      </el-dropdown-menu>
    </el-dropdown>
  </el-menu>
</template>

<script>
import { mapGetters, mapActions } from 'vuex'
import Hamburger from '@/components/Hamburger'
import logo from '@/assets/logo.png?imageView2/1/w/80/h/80'

export default {
  data: function () {
    return {
      logo: logo,
      theme: 0,
    }
  },
  components: {
    Hamburger,
  },
  computed: {
    ...mapGetters(['sidebar', 'name', 'themeStatus']),
  },
  watch: {
    theme() {
      this.toggleClass(document.body, 'custom-theme')
    },
  },
  mounted() {
    this.theme = Number(this.themeStatus)
    this.toggleClass(document.body, 'custom-theme')
  },
  methods: {
    ...mapActions(['signOutOidc', 'saveTheme']),
    toggleClass(element, className) {
      if (!element || !className) {
        return
      }
      let classString = element.className
      const nameIndex = classString.indexOf(className)
      if (nameIndex === -1) {
        classString += '' + className
      } else if (this.theme !== 1) {
        classString =
          classString.substr(0, nameIndex) +
          classString.substr(nameIndex + className.length)
      }
      element.className = this.theme === 1 ? classString : ''
      console.log("theme", this.theme)
      this.saveTheme(this.theme)
    },
    toggleSideBar() {
      this.$store.dispatch('ToggleSideBar')
    },
    logout() {
      this.$store.dispatch('LogOut').then(() => {
        location.reload() // 为了重新实例化vue-router对象 避免bug
      })
    },
    handleGoProfile() {
      this.$router.push('/profile')
    },
    handleCommand(name) {
      if (!name) return
      this[name]()
    },
  },
}
</script>

<style rel="stylesheet/scss" lang="scss" scoped>
</style>

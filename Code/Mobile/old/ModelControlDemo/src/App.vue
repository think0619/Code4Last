<template>
  <div id="app"> 
    <!-- <van-tabbar route v-model="active" active-color="#3385ff" inactive-color="#000">
      <van-tabbar-item icon="home-o" replace to="/">绿色能源</van-tabbar-item>
      <van-tabbar-item icon="search" replace to="/cart">智慧城市</van-tabbar-item>
      <van-tabbar-item icon="friends-o" replace to="/user">智慧工厂</van-tabbar-item>
      <van-tabbar-item icon="setting-o" replace to="/card">智慧煤矿</van-tabbar-item>
    </van-tabbar> -->

    <!-- <van-tabbar  v-model="active" active-color="#3385ff" inactive-color="#000">
      <van-tabbar-item icon="home-o" replace >绿色能源</van-tabbar-item>
      <van-tabbar-item icon="search" replace >智慧城市</van-tabbar-item>
      <van-tabbar-item icon="friends-o" replace >智慧工厂</van-tabbar-item>
      <van-tabbar-item icon="setting-o" replace >智慧煤矿</van-tabbar-item>
    </van-tabbar> -->
    <router-view />
  </div>
</template>

<!-- 引入样式文件 -->
<!-- <link rel="stylesheet" href="/static/css/vantindex.css"/> -->
<script>
// import Vue from 'vue';
// import { Button,Toast, Tabbar,TabbarItem, } from 'vant';
// Vue.use(Button); 
// Vue.use(Toast); 
// Vue.use(Tabbar);
// Vue.use(TabbarItem); 
 

export default {
  name: 'App',
  data() {
    return {
      active: 0,
    }
  },
  setup() {
    // const active = ref('cart');
    // const themeVars = {
    //   tabbarHeight: '50px',
    //   tabbarItemIconSize: '26px',
    //   tabbarItemFontSize: 'var(--van-font-size-sm)',
    // };
    // return { active, themeVars };
  },  
  //初始化页面选中状态
  // created() {
  //   //由于 vant 标签栏路由模式，无法自动加载页面，所以这里需要初始化
  //   console.log(this.$route);//打印当前路由属性
  //   if (this.$route.path === '/') {
  //     this.$router.push('/');
  //   }
  // }


}
</script>
<style>
body {
  height: 100%;
  font-size: 15px;
  background-color: #f8f8f8;
  -webkit-font-smoothing: antialiased;
}

html,
body,
#app {
  height: 100%;
  margin: 0;
  padding: 0; 
}

html,body{
    overflow-x: hidden;
  position: relative;
  width: 100%;
  height: 100%;
}
</style>

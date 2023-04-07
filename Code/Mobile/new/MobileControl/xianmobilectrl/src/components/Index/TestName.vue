<template>
  <div id="nav">

    <van-nav-bar v-bind:title=title  :fixed=true />
  </div>

<div class="mainbody"> 
  <h1>{{ a1 }}</h1>

      <div style="margin-bottom:50px">
        <van-config-provider :theme-vars="themeVars">
      <van-button type="primary" @click="clickDemo()" >Primary</van-button>

          <van-tabbar v-model="active" @change="onChange"
          safe-area-inset-bottom="true"
            active-color="#3385ff" inactive-color="#000"
           > 
            <van-tabbar-item icon="home-o" badge="5">绿色能源</van-tabbar-item>
            <van-tabbar-item icon="search" dot>智慧城市</van-tabbar-item>
            <van-tabbar-item icon="friends-o" badge="5">智慧工厂</van-tabbar-item>
            <van-tabbar-item icon="setting-o" dot>智慧煤矿</van-tabbar-item>
          </van-tabbar>
        </van-config-provider>
        </div>
</div> 

</template>

<script>
import { ref } from 'vue'; 
import { Button, NavBar , Tabbar, TabbarItem,Toast  } from 'vant';
 


export default {
  name: 'TestName',
  components: {
    [Button.name]: Button,
    [Tabbar.name]: Tabbar,
    [NavBar.name]: NavBar,
    [TabbarItem.name]: TabbarItem 
  },
  props: {

  },
  data() {
    return {
      title: '展会中控'
    }
  },
  methods: {
    clickDemo() {
      location.href="http://10.10.20.243:8080/#/main";
    },
    onChange(index) {
//       Toast.loading({
//   message: 'Loading...',
//   forbidClick: true,
// });
    }
  },
  setup() {
    const active = ref('home');

    const themeVars = { 
      tabbarHeight:'50px',
      tabbarItemIconSize:'26px',
      tabbarItemFontSize:'var(--van-font-size-sm)',
    };

    return { active,themeVars };
  },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped> 
h3 {
  margin: 40px 0 0;
}

.mainbody{
  margin-top: 60px;;
}
 
.van-nav-bar {
  background-color:#2f97ec;
}
#nav /deep/ .van-nav-bar__title{
  color: white !important;
  font-size: 1rem;
}

</style>

<style>

</style>

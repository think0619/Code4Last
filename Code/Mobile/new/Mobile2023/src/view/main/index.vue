<template>
  <van-nav-bar  class="vannav"
  :border=false
  title="绿色·低碳·智慧园区"
  left-text=""
  left-arrow
  @click-left="onClickLeft"
/>
  <div> 
        <van-tabbar v-model="active" @change="onChange" :border="false"   safe-area-inset-bottom="true" active-color="#333" inactive-color="#FFF" > 
          <van-tabbar-item icon="home-o" badge="5">绿色能源</van-tabbar-item>
          <van-tabbar-item icon="search" dot>智慧城市</van-tabbar-item>
          <van-tabbar-item icon="friends-o" badge="5">智慧工厂</van-tabbar-item>
          <van-tabbar-item badge="3">
      <span>自定义</span>
      <template #icon="props">
        <img :src="props.active ? icon.active : icon.inactive" />
      </template>
    </van-tabbar-item>
        </van-tabbar>
      
  </div>
</template>

<script>
import { createApp } from 'vue';
import { Tabbar, TabbarItem } from 'vant';
import { ref } from 'vue'; 
import { NavBar } from 'vant';
import { ConfigProvider } from 'vant';


const app = createApp();
app.use(Tabbar);
app.use(TabbarItem); 
app.use(NavBar); 
app.use(ConfigProvider);

export default {
  setup() {
    const onClickLeft = () =>   location.href="http://10.10.20.243:8081/";
    const active = ref(0);
    const icon = {
      active: 'https://fastly.jsdelivr.net/npm/@vant/assets/user-active.png',
      inactive:
        'https://fastly.jsdelivr.net/npm/@vant/assets/user-inactive.png',
    };
    return { icon,active,onClickLeft };
  },
};
</script>

<style >
:root:root {
  --van-nav-bar-title-text-color: #fff;
  --van-nav-bar-icon-color: #fff;
  --van-tabbar-background: transparent;
}
</style>
<style lang="less">
 


.van-nav-bar {
  background:url(../../assets/navbarbg.png);
  background-size: cover;
}
 

</style>


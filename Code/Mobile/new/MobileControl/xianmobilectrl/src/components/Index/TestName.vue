<template>
  <van-nav-bar title="Title" left-text="Back" left-arrow />
  <h1>{{ name }}</h1>
  <h1>{{ a1 }}</h1>
  <van-button type="primary" @click="clickDemo()">Primary</van-button>



  <van-tabbar v-model="active"  @change="onChange">
  <van-tabbar-item icon="home-o">标签</van-tabbar-item>
  <van-tabbar-item icon="search">标签</van-tabbar-item>
  <van-tabbar-item icon="friends-o">标签</van-tabbar-item>
  <van-tabbar-item icon="setting-o">标签</van-tabbar-item>
</van-tabbar>
</template>

<script>
import { ref } from 'vue'; 
import { Button, NavBar, Tabbar, TabbarItem,showToast } from 'vant';
 


export default {
  name: 'TestName',
  components: {
    [Button.name]: Button,
    [Tabbar.name]: Tabbar,
    [TabbarItem.name]: TabbarItem, 
  },
  props: {

  },
  data() {
    return {
      a1: '11a'
    }
  },
  methods: {
    clickDemo() {
      alert('hi');
    }
  },
  setup() {
    const active = ref(0);
    const onChange = (index) => showToast(`标签 ${index}`);
    return {
      icon,
      onChange,
    };
  },
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h3 {
  margin: 40px 0 0;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
